using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Marker: MarkerBase,ISerializable,ICloneable
	{
		//Property variables
		private MarkerStyle mMarkerStyle;
		
		#region Interface

		//Constructor
		public Marker()
		{
			mMarkerStyle = MarkerStyle.Ellipse;
			DrawPath();
		}

		public Marker(MarkerStyle style)
		{
			mMarkerStyle = style;
			DrawPath();
		}

		public Marker(Marker prototype): base(prototype)
		{
			mMarkerStyle = prototype.Style;
			DrawPath();
		}

		//Deserializes info into a new solid element
		protected Marker(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			Style = (MarkerStyle) Enum.Parse(typeof(MarkerStyle), info.GetString("Style"),true);
			SuspendEvents = false;	
		}

		//Properties
		public override float Width
		{
			get
			{
				return base.Width;
			}
			set
			{
				base.Width = value;
				DrawPath();
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
				DrawPath();
				OnElementInvalid();
			}
		}


		//Sets or gets the marker style
		public virtual MarkerStyle Style
		{
			get
			{
				return mMarkerStyle;
			}
			set
			{
				mMarkerStyle = value;
				DrawPath();
				OnElementInvalid();
			}
		}

		#endregion

		#region Overrides

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Style",Convert.ToInt32(Style).ToString());
			base.GetObjectData(info,context);
		}

		public override object Clone()
		{
			return new Marker(this);
		}

		#endregion

		#region Implementation

		//Draws an BasicMarker
		protected virtual void DrawPath()
		{
			GraphicsPath path = new GraphicsPath();
			PointF middle = new PointF(Width / 2, Height /2);

			switch (mMarkerStyle)
			{
				case MarkerStyle.Diamond:
					path.AddLine(middle.X,0,Width,middle.Y);
					path.AddLine(Width,middle.Y,middle.X,Height);
					path.AddLine(middle.X,Height,0,middle.Y);
					break;

				case MarkerStyle.Ellipse:
					path.AddEllipse(0,0,Width,Height);
					break;

				case MarkerStyle.Rectangle:
					path.AddRectangle(new RectangleF(0,0,Width,Height));
					break;
			}
			
			SetPath(path);
		}

		#endregion
	}
}
