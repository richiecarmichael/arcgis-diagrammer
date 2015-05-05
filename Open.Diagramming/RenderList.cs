using System;
using System.Collections;
using System.Drawing;

namespace Crainiate.Diagramming
{
	public class RenderList : CollectionBase, ICloneable	
	{
		//Property variables
		private IComparer mComparer;
		
		#region Interface

		//Constructors
		public RenderList()
		{
			mComparer = new RenderListComparer();
		}

		//Collection indexer
		public virtual Element this[int index]  
		{
			get  
			{
				return( (Element) List[index]);
			}
			set  
			{
				List[index] = value;
			}
		}

		public virtual Element this[string key]  
		{
			get  
			{
				Element element;
				for (int i = 0; i < List.Count; i++)
				{
					element = (Element) List[i];
					if (element.Key == key) return element;
				}
				return null;
			}
		}

		//Adds an elemnt to the list 
		public virtual int Add(Element value)  
		{
			return List.Add(value);
		}

		//Add a range of items to the list
		public virtual void AddRange(IList list)
		{
			Element element;
			for (int i = 0; i < list.Count; i++)
			{
				object obj = list[i];
				if (obj is Element)
				{
					element = (Element) obj;
					List.Add(element);
				}
				else
				{
					throw new RenderListException("List contains an object which is not of type Element");
				}
			}
		}

		//Returns the index of an element
		public virtual int IndexOf(Element value)  
		{
			return List.IndexOf(value);
		}

		//Inserts an elelemnt into the list
		public virtual void Insert(int index, Element value)  
		{
			List.Insert(index, value);
		}

		//Removes an element from the list
		public virtual void Remove(Element value )  
		{
			List.Remove(value);
		}

		//Returns true if list contains element
		public virtual bool Contains(Element value)  
		{
			return List.Contains(value);
		}

		//Returns true if the list contains an element with a matching key
		public virtual bool ContainsKey(string key)
		{
			Element element;
			for (int i = 0; i < List.Count; i++)
			{
				element = (Element) List[i];
				if (element.Key == key) return true;
			}
			return false;
		}

		//Sets he comparer used to sort elements contained in the collection
		public virtual IComparer Comparer
		{
			get
			{
				return mComparer;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("Comparer","Comparer may not be null.");
				mComparer = value;
			}
		}

		//Sorts the renderlist using the Comparer property
		public virtual void Sort()
		{	
			if (base.Count > 1)
			{
				base.InnerList.Sort(mComparer);
			}
		}

		//Sorts the renderlist using the Comparer property
		public virtual void Sort(IComparer comparer)
		{	
			if (comparer == null) throw new ArgumentNullException("comparer","Comparer may not be null.");

			if (base.Count > 1)
			{
				base.InnerList.Sort(comparer);
			}
		}

		//Implements IClonable
		public object Clone()
		{
			RenderList copy = new RenderList();

			foreach (Element element in this)
			{
				copy.Add(element);
			}
			return copy;
		}

		//Returns the rectangle bounding the elements in the renderlist
		public RectangleF GetBounds()
		{
			float minx = 0;
			float miny = 0;
			float maxx = 0;
			float maxy = 0;

			bool flag = true;

			foreach (Element element in this)
			{
				if (element.Visible)
				{
					RectangleF bounds = element.Rectangle;

					if (flag)
					{
						flag = false;
						minx = bounds.Left;
						miny = bounds.Top;
						maxx = bounds.Right;
						maxy = bounds.Bottom;
					}
					else
					{
						if (bounds.Left < minx) minx = bounds.Left;
						if (bounds.Top < miny) miny = bounds.Top;
						if (bounds.Right > maxx) maxx = bounds.Right;
						if (bounds.Bottom > maxy) maxy = bounds.Bottom;
					}
				}
			}

			return new RectangleF(minx, miny, maxx - minx, maxy - miny);
		}
	
		#endregion
	}
}