using System;
using System.Collections;

namespace Crainiate.Diagramming.Navigation
{
	internal struct Node
	{
		public Shape Shape;
		public Elements Children;
		public Elements Parents;
	}
	
	public class Navigate
	{
		//Property variables
		private Elements mShapes;
		private Elements mLines;
		private Shape mStart;
		private Hashtable mNodes;
		
		#region Interface
		
		//Constructors
		public Navigate()
		{

		}

		public Navigate(Elements shapes,Elements lines)
		{
			Shapes = shapes;
			Lines = lines;
		}

		public Navigate(IContainer container)
		{
			Shapes = container.Shapes;
			Lines = container.Lines;
		}

		//Properties
		public virtual Elements Shapes
		{
			get
			{
				return mShapes;
			}
			set
			{
				if (mShapes != null)
				{
					mShapes.InsertElement -=new ElementsEventHandler(Elements_InsertElement);
					mShapes.RemoveElement -=new ElementsEventHandler(Elements_RemoveElement);
				}
				mNodes = null;
				mShapes = value;

				if (value != null)
				{
					mShapes.InsertElement +=new ElementsEventHandler(Elements_InsertElement);
					mShapes.RemoveElement +=new ElementsEventHandler(Elements_RemoveElement);
				}
			}
		}

		public virtual Elements Lines
		{
			get
			{
				return mLines;
			}
			set
			{
				//Remove any handlers
				if (mLines != null)
				{
					mLines.InsertElement -=new ElementsEventHandler(Elements_InsertElement);
					mLines.RemoveElement -=new ElementsEventHandler(Elements_RemoveElement);

					foreach (Line line in mLines.Values)
					{
						line.Start.DockChanged -=new EventHandler(Origin_DockChanged);
						line.End.DockChanged -=new EventHandler(Origin_DockChanged);
					}
				}

				mNodes = null;
				mLines = value;
				mLines.InsertElement+=new ElementsEventHandler(Elements_InsertElement);
				mLines.RemoveElement+=new ElementsEventHandler(Elements_RemoveElement);

				foreach (Line line in mLines.Values)
				{
					line.Start.DockChanged +=new EventHandler(Origin_DockChanged);
					line.End.DockChanged +=new EventHandler(Origin_DockChanged);
				}
			}
		}
		
		public virtual Shape Start
		{
			get 
			{
				return mStart;
			}
			set
			{
				mStart = value;
			}
		}

		//Methods
		//Returns all the parents of the start shape
		public virtual Elements Parents()
		{
			return GetNodes(true,false,1);
		}

		//Returns all the parents of the start shape
		public virtual Elements Parents(int generations)
		{
			return GetNodes(true,false,generations);
		}

		//Returns all the parents of the start shape for all generations
		public virtual Elements AllParents()
		{
			return GetNodes(true,false,Shapes.Count);
		}

		//Returns all the children of the start shape
		public virtual Elements Children()
		{
			return GetNodes(false,true,1);
		}

		//Returns all the children of the start shape
		public virtual Elements Children(int generations)
		{
			return GetNodes(false,true,generations);
		}

		//Returns all children for the start node for all generations
		public virtual Elements AllChildren()
		{
			return GetNodes(false,true,Shapes.Count);
		}

		//Returns all the parents and children of the start shape
		public virtual Elements Relatives()
		{
			return GetNodes(true,true,1);
		}
		
		//Returns all the parents and children for the specified generations 
		public virtual Elements Relatives(int generations)
		{
			return GetNodes(true,true,generations);
		}

		//Returns all the parents and children of the start shape
		public virtual Elements AllRelatives()
		{
			return GetNodes(true,true,Shapes.Count);
		}

		//Returns all inwards links for the starting shape (start is line end)
		public virtual Elements InwardLinks()
		{
			return GetLinks(true,false);	
		}

		//Returns all inwards links for the starting shape (shape is line start)
		public virtual Elements OutwardLinks()
		{	
			return GetLinks(false,true);	
		}

		//Returns inward and outward links for this shape
		public virtual Elements Links()
		{
			return GetLinks(true,true);	
		}

		//Resets the internal search tree
		public virtual void Reset()
		{
			mNodes = null;
		}

		#endregion

		#region Events

		//Check if shapes or line collection has changed, then rebuild search tree
		private void Elements_InsertElement(object sender, ElementsEventArgs e)
		{
			if (sender is Line)
			{
				Line line = (Line) e.Value;

				line.Start.DockChanged +=new EventHandler(Origin_DockChanged);
				line.End.DockChanged +=new EventHandler(Origin_DockChanged);
				Reset();
			}
		}

		private void Elements_RemoveElement(object sender, ElementsEventArgs e)
		{
			Reset();
		}

		private void Origin_DockChanged(object sender, EventArgs e)
		{
			Reset();
		}

		#endregion

		#region Implementation

		private void BuildSearchTree()
		{
			mNodes = new Hashtable();
			Shape start;
			Shape end;

			//Loop through each shape and build a node of parent and child shapes
			foreach(Shape shape in Shapes.Values)
			{
				Node node = new Node();
				node.Shape = shape;
				node.Parents = new Elements();
				node.Children = new Elements();

				//Loop through each line and check the start and end shapes
				foreach (Line line in Lines.Values)
				{
					//Set the start and end shape
					if (line.Start.Docked && line.End.Docked)
					{
						if (line.Start.DockedElement == shape) node.Children.Add(line.End.DockedElement.Key,line.End.DockedElement);
						if (line.End.DockedElement == shape) node.Parents.Add(line.Start.DockedElement.Key,line.Start.DockedElement);
					}
				}
				mNodes.Add(shape.Key,node);
			}
		}

		//Loop through the search tree and get all parents and or children to a specified depth
		private Elements GetNodes(bool parents,bool children,int depth)
		{
			if (mNodes == null) BuildSearchTree();
			Elements elements = new Elements();
			GetChildNodes(elements,(Node) mNodes[Start.Key],parents,children,depth,0);		
			return elements;
		}

		//Gets all the immediate nodes for the current node
		private void GetChildNodes(Elements nodes, Node currentNode, bool parents, bool children, int maxDepth, int currentDepth)
		{
			currentDepth += 1;
			
			if (parents) AddRange(nodes,currentNode.Parents);
			if (children) AddRange(nodes,currentNode.Children);
			
			if (currentDepth < maxDepth)
			{
				foreach (Element element in currentNode.Parents.Values)
				{
					GetChildNodes(nodes,(Node) mNodes[element.Key],parents,children,maxDepth,currentDepth);
				}
				foreach (Element element in currentNode.Children.Values)
				{
					GetChildNodes(nodes,(Node) mNodes[element.Key],parents,children,maxDepth,currentDepth);
				}
			}
		}
		
		private Elements GetLinks(bool inward,bool outward)
		{
			Elements elements = new Elements();

			foreach (Line line in Lines.Values)
			{
				//Set the start and end shape
				if (inward && line.End.DockedElement == Start && !elements.Contains(line.Key)) elements.Add(line.Key,line); 
				if (outward && line.Start.DockedElement == Start && !elements.Contains(line.Key)) elements.Add(line.Key,line); 
			}

			return elements;
		}

		//Adds a renge of elements, skipping duplicates
		private void AddRange(Elements nodes, Elements add)
		{
			foreach (Element element in add.Values)
			{
				if (!nodes.Contains(element.Key)) nodes.Add(element.Key,element);
			}
		}

		#endregion


	}
}
