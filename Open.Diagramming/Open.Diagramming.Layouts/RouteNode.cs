using System;
using System.Drawing;
using System.Collections;

namespace Crainiate.Diagramming.Layouts
{
	internal class RouteNode: GridNode, IComparable
	{
		public RouteNode Parent; //Parent of this node
		public int MovementCost; //G cost
		public int Heuristic; //H cost = best guess distance to move to goal (Heuristic). Must not be greater than possible
		public int TotalCost; //F cost = G Cost + H (heuristic) cost
		public bool Closed;
		public int Generation;

		public RouteNode() : base()
		{

		}

		public RouteNode(int x, int y) : base(x,y)
		{

		}

		public bool Equals(RouteNode node)
		{
			return (node.X == X && node.Y == Y);
		}

		public bool Near(RouteNode node, int grain)
		{
			return (Math.Abs(node.X - X) < grain && Math.Abs(node.Y - Y) < grain);
		}

		public int CompareTo(object obj)
		{
			return TotalCost.CompareTo(((RouteNode) obj).TotalCost);
		}

		public override string ToString()
		{
			return X.ToString() + "," + Y.ToString() + " (" + TotalCost.ToString() + ")";
		}
	}
}
