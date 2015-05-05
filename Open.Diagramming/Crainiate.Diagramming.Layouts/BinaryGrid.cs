using System;
using System.Collections;

namespace Crainiate.Diagramming.Layouts
{
	//Creates a 2 dimensional arraylist to hold coordinate values
	//Contains a list of ArrayList items, each arraylist is a set of x co ordinates, along a y coordinate
	internal class BinaryGrid
	{
		ArrayList mYList; 
		GridComparerY mComparerY;
		GridComparerX mComparerX;

		public BinaryGrid()
		{
			mYList = new ArrayList();
			mComparerX = new GridComparerX();
			mComparerY = new GridComparerY();
		}

		public IEnumerator GetEnumerator()
		{
			return mYList.GetEnumerator();
		}

		public GridNode Item(int x, int y)
		{
			GridNode node = new GridNode(x, y);

			//Get the index on the y axis
			int index = mYList.BinarySearch(node, mComparerY);
			if (index < 0) return null;

			//Get the x arraylist
			ArrayList xList = (ArrayList) mYList[index];

			index = xList.BinarySearch(node, mComparerX);
			if (index < 0) return null;

			return (GridNode) xList[index];
		}

		public virtual void Add(GridNode node)
		{
			//Get the index on the y axis
			int index = mYList.BinarySearch(node, mComparerY);
			
			//If not found, add a new x list with a new item
			if (index < 0)
			{
				ArrayList xList = new ArrayList();
				xList.Add(node);
				mYList.Insert(~index, xList); //Insert the node at the correct index which is the bitwise complement
				return;
			}

			//Get the x arraylist
			ArrayList x = (ArrayList) mYList[index];

			index = x.BinarySearch(node, mComparerX);
			if (index < 0) 
			{
				x.Insert(~index, node); //Insert the node at the correct index
			}
		}

		public bool IsEmpty
		{
			get
			{
				return (mYList == null || mYList.Count == 0);
			}
		}

		public int Count()
		{
			int count = 0;
			
			foreach(ArrayList list in mYList)
			{
				count += list.Count;	
			}

			return count;
		}
	}
}
