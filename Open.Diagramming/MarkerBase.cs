using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Drawing2D;

namespace Crainiate.Diagramming
{
	[Serializable]
	public abstract class MarkerBase: Element, ISerializable, ICloneable
	{
		//Properties
		private Color mBackColor;
		private float mWidth;
		private float mHeight;
		private bool mDrawBackground;
		private bool mCentered;

		private Image mImage;
		private TextLabel mLabel;

		#region Interface

		//Constructor
		public MarkerBase()
		{
			SuspendEvents = true;

			DrawBackground = true;
			BackColor = System.Drawing.Color.Black;
			Width = 8;
			Height = 8;

			SuspendEvents = false;
		}

		public MarkerBase(MarkerBase prototype): base(prototype)
		{
			mWidth = prototype.Width;
			mHeight = prototype.Height;
			mBackColor = prototype.BackColor;
			mDrawBackground = prototype.DrawBackground;
			mCentered = prototype.Centered;
		}
		
		//Deserializes info into a new solid element
		protected MarkerBase(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			
			Width = info.GetSingle("Width");
			Height = info.GetSingle("Height");
			BackColor = Color.FromArgb(Convert.ToInt32(info.GetString("BackColor")));
			DrawBackground = info.GetBoolean("DrawBackground");
			Centered = info.GetBoolean("Centered");

			Label = (TextLabel) info.GetValue("Label",typeof(TextLabel));
			Image = (Image) info.GetValue("Image",typeof(Image));

			SuspendEvents = false;	
		}

		//Properties
		//The color used to draw the shape's background.
		public virtual System.Drawing.Color BackColor
		{
			get
			{
				return mBackColor;
			}
			set
			{
				if (! mBackColor.Equals(value))
				{
					mBackColor = value;
					OnElementInvalid();
				}
			}
		}

		public virtual bool DrawBackground
		{
			get
			{
				return mDrawBackground;
			}
			set
			{
				if (mDrawBackground != value)
				{
					mDrawBackground = value;
					OnElementInvalid();
				}
			}
		}

		public virtual bool Centered
		{
			get
			{
				return mCentered;
			}
			set
			{
				if (mCentered != value)
				{
					mCentered = value;
					OnElementInvalid();
				}
			}
		}

		//Sets or gets the width of the marker
		public virtual float Width
		{
			get
			{
				return mWidth;
			}
			set
			{
				mWidth = value;
				OnElementInvalid();
			}
		}

		//Sets or gets the height of the marker
		public virtual float Height
		{
			get
			{
				return mHeight;
			}
			set
			{
				mHeight = value;
				OnElementInvalid();
			}
		}

		//Returns the Image object which which displays an image for this marker.
		public Image Image
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

		//Returns the annotation for this segment
		public virtual TextLabel Label
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
				}
				OnElementInvalid();
			}
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

		//fill the marker background
		protected internal override void Render(Graphics graphics, IRender render)
		{
			graphics.SmoothingMode = SmoothingMode;
			if (DrawBackground)
			{
				SolidBrush brush;
				brush = new SolidBrush(render.AdjustColor(mBackColor,0,this.Opacity));
				graphics.FillPath(brush,GetPathInternal());
			}
			
			base.Render (graphics, render);

			//Draw any images and annotations
			if (Image != null) Image.Render(graphics,render);
			if (Label != null) Label.Render(graphics,render);
		}

		//fill the marker background
		protected internal override void RenderShadow(Graphics graphics, IRender render)
		{
			Layer layer = render.CurrentLayer;
			Color shadowColor = render.AdjustColor(layer.ShadowColor,BorderWidth,Opacity);
			
			if (DrawBackground)
			{
				SolidBrush brush = new SolidBrush(shadowColor);
				
				//Draw soft shadows
				if (layer.SoftShadows) 
				{
					shadowColor = Color.FromArgb(10,shadowColor);
					graphics.CompositingQuality = CompositingQuality.HighQuality;
					graphics.SmoothingMode = SmoothingMode.HighQuality;
				}

				graphics.FillPath(brush,GetPathInternal());
				
				if (layer.SoftShadows)
				{
					graphics.CompositingQuality = render.CompositingQuality;
					graphics.SmoothingMode = SmoothingMode;
				}
			}

			base.RenderShadow(graphics,render);
		}

		#endregion

		#region Implementation
		
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Width",Width);
			info.AddValue("Height",Height);
			info.AddValue("BackColor",BackColor.ToArgb().ToString());
			info.AddValue("DrawBackground",DrawBackground);
			info.AddValue("Centered",Centered);
			info.AddValue("Label",Label);
			info.AddValue("Image",Image);

			base.GetObjectData(info,context);
		}

		public override object Clone()
		{
			return null;
		}

		#endregion
	}
}
