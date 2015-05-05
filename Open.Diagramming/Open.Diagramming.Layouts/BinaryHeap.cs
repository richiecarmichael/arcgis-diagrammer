using System;
using System.Collections;

namespace Crainiate.Diagramming.Layouts
{
	internal class BinaryHeap: ICollection, IEnumerable
	{
		private ArrayList InnerList = new ArrayList();
		private IComparer Comparer;

		public BinaryHeap()
		{
			Comparer = System.Collections.Comparer.Default;
		}

		public BinaryHeap(IComparer c)
		{
			Comparer = c;
		}

		protected void SwitchElements(int i, int j)
		{
			object h = InnerList[i];
			InnerList[i] = InnerList[j];
			InnerList[j] = h;
		}

		protected virtual int OnCompare(int i, int j)
		{
			return Comparer.Compare(InnerList[i],InnerList[j]);
		}

		// Push an object onto the PQ
		// Returns the index in the list where the object is _now_. This will change when objects are taken from or put onto the PQ.
		public int Push(GridNode O)
		{
			int p = InnerList.Count, p2;

			InnerList.Add(O); // E[p] = O
		
			while (true)
			{
				if (p==0) break;
				p2 = (p-1) / 2;
			
				if (OnCompare(p, p2) < 0)
				{
					SwitchElements(p, p2);
					p = p2;
				}
				else
				{
					break;
				}
			}

			return p;
		}

		// Get the smallest object and remove it.
		public GridNode Pop()
		{
			GridNode result = (GridNode) InnerList[0];
			int p = 0, p1, p2, pn;

			InnerList[0] = InnerList[InnerList.Count-1];
			InnerList.RemoveAt(InnerList.Count-1);
		
			while (true)
			{
				pn = p;
				p1 = 2*p + 1;
				p2 = 2*p + 2;
				if (InnerList.Count > p1 && OnCompare(p,p1) > 0) p = p1; 
				if (InnerList.Count > p2 && OnCompare(p,p2) > 0) p = p2;
			
				if (p == pn) break;
				SwitchElements(p, pn);
			}
			return result;
		}

		// Get the smallest object without removing it.
		public GridNode Peek()
		{
			if (InnerList.Count > 0) return (GridNode) InnerList[0];
			return null;
		}

		public void Update(GridNode node)
		{
			//Get the index of the node into i
			int i = InnerList.BinarySearch(node);
			if (i < 0) return;

			int p = i,pn;
			int p1, p2;
			
			while (true)
			{
				if(p==0) break;
				p2 = (p-1)/2;
				
				if(OnCompare(p,p2) < 0)
				{
					SwitchElements(p,p2);
					p = p2;
				}
				else
				{
					break;
				}
			}

			if (p < i) return;
			
			while (true)
			{
				pn = p;
				p1 = 2*p+1;
				p2 = 2*p+2;
				if (InnerList.Count > p1 && OnCompare(p,p1) > 0) p = p1;
				if (InnerList.Count > p2 && OnCompare(p,p2) > 0) p = p2;
				
				if(p==pn) break;
				SwitchElements(p,pn);
			}
		}


		public bool Contains(object value)
		{
			return InnerList.Contains(value);
		}

		public void Clear()
		{
			InnerList.Clear();
		}

		public int Count
		{
			get
			{
				return InnerList.Count;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return InnerList.GetEnumerator();
		}

		public void CopyTo(Array array, int index)
		{
			InnerList.CopyTo(array,index);
		}

		public bool IsSynchronized
		{
			get
			{
				return InnerList.IsSynchronized;
			}
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}
	}
}


