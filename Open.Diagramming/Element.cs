using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void ElementEventHandler(object sender, ElementEventArgs e);

	// Base class for an object contained in diagram
	[Serializable]
	public class Element: ISerializable, ICloneable
	{
		//Property variables
		internal int mZOrder; //internal for speed when sorting
		internal string mKey; //internal for assignment from collection and override setkey behaviour
		private string mLayerKey;

		private GraphicsPath mPath;
		private Layer mCurrentLayer;
		private IContainer mContainer;
		private Pen mCustomPen;

		private object mTag;
		private bool mVisible;
		private byte mOpacity;
		private string mTooltip;
		private bool mDrawShadow;

		private Color mBorderColor;
		private DashStyle mBorderStyle;
		private float mBorderWidth;
		private SmoothingMode mSmoothingMode;

		private Handles mHandles;
		private Cursor mCursor;
		
		//Working variables
		private bool mSuspendEvents;
		private RectangleF mRectF; //The size of the element
		private Element mActionElement; //associates an action element with this element
		private Element mUpdateElement; //the cloned element for an action

		#region Interface

		//Events
		public event EventHandler ElementInvalid;
		public event EventHandler VisibleChanged;

		//Constructors
		public Element()
		{
			SuspendEvents = true;
			
			mKey = string.Empty;
			mLayerKey = string.Empty;
			Visible = true;
			SmoothingMode = SmoothingMode.AntiAlias;
			Opacity = Component.Instance.DefaultOpacity;
			DrawShadow = true;

			BorderColor = System.Drawing.Color.Black;
			BorderStyle = DashStyle.Solid;
			BorderWidth = Component.Instance.DefaultBorderWidth;
			
			mPath = new GraphicsPath();
			mCursor = null;

			SuspendEvents = false;
		}

		//Creates a new element by copying an existing element
		//Layer should be assigned by the system and should not be copied
		public Element(Element prototype)
		{
			mKey = string.Empty;
			mLayerKey = string.Empty;

			mBorderColor = prototype.BorderColor;
			mBorderStyle = prototype.BorderStyle;
			mBorderWidth = prototype.BorderWidth;
			mSmoothingMode = prototype.SmoothingMode;
			mCustomPen = prototype.CustomPen;
			mDrawShadow = prototype.DrawShadow;
			mOpacity = prototype.Opacity;
			mTooltip = prototype.Tooltip;
			mVisible = prototype.Visible;
			mCursor = prototype.Cursor;

			mPath = prototype.GetPath();
			mRectF = prototype.Rectangle;
			mContainer = prototype.Container;
			
			mTag = prototype.Tag;
		}

		//Creates a new element from the supplied XML.
		protected internal Element(SerializationInfo info, StreamingContext context)
		{
			mCursor = null;
			
			SuspendEvents = true;

			SetPath(Serialize.GetPath(info.GetString("Path")));
			
			mKey = info.GetString("Key");
			mLayerKey = string.Empty;

			BorderColor = Color.FromArgb(Convert.ToInt32(info.GetString("BorderColor")));
			BorderStyle = (DashStyle) Enum.Parse(typeof(DashStyle), info.GetString("BorderStyle"));
			BorderWidth = info.GetSingle("BorderWidth");
			DrawShadow = info.GetBoolean("DrawShadow");
			SmoothingMode = (SmoothingMode) Enum.Parse(typeof(SmoothingMode), info.GetString("SmoothingMode"));
			Opacity = info.GetByte("Opacity");
			Tooltip = info.GetString("Tooltip");
			Visible = info.GetBoolean("Visible");
			mZOrder = info.GetInt32("ZOrder");

			if (Serialize.Contains(info,"Tag")) Tag = info.GetValue("Tag",typeof(object));

			SuspendEvents = false;
		}

		//Properties
		public virtual IContainer Container
		{
			get
			{
				return mContainer;
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
				OnElementInvalid();
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
				OnElementInvalid();
			}
		}

		//Sets or retrieves the width used to draw the shape's border.
		public virtual float BorderWidth
		{
			get
			{
				return mBorderWidth;
			}
			set
			{
				mBorderWidth = value;
				OnElementInvalid();
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
				if (mSmoothingMode != value)
				{
					mSmoothingMode = value;
					OnElementInvalid();
				}
			}
		}

		//Returns the key for this element when contained in a collection.
		public virtual string Key
		{
			get
			{
				return mKey;
			}
		}

		//Returns the key for this element when contained in a layer elements collection.
		public virtual string LayerKey
		{
			get
			{
				return mLayerKey;
			}
		}

		//Sets or gets the tag for this object.
		public virtual object Tag
		{
			get
			{
				return mTag;
			}
			set
			{
				mTag = value;
			}
		}

		public virtual Cursor Cursor
		{
			get
			{
				return mCursor;
			}
			set
			{
				mCursor = value;
			}
		}

		//Determines whether events are prevented from being raised by this class.
		protected internal virtual bool SuspendEvents
		{
			get
			{
				return mSuspendEvents;
			}
			set
			{
				mSuspendEvents = value;
			}
		}

		//Returns the zorder for this shape.
		public virtual int ZOrder
		{
			get
			{
				return mZOrder;
			}
		}

		//Gets or sets the tooltip for this shape.
		public virtual string Tooltip
		{
			get
			{
				return mTooltip;
			}
			set
			{
				mTooltip = value;
			}
		}

		//Defines the percentage opacity for the background of this shape.
		public virtual byte Opacity
		{
			get
			{
				return mOpacity;
			}
			set
			{
				if (mOpacity != value)
				{
					mOpacity = value;
					OnElementInvalid();
				}
			}
		}

		//The rectangle which completely bounds the exterior of this shape.
		public virtual RectangleF Rectangle
		{
			get
			{
				return mRectF;
			}
		}

		//Indicates whether the shape is currently visible and rendered during drawing operations.
		public virtual bool Visible
		{
			get
			{
				return mVisible;
			}
			set
			{
				if (mVisible != value)
				{
					mVisible = value;
					OnVisibleChanged();
					OnElementInvalid();
				}
			}
		}

		//Indicates whether the selection path decoration is shown when the element is selected.
		public virtual bool DrawShadow
		{
			get
			{
				return mDrawShadow;
			}
			set
			{
				if (mDrawShadow != value)
				{
					mDrawShadow = value;
					OnElementInvalid();
				}
			}
		}

		public virtual Pen CustomPen
		{
			get
			{
				return mCustomPen;
			}
			set
			{
				mCustomPen = value;
				OnElementInvalid();
			}
		}

		public virtual Layer Layer
		{
			get
			{
				return mCurrentLayer;
			}
		}

		//Returns the collection of handles this shape holds.
		public virtual Handles Handles
		{
			get
			{
				return mHandles;
			}
		}

		//Sets the element on which an action is being performed
		//Must remain public
		public Element ActionElement
		{
			get
			{
				return mActionElement;
			}
			set
			{
				if (value == null && mActionElement != null) mActionElement.UpdateElement = null;
				mActionElement = value;
				if (mActionElement != null) mActionElement.UpdateElement = this;

			}
		}

		//Sets the element on which an action is being performed
		public Element UpdateElement
		{
			get
			{
				return mUpdateElement;
			}
			set
			{
				mUpdateElement = value;
			}
		}

		//Methods
		//Returns the vector path for this element.
		public virtual GraphicsPath GetPath()
		{
			return (GraphicsPath) mPath.Clone();
		}

		internal GraphicsPath GetPathInternal()
		{
			return mPath;
		}

		//Adds a vector path to this element.
		public virtual void AddPath(GraphicsPath path,bool connect)
		{
			mPath.AddPath(path,connect);
			Geometry.MovePathToOrigin(mPath);
			SetRectangle(GetBoundingRectangle().Size);
			OnElementInvalid();
		}

		//Sets the vector path for this element.
		public virtual void SetPath(GraphicsPath path)
		{
			mPath = path;
			Geometry.MovePathToOrigin(mPath);
			SetRectangle(GetBoundingRectangle().Size);
			OnElementInvalid();
		}	
	
		//Sets the vector path for this element.
		public virtual void ResetPath()
		{
			mPath = new GraphicsPath();
			SetRectangle(new Rectangle(0,0,0,0));
			SetHandles(new Handles());
			OnElementInvalid();
		}

		//Scales the path
		public virtual void ScalePath(float sx, float sy, float dx, float dy)
		{
			GraphicsPath path = Geometry.ScalePath(GetPathInternal(), sx,sy);

			//Calculate path rectangle
			RectangleF rect = GetBoundingRectangle();
			rect.Location = Rectangle.Location;
			rect.Offset(dx,dy);

			SetRectangle(rect); //Sets the bounding rectangle
			SetPath(path); //setpath moves the path to 0,0;
		}

		//Sends a message to the container saying that the elment is invalid
		public virtual void Invalidate()
		{
			OnElementInvalid();
		}
	
		//Returns a string representation of this class
		public virtual string ToString()
		{
			return this.Key;
		}

		//Determines whether the supplied co-ordinates are over the element
		public virtual bool Contains(PointF location)
		{
			return ElementContains(location);
		}

		//Determines whether this element intersects with the rectangle provided
		public virtual bool Intersects(RectangleF rectangle)
		{
			return ElementIntersects(rectangle);
		}

		//Returns the type of cursor from this point
		public virtual Handle Handle(PointF location)
		{
			return Component.Instance.DefaultHandle;
		}

		//Sets the current layer the element is in
		protected internal void SetLayer(Layer layer)
		{
			mCurrentLayer = layer;
		}

		protected internal void SetContainer(IContainer container)
		{
			mContainer = container;
		}

		protected internal void SetHandles(Handles handles)
		{
			mHandles = handles;
		}

		//Updates the rectangle used to store location and size for shapes and lines
		protected internal virtual void SetRectangle(RectangleF rect)
		{
			if (rect.Width <= 0) rect.Width = 1;
			if (rect.Height <= 0) rect.Height = 1;
			mRectF = rect;
		}

		//Updates the rectangle used to store location and size for shapes and lines
		protected internal virtual void SetRectangle(PointF location)
		{
			mRectF.Location = location;
		}

		//Updates the rectangle used to store location and size for shapes and lines
		protected internal virtual void SetRectangle(SizeF size)
		{
			if (size.Width <= 0) size.Width = 1;
			if (size.Height <= 0) size.Height = 1;

			mRectF.Size = size;
		}

		//Return an element point from a diagram point
		protected internal virtual PointF PointToElement(PointF location)
		{
			IContainer container = this.Container;
			return new PointF(location.X-Rectangle.X-container.Offset.X,location.Y-Rectangle.Y-container.Offset.Y);
		}

		//Used to set the key value when the element is contained in a collection
		protected internal void SetKey(string key)
		{
			if (mKey == string.Empty) mKey = key;
		}

		//Used to set the key value when the element is contained in a collection
		protected internal void SetLayerKey(string key)
		{
			if (mLayerKey == string.Empty) mLayerKey = key;
		}

		protected internal void SetOrder(int zorder)
		{
			mZOrder = zorder;
		}

		//Event Methods
		
		//Raises the element invalid event.
		protected virtual void OnElementInvalid()
		{
			if (!(mSuspendEvents) && ElementInvalid!=null) ElementInvalid(this,new EventArgs());
		}

		//Raises the visible event.
		protected virtual void OnVisibleChanged()
		{
			if (!(mSuspendEvents) && VisibleChanged!=null) VisibleChanged(this,new EventArgs());
		}

		#endregion

		#region Implementation

		//Implement ISerializable
		//No layer is required
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("Key",Key);
			info.AddValue("BorderColor",BorderColor.ToArgb().ToString());
			info.AddValue("BorderStyle",Convert.ToInt32(BorderStyle).ToString());
			info.AddValue("BorderWidth",BorderWidth);
			info.AddValue("DrawShadow",DrawShadow);
			info.AddValue("Opacity",Opacity);
			info.AddValue("SmoothingMode",Convert.ToInt32(SmoothingMode).ToString());
			info.AddValue("Tooltip",Tooltip);
			info.AddValue("Visible",Visible);
			info.AddValue("ZOrder",ZOrder);
			info.AddValue("Path",Serialize.AddPath(GetPathInternal()));

			//Check if tag can be added
			Serialize.SerializeTag(info,Tag);
		}

		//Clones an element
		public virtual object Clone()
		{
			return new Element(this);
		}

		//Implement a base rendering of an element
		protected internal virtual void Render(Graphics graphics,IRender render)
		{
			if (mPath == null) return;

			RenderElement(graphics,render);
		}

		//Implement a base rendering of an element
		protected internal virtual void RenderShadow(Graphics graphics,IRender render)
		{
			if (this.Layer == null) return;
			
			Layer layer = Layer;
			Pen shadowPen = new Pen(render.AdjustColor(layer.ShadowColor,BorderWidth,Opacity));
			GraphicsPath shadowPath = GetPathInternal();
			
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

		//Implement a base rendering of an element selection
		protected internal virtual void RenderSelection(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			CreateHandles();
		}

		//Implement a base rendering of an element selection
		protected internal virtual void RenderAction(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			if (renderDesign.ActionStyle == ActionStyle.Default)
			{
				RenderElement(graphics,render);	
			}
			else
			{
				if (GetPathInternal() == null) return;
				graphics.DrawPath(Component.Instance.ActionPen,GetPathInternal());
			}
		}

		//Implement a base rendering of an element selection
		protected internal virtual void RenderHighlight(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			if (GetPathInternal() == null) return;
			//graphics.FillPath(Component.Instance.HighlightBrush,GetPathInternal());
			graphics.DrawPath(Component.Instance.HighlightPen,GetPathInternal());
		}

		//Performs hit testing for an element from a location
		//if a valid diagram provided, hit testing is performed using current transform
		private bool ElementContains(PointF location)
		{
			//Get bounding rect
			float width = BorderWidth + 2;
			RectangleF bound = Rectangle;
			bound.Inflate(width, width);

			//If inside boundary
			if (bound.Contains(location) || bound.Height <= width || bound.Width <= width)
			{
				//Check the outline offset to the path (0,0)
				location.X -= Rectangle.X;
				location.Y -= Rectangle.Y;
				if (GetPathInternal().IsOutlineVisible(location,new Pen(Color.Black, width))) return true;
			}
			
			return false;
		}

		private void RenderElement(Graphics graphics,IRender render)
		{
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
			
			//Can throw an out of memory exception in System.Drawing
			try
			{				
				graphics.SmoothingMode = SmoothingMode;
				graphics.DrawPath(pen, mPath);
			}
			catch
			{
				
			}
		}

		//Determines whether this element intersects with the rectangle provided
		private bool ElementIntersects(RectangleF rectangle)
		{
			return rectangle.IntersectsWith(Rectangle);
		}

		protected internal IRender RenderFromContainer()
		{
			IDiagram diagram = null;
			Group group;
			
			//Set a reference to the parent (group or diagram)
			if (Container is IDiagram) diagram = (IDiagram) Container;
			if (Container is Group) 
			{
				group = (Group) Container;
				diagram = (IDiagram) group.Container;
			}
			return diagram.Render;
		}

		internal virtual RectangleF GetBoundingRectangle()
		{
			RectangleF rect = mPath.GetBounds();
			DiagramUnit unit = DiagramUnit.Pixel;

			return Geometry.RoundRectangleF(rect,unit);
		}

		protected virtual void CreateHandles()
		{
			SetHandles(new Handles());
		}

		#endregion
	}
}
