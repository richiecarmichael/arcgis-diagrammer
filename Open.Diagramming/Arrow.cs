using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Arrow: MarkerBase, ISerializable, ICloneable
	{
		private float mInset;
		
		#region Interface

		//Constructor
		public Arrow()
		{
			Width = 10;
			Inset = 6;
			DrawPath(Width,Height,mInset);
		}

		public Arrow(bool drawBackground)
		{
			Width = 10;
			Inset = 6;
			DrawBackground = drawBackground;
			DrawPath(Width,Height,mInset);
		}

		public Arrow(Arrow prototype): base(prototype)
		{
			mInset = prototype.Inset;
		}

		//Deserializes info into a new solid element
		protected Arrow(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			Inset = info.GetSingle("Inset");
			SuspendEvents = false;	
		}

		//Properties
		//Sets or gets the height of the marker
		public virtual float Inset
		{
			get
			{
				return mInset;
			}
			set
			{
				mInset = value;
				DrawPath(Width,Height,mInset);
				OnElementInvalid();
			}
		}
		#endregion

		#region Overrides

		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				DrawPath(value,Height,mInset);;
				OnElementInvalid();
			}
		}

		public override float Height
		{
			get
			{
				return base.Height;
			}
			set
			{
				base.Height = value;
				DrawPath(Width,value,mInset);
				OnElementInvalid();
			}
		}

		public override object Clone()
		{
			return new Arrow(this);
		}

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Inset",Inset);
			base.GetObjectData(info,context);
		}

		#endregion

		#region Implementation

		//Draws an arrow
		protected virtual void DrawPath(float width,float height,float inset)
		{
			GraphicsPath path = new GraphicsPath();
			float middle = width / 2;
			
			path.AddLine(middle,0,width,height);
			path.AddLine(width,height,middle,inset);
			path.AddLine(middle,inset,0,height);
			path.CloseFigure();
			
			SetPath(path);
		}

		#endregion
	}
}
