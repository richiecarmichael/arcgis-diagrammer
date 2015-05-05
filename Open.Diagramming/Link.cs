using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Link : Line, ISerializable, ICloneable, ILabelContainer	
	{
		//Property variables
		private Label mLabel;
		private Image mImage;

		#region Interface

		//Constructors
		public Link()
		{
			SuspendEvents = true;

			End.Marker = new Arrow(false);

			SuspendEvents = false;
		}
		public Link(Shape start,Shape end) : base(start,end)
		{
			SuspendEvents = true;

			End.Marker = new Arrow(false);

			SuspendEvents = false;		
		}
		public Link(PointF start,PointF end) : base(start,end)
		{
			SuspendEvents = true;

			End.Marker = new Arrow(false);

			SuspendEvents = false;
		}
		public Link(Port start,Port end) : base(start,end)
		{
			SuspendEvents = true;

			End.Marker = new Arrow(false);

			SuspendEvents = false;
		}

		public Link(Link prototype): base(prototype)
		{
			if (prototype.Label != null) Label = (Label) prototype.Label.Clone();
			if (prototype.Image != null) Image = (Image) prototype.Image.Clone();
		}
		
		protected Link(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
	
			if (Serialize.Contains(info,"Label",typeof(Label))) Label = (Label) info.GetValue("Label",typeof(Label)); 
			if (Serialize.Contains(info,"Image",typeof(Image))) Image = (Image) info.GetValue("Image",typeof(Image));
			
			SuspendEvents = false;	
		}

		//Properties
	
		//Returns the label for this link
		public virtual Label Label
		{
			get
			{
				return mLabel;
			}
			set
			{
				//Remove any existing handlers
				if (mLabel != null)
				{
					mLabel.LabelInvalid -= new EventHandler(Label_LabelInvalid);
				}

				mLabel = value;
				if (mLabel != null)
				{
					mLabel.LabelInvalid += new EventHandler(Label_LabelInvalid);
					mLabel.SetParent(this);
				}
				OnElementInvalid();
			}
		}

		//Returns the Image object which which displays an image for this link
		public virtual Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				//Remove any existing handlers
				if (mImage != null) 
				{
					mImage.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}
				
				mImage = value;
				if (mImage != null) 
				{
					mImage.ImageInvalid += new EventHandler(Image_ImageInvalid);
				}
				OnElementInvalid();
			}
		}

		//Methods
		public virtual PointF GetLabelLocation()
		{
			//Determine central point
			//If even number of points, then position half way on segment
			PointF center;

			PointF start = (PointF) Points[0];
			PointF end = (PointF) Points[1];
			center = new PointF(start.X + ((end.X - start.X) /2),start.Y + ((end.Y - start.Y) /2));

			//Offset to line co-ordinates
			return new PointF(center.X - Rectangle.X,center.Y - Rectangle.Y);
		}

		public virtual SizeF GetLabelSize()
		{
			Graphics graphics = Component.Instance.CreateGraphics();
			return graphics.MeasureString(Label.Text,Label.Font);
		}

		#endregion

		#region Events

		//Handles annotation invalid events
		private void Label_LabelInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		//Handles image invalid events
		private void Image_ImageInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Link(this);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			base.Render (graphics, render);

			//Render Image and Label
			if (Image != null || Label != null)
			{
				PointF center = GetLabelLocation();

				graphics.TranslateTransform(center.X,center.Y);
				if (Image != null) Image.Render(graphics,render);
				if (Label != null) Label.Render(graphics,render);
				graphics.TranslateTransform(-center.X,-center.Y);						
			}
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (Label != null) info.AddValue("Label",Label);
			if (Image != null)info.AddValue("Image",Image);

			base.GetObjectData(info,context);
		}

		#endregion

	}
}
