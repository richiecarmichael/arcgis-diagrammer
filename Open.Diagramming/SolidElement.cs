using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class SolidElement: Element, ISerializable, ICloneable, ILabelContainer, ITransformable
	{
		//Properties
		private Color mBackColor;
		private Color mGradientColor;
		private LinearGradientMode mGradientMode;
		private Blend mBlend;

		private Brush mCustomBrush;
		private bool mClip;

		private bool mDrawGradient;
		private bool mDrawBorder;

		private TextLabel mLabel;
		private Image mImage;
		private StencilItem mStencilItem;

		private float mRotation;

		//Working variables
		private RectangleF mInternalRectangle;
		internal bool mDrawBackground; // internal for print renderer to set
		private GraphicsPath mTransformPath;
		private RectangleF mTransformRectangle;
		private RectangleF mTransformInternalRectangle;

		#region Interface

		//Constructor
		//Creates a new solid element
		public SolidElement()
		{
			SuspendEvents = true;

			BackColor = System.Drawing.Color.White;
			GradientColor = System.Drawing.Color.White;
			GradientMode = LinearGradientMode.ForwardDiagonal;
			Clip = true;
			DrawGradient = false;
			DrawBorder = true;
			DrawBackground = true;

			SetRectangle(Component.Instance.DefaultSize);
			StencilItem = Component.Instance.DefaultStencilItem;

			SuspendEvents = false;
		}

		public SolidElement(SolidElement prototype): base(prototype)
		{
			mBlend = prototype.Blend;
			mCustomBrush = prototype.CustomBrush;
			mDrawBackground = prototype.DrawBackground;
			mDrawBorder = prototype.DrawBorder;
			mDrawGradient = prototype.DrawGradient;
			mBackColor = prototype.BackColor;
			mGradientColor = prototype.GradientColor;
			mGradientMode = prototype.GradientMode;
			mRotation = prototype.Rotation;

			mTransformPath = prototype.TransformPath;
			mTransformRectangle = prototype.TransformRectangle;
			mTransformInternalRectangle = prototype.TransformInternalRectangle;
			
			if (prototype.Label != null) Label = (TextLabel) prototype.Label.Clone();
			if (prototype.Image != null) Image = (Image) prototype.Image.Clone();
			
			mStencilItem = prototype.StencilItem;
			//if (prototype.StencilItem != null) mStencilItem = (StencilItem) prototype.StencilItem.Clone();

			mInternalRectangle = prototype.InternalRectangle;
		}
		
		//Deserializes info into a new solid element
		protected SolidElement(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;

			BackColor = Color.FromArgb(Convert.ToInt32(info.GetString("BackColor")));
			Clip = info.GetBoolean("Clip");
			GradientMode = (LinearGradientMode) Enum.Parse(typeof(LinearGradientMode), info.GetString("GradientMode"),true);
			GradientColor = Color.FromArgb(Convert.ToInt32(info.GetString("GradientColor")));
			DrawGradient = info.GetBoolean("DrawGradient");
			DrawBorder = info.GetBoolean("DrawBorder");
			DrawBackground = info.GetBoolean("DrawBackground");
			Location = Serialize.GetPointF(info.GetString("Location"));
			SetInternalRectangle(Serialize.GetRectangleF(info.GetString("InternalRectangle")));
			if (Serialize.Contains(info,"Rotation")) Rotation = info.GetSingle("Rotation");

			if (Serialize.Contains(info,"Label",typeof(TextLabel))) Label = (TextLabel) info.GetValue("Label",typeof(TextLabel));
			if (Serialize.Contains(info,"Image",typeof(Image))) Image = (Image) info.GetValue("Image",typeof(Image));
			if (Serialize.Contains(info,"StencilItem",typeof(StencilItem))) mStencilItem = (StencilItem) info.GetValue("StencilItem",typeof(StencilItem));
						
			SuspendEvents = false;	
		}

		//Properties

		//Returns the shape Label which defines the text for this shape.
		public TextLabel Label
		{
			get
			{
				return mLabel;
			}
			set
			{
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

		//Returns the Image object which which displays an image for this shape.
		public Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				if (mImage != null) 
				{
					mImage.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}

				mImage = value;
				if (mImage != null) 
				{
					mImage.ImageInvalid += new EventHandler(Image_ImageInvalid);
					mImage.SetParent(this);
				}

				OnElementInvalid();
			}
		}

		//Gets or sets the stencil used to draw this shape
		public StencilItem StencilItem
		{
			get
			{
				return mStencilItem;
			}
			set
			{
				mStencilItem = value;
				if (value != null) 
				{
					SetPath(value.Draw(Width,Height),value.InternalRectangle(Width,Height));
					value.CopyTo(this);
				}
			}
		}

		//Gets or sets the size of the shape.
		//Doesnt do equality check becuase min or max size may have changed
		public virtual SizeF Size
		{
			get
			{
				return Rectangle.Size;
			}
			set
			{
				SetSizeInternal(value.Width, value.Height);
				OnElementInvalid();
			}
		}

		//Gets or sets the Width of the shape.
		public virtual float Width
		{
			get
			{
				return Rectangle.Width;
			}
			set
			{
				SetSizeInternal(value, Rectangle.Height);
				OnElementInvalid();
			}
		}

		//Gets or sets the Height of the shape.
		public virtual float Height
		{
			get
			{
				return Rectangle.Height;
			}
			set
			{
				SetSizeInternal(Rectangle.Width, value);
				OnElementInvalid();
			}
		}

		//Specifys the clockwise direction of the shape in degrees.
		public virtual float Rotation
		{
			get
			{
				return mRotation;
			}
			set
			{
				if (mRotation != value)
				{
					mRotation = value;

					mTransformPath = Geometry.RotatePath(GetPathInternal(),Location, value);
					mTransformRectangle = mTransformPath.GetBounds();
					
					mTransformPath = Geometry.RotatePath(GetPathInternal(), value);
					mTransformInternalRectangle = Geometry.GetInternalRectangle(mTransformPath);

					OnElementInvalid();
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
					OnElementInvalid();
				}
			}
		}

		//Determines whether the outline determined by the graphicspath is shown for this shape.
		public virtual bool DrawBorder
		{
			get
			{
				return mDrawBorder;
			}
			set
			{
				if (mDrawBorder != value)
				{
					mDrawBorder = value;
					OnElementInvalid();
				}
			}
		}

		//Determines whether the outline determined by the graphicspath is shown for this shape.
		public virtual bool DrawBackground
		{
			get
			{
				return mDrawBackground ;
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

		//Determines whether the image and annotation are confined to the shape's outline.
		public virtual bool Clip
		{
			get
			{
				return mClip;
			}
			set
			{
				if (mClip != value)
				{
					mClip = value;
					OnElementInvalid();
				}
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
					OnElementInvalid();
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
					OnElementInvalid();
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
					OnElementInvalid();
				}
			}
		}

		//Determines how gradients are blended for this shape.
		public virtual Blend Blend
		{
			get
			{
				return mBlend;
			}
			set
			{
				mBlend = value;
				OnElementInvalid();
			}
		}

		//Gets or sets a custom brush used for drawing this shape.
		public virtual Brush CustomBrush
		{
			get
			{
				return mCustomBrush;
			}
			set
			{
				mCustomBrush = value;

				OnElementInvalid();
			}
		}

		//Gets or sets the x co-ordinate of the position of the shape in pixels.
		public virtual float X
		{
			get
			{
				return Rectangle.X;
			}
			set
			{
				if (Rectangle.X != value)
				{
					//Store values before updating underlying rectangle
					float dx = value - Rectangle.X;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y));
					SetRectangle(new PointF(value,Rectangle.Y));
					OnElementInvalid();
				}
			}
		}

		//Gets or sets y co-ordinate of the position of the shape in pixels.
		public virtual float Y
		{
			get
			{
				return Rectangle.Y;
			}
			set
			{
				if (Rectangle.Y != value)
				{
					//Store values before updating underlying rectangle
					float dy = value - Rectangle.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X, TransformRectangle.Y + dy));
					SetRectangle(new PointF(Rectangle.X,value));
					OnElementInvalid();
				}
			}
		}

		//Gets or sets the location of the solid element
		public virtual PointF Location
		{
			get
			{
				return Rectangle.Location;
			}
			set
			{
				if (! Rectangle.Location.Equals(value))
				{
					//Store values before updating underlying rectangle
					float dx = value.X - Rectangle.X;
					float dy = value.Y - Rectangle.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
					SetRectangle(value);
					OnElementInvalid();
				}
			}
		}

		public virtual PointF Center
		{
			get
			{
				return new PointF(Location.X + Rectangle.Width / 2,Location.Y + Rectangle.Height / 2);
			}
			set
			{
				Location = new PointF(value.X - Rectangle.Width / 2, value.Y - Rectangle.Height / 2);
			}
		}

		//Returns the internal rectangle
		public virtual RectangleF InternalRectangle
		{
			get
			{
				return mInternalRectangle;
			}
		}

		//Returns the current path with the current transformation
		public virtual GraphicsPath TransformPath
		{
			get
			{
				return mTransformPath;
			}
		}

		//Returns the rectangle bounding the current transformation
		public virtual RectangleF TransformRectangle
		{
			get
			{
				return mTransformRectangle;
			}
		}

		//Returns the rectangle inside the current transformation
		public virtual RectangleF TransformInternalRectangle
		{
			get
			{
				return mTransformInternalRectangle;
			}
		}

		//Methods
		//Returns the intercept of a line drawn from the point provided to the centre of this shape.
		public virtual PointF Intercept(PointF location)
		{
			return GetIntercept(location);
		}

		//Moves a shape by the offset values supplied.
		public virtual void Move(float dx, float dy)
		{
			SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
			SetRectangle(new PointF(Rectangle.X + dx, Rectangle.Y + dy));
			OnElementInvalid();
		}

		protected internal void SetInternalRectangle(RectangleF rectangle)
		{
			mInternalRectangle = rectangle;
		}

		protected internal void SetSize(float width, float height, RectangleF internalRectangle)
		{
			SetSizeInternal(width,height,internalRectangle);
		}

		protected internal void SetSize(SizeF size, RectangleF internalRectangle)
		{
			SetSizeInternal(size.Width,size.Height,internalRectangle);
		}

		protected internal void SetTransformPath(GraphicsPath path)
		{
			mTransformPath = path;
		}

		protected internal void SetTransformRectangle(RectangleF rectangle)
		{
			mTransformRectangle = rectangle;
		}

		protected internal void SetTransformRectangle(PointF location)
		{
			mTransformRectangle = new RectangleF(location, mTransformRectangle.Size);
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new SolidElement(this);
		}

		protected internal override void Render(Graphics graphics,IRender render)
		{
			if (GetPathInternal() == null) return;
			
 			//Fill the solid
			RenderSolid(graphics, render);
	
			//Set clipping for this shape
			Region current = null;

			//Add local clipping
			if (Clip)
			{
				Region region = new Region(GetPathInternal());
				current = graphics.Clip;
				graphics.SetClip(region,CombineMode.Intersect);
			}
			
			//Render image
			if (Image != null) Image.Render(graphics,render);

			//Render label
			if (Label != null) Label.Render(graphics,InternalRectangle,render);	

			//Restore clipping
			if (Clip) graphics.Clip = current;


			//Call the base implementation of render to draw border
			if (DrawBorder) base.Render(graphics,render);
		}

		protected internal override void RenderShadow(Graphics graphics, IRender render)
		{
			if (DrawBackground)
			{
				if (Layer == null) return;
			
				//Use transformed path as shadows are not rotated
				GraphicsPath shadowPath = Geometry.ScalePath(TransformPath,1F,1F);    

				graphics.TranslateTransform(Layer.ShadowOffset.X ,Layer.ShadowOffset.Y);
				graphics.SmoothingMode = SmoothingMode.AntiAlias;

				//Draw soft shadows
				if (Layer.SoftShadows && StencilItem != null && ((StencilItem.Options & StencilItemOptions.SoftShadow) == StencilItemOptions.SoftShadow))
				{
					PathGradientBrush brush = new PathGradientBrush(shadowPath);

					//Calculate position factor based on 0.3 for 100 pixels
					//0.6 for 50 pixels, 0.15 for 200 pixels
					float factor = Convert.ToSingle(0.3 * (100 / Rectangle.Width));  

					//Set up the brush blend
					Blend blend = new Blend();
					blend.Positions = new float[] {0F,factor,1F};
					blend.Factors = new float[] {0F,0.8F,1F};
					brush.Blend = blend;

					brush.CenterColor = render.AdjustColor(Layer.ShadowColor,1,Opacity);
					//brush.CenterColor = Color.FromArgb(brush.CenterColor.A * 30 / 100,brush.CenterColor);
					brush.SurroundColors = new Color[] {Color.FromArgb(0, Layer.ShadowColor)};
					
					graphics.FillPath(brush, shadowPath);

					brush.Dispose();
				}
				else
				{
					SolidBrush shadowBrush = new SolidBrush(render.AdjustColor(Color.FromArgb(10,Layer.ShadowColor),1,Opacity));
					graphics.FillPath(shadowBrush,shadowPath);
				}

				//Restore graphics
				graphics.TranslateTransform(-Layer.ShadowOffset.X ,-Layer.ShadowOffset.Y);
				graphics.SmoothingMode = SmoothingMode;
			}
			else
			{
				if (DrawBorder) 
				{

					if (this.Layer == null) return;
			
					Layer layer = Layer;
					Pen shadowPen = new Pen(render.AdjustColor(layer.ShadowColor,BorderWidth,Opacity));
					GraphicsPath shadowPath = TransformPath;
			
					graphics.TranslateTransform(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
			
					if (layer.SoftShadows)
					{
						graphics.CompositingQuality = CompositingQuality.HighQuality;
						graphics.SmoothingMode = SmoothingMode.HighQuality;
						graphics.DrawPath(shadowPen,shadowPath);
						graphics.CompositingQuality = render.CompositingQuality;
						graphics.SmoothingMode = SmoothingMode;
					}
					else
					{
						graphics.DrawPath(shadowPen,shadowPath);
					}

					//Restore graphics
					graphics.TranslateTransform(-layer.ShadowOffset.X ,-layer.ShadowOffset.Y);
				}
			}
		}

		protected internal override void RenderAction(Graphics graphics, IRender render,IRenderDesign renderDesign)
		{
			if (DrawBackground)
			{
				if (renderDesign.ActionStyle == ActionStyle.Default)
				{
					RenderSolid(graphics,render);

					//Add local clipping
					Region current = null;
					if (Clip)
					{
						Region region = new Region(GetPathInternal());
						current = graphics.Clip;
						graphics.SetClip(region,CombineMode.Intersect);
					}
					//Render annotation and image
					if (Label != null) Label.RenderAction(graphics,InternalRectangle,render);	
			
					//Restore clipping
					if (Clip) graphics.Clip = current;
				}
				else
				{
					GraphicsPath path = GetPathInternal();
					if (path == null) return;

					graphics.FillPath(Component.Instance.ActionBrush,path);
				}
			}
			if (DrawBorder) base.RenderAction (graphics, render,renderDesign);
		}

		//Adds a vector path to this element.
		public override void AddPath(GraphicsPath path, bool connect)
		{
			if (path.PointCount == 0) return;
			
			base.AddPath(path,connect);
		}

		//Set the rotated path
		public override void SetPath(GraphicsPath path)
		{
			base.SetPath(path);

			if (Rotation == 0)
			{
				SetTransformRectangle(Rectangle);
				SetTransformPath(path);
				mTransformInternalRectangle = InternalRectangle;
			}
			else
			{
				SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds());
				SetTransformPath(Geometry.RotatePath(path, Rotation));
				mTransformInternalRectangle = Geometry.GetInternalRectangle(TransformPath);
			}
		}

		public virtual void SetPath(GraphicsPath path, RectangleF internalRectangle)
		{
			base.SetPath(path);
			SetInternalRectangle(internalRectangle);
			
			if (Rotation == 0)
			{
				SetTransformRectangle(Rectangle);
				SetTransformPath(path);
				mTransformInternalRectangle = internalRectangle;
			}
			else
			{
				SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds()); //Bounds with location
				SetTransformPath(Geometry.RotatePath(path, Rotation)); //Transformed without location
				mTransformInternalRectangle = Geometry.GetInternalRectangle(TransformPath); //only used for path docking optimization
			}
		}

		//Sets the vector path for this element.
		public override void ResetPath()
		{
			SetInternalRectangle(new RectangleF());
			base.ResetPath();
		}

		public override void ScalePath(float x, float y, float dx, float dy)
		{
			base.ScalePath (x, y, dx, dy);

			GraphicsPath path = GetPathInternal(); 
			SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds());
			SetTransformPath(Geometry.RotatePath(path, Rotation));
		}

		public virtual void ScalePath(float x, float y, float dx, float dy, RectangleF internalRectangle)
		{
			base.ScalePath(x,y,dx,dy);

			SetInternalRectangle(internalRectangle);

			GraphicsPath path = GetPathInternal(); 
			SetTransformRectangle(Geometry.RotatePath(path, Location, Rotation).GetBounds());
			SetTransformPath(Geometry.RotatePath(path, Rotation));

			if (Rotation == 0) mTransformInternalRectangle = internalRectangle;
		}

		public virtual void RotatePath(float degrees)
		{
			SetPath(Geometry.RotatePath(GetPath(), degrees));
		}

		//Determines whether this solid element contains the location
		public override bool Contains(PointF location)
		{
			if (DrawBackground)
			{
				return SolidContains(location);
			}
			else
			{
				return base.Contains(location);
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

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("BackColor",BackColor.ToArgb().ToString());
			info.AddValue("Clip",Clip);
			info.AddValue("GradientMode",Convert.ToInt32(GradientMode).ToString());				
			info.AddValue("GradientColor",GradientColor.ToArgb().ToString());				
			info.AddValue("DrawGradient",DrawGradient);
			info.AddValue("DrawBorder",DrawBorder); 
			info.AddValue("DrawBackground",DrawBackground);
			info.AddValue("Location",Serialize.AddPointF(Location));
			info.AddValue("InternalRectangle",Serialize.AddRectangleF(InternalRectangle));

			if (Label != null) info.AddValue("Label",Label);
			if (Image != null) info.AddValue("Image",Image);
			if (StencilItem != null) info.AddValue("StencilItem",StencilItem);
			if (Rotation != 0) info.AddValue("Rotation", Rotation);

			base.GetObjectData(info,context);
		}

		private void RenderSolid(Graphics graphics,IRender render)
		{
			//Create a brush if no custom brush defined
			if (DrawBackground)
			{
				if (CustomBrush == null)
				{
					//Use a linear gradient brush if gradient requested
					if (DrawGradient)
					{
						LinearGradientBrush brush;
						brush = new LinearGradientBrush(new RectangleF(0,0,Rectangle.Width,Rectangle.Height),render.AdjustColor(BackColor,0,Opacity),render.AdjustColor(GradientColor,0,Opacity),mGradientMode);
						brush.GammaCorrection = true;
						if (Blend != null) brush.Blend = Blend;
						graphics.FillPath(brush, GetPathInternal());
					}
					//Draw normal solid brush
					else
					{
						SolidBrush brush;
						brush = new SolidBrush(render.AdjustColor(BackColor,0,this.Opacity));
						graphics.FillPath(brush,GetPathInternal());
					}
				}
				else	
				{
					graphics.FillPath(CustomBrush, GetPathInternal());
				}
			}

//			//Render internal rectangle
//			Pen tempPen = new Pen(Color.Red,1);
//			graphics.DrawRectangle(tempPen,mInternalRectangle.X,mInternalRectangle.Y,mInternalRectangle.Width,mInternalRectangle.Height);
//
//			tempPen = new Pen(Color.Green,1);
//			graphics.DrawRectangle(tempPen,mTransformInternalRectangle.X,mTransformInternalRectangle.Y,mTransformInternalRectangle.Width,mTransformInternalRectangle.Height);

		}

		private PointF GetIntercept(PointF location)
		{
			//Cache the location properties
			float x = X;
			float y = Y;
			PointF center = Center;

			//Create transform location moved to the path origin and check if inside path
			PointF transform = new PointF(location.X - x,location.Y - y);
			if (TransformPath.IsVisible(transform)) return center;

			//Get the bounding rectangle intercept and move it to the origin
			//because all path measurements are from the origin
			location = Geometry.RectangleIntersection(location, center, TransformRectangle);
			location = new PointF(location.X - x,location.Y - y);
			
			//Get the center of the shape offset to the path origin
			center = new PointF(center.X - x,center.Y - y);
			
			location = Geometry.GetPathIntercept(location, center, TransformPath, Component.Instance.DefaultPen, mTransformInternalRectangle);
			location = Units.RoundPoint(location,1);

			return new PointF(location.X + x, location.Y + y);
		}

		private void SetSizeInternal(float width, float height)
		{
			if (StencilItem != null && StencilItem.Redraw)
			{
				SetPath(StencilItem.Draw(width,height),StencilItem.InternalRectangle(width,height));
			}
			else
			{
				float scaleX = Convert.ToSingle(width / Rectangle.Width);
				float scaleY = Convert.ToSingle(height / Rectangle.Height);
				
				//Scale rectangle
				RectangleF original = InternalRectangle;
				RectangleF rect = new RectangleF(original.X * scaleX,original.Y * scaleY,original.Width * scaleX,original.Height * scaleY);
				ScalePath(scaleX,scaleY,0,0,rect);
			}
		}

		private void SetSizeInternal(float width, float height, RectangleF internalRectangle)
		{
			if (StencilItem != null && StencilItem.Redraw)
			{
				SetPath(StencilItem.Draw(width,height),internalRectangle);
			}
			else
			{
				float scaleX = Convert.ToSingle(width / Rectangle.Width);
				float scaleY = Convert.ToSingle(height / Rectangle.Height);
				ScalePath(scaleX,scaleY,0,0,internalRectangle);
			}
		}

		private bool SolidContains(PointF location)
		{
			//Get boundary
			RectangleF bounds = TransformRectangle;

			//If inside inflate boundary
			if (bounds.Contains(location))
			{
				//Check the outline offset to the path (0,0)
				location.X -= TransformRectangle.X;
				location.Y -= TransformRectangle.Y;
				
				//Can return in use error
				try
				{
					if (TransformPath.IsVisible(location)) return true;
				}
				catch
				{
					
				}
			}
			
			return false;
		}

		public virtual PointF GetLabelLocation()
		{
			if (Label.Size.IsEmpty || Label.Offset.IsEmpty) return InternalRectangle.Location;
			return Label.Offset;
		}

		public virtual SizeF GetLabelSize()
		{
			if (Label.Size.IsEmpty || Label.Offset.IsEmpty) return InternalRectangle.Size;
			return Label.Size;
		}

		#endregion

	}
}
