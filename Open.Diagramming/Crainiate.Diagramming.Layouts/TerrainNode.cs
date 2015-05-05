using System;
using System.Drawing;
using System.Collections;

namespace Crainiate.Diagramming.Layouts
{
	internal class TerrainNode: GridNode
	{
		public int MovementCost;

		public TerrainNode() : base()
		{
			MovementCost = -1;
		}

		public TerrainNode(int x, int y) : base(x,y)
		{
			MovementCost = -1;
		}

		public TerrainNode(int x, int y, int movementCost) : base(x,y)
		{
			MovementCost = movementCost;
		}

		public bool IsPassable
		{
			get
			{
				return (MovementCost > -1);
			}
		}			

		public override string ToString()
		{
			return X.ToString() + "," + Y.ToString() + " (" + MovementCost.ToString() + ")";
		}
	}
}
