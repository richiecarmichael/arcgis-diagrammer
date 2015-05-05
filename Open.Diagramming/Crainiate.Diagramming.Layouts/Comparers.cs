using System;
using System.Collections;

namespace Crainiate.Diagramming.Layouts
{
	internal class GridComparerY: IComparer
	{
		public int Compare(object x, object y)
		{
			ArrayList list = (ArrayList) x;
			int yy = ((GridNode) y).Y; //Compare y which is a node, to the first node in the list (x)
			int xy = ((GridNode) list[0]).Y;

			return  xy - yy;
		}
	}

	internal class GridComparerX: IComparer
	{
		public int Compare(object x, object y)
		{
			return ((GridNode) x).X - ((GridNode) y).X;
		}
	}
}
