using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Drawing2D;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Port: SolidElement, ISelectable, ISerializable, ICloneable, IUserInteractive
	{
		//Property variables
		private PortAlignment mAlignment;
		
		private bool mValidate;
		private PointF mOffset;
		private bool mAllowMove;
		private bool mAllowRotate;
		private bool mDrawSelected;
		private bool mSelected;
		private Direction mDirection;
		private UserInteraction mInteraction;
		private PortStyle mPortStyle;

		//Working variables
		internal IPortContainer mParent;
		private float mPercent = 50F;
		private PortOrientation mOrientation;
		private bool mSuspendValidation;
		
		#region Interface

		//Events
		public event LocationChangedEventHandler LocationChanged;
		public event EventHandler SelectedChanged;

		//Constructors
		//Sets the inital orientation
		public Port(PortOrientation orientation)
		{
			SuspendEvents = true;

			Label = null;
			StencilItem = null;

			mValidate  = true;
			mAllowMove = true;
			mAllowRotate = true;
			mOffset = new PointF();
			mOrientation = orientation;
			Alignment =  PortAlignment.Center;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;
			Style = PortStyle.Default;
			SuspendEvents = false;
		}

		//Sets the inital orientation and percentage
		public Port(PortOrientation orientation,float percent)
		{
			SuspendEvents = true;

			Label = null;
			StencilItem = null;

			mValidate  = true;
			mAllowMove = true;
			mAllowRotate = true;
			mOffset = new PointF();
			mOrientation = orientation;
			mPercent = percent;
			Alignment =  PortAlignment.Center;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;
			Style = PortStyle.Default;

			SuspendEvents = false;
		}

		//Sets the inital orientation and percentage
		public Port(float percent)
		{
			SuspendEvents = true;

			Label = null;
			StencilItem = null;

			mValidate  = true;
			mAllowMove = true;
			mAllowRotate = true;
			mOffset = new PointF();
			mOrientation = PortOrientation.Top;
			mPercent = percent;
			Alignment =  PortAlignment.Center;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;
			Style = PortStyle.Default;
			Label = null;

			SuspendEvents = false;
		}

		public Port(Port prototype): base(prototype)
		{
			SuspendEvents = true;

			Label = null;
			StencilItem = null;

			mAlignment = prototype.Alignment;				
			mOffset = prototype.Offset;
			mAllowMove = prototype.AllowMove;
			mAllowRotate = prototype.AllowRotate;
			mDirection = prototype.Direction;
			mInteraction = prototype.Interaction;
			Label = null;
			mPortStyle = prototype.Style;
			Cursor = prototype.Cursor;

			mPercent = prototype.Percent;
			mOrientation = prototype.Orientation;
			
			//Needed for action mvoe
			mParent = prototype.Parent;

			SuspendEvents = false;
		}
		
		//Deserializes info into a new solid element
		protected Port(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;

			Validate = false;

			Alignment = (PortAlignment) Enum.Parse(typeof(PortAlignment), info.GetString("Alignment"),true);
			mPortStyle = (PortStyle) Enum.Parse(typeof(PortStyle), info.GetString("Style"),true);
			AllowMove = info.GetBoolean("AllowMove");
			AllowRotate = info.GetBoolean("AllowRotate");
			DrawSelected = info.GetBoolean("DrawSelected");
			Selected = info.GetBoolean("Selected");
			Direction = (Direction) Enum.Parse(typeof(Direction), info.GetString("Direction"),true);
			Interaction = (UserInteraction) Enum.Parse(typeof(UserInteraction), info.GetString("Interaction"),true);

			mOrientation = (PortOrientation) Enum.Parse(typeof(PortOrientation), info.GetString("Orientation"),true);
			mPercent = info.GetSingle("Percent");

			Validate = true;
			SuspendEvents = false;
		}

		//Properties
		public virtual PortStyle Style
		{
			get
			{
				return mPortStyle;
			}
			set
			{
				mPortStyle = value;
				DrawPortStyle();
				
				if (Orientation == PortOrientation.Right) RotatePath(90);
				if (Orientation == PortOrientation.Bottom) RotatePath(180);
				if (Orientation == PortOrientation.Left) RotatePath(270);

				OnElementInvalid();
			}
		}

		public virtual Direction Direction
		{
			get
			{
				return mDirection;
			}
			set
			{
				mDirection = value;
			}
		}

		public virtual UserInteraction Interaction
		{
			get
			{
				return mInteraction;
			}
			set
			{
				mInteraction = value;
			}
		}

		//The starting orientation of this port
		public virtual PortOrientation Orientation
		{
			get
			{
				return mOrientation;
			}
			set
			{
				if (mOrientation != value)
				{
					int rotation = GetPortRotation(value) - GetPortRotation(mOrientation);
					RotatePath(rotation);
				
					mOrientation = value;
				}
			}
		}

		//The starting percentage of this port
		public virtual float Percent
		{
			get
			{
				return mPercent;
			}
			set
			{
				if (mPercent != value)
				{
					mPercent = value;
					if (Parent != null) Parent.LocatePort(this);
				}
			}
		}

		public virtual PointF Offset
		{
			get
			{
				return mOffset;
			}
		}

		//Returns the port's parent (shape or line)
		public virtual IPortContainer Parent
		{
			get
			{
				return mParent;
			}
		}

		public virtual PortAlignment Alignment
		{
			get
			{
				return mAlignment;
			}
			set
			{
				if (mAlignment != value)
				{
					mAlignment = value;
					SetOffset(CalculateOffset());
					OnElementInvalid();
				}
			}
		}

		//Indicates whether the port can be moved at runtime
		public virtual bool AllowMove
		{
			get
			{
				return mAllowMove;
			}

			set
			{
				mAllowMove = value;
			}
		}

		//Determines whether ports can be moved from one orientation to another by the user
		public virtual bool AllowRotate
		{
			get
			{
				return mAllowRotate;
			}

			set
			{
				mAllowRotate = value;
			}
		}

		//Indicates whether the selection path decoration is shown when the element is selected.
		public virtual bool DrawSelected
		{
			get
			{
				return mDrawSelected;
			}
			set
			{
				if (mDrawSelected != value)
				{
					mDrawSelected = value;
					OnElementInvalid();
				}
			}
		}

		//Indicates whether or the shape is currently selected.
		public virtual bool Selected
		{
			get
			{
				return mSelected;
			}
			set
			{
				if (mSelected != value)
				{
					mSelected = value;
					OnSelectedChanged();
					OnElementInvalid();
				}
			}
		}

		protected internal virtual bool Validate
		{
			get
			{
				return mValidate;
			}
			set
			{
				mValidate = value;
			}
		}

		protected internal virtual void SuspendValidation()
		{
			mSuspendValidation = true;
		}

		protected internal virtual void ResumeValidation()
		{
			mSuspendValidation = false;
		}

		protected internal virtual void SetParent(IPortContainer parent)
		{
			mParent = parent;
		}
		
		protected internal virtual void SetOffset(PointF offset)
		{
			mOffset = offset;
		}

		protected internal virtual void SetPercent(float percent)
		{
			mPercent = percent;
		}
		
		//Raises the shape move event.
		protected virtual void OnLocationChanged(System.Drawing.PointF location)
		{
			if (! SuspendEvents && LocationChanged != null)  LocationChanged(this, new LocationChangedEventArgs(location));
		}

		//Raises the element SelectedChanged event.
		protected virtual void OnSelectedChanged()
		{
			if (!(SuspendEvents) && SelectedChanged!=null) SelectedChanged(this,new EventArgs());
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Port(this);
		}

		public override float X
		{
			get
			{
				return Rectangle.X; 
			}
			set
			{
				//Check if location is valid
				if (!Validate || mSuspendValidation || Parent.ValidatePortLocation(this,new PointF(value,Rectangle.Y))) 
				{
					//Store values before updating underlying rectangle
					float dx = value - Rectangle.X;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y));
					SetRectangle(new PointF(value,Rectangle.Y));
					
					if (Parent != null)
					{
						Orientation = Parent.GetPortOrientation(this,Location);
						SetOffset(CalculateOffset());
					}

					OnLocationChanged(new PointF(value,Rectangle.Y));
					OnElementInvalid();
				}
			}
		}

		public override float Y
		{
			get
			{
				return Rectangle.Y; 
			}
			set
			{
				//Check if location is valid
				if (!Validate || mSuspendValidation || Parent.ValidatePortLocation(this,new PointF(Rectangle.X,value))) 
				{
					//Store values before updating underlying rectangle
					float dy = value - Rectangle.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X, TransformRectangle.Y + dy));
					SetRectangle(new PointF(Rectangle.X,value));
					
					if (Parent != null)
					{
						Orientation = Parent.GetPortOrientation(this,Location);
						SetOffset(CalculateOffset());
					}

					OnLocationChanged(new PointF(Rectangle.X,value));
					OnElementInvalid();
				}
			}
		}

		public override PointF Location
		{
			get
			{
				return Rectangle.Location;
			}
			set
			{
				//Check if location is valid
				if (!Validate || mSuspendValidation || (Parent != null && Parent.ValidatePortLocation(this,value))) 
				{
					//Store values before updating underlying rectangle
					float dx = value.X - Rectangle.X;
					float dy = value.Y - Rectangle.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
					SetRectangle(value);
					
					//Parent may be null if deserializing
					if (Parent != null)
					{
						Orientation = Parent.GetPortOrientation(this,Location);
						SetOffset(CalculateOffset());
					}

					OnLocationChanged(value);
					OnElementInvalid();
				}
			}
		}

		//Moves the port along the path of the parent shape
		public override void Move(float dx, float dy)
		{
			if (dx == 0 && dy == 0) return;

			if (!Validate  || mSuspendValidation || Parent.ValidatePortLocation(this,new PointF(Location.X + dx,Location.Y + dy)))
			{
				SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
				SetRectangle(new PointF(Rectangle.X + dx, Rectangle.Y + dy));

				if (Parent != null)
				{
					Orientation = Parent.GetPortOrientation(this,Location);
					SetOffset(CalculateOffset());
				}

				OnLocationChanged(Rectangle.Location);
				OnElementInvalid();
			}
		}

		//Adjust the intercept for the port offset
		public override PointF Intercept(PointF location)
		{
			PointF intercept;
			
			//If default style then do normal intercept else return center
			if (Style == PortStyle.Default)
			{
				intercept = base.Intercept(new PointF(location.X - mOffset.X,location.Y - mOffset.Y));
			}
			else
			{
				intercept = Center;
			}

			//Offset depending on the port offset
			return new PointF(intercept.X + mOffset.X,intercept.Y + mOffset.Y);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			if (GetPathInternal() == null) return;

			//Translate by offset
			graphics.TranslateTransform(mOffset.X,mOffset.Y);
			
			//Fill and draw the port
			RenderPort(graphics, render);
	
			//Render image
			if (Image != null) Image.Render(graphics,render);

			//Render label
			if (Label != null) Label.Render(graphics,render);	

			graphics.TranslateTransform(-mOffset.X,-mOffset.Y);
		}

		protected internal override void RenderAction(Graphics graphics, IRender render,IRenderDesign renderDesign)
		{
			graphics.TranslateTransform(mOffset.X,mOffset.Y);
			base.RenderAction(graphics,render,renderDesign);
			graphics.TranslateTransform(-mOffset.X,-mOffset.Y);
		}
		
		protected internal override void RenderHighlight(Graphics graphics, IRender render,IRenderDesign renderDesign)
		{
			graphics.TranslateTransform(mOffset.X,mOffset.Y);
			base.RenderHighlight (graphics,render,renderDesign);
			graphics.TranslateTransform(-mOffset.X,-mOffset.Y);
		}

		//Adjust for offset and return
		public override bool Contains(PointF location)
		{
			location.X -= Offset.X;
			location.Y -= Offset.Y;
			
			//If default style then return default else just use rectangle
			if (Style == PortStyle.Simple)
			{
				return Rectangle.Contains(location);
			}
			else
			{
				return base.Contains(location);				
			}
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Alignment",Convert.ToInt32(Alignment).ToString());				
			info.AddValue("Style",Convert.ToInt32(Style).ToString());				
			info.AddValue("AllowMove",AllowMove);
			info.AddValue("AllowRotate",AllowRotate);
			info.AddValue("DrawSelected",DrawSelected);
			info.AddValue("Selected",Selected);
			info.AddValue("Direction",Convert.ToInt32(Direction).ToString());				
			info.AddValue("Interaction",Convert.ToInt32(Interaction).ToString());
			info.AddValue("Orientation",Convert.ToInt32(Orientation).ToString());
			info.AddValue("Percent",Parent.GetPortPercentage(this,Location));

			base.GetObjectData(info,context);
		}	
	
		private void RenderPort(Graphics graphics,IRender render)
		{
			GraphicsPath path = GetPathInternal();

			//Create a brush if no custom brush defined
			if (DrawBackground)
			{
				if (CustomBrush == null)
				{
					//Use a linear gradient brush if gradient requested
					if (DrawGradient)
					{
						LinearGradientBrush brush;
						brush = new LinearGradientBrush(new RectangleF(0,0,Rectangle.Width,Rectangle.Height),render.AdjustColor(BackColor,0,Opacity),render.AdjustColor(GradientColor,0,Opacity),GradientMode);
						brush.GammaCorrection = true;
						if (Blend != null) brush.Blend = Blend;
						graphics.FillPath(brush, path);
					}
						//Draw normal solid brush
					else
					{
						SolidBrush brush;
						brush = new SolidBrush(render.AdjustColor(BackColor,0,this.Opacity));
						graphics.FillPath(brush,path);
					}
				}
				else	
				{
					graphics.FillPath(CustomBrush, path);
				}
			}

			Pen pen = null;

			if (CustomPen == null)
			{
				pen = new Pen(BorderColor,BorderWidth);
				pen.DashStyle = BorderStyle;
				
				//Check if winforms renderer and adjust color as required
				pen.Color = render.AdjustColor(BorderColor,BorderWidth,Opacity);
			}
			else	
			{
				pen = CustomPen;
			}
			
			graphics.SmoothingMode = SmoothingMode;
			graphics.DrawPath(pen,path);

			//Render internal rectangle
			//Pen tempPen = new Pen(Color.Red,2);
			//graphics.DrawRectangle(tempPen,mInternalRectangle.X,mInternalRectangle.Y,mInternalRectangle.Width,mInternalRectangle.Height);
		}

		protected internal PointF CalculateOffset()
		{	
			float width = Rectangle.Width;
			float height = Rectangle.Height;
			float halfwidth = width / 2;
			float halfheight = height / 2;

			if (Alignment == PortAlignment.Center || Parent ==null) return new PointF(- halfwidth, - halfheight);

			//Return outset or inset values
			if (Alignment == PortAlignment.Outset)
			{
				if (mOrientation == PortOrientation.Top) return new PointF(- halfwidth,-height);
				if (mOrientation == PortOrientation.Bottom) return new PointF(- halfwidth,0);
				if (mOrientation == PortOrientation.Left) return new PointF(-width,-halfheight);
				return new PointF(0,-halfheight);
			}
			else
			{
				if (mOrientation == PortOrientation.Top) return new PointF(- halfwidth,0);
				if (mOrientation == PortOrientation.Bottom) return new PointF(- halfwidth,-height);
				if (mOrientation == PortOrientation.Left) return new PointF(0,-halfheight);
				return new PointF(-width,-halfheight);
			}
		}

		private int GetPortRotation(PortOrientation orientation)
		{
			if (orientation == PortOrientation.Right) return 90;
			if (orientation == PortOrientation.Bottom) return 180;
			if (orientation == PortOrientation.Left) return 270;
			return 0;
		}

		private void DrawPortStyle()
		{
			GraphicsPath path = new GraphicsPath();
			RectangleF inner = new RectangleF();
			
			//Default rectangle
			if (Style == PortStyle.Default)
			{
				path.AddRectangle(new Rectangle(0,0,10,10));
				inner = new RectangleF(1,1,8,8);
				SmoothingMode = SmoothingMode.None;
			}
				//Input
			else if (Style == PortStyle.Input)
			{
				path.AddLine(0,0,2,10);
				path.AddLine(2,10,10,10);
				path.AddLine(10,10,12,0);
				path.CloseFigure();
				inner = new RectangleF(1,1,8,8);
				SmoothingMode = SmoothingMode.HighQuality;
			}
				//Output
			else if (Style == PortStyle.Output)
			{
				path.AddLine(2,0,0,10);
				path.AddLine(0,10,12,10);
				path.AddLine(12,10,10,0);
				path.CloseFigure();
				inner = new RectangleF(1,1,8,8);
				SmoothingMode = SmoothingMode.HighQuality;
			}

				//Cross
			else if (Style == PortStyle.Simple)
			{
				path.AddLine(0,0,9,9);
				path.CloseFigure();
				path.AddLine(0,9,9,0);
				path.CloseFigure();
				SmoothingMode = SmoothingMode.None;
			}
			SetPath(path,inner);
		}

		#endregion
	}
}
