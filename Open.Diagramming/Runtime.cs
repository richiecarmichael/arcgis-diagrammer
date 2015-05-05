using System;
using System.Windows.Forms;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Editing;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Runtime
	{
		//Property variables
		private InteractiveMode mInteractiveMode;
		private bool mFeedback;
		private DiagramUnit mFeedbackUnit;
		private IContainer mActiveContainer;
		private IDiagram mDiagram;

		private bool mCut;
		private bool mCopy;
		private bool mPaste;
		private bool mDelete;
		private bool mUndo;
		private bool mRedo;

		private bool mRoundPixels;

		#region Interface

		//Events
		public event EventHandler CreateElement;

		//Constructors
		public Runtime()
		{
			InteractiveMode = InteractiveMode.Normal;
			Feedback = true;
			FeedbackUnit = DiagramUnit.Pixel;
			Cut = true;
			Copy = true;
			Paste = true;
			Delete = true;
			Undo = true;
			Redo = true;
			RoundPixels = true;
		}

		//Deserializes info into a new solid element
		protected Runtime(SerializationInfo info, StreamingContext context)
		{
			InteractiveMode = (InteractiveMode) Enum.Parse(typeof(InteractiveMode), info.GetString("InteractiveMode"),true);
			FeedbackUnit = (DiagramUnit) Enum.Parse(typeof(DiagramUnit),info.GetString("FeedbackUnit"),true);
			Feedback = info.GetBoolean("Feedback");
			Cut = info.GetBoolean("Cut");
			Copy = info.GetBoolean("Copy");
			Paste = info.GetBoolean("Paste");
			Delete = info.GetBoolean("Delete");
			Undo = info.GetBoolean("Undo");
			Redo = info.GetBoolean("Redo");
		}

		public virtual IDiagram Diagram
		{
			get
			{
				return mDiagram;
			}
			set
			{
				mDiagram = value;
			}
		}

		public virtual InteractiveMode InteractiveMode
		{
			get
			{
				return mInteractiveMode;
			}
			set
			{
				mInteractiveMode = value;
			}
		}

		public virtual bool Feedback
		{
			get
			{
				return mFeedback;
			}
			set
			{
				mFeedback = value;
			}
		}

		public virtual DiagramUnit FeedbackUnit
		{
			get
			{
				return mFeedbackUnit;
			}
			set
			{
				mFeedbackUnit = value;
			}
		}

		public virtual bool Cut
		{
			get
			{
				return mCut;
			}
			set
			{
				mCut = value;
			}
		}

		public virtual bool Copy
		{
			get
			{
				return mCopy;
			}
			set
			{
				mCopy = value;
			}
		}

		public virtual bool Paste
		{
			get
			{
				return mPaste;
			}
			set
			{
				mPaste = value;
			}
		}

		public virtual bool Delete
		{
			get
			{
				return mDelete;
			}
			set
			{
				mDelete = value;
			}
		}

		public virtual bool Undo
		{
			get
			{
				return mUndo;
			}
			set
			{
				mUndo = value;
			}
		}

		public virtual bool Redo
		{
			get
			{
				return mRedo;
			}
			set
			{
				mRedo = value;
			}
		}

		public virtual bool RoundPixels
		{
			get
			{
				return mRoundPixels;
			}
			set
			{
				mRoundPixels = value;
			}
		}


			public virtual IContainer ActiveContainer
		{
			get
			{
				return mActiveContainer;
			}
			set
			{
				if (value != null)
				{
					mActiveContainer = value;
				}
			}
		}

		//Methods
		public virtual Line CreateLine()
		{
			Line line = new Line();

			OnCreateElement(line);
			return line;
		}

		public virtual Line CreateLine(PointF start,PointF end)
		{
			Line line = new Line(start,end);

			OnCreateElement(line);
			return line;
		}

		public virtual Connector CreateConnector()
		{
			Connector line = new Connector();

			OnCreateElement(line);
			return line;
		}

		public virtual Connector CreateConnector(PointF start,PointF end)
		{
			Connector line = new Connector(start,end);

			OnCreateElement(line);
			return line;
		}

		public virtual ComplexLine CreateComplexLine()
		{
			ComplexLine line = new ComplexLine();

			OnCreateElement(line);
			return line;
		}


		public virtual ComplexLine CreateComplexLine(PointF start,PointF end)
		{
			ComplexLine line = new ComplexLine(start,end);

			OnCreateElement(line);
			return line;
		}

		public virtual Curve CreateCurve()
		{
			Curve curve = new Curve();

			OnCreateElement(curve);
			return curve;
		}

		public virtual Curve CreateCurve(PointF start,PointF end)
		{
			Curve curve = new Curve(start,end);

			OnCreateElement(curve);
			return curve;
		}

		public virtual Shape CreateShape()
		{
			Shape shape = new Shape();

			OnCreateElement(shape);
			return shape;
		}

		public virtual Shape CreateShape(PointF start,SizeF size)
		{
			Shape shape = new Shape();
			shape.Location = start;
			if (! size.IsEmpty) shape.Size = size;

			OnCreateElement(shape);
			return shape;
		}

		public virtual ComplexShape CreateComplexShape()
		{
			ComplexShape shape = new ComplexShape();

			OnCreateElement(shape);
			return shape;
		}

		public virtual ComplexShape CreateComplexShape(PointF start,SizeF size)
		{
			ComplexShape shape = new ComplexShape();
			shape.Location = start;
			if (! size.IsEmpty) shape.Size = size;

			OnCreateElement(shape);
			return shape;
		}

		public virtual Shape CreateTable()
		{
			Table table = new Table();

			OnCreateElement(table);
			return table;
		}

		public virtual Shape CreateTable(PointF start,SizeF size)
		{
			Table table = new Table();
			table.Location = start;
			if (!size.IsEmpty) table.Size = size;

			OnCreateElement(table);
			return table;
		}

		public virtual Group CreateGroup()
		{
			Group group = new Group();

			OnCreateElement(group);
			return group;
		}

		public virtual Port CreatePort(PortOrientation orientation)
		{
			Port port = new Port(orientation);

			OnCreateElement(port);
			return port;
		}

		public virtual ILabelEdit CreateLabelEdit()
		{
			return new LabelEdit();
		}

		//Raises the create element event
		protected virtual void OnCreateElement(object sender)
		{
			if (CreateElement != null) CreateElement(sender,new EventArgs());
		}

		//Sett he parent diagram reference
		protected internal void SetDiagram(IDiagram diagram)
		{
			mDiagram = diagram;
		}


		#endregion

		#region Implementation

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("InteractiveMode",Convert.ToInt32(InteractiveMode).ToString());
			info.AddValue("FeedbackUnit",Convert.ToInt32(FeedbackUnit).ToString());
			info.AddValue("Feedback",Feedback);
			info.AddValue("Cut",Cut);
			info.AddValue("Copy",Copy);
			info.AddValue("Paste",Paste);
			info.AddValue("Delete",Delete);
			info.AddValue("Undo",Undo);
			info.AddValue("Redo",Redo);
		}	
	
		//Determine if an element can dock
		public virtual bool CanDock(Diagram.MouseElements mouseElements)
		{
			//Check is shape or port
			if (InteractiveMode == InteractiveMode.Normal)
			{
				if (mouseElements.MouseMoveElement is Shape || mouseElements.MouseMoveElement is Port)
				{
					//return false if permission not available to dock
					if (mouseElements.MouseStartOrigin != null)
					{
						Origin origin = mouseElements.MouseStartOrigin;
						Line line = (Line) mouseElements.MouseStartElement;
				
						if (mouseElements.MouseMoveElement is Shape)
						{
							Shape shape = (Shape) mouseElements.MouseMoveElement;
							if (shape.Direction == Direction.None) return false;
							if (origin == line.Start && shape.Direction== Direction.In) return false;
							if (origin == line.End && shape.Direction == Direction.Out) return false;
						}
				
						if (mouseElements.MouseMoveElement is Port)
						{
							Port port = (Port) mouseElements.MouseMoveElement;
							if (port.Direction == Direction.None) return false;
							if (origin == line.Start && port.Direction == Direction.In) return false;
							if (origin == line.End && port.Direction == Direction.Out) return false;
						}
					}
			
					//Check that start and target elements have the same container
					if (mouseElements.MouseStartElement.Container == mouseElements.MouseMoveElement.Container)
					{
						return true;
					}

					//Check for case where line in group is docked to group port
					if (mouseElements.MouseStartElement.Container is Group && mouseElements.MouseMoveElement is Port)
					{
						Port port = (Port) mouseElements.MouseMoveElement;
						if (port.Parent == mouseElements.MouseStartElement.Container) return true;
					}
				}
			}
				//Can dock for interactive elements
			else
			{
				if (mouseElements.InteractiveElement is Line)
				{
					Line line = (Line) mouseElements.InteractiveElement;
				
					//Determine if applies to start or end origin
					if (mouseElements.InteractiveOrigin == line.Start)
					{
						if (mouseElements.MouseMoveElement is Shape)
						{
							Shape shape = (Shape) mouseElements.MouseMoveElement;
							if (shape.Direction == Direction.None) return false;
							if (shape.Direction== Direction.In) return false;
						}

						if (mouseElements.MouseMoveElement is Port)
						{
							Port port = (Port) mouseElements.MouseMoveElement;
							if (port.Direction == Direction.None) return false;
							if (port.Direction == Direction.In) return false;
						}
					}
					else
					{
						if (mouseElements.MouseMoveElement is Shape)
						{
							Shape shape = (Shape) mouseElements.MouseMoveElement;
							if (shape.Direction == Direction.None) return false;
							if (shape.Direction == Direction.Out) return false;
						}

						if (mouseElements.MouseMoveElement is Port)
						{
							Port port = (Port) mouseElements.MouseMoveElement;
							if (port.Direction == Direction.None) return false;
							if (port.Direction == Direction.Out) return false;
						}
					}

					return true;

					//Check that start and target elements have the same container
					if (line.Container == mouseElements.MouseMoveElement.Container)
					{
						return true;
					}

					//Check for case where line in group is docked to group port
					if (line.Container is Group && mouseElements.MouseMoveElement is Port)
					{
						Port port = (Port) mouseElements.MouseMoveElement;
						if (port.Parent == line.Container) return true;
					}
				}
			}
			
			return false;
		}

		//Determines if an element can be added interactively
		public virtual bool CanAdd(Element element)
		{
			return true;
		}

		public virtual bool CanDelete(Element element)
		{
			return true;
		}

		public virtual void SetFeedback(HandleType mouseHandleType)
		{
			if (Diagram == null) return;
			if (! (Diagram.Render is IRenderDesign)) return;
			if (! (Diagram is Diagram)) return;

			Diagram diagram = (Diagram) Diagram;
			IRenderDesign render = (IRenderDesign) Diagram.Render;
			
			if (render.Actions == null) return;

			foreach (Element element in render.Actions)
			{
				if (element.ActionElement == diagram.CurrentMouseElements.MouseStartElement)
				{
					System.Text.StringBuilder builder = new System.Text.StringBuilder();
					string abbrev = Units.Abbreviate(FeedbackUnit);

					if (element is Shape)
					{
						int decimals = Units.DecimalsFromUnit(FeedbackUnit);

						if (mouseHandleType == HandleType.Move)
						{
							PointF location = element.Rectangle.Location;

							//Check if must translate unit
							if (FeedbackUnit != DiagramUnit.Pixel)
							{
								location = Component.Instance.Units.ConvertPoint(location,FeedbackUnit);
							}

							//Round units
							location = new PointF(Convert.ToSingle(Math.Round(location.X,decimals)),Convert.ToSingle(Math.Round(location.Y,decimals)));

							builder.Append("x: ");
							builder.Append(location.X.ToString());
							builder.Append(" ");
							builder.Append(abbrev);
							builder.Append(" ");
							builder.Append(" y: ");
							builder.Append(location.Y.ToString());
							builder.Append(" ");
							builder.Append(abbrev);
						
							render.Feedback = builder.ToString();
						}
						else if (mouseHandleType == HandleType.Rotate)
						{
							ITransformable transform = (ITransformable) element;
							builder.Append(Convert.ToSingle(Math.Round(transform.Rotation,decimals)).ToString());
							builder.Append(" degrees");

							render.Feedback = builder.ToString();
						}
						else
						{
							SizeF size = element.Rectangle.Size;

							//Check if must translate unit
							if (FeedbackUnit != DiagramUnit.Pixel)
							{
								Graphics graphics = Component.Instance.CreateGraphics();
								size = Component.Instance.Units.ConvertSize(size,FeedbackUnit);
								graphics.Dispose();
							}

							//Round units
							size = new SizeF(Convert.ToSingle(Math.Round(size.Width,decimals)),Convert.ToSingle(Math.Round(size.Height,decimals)));

							builder.Append("w: ");
							builder.Append(size.Width.ToString());
							builder.Append(" ");
							builder.Append(abbrev);
							builder.Append(" ");
							builder.Append(" h: ");
							builder.Append(size.Height.ToString());
							builder.Append(" ");
							builder.Append(abbrev);

							render.Feedback = builder.ToString();
						}
					}
				}
			}
		}


		#endregion
	}
}
