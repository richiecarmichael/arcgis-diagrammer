using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void LocationChangedEventHandler(object sender, LocationChangedEventArgs e);
	public delegate void SizeChangedEventHandler(object sender, SizeChangedEventArgs e);
	public delegate void RotationChangedEventHandler(object sender, RotationChangedEventArgs e);

	[Serializable]
	public class Shape:SolidElement, ISerializable, ICloneable, ISelectable, IUserInteractive, IPortContainer, IAnimatable
	{
		//Properties
		private bool mAllowMove;
		private bool mAllowScale;
		private bool mAllowRotate;

		private SizeF mMinimumSize;
		private SizeF mMaximumSize;

		private bool mDrawSelected;
		private bool mSelected;
		private bool mKeepAspect;
		private Direction mDirection;
		private UserInteraction mInteraction;

		private Elements mPorts;
		private Animation mAnimation;

		//Working variables

		#region Interface
		
		//Events
		public event LocationChangedEventHandler LocationChanged;
		public event SizeChangedEventHandler SizeChanged;
		public event RotationChangedEventHandler RotationChanged;
		public event EventHandler SelectedChanged;

		//Constructors
		public Shape()
		{
			SuspendEvents = true;

			AllowMove = true;
			AllowScale = true;
			AllowRotate = false;
			DrawSelected = true;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;

			MinimumSize = new SizeF(32, 32);
			MaximumSize = new SizeF(320, 320);

			Ports = new Elements(typeof(Port),"Port");

			SuspendEvents = false;
		}

		public Shape(StencilItem stencil)
		{
			SuspendEvents = true;

			AllowMove = true;
			AllowScale = true;
			AllowRotate = false;
			DrawSelected = true;
			Direction = Direction.Both;
			Interaction = UserInteraction.BringToFront;

			MinimumSize = new SizeF(32, 32);
			MaximumSize = new SizeF(320, 320);

			//Set up stencil
			StencilItem = stencil;
			stencil.CopyTo(this);

			Ports = new Elements(typeof(Port),"Port");

			SuspendEvents = false;
		}

		public Shape(Shape prototype): base(prototype)
		{
			mAllowMove = prototype.AllowMove;
			mAllowScale = prototype.AllowScale;
			mAllowRotate = prototype.AllowRotate;
			mDrawSelected = prototype.DrawSelected;
			mDirection = prototype.Direction;
			mInteraction = prototype.Interaction;
			
			mMaximumSize = prototype.MaximumSize;
			mMinimumSize = prototype.MinimumSize;
			mKeepAspect = prototype.KeepAspect;

			//Copy ports
			Ports = new Elements(typeof(Port),"Port");
			foreach (Port port in prototype.Ports.Values)
			{
				Port clone = (Port) port.Clone();
				Ports.Add(port.Key,clone);
				
				clone.SuspendValidation();
				clone.Location = port.Location;
				clone.ResumeValidation();
			}

			if (prototype.Animation != null) mAnimation = (Animation) prototype.Animation.Clone();
		}

		//Deserializes info into a new solid element
		protected Shape(SerializationInfo info, StreamingContext context): base(info,context)
		{
			Ports = new Elements(typeof(Port),"Port");
			
			SuspendEvents = true;

			AllowMove = info.GetBoolean("AllowMove");
			AllowScale = info.GetBoolean("AllowScale");
			if (Serialize.Contains(info,"AllowRotate")) AllowRotate = info.GetBoolean("AllowRotate");
			DrawSelected = info.GetBoolean("DrawSelected");
			Selected = info.GetBoolean("Selected");
			KeepAspect = info.GetBoolean("KeepAspect");
			MinimumSize = Serialize.GetSizeF(info.GetString("MinimumSize"));
			MaximumSize = Serialize.GetSizeF(info.GetString("MaximumSize"));
			
			Direction = (Direction) Enum.Parse(typeof(Direction), info.GetString("Dock"),true);
			Interaction = (UserInteraction) Enum.Parse(typeof(UserInteraction), info.GetString("Interaction"),true);
			
			if (Serialize.Contains(info,"Ports",typeof(Elements))) Ports = (Elements) info.GetValue("Ports",typeof(Elements));
			if (Serialize.Contains(info,"Animation",typeof(Animation))) Animation = (Animation) info.GetValue("Animation",typeof(Animation));

			SuspendEvents = false;

		}

		//Properties
		public virtual Animation Animation
		{
			get
			{
				return mAnimation;
			}
			set
			{
				mAnimation = value;
			}
		}

		//Determines whether this shape can be moved by a model move action.
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

		//Determines whether this shape can be scaled by a model size action.
		public virtual bool AllowScale
		{
			get
			{
				return mAllowScale;
			}
			set
			{
				mAllowScale = value;
			}
		}

		//Determines whether this shape can be scaled by a model size action.
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

		//Determines if the shape maintains it's ratio of width to height when sized.
		public virtual bool KeepAspect
		{
			get
			{
				return mKeepAspect;
			}
			set
			{
				if (value != mKeepAspect)
				{
					mKeepAspect = value;
					CreateHandles();
				}
			}
		}

		//Determines the minimum width and height the shape can be resized to through the model interface.
		public virtual SizeF MinimumSize
		{
			get
			{
				return mMinimumSize;
			}
			set
			{
				mMinimumSize = value;
			}
		}

		//Determines the maximum width and height the shape can be resized to.
		public virtual SizeF MaximumSize
		{
			get
			{
				return mMaximumSize;
			}
			set
			{
				mMaximumSize = value;
			}
		}

		//Gets or sets the size of the shape.
		//Doesnt do equality check becuase min or max size may have changed
		public virtual SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(-Rotation);

								

				//Use old rectangle size to calculate port offset from new size
				SizeF existing = Size;
				SizeF size = ValidateSize(value.Width, value.Height); 
				
				base.Size = size;

				ScalePorts(size.Width / existing.Width, size.Height / existing.Height, 0 ,0);

				OnSizeChanged(Rectangle);

				//Rotate ports back to original location
				if (Rotation != 0) RotatePorts(Rotation);
			}
		}

		public virtual Elements Ports
		{
			get
			{
				return mPorts;
			}
			set
			{
				//Reset handlers
				if (mPorts != null)
				{
					mPorts.InsertElement -=new ElementsEventHandler(Ports_InsertElement);
					
					foreach (Port port in mPorts.Values)
					{
						port.ElementInvalid -=new EventHandler(Port_ElementInvalid);
					}
				}

				if (value == null) 
				{
					mPorts = new Elements(typeof(Port),"Port");
				}
				else
				{					
					mPorts = value;
					mPorts.InsertElement +=new ElementsEventHandler(Ports_InsertElement);
					
					//Set the back references for the ports
					foreach (Port port in mPorts.Values)
					{
						port.SetParent(this);
						port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
					}
				}
				OnElementInvalid();
			}
		}

		//Methods

		//Scales and moves a shape by the new supplied ratios and changes.
		public virtual void Scale(float scaleX, float scaleY, float dx, float dy, bool maintainAspect)
		{
			//Rotate ports to zero for scaling
			if (Rotation != 0) RotatePorts(-Rotation);

			ScaleShape(scaleX, scaleY, dx, dy, maintainAspect);
			OnSizeChanged(Rectangle);

			//Rotate ports to zero for scaling
			if (Rotation != 0) RotatePorts(Rotation);
		}

		//Rotates the shape clockwise by the specified number in degrees.
		public virtual void Rotate(float degrees)
		{
			//Call the property so that the correct code is run
			Rotation += degrees;
		}

		//Raises the shape move event.
		protected virtual void OnLocationChanged(System.Drawing.PointF location)
		{
			if (! SuspendEvents && LocationChanged != null)  LocationChanged(this, new LocationChangedEventArgs(location));
		}
		
		//Raises the shape scale event.
		protected virtual void OnSizeChanged(RectangleF rect)
		{
			if (! SuspendEvents && SizeChanged != null) SizeChanged(this, new SizeChangedEventArgs(rect));
		}

		//Raises the shape rotate event.
		protected virtual void OnRotationChanged(float degrees)
		{
			if (! SuspendEvents && RotationChanged != null) RotationChanged(this, new RotationChangedEventArgs(degrees));
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
			return new Shape(this);
		}

		//Change the x co ordinate of the shape's location
		public override float X
		{
			get
			{
				return Rectangle.X;
			}
			set
			{
				if (Rectangle.X != value)
				{
					//Move ports by change in x and y
					foreach(Port port in Ports.Values)
					{
						port.SuspendValidation();
						port.Move(value - Rectangle.X,0);
						port.ResumeValidation();
					}

					//Adjust the location rectangle and transform rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + (value - Rectangle.X), TransformRectangle.Y));
					SetRectangle(new PointF(value,Rectangle.Y));

					OnLocationChanged(Rectangle.Location);
					OnElementInvalid();
				}
			}
		}

		//Change the y co ordinate of the shape's location
		public override float Y
		{
			get
			{
				return base.Y;
			}
			set
			{
				if (Rectangle.Y != value)
				{
					//Move ports by change in x and y
					foreach(Port port in Ports.Values)
					{
						port.SuspendValidation();
						port.Move(0,value - Rectangle.Y);
						port.ResumeValidation();
					}

					//Adjust the location rectangle and transform rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X, TransformRectangle.Y + (value - Rectangle.Y)));
					SetRectangle(new PointF(Rectangle.X,value));

					//Raise the appropriate events
					OnLocationChanged(Rectangle.Location);
					OnElementInvalid();
				}
			}
		}

		//Changes the location by using the move method
		public override PointF Location
		{
			get
			{
				return base.Location;
			}
			set
			{
				if (!Rectangle.Location.Equals(value))
				{
					//Store values before updating underlying rectangle
					float dx = value.X - Rectangle.X;
					float dy = value.Y - Rectangle.Y;
					
					//Update control rectangle
					SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
					SetRectangle(value);

					//Move ports by change in x and y
					if (Ports != null)
					{
						foreach(Port port in Ports.Values)
						{
							port.SuspendValidation();
							port.Move(dx,dy);
							port.ResumeValidation();
						}
					}

					OnLocationChanged(Rectangle.Location);
					OnElementInvalid();
				}
			}
		}
		
		//Moves the shape and the ports
		public override void Move(float dx, float dy)
		{
			//Move each of the ports
			foreach(Port port in Ports.Values)
			{
				port.SuspendValidation();
				port.Move(dx, dy);
				port.ResumeValidation();
			}

			//Update the shape rectangle
			SetTransformRectangle(new PointF(TransformRectangle.X + dx, TransformRectangle.Y + dy));
			SetRectangle(new PointF(Rectangle.X + dx, Rectangle.Y + dy));

			//Raise the appropriate events
			OnLocationChanged(Rectangle.Location);
			OnElementInvalid();
		}

		//Gets or sets the Width of the shape.
		public override float Width
		{
			get
			{
				return Rectangle.Width;
			}
			set
			{
				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(-Rotation);

				//Use the old values to validate size and scale ports
				float existing = Width;
				float width = ValidateSize(value, Rectangle.Height).Width;
				
				base.Width = width;

				ScalePorts(width / existing, 1, 0 ,0);

				OnSizeChanged(Rectangle);

				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(Rotation);
			}
		}

		//Gets or sets the Height of the shape.
		public override float Height
		{
			get
			{
				return Rectangle.Height;
			}
			set
			{
				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(-Rotation);

				//Validate the new height
				float existing = Height;
				float height = ValidateSize(Rectangle.Width, value).Height;
				
				base.Height = height;

				ScalePorts(1, height / existing, 0 ,0);
				
				OnSizeChanged(Rectangle);

				//Rotate ports to zero for scaling
				if (Rotation != 0) RotatePorts(Rotation);
			}
		}

		//Raises the roation changed event when the rotation is changed
		public override float Rotation
		{
			get
			{
				return base.Rotation;
			}
			set
			{
				if (base.Rotation != value)
				{
					//Get the change in rotation
					float dr = value - Rotation;

					base.Rotation = value;
					RotatePorts(dr);

					OnRotationChanged(value);
				}
			}
		}

		//Returns the type of cursor from this point
		public override Handle Handle(PointF location)
		{
			return GetShapeHandle(location);
		}

		//Create a list of handles 
		protected override void CreateHandles()
		{
			if (Container == null) return;
			SetHandles(new Handles());

			//Get the default graphics path and scale it
			IRender render = RenderFromContainer();
			GraphicsPath defaultPath = (GraphicsPath) Component.Instance.DefaultHandlePath.Clone();
			Matrix matrix = new Matrix();
			matrix.Scale(render.ZoomFactor,render.ZoomFactor);
			defaultPath.Transform(matrix);

			RectangleF pathRectangle = defaultPath.GetBounds();
			RectangleF halfRectangle = new RectangleF(0,0,pathRectangle.Width /2, pathRectangle.Height /2);
			float onePixel = 1 * render.ZoomFactor;

			//Add top left
			GraphicsPath path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(-pathRectangle.Width - onePixel,-pathRectangle.Height - onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path, HandleType.TopLeft));

			//Add top right
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(TransformRectangle.Width, -pathRectangle.Height - onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.TopRight));

			//Add bottom left
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(- pathRectangle.Width - onePixel, TransformRectangle.Height + onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.BottomLeft));

			//Add bottom right
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(TransformRectangle.Width + onePixel, TransformRectangle.Height + onePixel);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.BottomRight));

			if (!KeepAspect)
			{
				//Add top middle
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate((TransformRectangle.Width / 2) - halfRectangle.Width, -pathRectangle.Height - onePixel);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Top));

				//Add left
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(- pathRectangle.Width  -onePixel , (TransformRectangle.Height / 2) - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Left));

				//Add right
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(TransformRectangle.Width + onePixel, (TransformRectangle.Height / 2) - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Right));

				//Add bottom
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate((TransformRectangle.Width / 2) - halfRectangle.Width, TransformRectangle.Height + onePixel);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Bottom));
			}

			//Add rotation handle
			if (AllowRotate)
			{
				//Add top middle
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate((TransformRectangle.Width / 2) - halfRectangle.Width, (TransformRectangle.Height / 2) - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path,HandleType.Rotate));
			}
		}

		protected SizeF ValidateSize(float width, float height)
		{
			if (!KeepAspect || width > height)
			{
				if (width < mMinimumSize.Width) width = mMinimumSize.Width;
				if (width > mMaximumSize.Width) width = mMaximumSize.Width;
			}
			
			if (!KeepAspect || height > width)
			{
				if (height < mMinimumSize.Height) height = mMinimumSize.Height;
				if (height > mMaximumSize.Height) height = mMaximumSize.Height;
			}
			
			return new SizeF(width,height);
		}

		public override bool Contains(PointF location)
		{
			return ShapeContains(location);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			//Render this shape
			base.Render (graphics, render);

			//Undo the rotate transform
			if (Rotation != 0) 
			{
				//Get the local center
				PointF center = new PointF(Rectangle.Width / 2 , Rectangle.Height / 2);
				Matrix matrix = graphics.Transform;
				matrix.RotateAt(-Rotation,center);
			
				graphics.Transform = matrix; 
			}
			
			//Render the ports
			if (Ports != null)
			{
				foreach (Port port in Ports.Values)
				{
					if (port.Visible)
					{
						graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
						graphics.RotateTransform(port.Rotation);		
						port.SuspendValidation();
						port.Render(graphics,render);
						port.ResumeValidation();
						graphics.RotateTransform(-port.Rotation);		
						graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
					}
				}
			}

			//Redo the rotate transform
			if (Rotation != 0) 
			{
				//Get the local center
				PointF center = new PointF(Rectangle.Width / 2 , Rectangle.Height / 2);
				Matrix matrix = graphics.Transform;
				matrix.RotateAt(Rotation,center);
			
				graphics.Transform = matrix; 
			}
		}

		//Implement a base rendering of an element selection
		protected internal override void RenderSelection(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			CreateHandles();

			RectangleF boundRect = TransformRectangle;
			float boundsSize = 4 * render.ZoomFactor;
			float handleSize = 6 * render.ZoomFactor;
			boundRect.Inflate(boundsSize,boundsSize);
			
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			SolidBrush brush = new SolidBrush(Color.White);
			Pen pen = (Pen) Component.Instance.SelectionHatchPen.Clone();
			pen.Width = handleSize;
			graphics.DrawRectangle(pen, -boundsSize, -boundsSize, boundRect.Width, boundRect.Height); 

			foreach (Handle handle in Handles)
			{
				if (handle.Type == HandleType.Rotate)
				{
					graphics.FillPath(brush,handle.Path);
					graphics.FillPath(Component.Instance.SelectionRotateBrush,handle.Path);
					graphics.DrawPath(Component.Instance.SelectionRotatePen,handle.Path);
				}
				else
				{
					graphics.FillPath(brush,handle.Path);
					graphics.FillPath(Component.Instance.SelectionBrush,handle.Path);
					graphics.DrawPath(Component.Instance.SelectionPen,handle.Path);
				}
			}

			graphics.SmoothingMode = SmoothingMode;
		}

		protected internal override void RenderAction(Graphics graphics, IRender render, IRenderDesign renderDesign)
		{
			base.RenderAction (graphics, render, renderDesign);

            return;

			//Render the ports
            //Port actions are currently not supported
			if (Ports != null && renderDesign.ActionStyle == ActionStyle.Default)
			{
				foreach (Port port in Ports.Values)
				{
					if (port.Visible)
					{
						graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
						port.SuspendValidation();
						port.Render(graphics,render);
						port.ResumeValidation();
						graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
					}
				}
			}
		}

		#endregion

		#region Events
		
		private void Ports_InsertElement(object sender, ElementsEventArgs e)
		{
			//Sets the shape of the port
			Port port = (Port) e.Value;
			port.SetParent(this);
			port.SetLayer(Layer);
			port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
			port.SetContainer(Container);
			port.SetOrder(mPorts.Count -1);

			//If not deserializing then locate port
			if (port.Location.IsEmpty) 
			{
				LocatePort(port);
			}
			//Else just set orientation and offset
			else
			{
				port.Orientation = GetPortOrientation(port, port.Location);
				port.SetOffset(port.CalculateOffset());
			}
		}

		//Occurs when a port becomes invalid
		private void Port_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("AllowMove",AllowMove);
			info.AddValue("AllowScale",AllowScale);
			info.AddValue("AllowRotate",AllowRotate);
			info.AddValue("DrawSelected",DrawSelected);
			info.AddValue("Selected",Selected);
			info.AddValue("KeepAspect",KeepAspect);
			info.AddValue("MinimumSize",Serialize.AddSizeF(MinimumSize));
			info.AddValue("MaximumSize",Serialize.AddSizeF(MaximumSize));
			info.AddValue("Dock",Convert.ToInt32(Direction).ToString());
			info.AddValue("Interaction",Convert.ToInt32(Interaction).ToString());

			if (Ports.Count > 0) info.AddValue("Ports",Ports);
			if (Animation != null) info.AddValue("Animation",Animation);
			
			base.GetObjectData(info,context);
		}	
	
		//Performs hit testing for an element from a location
		//if a valid diagram provided, hit testing is performed using current transform
		private bool ShapeContains(PointF location)
		{
			//Inflate rectangle to include selection handles
			RectangleF bound = TransformRectangle;
			
			//Inflate rectangle to include grab handles
			IRender render = RenderFromContainer();
			float handle = 6 * render.ZoomFactor;
			bound.Inflate(handle,handle);

			//If inside inflate boundary
			if (bound.Contains(location))
			{
				//Return true if clicked in selection rectangle but not path rectangle
				if (Selected && !TransformRectangle.Contains(location)) return true;

				//Check the outline offset to the path (0,0)
				location.X -= Rectangle.X;
				location.Y -= Rectangle.Y;

			
				if (DrawBackground)
				{
					if (TransformPath.IsVisible(location)) return true;
				}
				else
				{
					//Get bounding rect
					float width = BorderWidth + 2;

					if (TransformPath.IsOutlineVisible(location,new Pen(Color.Black, width))) return true;
				}
			}
			
			return false;
		}

		//Scale a shape using ratios
		private void ScaleShape(float sx, float sy, float dx, float dy, bool maintainAspect)
		{
			//Check min and max sizes
			if (!KeepAspect || Width >= Height)
			{
				if (mMaximumSize.Width < (Rectangle.Width * sx)) sx = Convert.ToSingle(mMaximumSize.Width / Rectangle.Width);
				if (mMinimumSize.Width > (Rectangle.Width * sx)) sx = Convert.ToSingle(mMinimumSize.Width / Rectangle.Width);
			}
			
			if (!KeepAspect || Height >= Width)
			{
				if (mMaximumSize.Height < (Rectangle.Height * sy)) sy = Convert.ToSingle(mMaximumSize.Height / Rectangle.Height);
				if (mMinimumSize.Height > (Rectangle.Height * sy)) sy = Convert.ToSingle(mMinimumSize.Height / Rectangle.Height);
			}

			if (maintainAspect)
			{
				if (sx < sy)
				{
					sy = sx;
				}
				else
				{
					sx = sy;
				}
			}

			//Get the new size
			float width = Width * sx;
			float height = Height * sy;

			if (StencilItem != null && StencilItem.Redraw)
			{
				SetPath(StencilItem.Draw(width,height),StencilItem.InternalRectangle(width,height));
				SetRectangle(new PointF(Location.X + dx,Location.Y + dy));
			}
			else
			{
				if (StencilItem == null)
				{
					RectangleF original = InternalRectangle;
					RectangleF rect = new RectangleF(original.X * sx,original.Y * sy,original.Width * sx,original.Height * sy);
					ScalePath(sx,sy,dx,dy,rect);
				}
				else
				{
					ScalePath(sx,sy,dx,dy,StencilItem.InternalRectangle(width,height));
				}
			}
			
			//Offset the ports before the new rectangle is updated
			ScalePorts(sx,sy,dx,dy);
		}

		private void ScalePorts(float sx, float sy, float dx, float dy)
		{
			//Change positions of ports
			SizeF offset;

			if (Ports != null)
			{
				foreach (Port port in Ports.Values)
				{
					Matrix matrix = new Matrix();

					//Set the origin to the rectangle location
					matrix.Translate(Rectangle.X, Rectangle.Y);

					//Scale the matrix so that the offset is updated
					matrix.Scale(sx, sy);

					//Offset the port using the difference between the port and the rectangle
					matrix.Translate(port.Location.X - Rectangle.X, port.Location.Y - Rectangle.Y);

					//Reset the scale
					matrix.Scale(1/sx, 1/sy);

					//Once the matrix transforms have been reset, move by the dx and dy values
					matrix.Translate(dx, dy);

					port.SuspendValidation();
					port.Location = new PointF(matrix.OffsetX, matrix.OffsetY);
					port.ResumeValidation();
				}
			
			}
		}

		private void RotatePorts(float degrees)
		{
			if (Ports != null)
			{
				PointF rotateat = new PointF(Center.X - Rectangle.X, Center.Y - Rectangle.Y);
				
				foreach (Port port in Ports.Values)
				{
					Matrix matrix = new Matrix();

					//Set the origin to the rectangle location
					matrix.Translate(Rectangle.X, Rectangle.Y);

					//Rotate around the center 
					matrix.RotateAt(degrees, rotateat);

					//Offset the port using the difference between the port and the rectangle
					matrix.Translate(port.Location.X - Rectangle.X, port.Location.Y - Rectangle.Y);
		
					port.SuspendValidation();
					port.Location = new PointF(matrix.OffsetX, matrix.OffsetY);			
					port.Rotation = Rotation;
					port.ResumeValidation();
				}
			}
		}

		//Locate a port based on the orientation(side) of the parent and the percentage
		public virtual void LocatePort(Port port)
		{
			RectangleF shapeRect = TransformRectangle;
			PointF start = new PointF();
			float ratio = port.Percent / 100;

			switch (port.Orientation)
			{
				case PortOrientation.Top:
					start = new PointF(shapeRect.X +(shapeRect.Width * ratio),shapeRect.Y-1);
					break;
				case PortOrientation.Bottom:
					start = new PointF(shapeRect.X +(shapeRect.Width * ratio),shapeRect.Y+shapeRect.Height+1);
					break;
				case PortOrientation.Left:
					start = new PointF(shapeRect.X-1,shapeRect.Y +(shapeRect.Height * ratio));
					break;
				case PortOrientation.Right:
					start = new PointF(shapeRect.X + shapeRect.Width +1,shapeRect.Y +(shapeRect.Height * ratio));
					break;
				default:
					break;
			}
			
			port.Validate = false;
			port.Location = Intercept(start);
			port.Validate = true;
		}

		public virtual PortOrientation GetPortOrientation(Port port,PointF location)
		{
			return Geometry.GetOrientation(location,Center,Rectangle);
		}

		public virtual float GetPortPercentage(Port port,PointF location)
		{
			float ratio = 0;

			if (port.Orientation == PortOrientation.Top || port.Orientation == PortOrientation.Bottom)
			{
				ratio = (location.X-Rectangle.X) / (Rectangle.Right - Rectangle.Left);
			}
			else
			{
				ratio = (location.Y-Rectangle.Y) / (Rectangle.Bottom - Rectangle.Top);
			}

			return Convert.ToSingle(Math.Round(ratio * 100,1));
		}

		//Takes the port and validates its location against the shape's path
		public bool ValidatePortLocation(Port port,PointF location)
		{
			//Check for switch changes
			if (!port.AllowRotate)
			{
				PortOrientation orientation = Geometry.GetOrientation(location,Center,Rectangle);
				if (port.Orientation != orientation) return false;
			}

			//Offset location to local co-ordinates and check outline
			location.X -= Rectangle.X;
			location.Y -= Rectangle.Y;

			return TransformPath.IsOutlineVisible(location,new Pen(Color.Black,1));
		}

		//Gets the cursor from the diagram point
		private Handle GetShapeHandle(PointF location)
		{
			if (!Selected || Handles == null) return Component.Instance.DefaultHandle;

			//Offset location to local co-ordinates
			location = new PointF(location.X - TransformRectangle.X - Container.Offset.X, location.Y - TransformRectangle.Y - Container.Offset.Y);

			//Check each handle
			foreach (Handle handle in Handles)
			{
				if (handle.Path.IsVisible(location)) return handle;
			}

			return Component.Instance.DefaultHandle;
		}

		#endregion

	}
}
