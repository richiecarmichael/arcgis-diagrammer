using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void DrawShapeEventHandler(object sender, DrawShapeEventArgs e);
	
	[Serializable]
	public class StencilItem: ISerializable, ICloneable	
	{
		//Property variables
		private bool mRedraw;
		internal string mKey; //internal for assignment from collection

		private Color mBorderColor;
		private DashStyle mBorderStyle;
		private SmoothingMode mSmoothingMode;
		private Color mBackColor;
		private Color mGradientColor;
		private LinearGradientMode mGradientMode;
		private bool mDrawGradient;
		private TextLabel mLabel;
		private Image mImage;
		private StencilItemOptions mStencilItemOptions;
		private bool mKeepAspect;

		//Working variables
		private SizeF mBaseSize;
		private SizeF mLastRequest;
		private RectangleF mBaseInternalRectangle;
		private GraphicsPath mBasePath;
		private float mAspectRatio;

		#region Interface

		//Events
		public event DrawShapeEventHandler DrawShape;

		//Constructors
		public StencilItem()
		{
			mDrawGradient = true;
			mBorderColor = Color.FromArgb(66,65,66);
			mBackColor = Color.White;
			mGradientMode = LinearGradientMode.ForwardDiagonal;
			mGradientColor = Color.White;
			mSmoothingMode = SmoothingMode.HighQuality;
			mStencilItemOptions = StencilItemOptions.InnerRectangleFull | StencilItemOptions.SoftShadow;
			mAspectRatio = 1.0F;
		}

		protected StencilItem(SerializationInfo info, StreamingContext context)
		{
			mKey = info.GetString("Key");
			mRedraw = info.GetBoolean("Redraw");
			mBasePath = Serialize.GetPath(info.GetString("BasePath"));
			mBaseSize = Serialize.GetSizeF(info.GetString("BaseSize"));
			mBaseInternalRectangle = Serialize.GetRectangleF(info.GetString("BaseInternalRectangle"));

			BorderColor = Color.FromArgb(Convert.ToInt32(info.GetString("BorderColor")));
			BorderStyle = (DashStyle) Enum.Parse(typeof(DashStyle), info.GetString("BorderStyle"));
			SmoothingMode = (SmoothingMode) Enum.Parse(typeof(SmoothingMode), info.GetString("SmoothingMode"));
			BackColor = Color.FromArgb(Convert.ToInt32(info.GetString("BackColor")));
			GradientColor = Color.FromArgb(Convert.ToInt32(info.GetString("GradientColor")));
			GradientMode = (LinearGradientMode) Enum.Parse(typeof(LinearGradientMode), info.GetString("GradientMode"),true);
			DrawGradient = info.GetBoolean("DrawGradient");
			//if (Serialize.Contains(info,"Options",typeof(StencilItemOptions)))  Options = (StencilItemOptions) Enum.Parse(typeof(StencilItemOptions), info.GetString("Options"),true);
			mStencilItemOptions = StencilItemOptions.InnerRectangleFull | StencilItemOptions.SoftShadow;
			
			mAspectRatio = 1.0F;
		}

		//Properties
		public virtual bool Redraw
		{
			get
			{
				return mRedraw;
			}
			set
			{
				mRedraw = value;
			}
		}

		public virtual string Key
		{
			get
			{
				return mKey;
			}
			set
			{
				mKey = value;
			}
		}

		public virtual TextLabel Label
		{
			get
			{
				return mLabel;
			}
			set
			{
				mLabel = value;
			}
		}

		public virtual Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				mImage = value;
			}
		}

		public GraphicsPath BasePath 
		{
			get
			{
				return mBasePath;
			}
		}

		public RectangleF BaseInternalRectangle
		{
			get
			{
				return mBaseInternalRectangle;
			}
		}

		public SizeF BaseSize
		{
			get
			{
				return mBaseSize;
			}
		}

		public float AspectRatio
		{
			get
			{
				return mAspectRatio;
			}
		}

		//The color used to draw the shape's borders.
		public virtual Color BorderColor
		{
			get
			{
				return mBorderColor;
			}
			set
			{
				mBorderColor = value;
			}
		}

		//Sets or retrieves the dash style used to draw the shape's border.
		public virtual DashStyle BorderStyle
		{
			get
			{
				return mBorderStyle;
			}
			set
			{
				mBorderStyle = value;
			}
		}

		//gets or sets the value determining how anti aliasing is performed
		public virtual SmoothingMode SmoothingMode
		{
			get
			{
				return mSmoothingMode;
			}
			set
			{
				mSmoothingMode = value;
			}
		}

		//The color used to draw the shape's background.
		public virtual Color BackColor
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
				}
			}
		}

		//The color used to combine the shape's background color when drawing a gradient effect.
		public virtual Color GradientColor
		{
			get
			{
				return mGradientColor;
			}
			set
			{
				if (!mGradientColor.Equals(value))
				{
					mGradientColor = value;
				}
			}
		}

		//Determines how gradients are drawn for this shape.
		public virtual LinearGradientMode GradientMode
		{
			get
			{
				return mGradientMode;
			}
			set
			{
				if (! mGradientMode.Equals(value))
				{
					mGradientMode = value;
				}
			}
		}

		//Determines whether the gradient color is used to render a shape gradient.
		public virtual bool DrawGradient
		{
			get
			{
				return mDrawGradient;
			}
			set
			{
				if (mDrawGradient != value)
				{
					mDrawGradient = value;
				}
			}
		}

		//Determines whether items drawn with the stencil item are intended to keep their height to width ratio the same
		public virtual bool KeepAspect
		{
			get
			{
				return mKeepAspect;
			}
			set
			{
				mKeepAspect = value;
			}
		}

		public virtual StencilItemOptions Options
		{
			get
			{
				return mStencilItemOptions;
			}
			set
			{
				mStencilItemOptions = value;
			}
		}

		//Methods
		//Draw the graphics path by raising the draw event, and set the internal rectangle
		//The internal rectangle should be set by the draw method to prevent it from being calculated
		public virtual GraphicsPath Draw(float width, float height)
		{
			return GetPath(width,height);
		}

		//Returns the internal rectangle for this stencil item
		public virtual RectangleF InternalRectangle(float width, float height)
		{
			if (BasePath == null) return new RectangleF();
			if (Redraw && width == mLastRequest.Width && height == mLastRequest.Height) return mBaseInternalRectangle;

			//Cache the rectangle for next time
			mLastRequest = new SizeF(width,height);

			//Recalculate if first time or a redraw for a different size
			if (mBaseInternalRectangle.IsEmpty || Redraw)
			{
				//Get internal rectangle
				mBaseInternalRectangle = Geometry.GetInternalRectangle(GetPath(width,height));

				//Shrink by one pixel
				mBaseInternalRectangle.Inflate(-1,-1);

				mBaseInternalRectangle = Rectangle.Round(mBaseInternalRectangle);
				return mBaseInternalRectangle;
			}

			return GetInternalRectangle(width,height);
		}

		//Copies the stencil default values to the element supplied
		public virtual void CopyTo(SolidElement element)
		{
			//Set the element values
			element.SuspendEvents = true;

			element.BorderColor = BorderColor;
			element.BorderStyle = BorderStyle;
			element.SmoothingMode = SmoothingMode;
			element.BackColor = BackColor;
			element.GradientColor = GradientColor;
			element.GradientMode = GradientMode;
			element.DrawGradient = DrawGradient;
			element.Label = Label;
			element.Image = Image;
			
			if (element is Shape)
			{
				Shape shape = (Shape) element;
				shape.KeepAspect = KeepAspect;

				//Make sure shape is resized to correct aspect
				if (KeepAspect && AspectRatio != 1)
				{
					//If width greater than height
					if (AspectRatio > 1)
					{
						shape.Height = shape.Width / AspectRatio;
					}
					else
					{
						shape.Width = shape.Height * AspectRatio;
					}
				}
			}

			element.SuspendEvents = false;
		}

		//Sets the base graphics path manually
		protected internal virtual void SetBasePath(GraphicsPath path)
		{
			if (path == null) return;
			mBaseSize = path.GetBounds().Size;
			mBasePath = path;
			Geometry.MovePathToOrigin(mBasePath);
		}

		//Sets the base internal rectangle manually
		protected internal virtual void SetBaseInternalRectangle(RectangleF rectangle, float width, float height)
		{
			mBaseInternalRectangle = rectangle;
			mLastRequest = new SizeF(width,height);
		}

		//Raises the DrawShape event.
		protected virtual void OnDrawShape(GraphicsPath path,float width, float height)
		{
			//If there are no event handlers then draw the default
			if (DrawShape == null) 
			{
				DrawDefault(path,width,height);
			}
			else
			{
				DrawShape(this,new DrawShapeEventArgs(path,width,height));
			}
		}

		#endregion

		#region Implementation

		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("Key",Key);
			info.AddValue("Redraw",Redraw);
			info.AddValue("BasePath",Serialize.AddPath(mBasePath));
			info.AddValue("BaseSize",Serialize.AddSizeF(BaseSize));
			info.AddValue("BaseInternalRectangle",Serialize.AddRectangleF(BaseInternalRectangle));

			info.AddValue("BorderColor",BorderColor.ToArgb().ToString());
			info.AddValue("BorderStyle",Convert.ToInt32(BorderStyle).ToString());
			info.AddValue("SmoothingMode",Convert.ToInt32(SmoothingMode).ToString());
			info.AddValue("BackColor",BackColor.ToArgb().ToString());
			info.AddValue("GradientColor",GradientColor.ToArgb().ToString());				
			info.AddValue("GradientMode",Convert.ToInt32(GradientMode).ToString());				
			info.AddValue("DrawGradient",DrawGradient);
			info.AddValue("Options",Convert.ToInt32(Options).ToString());
		}

		public object Clone()
		{
			StencilItem item = new StencilItem();
			item.Redraw = mRedraw;
		
			item.BorderColor = mBorderColor;
			item.BorderStyle =  mBorderStyle;
			item.SmoothingMode = mSmoothingMode;
			item.BackColor = mBackColor;
			item.GradientColor =  mGradientColor;
			item.GradientMode = mGradientMode;
			item.DrawGradient = mDrawGradient;
			item.Options = mStencilItemOptions;

			item.SetBasePath(mBasePath);
			item.SetBaseInternalRectangle(mBaseInternalRectangle,mBaseSize.Width,mBaseSize.Height);

			return item;
		}

		private void DrawDefault(GraphicsPath path,float width, float height)
		{
			path.AddArc(0, 0, 20, 20, 180, 90);
			path.AddArc(width - 20, 0, 20, 20, 270, 90);
			path.AddArc(width - 20, height - 20, 20, 20, 0, 90);
			path.AddArc(0, height - 20, 20, 20, 90, 90);
			path.CloseFigure();

			SetBaseInternalRectangle(new RectangleF(5, 5, width - 10, height - 10), width, height);
		}

		private GraphicsPath GetPath(float width, float height)
		{
			GraphicsPath path = new GraphicsPath();

			if (Redraw || BasePath == null) 
			{
				//Raise the draw event in which the path is drawn
				OnDrawShape(path,width,height);

				//Scale the path to correct size, calculating ration drawn
				RectangleF rect = path.GetBounds();
				path = Geometry.ScalePath(path, width / rect.Width, height / rect.Height);
				mAspectRatio = (width / rect.Width * height / rect.Height);
				
				//Cache the path for later use, or to calculate internal rectangle
				mBaseSize = new SizeF(width,height); // do not measure size because of rounding issues
				mBasePath = path;
				Geometry.MovePathToOrigin(mBasePath);
			}
			else
			{
				float sx = width / BaseSize.Width;
				float sy = height / BaseSize.Height;
			
				path = Geometry.ScalePath(BasePath,sx,sy);
			}
			
			return path;
		}

		//Return a scaled verion of the cached rectangle
		private RectangleF GetInternalRectangle(float width,float height)
		{
			float sx = width / BaseSize.Width;
			float sy = height / BaseSize.Height;
			return Geometry.ScaleRectangle(BaseInternalRectangle,sx,sy);
		}

		#endregion


	}
}
