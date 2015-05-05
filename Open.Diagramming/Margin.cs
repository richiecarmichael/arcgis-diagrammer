using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Margin: ISerializable, ICloneable
	{
		//Property variables
		private float mTop;
		private float mLeft;
		private float mBottom;
		private float mRight;

		#region Interface

		//Constructors
		public Margin()
		{
		}

		public Margin(float left, float top,float right, float bottom)
		{
			mTop = top;
			mLeft = left;
			mBottom = bottom;
			mRight = right;
		}

		//Creates a new element from the supplied XML.
		protected Margin(SerializationInfo info, StreamingContext context)
		{
			Left = info.GetSingle("Left");
			Top =  info.GetSingle("Top");
			Right =  info.GetSingle("Right");
			Bottom =  info.GetSingle("Bottom");
		}

		//Properties
		//Top
		public virtual float Top
		{
			get
			{
				return mTop;
			}
			set
			{
				mTop = value;
			}
		}

		//Left
		public virtual float Left
		{
			get
			{
				return mLeft;
			}
			set
			{
				mLeft = value;
			}
		}

		//Bottom
		public virtual float Bottom
		{
			get
			{
				return mBottom;
			}
			set
			{
				mBottom = value;
			}
		}

		//Right
		public virtual float Right
		{
			get
			{
				return mRight;
			}
			set
			{
				mRight = value;
			}
		}

		public virtual bool IsEmpty
		{
			get
			{
				return (mTop == 0 && mLeft == 0 && mRight ==0 && mBottom == 0);
			}
		}

		public override string ToString()
		{
			return "{Left="+Left.ToString()+",Top="+Top.ToString()+",Right="+Right.ToString()+",Bottom="+Bottom.ToString()+"}";
		}

		//Methods
		public virtual bool Equals(Margin margin)
		{
			return (mTop == margin.Top && mLeft == margin.Left && mRight == margin.Right && mBottom == margin.Bottom);
		}

		public object Clone()
		{
			return new Margin(Left,Top,Right,Bottom);
		}

		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("Left",Left);
			info.AddValue("Top",Top);
			info.AddValue("Right",Right);
			info.AddValue("Bottom",Bottom);
		}

		#endregion

	}
}
