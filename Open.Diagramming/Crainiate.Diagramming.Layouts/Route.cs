using System;
using System.Drawing;
using System.Collections;
using Crainiate.Diagramming.Drawing2D;

namespace Crainiate.Diagramming.Layouts
{
	public sealed class Route
	{
		//Property variables
		private int mGrain;
		private int mModifier; //D value used to amplify movement costs together with the heuristic
		private int mTerrainCost; //Terrain cost modifier, higher value = more likely to avoid
		private IContainer mContainer;
		private bool mAvoid;
		private Size mPadding;
		private Layer mLayer;
		private Shape mStart;
		private Shape mEnd;

		//Working variables
		private BinaryHeap mOpenList; //An open list of priority items
		private BinaryGrid mGrid; //A grid containing all items visited
		private BinaryGrid mTerrain; //A grid containing all items visited

		private Rectangle mBoundary;
		private RouteNode mSolution;
		private RouteNode mGoal;

		private int mGeneration; //Counts the number of nodes created

		#region Interface

		//Constructors
		public Route()
		{
			Reset();
			mGrain = 10;
			mModifier = 10; 
			mTerrainCost = 30;
		}

		//Properties
		public IContainer Container
		{
			get
			{
				return mContainer;
			}
			set
			{
				mContainer = value;
			}
		}

		public Layer Layer
		{
			get
			{
				return mLayer;
			}
			set
			{
				if (value != mLayer)
				{
					Reform();
					mLayer = value;
				}
			}
		}

		public bool Avoid
		{
			get
			{
				return mAvoid;
			}
			set
			{
				mAvoid = value;
			}
		}

		public Rectangle Boundary
		{
			get
			{
				return mBoundary;
			}
			set
			{
				mBoundary = value;
			}
		}

		public int TerrainCost
		{
			get
			{
				return mTerrainCost;
			}
			set
			{
				mTerrainCost = value;
			}
		}

		public Size Padding
		{
			get
			{
				return mPadding;
			}
			set
			{
				if (!mPadding.Equals(value))
				{
					Reform();
					mPadding = value;
				}
			}
		}

		public Shape Start
		{
			get
			{
				return mStart;
			}
			set
			{
				if (mStart != value)
				{
					Reform();
					mStart = value;
				}
			}
		}

		public Shape End
		{
			get
			{
				return mEnd;
			}
			set
			{
				if (mEnd != value)
				{
					Reform();
					mEnd = value;
				}
			}
		}

		//Methods
		public void Reset()
		{
			mOpenList = new BinaryHeap();
			mGrid = new BinaryGrid();
			mGeneration = 1;
			mSolution = null;
			mGoal = null;
		}

		//Determines if the impassable structures must be recreated
		public void Reform()
		{
			mTerrain = null;
		}

		public ArrayList GetRoute(PointF begin, PointF end)
		{
			//Move the begin and end onto the grid
			Point beginAligned = Point.Round(begin);
			Point endAligned = Point.Round(end);

			beginAligned = new Point((beginAligned.X / mGrain) * mGrain, (beginAligned.Y / mGrain) * mGrain);
			endAligned = new Point((endAligned.X / mGrain) * mGrain, (endAligned.Y / mGrain) * mGrain);

			//Recreate the rectangles surrounding the shapes in the container
			if (mTerrain == null) CreateTerrain(beginAligned, endAligned);	

			RouteNode start = new RouteNode(beginAligned.X, beginAligned.Y);
			RouteNode goal = new RouteNode(endAligned.X, endAligned.Y);

			//Check for exclusions
			if (Start != null && End != null)
			{
				Rectangle startRect = RectangleToGrid(GetInflatedRectangle(Start));
				Rectangle endRect = RectangleToGrid(GetInflatedRectangle(End));

				startRect.Inflate(mGrain, mGrain);
				endRect.Inflate(mGrain, mGrain);

				//If rectangles intersect or are adjacent then just make connector
				if (startRect.IntersectsWith(endRect) || Geometry.AreAdjacent(startRect, endRect))
				{
					return MakeConnector(begin, end);
				}
			}

			//Check to see if a route must be calculated, or just a connector shape created
			if (mTerrain != null && !mTerrain.IsEmpty) 
			{
				if (CalculateNodes(start, goal)) 
				{
					ArrayList solution = GetSolution();
					if (solution != null && solution.Count > 1) 
					{
						AlignSolution(solution, begin, end);
						return solution;
					}
				}
			}

			return MakeConnector(begin, end);
		}

		#endregion

		#region Implementation

		//Creates the solution from the calculated nodes as vectors
		public ArrayList GetSolution()
		{
			///Add an additional node to the end if the solution didnt match the goal
			if (mSolution.Equals(mGoal) || mSolution.Parent == null)
			{
				mGoal = mSolution;
			}
			else
			{
				//If in line then add the goal as a node to the solution, else move the solution node
				if (mSolution.X == mGoal.X || mSolution.Y == mGoal.Y)
				{
					mGoal.Parent = mSolution;
				}
				else
				{
					RouteNode extra = new RouteNode();
					extra.Parent = mSolution; 

					//Set node coordinates
					extra.X = (mSolution.X == mSolution.Parent.X) ? mSolution.X : mGoal.X;
					extra.Y = (mSolution.Y == mSolution.Parent.Y) ? mSolution.Y : mGoal.Y;
					
					//Link the goal to the new node and add it
					mGoal.Parent = extra;
				}
			}

			ArrayList list = new ArrayList();
			
			RouteNode previous = mGoal;
			RouteNode node = mGoal.Parent;
			
			list.Add(new PointF(previous.X, previous.Y));

			//Only one or two items
			if (node == null || node.Parent == null) return list;

			while (node.Parent != null)
			{
				if (!((previous.X == node.Parent.X) || (previous.Y == node.Parent.Y)))
				{
					list.Insert(0, new PointF(node.X, node.Y));
					previous = node;
				}
				node = node.Parent;
			}

			//Add the start node
			PointF start = new PointF(node.X, node.Y);
			if (!start.Equals((PointF) list[0])) list.Insert(0, start);
			
			return list;
		}

		private bool CalculateNodes(RouteNode start, RouteNode goal)
		{
			mGoal = goal;

			//Set up the movement costs and heuristic (manhattan distance)
			start.MovementCost = 0;
			start.Heuristic = GetHeuristic(start);
			start.TotalCost = start.Heuristic; //Since movement cost is 0

			//Add the start node to the open list
			mOpenList.Push(start);

			int movementcost = mGrain * mModifier;

			//Keep looping until goal is found or there are no more open nodes
			while (mOpenList.Count > 0)
			{
				//Remove from open list
				RouteNode parent = GetNextNode();	// pops node off open list based on cost and generation			
				
				//Check to see if we have found the goal node
				//if (parent.Near(goal, mGrain)) 
				if (parent.Equals(goal)) 
				{
					mSolution = parent;
					return true;
				}

				//Close the node
				parent.Closed = true;
				
				//Add or check four adjacent squares to the open list
				AddAdjacentNode(mGrain, 0, movementcost, parent);
				AddAdjacentNode(0, mGrain, movementcost, parent);
				AddAdjacentNode(0, -mGrain, movementcost, parent);
				AddAdjacentNode(-mGrain, 0, movementcost, parent);
			}

			return false;
		}


		//Pops the most recent lowest f cost node off the heap
		private RouteNode GetNextNode()
		{
			RouteNode pop = (RouteNode) mOpenList.Pop();
			if (mOpenList.Count < 2) return pop;

			RouteNode peek = (RouteNode) mOpenList.Peek();

			return pop;
		}

		//Add a node to a parent node, ignoring if impassable or already closed,
		//adding if new, updating if new and better route
		private RouteNode AddAdjacentNode(int dx, int dy, int cost, RouteNode parent)
		{
			int x = parent.X + dx;
			int y = parent.Y + dy;
			int newCost = parent.MovementCost + cost;
			
			//Get terrain cost. -1 is not passable, 0 no cost, 1,2,3 etc higher cost
			int terraincost = GetTerrainCost(x, y);
			if (terraincost == -1) return null;			
			
			//Check if item has been added to the grid already
			RouteNode existing = (RouteNode) mGrid.Item(x, y);
			if (existing != null) 
			{
				if (!existing.Closed && newCost < existing.MovementCost)
				{
					existing.MovementCost = newCost;
					existing.TotalCost = existing.MovementCost + existing.Heuristic;
					existing.Parent = parent;

					mOpenList.Update(existing);
				}
				return existing;
			}

			//Create a new node
			RouteNode node = new RouteNode();
			node.Parent = parent;

			node.X = x;
			node.Y = y;

			node.MovementCost = newCost;	//Add the cost to the parent cost
			node.Heuristic = GetHeuristic(node);
			node.TotalCost = node.MovementCost + node.Heuristic;

			//Add terrain cost
			node.MovementCost += terraincost;
			node.TotalCost += terraincost;

			//Set generation
			node.Generation = mGeneration;
			mGeneration ++;

			//Add to the open list and the grid
			mOpenList.Push(node);
			mGrid.Add(node);

			return node;
		}

		//Calculates the manhattan distance as the best guess distance to move to goal. 
		//Must not be greater than possible distance
		//We use a unit of 10 for each 1 unit of movement
		private int GetHeuristic(RouteNode current)
		{
			return mModifier * (Math.Abs(mGoal.X - current.X) + Math.Abs(mGoal.Y - current.Y));
		}

		//Check that a node can be created here
		private int GetTerrainCost(int x, int y)
		{
			//Check the boundary
			if (!mBoundary.Contains(x,y)) return -1;

			x = Convert.ToInt32(x / mGrain) * mGrain;
			y = Convert.ToInt32(y / mGrain) * mGrain;

			TerrainNode node = (TerrainNode) mTerrain.Item(x,y);
			if (node == null) return 0;

			return node.MovementCost;
		}

		//Loop through each rectangle and create a terrain grid
		//Make sure grid has been cleared
		private void CreateTerrain(Point startPoint, Point endPoint)
		{
			if (mContainer == null) return;

			mTerrain = new BinaryGrid();

			int terraincost = mGrain * mModifier * mTerrainCost;

			//Add all shapes if avoid is on, otherwise only the start and end
			if (Avoid)
			{
				foreach (Shape shape in mContainer.Shapes.Values)
				{
					//Reduce the terrain rectangle if rectangle intersection 
					//and starting or ending shapes
					if (shape == Start || shape == End)
					{
						Rectangle rect = GetInflatedRectangle(shape);
						AddRectangleToTerrain(rect, terraincost);
					}
					else
					{
						Rectangle rect = GetInflatedRectangle(shape);
						if (!rect.Contains(startPoint) && !rect.Contains(endPoint)) AddRectangleToTerrain(rect, terraincost);
					}
				}
			}
			else
			{
				if (Start != null) AddRectangleToTerrain(GetInflatedRectangle(Start), terraincost);
				if (End != null) AddRectangleToTerrain(GetInflatedRectangle(End), terraincost);
			}
		}

		private Rectangle GetInflatedRectangle(Shape shape)
		{
			Rectangle rect = Rectangle.Round(shape.Rectangle);
			if (shape.UpdateElement != null) rect = Rectangle.Round(shape.UpdateElement.Rectangle);
			rect.Inflate(mPadding.Width / 2, mPadding.Height / 2);
			return rect;
		}

		private void AddRectangleToTerrain(Rectangle rect, int terraincost)
		{
			//Round down rect
			int x1 = Convert.ToInt32(rect.X / mGrain) * mGrain;
			int y1 = Convert.ToInt32(rect.Y / mGrain) * mGrain;
			int x2 = Convert.ToInt32(rect.Right / mGrain) * mGrain;
			int y2 = Convert.ToInt32(rect.Bottom / mGrain) * mGrain;
				
			//Add the top and bottom, if already exists then is ignored
			for (int i = x1; i <= x2; i+= mGrain) 
			{
				mTerrain.Add(new TerrainNode(i, y1, terraincost));
				mTerrain.Add(new TerrainNode(i, y2, terraincost));
			}

			//Add the left and right, if already exists then is ignored
			for (int i = y1; i <= y2; i+= mGrain) 
			{
				mTerrain.Add(new TerrainNode(x1, i, terraincost));
				mTerrain.Add(new TerrainNode(x2, i, terraincost));
			}
		}

		private ArrayList MakeConnector(PointF start,PointF end)
		{
			ArrayList output = new ArrayList();

			//Create start and end points
			output.Add(start);
			output.Add(end);				

			//Check if horizontal or vertical
			if (start.X == end.X || start.Y == end.Y) return output;

			//Insert two mid points
			output.Insert(1,new PointF()); //new point 1
			output.Insert(2,new PointF()); //new point 2

			bool horizontal = false;

			//Work out if vertical or horizontal
			horizontal = end.X - start.X > end.Y - start.Y;

			//Get mid point
			PointF mid = new PointF((start.X + end.X) / 2, (start.Y + end.Y) / 2);
			PointF new1 = new PointF();
			PointF new2 = new PointF();

			//Create 2 new mid points
			if (horizontal)
			{
				new1.X = mid.X;
				new2.X = mid.X;
				new1.Y = start.Y;
				new2.Y = end.Y;
			}
			else
			{
				new1.X = start.X;
				new2.X = end.X;
				new1.Y = mid.Y;
				new2.Y = mid.Y;
			}
			output[1] = new1;
			output[2] = new2;

			return output;
		}

		//Apply smoothing to the calculated route by checking the bends and the terrain
		//Replaces a step by moving the 3 point in 5
		public void SmoothRoute(ArrayList list) 
		{
			if (list.Count < 5) return;

			int index = 0;

			while ((index + 4) < list.Count)
			{
				PointF p0 = (PointF) list[index];
				PointF p1 = (PointF) list[index + 1];
				PointF p2 = (PointF) list[index + 2];
				PointF p3 = (PointF) list[index + 3];
				PointF p4 = (PointF) list[index + 4];

				//Steps that begin up or down
				if ((p1.Y > p0.Y && p3.Y > p2.Y) || (p1.Y < p0.Y && p3.Y < p2.Y))
				{
					if ((p2.X > p1.X && p4.X > p3.X) || (p2.X < p1.X && p4.X < p3.X))
					{
						PointF newPoint = new PointF(p1.X, p3.Y);
						
						//Check terrain for any points between (can be increasing or decreasing)
						if (CheckTerrain(Point.Round(p1), Point.Round(newPoint)) && CheckTerrain(Point.Round(newPoint), Point.Round(p3)))
						{
							list[index+2] = newPoint;
							list.RemoveAt(index+1);
							list.RemoveAt(index+2); //The old 3
							index = 0;
							continue;
						}
					}
				}
				//Steps that begin to the right or left
				else if ((p1.X > p0.X && p3.X > p2.X) || (p1.X < p0.X && p3.X < p2.X))
				{
					if ((p2.Y > p1.Y && p4.Y > p3.Y) || (p2.Y < p1.Y && p4.Y < p3.Y))
					{
						PointF newPoint = new PointF(p3.X, p1.Y);
						
						//Check terrain for any points between (can be increasing or decreasing)
						if (CheckTerrain(Point.Round(p1), Point.Round(newPoint)) && CheckTerrain(Point.Round(newPoint), Point.Round(p3)))
						{
							list[index+2] = newPoint;
							list.RemoveAt(index+1);
							list.RemoveAt(index+2); //The old 3
							index = 0;
							continue;
						}
					}
				}
				
				index++;
			}
		}

		//Check the terrain between the two points
		private bool CheckTerrain(Point a, Point b)
		{
			if (mTerrain == null) return true;


			int start;
			int end;

			if (a.X == b.X)
			{
				if (a.Y < b.Y)
				{
					start = a.Y;
					end = b.Y;
				}
				else
				{
					start = b.Y;
					end = a.Y;
				}
			}
			else
			{
				if (a.X < b.X)
				{
					start = a.X;
					end = b.X;
				}
				else
				{
					start = b.X;
					end = a.X;
				}
			}

			//Round the start and end onto the grid
			start = Convert.ToInt32(start / mGrain) * mGrain;
			end = Convert.ToInt32(end / mGrain) * mGrain;
			
			//Round the other coordinate of the pair
			int div = (a.X == b.X) ? a.X : a.Y;
			div = Convert.ToInt32(div / mGrain) * mGrain;

			//Get the starting movement cost
			TerrainNode node = (TerrainNode) mTerrain.Item(div, start);
			int cost = (node == null) ? 0 : node.MovementCost;

			//Loop through the terrain checking for a node with movement cost
			for(int i = start; i < end; i+= mGrain)
			{
				node = null;
				
				if (a.X == b.X)
				{
					node = (TerrainNode) mTerrain.Item(div, i);
				}
				else
				{
					node = (TerrainNode) mTerrain.Item(i, div);
				}
				  
				if (node != null && node.MovementCost != cost)
				{
					return false;
				}
				
			}

			return true;
		}

		//move the solution off the grid and back in line with the start and end
		private void AlignSolution(ArrayList solution, PointF begin, PointF end)
		{
			//Only process if 2 or more points
			if (solution.Count < 2) return;

			int upper = solution.Count - 1;

			PointF a = (PointF) solution[0];
			PointF b = (PointF) solution[1];

			solution[0] = begin;
			if (a.X == b.X)
			{
				solution[1] = new PointF(begin.X, b.Y);
			}
			else
			{
				solution[1] = new PointF(b.X, begin.Y);
			}

			//Return if less than 3 points
			if (solution.Count == 2) return;

			a = (PointF) solution[upper];
			b = (PointF) solution[upper - 1];

			solution[upper] = end;
			if (a.X == b.X)
			{
				solution[upper-1] = new PointF(end.X, b.Y);
			}
			else
			{
				solution[upper-1] = new PointF(b.X, end.Y);
			}
		}

		private Rectangle RectangleToGrid(RectangleF rectangle)
		{
			//Round down rect
			int x1 = Convert.ToInt32(rectangle.X / mGrain) * mGrain;
			int y1 = Convert.ToInt32(rectangle.Y / mGrain) * mGrain;
			int x2 = Convert.ToInt32(rectangle.Right / mGrain) * mGrain;
			int y2 = Convert.ToInt32(rectangle.Bottom / mGrain) * mGrain;

			return new Rectangle(x1,y1,Math.Abs(x2 - x1), Math.Abs(y2 - y1));
		}


		#endregion

	}
}
