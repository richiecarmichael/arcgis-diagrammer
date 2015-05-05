using System;
using System.Collections;
using System.Drawing;

namespace Crainiate.Diagramming
{
	internal class RenderListComparer : IComparer
	{
		public int Compare(object x, object y)
		{
			Element ey = (Element) y;
			Element ex = (Element) x;
			
			if (x is ISelectable && y is ISelectable)
			{
				ISelectable sy = (ISelectable) y;
				ISelectable sx = (ISelectable) x;
			
				if (ey.ZOrder == ex.ZOrder) 
				{
					if (!sx.Selected && !sy.Selected) return 1;
					if (sx.Selected && !sy.Selected) return 1;
					if (sy.Selected && !sx.Selected) return -1;
					if (sx.Selected && sx.Selected) return 1;
				}
			}
			return (ey.mZOrder - ex.mZOrder);
		}
	}

	internal class PointFComparer : IComparer 
	{
		private bool mHorizonal;
		private bool mAscending;

		public PointFComparer()
		{
			mHorizonal = true;
			mAscending = true;
		}

		public PointFComparer(bool horizontal, bool ascending)
		{
			mHorizonal = horizontal;
			mAscending = ascending;
		}

		public bool Horizontal
		{
			get
			{
				return mHorizonal;
			}
			set
			{
				mHorizonal = value;
			}
		}

		public bool Ascending
		{
			get
			{
				return mAscending;
			}
			set
			{
				mAscending = value;
			}
		}

		public int Compare(object x, object y)
		{
			PointF a = (PointF) x;
			PointF b = (PointF) y;

			if (mAscending)
			{
				if (mHorizonal)
				{
					return Convert.ToInt32(a.X - b.X);
				}
				else
				{
					return Convert.ToInt32(a.Y - b.Y);
				}
			}
			else
			{
				if (mHorizonal)
				{
					return Convert.ToInt32(b.X - a.X);
				}
				else
				{
					return Convert.ToInt32(b.Y - a.Y);
				}
			}
		}
	}
}
