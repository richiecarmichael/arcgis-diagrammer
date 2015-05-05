using System;
using System.IO;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Layouts;
using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Svg;

namespace Crainiate.Diagramming
{
	public delegate void SerializeEventHandler(object sender, SerializationEventArgs e);
	public delegate void SerializeCompleteEventHandler(object sender, SerializationCompleteEventArgs e);

	[ToolboxBitmap(typeof(Diagram), "Resource.diagram.bmp")]
	public class Diagram: System.Windows.Forms.UserControl, IDiagram, IContainer ,ICloneable
	{
		//Property variables
		private System.ComponentModel.Container components;

		private Elements mShapes;
		private Elements mLines;
		private Layers mLayers;
		private Navigation.Navigate mNavigate;
		private RenderList mRenderList;
		private IRender mRender;
		private Margin mMargin;
		private Animator mAnimator;

		private Size mDiagramSize;
		
		private bool mShowToolTips = true;
		private bool mCheckBounds = true;

		//Working variables
		private Rectangle mPageRect; //The currently visible area of the diagram
		private Rectangle mControlRect; //The control area in screen co-ordinates
		private Point mPageOffset;
		private int mSuspendCount = 0;
		private bool mSuspendEvents;
		private Route mRoute;

		private MouseElements mMouseElements;
		private MouseEventArgs mMouseEvent;

		private Status mSaveStatus;
		private Status mStatus;
		private int mRenderTimeRatio;

		private const int WM_HSCROLL = 0x114;
		private const int WM_VSCROLL = 0x115;
		private const int SB_ENDSCROLL = 8;
		
		#region Interface

		//Classes and structs
		public struct MouseElements
		{
			public Element MouseStartElement;
			public Element MouseMoveElement;
			public Element InteractiveElement;
			public Origin InteractiveOrigin;
			public Origin MouseStartOrigin;
            public MouseButtons StartButton;
			

			public MouseElements(MouseElements prototype)
			{
				MouseStartElement = prototype.MouseStartElement;
				MouseMoveElement = prototype.MouseMoveElement;
				MouseStartOrigin = prototype.MouseStartOrigin;
				InteractiveElement = prototype.InteractiveElement;
				InteractiveOrigin = prototype.InteractiveOrigin;
                StartButton = prototype.StartButton;
			}
			
			public void Clear()
			{
				MouseStartElement = null;
				MouseMoveElement = null;
				MouseStartOrigin = null;
				InteractiveElement = null;
				InteractiveOrigin = null;
                StartButton = MouseButtons.None;
			}
	
			public ISelectable MouseStartSelectable
			{
				get
				{
					return (ISelectable) MouseStartElement;
				}
			}

			public ISelectable MouseMoveSelectable
			{
				get
				{
					return (ISelectable) MouseMoveElement;
				}
			}
		}

		//Events
		//Element Mouse Events
		[Category("Mouse"),Description("Occurs when a mouse button is pressed over an element.")]
		public event MouseEventHandler ElementMouseDown;
		[Category("Mouse"),Description("Occurs when a mouse button is released over an element.")]
		public event MouseEventHandler ElementMouseUp;
		[Category("Action"),Description("Occurs when an element is clicked.")]
		public event EventHandler ElementClick;
		[Category("Action"),Description("Occurs when an element is double-clicked.")]
		public event EventHandler ElementDoubleClick;
		[Category("Mouse"),Description("Occurs when the mouse enters an element.")]
		public event EventHandler ElementEnter;
		[Category("Mouse"),Description("Occurs when the mouse leaves an element.")]
		public event EventHandler ElementLeave;

		//Diagram Mouse Events
		[Category("Mouse"),Description("Occurs when a mouse button is pressed over the diagram.")]
		public event MouseEventHandler DiagramMouseDown;
		[Category("Mouse"),Description("Occurs when a mouse button is released over the diagram.")]
		public event MouseEventHandler DiagramMouseUp;
		[Category("Action"),Description("Occurs when the diagram is clicked.")]
		public event EventHandler DiagramClick;
		[Category("Action"),Description("Occurs when the diagram is double-clicked.")]
		public event EventHandler DiagramDoubleClick;

		//Elements events
		[Category("Behavior"),Description("Occurs when an element is inserted into the diagram.")]
		public event ElementsEventHandler ElementInserted;
		[Category("Behavior"),Description("Occurs when an element is removed form the diagram.")]
		public event ElementsEventHandler ElementRemoved;
		[Category("Behavior"),Description("Occurs when an element has changed.")]
		public event ElementEventHandler ElementInvalid;
		
		//Diagram events
		[Category("Layout"),Description("Occurs when the value of the zoom property has changed.")]
		public event EventHandler ZoomChanged;
		[Category("Layout"),Description("Occurs when the value of the zoom property has changed.")]
		public event EventHandler PagedChanged;
		[Category("Layout"),Description("Occurs when diagram is scrolled inside the control.")]
		public event EventHandler Scroll;
		[Category("Data"),Description("Occurs before diagram data is saved.")]
		public event SerializeEventHandler Serialize;
		[Category("Data"),Description("Occurs before diagram data is loaded")]
		public event SerializeEventHandler Deserialize;
		[Category("Data"),Description("Occurs when the saving of diagram data is complete.")]
		public event SerializeEventHandler SerializeComplete;
		[Category("Data"),Description("Occurs when the loading of diagram data is complete.")]
		public event SerializeCompleteEventHandler DeserializeComplete;

		//Constructor
		public Diagram()
		{
			//Create License
			Component.Instance.GetLicense(typeof(Diagram), this);
			
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();
			SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
			SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			SetStatus(Status.Default);

			//Create a new renderer
			SetRender(new Render()); //Sets up current layers reference etc
			ClearDiagram();
			Margin = new Margin();
			Animator = new Animator(this);
			
			//Create an initial render and draw onto control surface
			mRender.RenderDiagram(new Rectangle(0,0,this.Width,this.Height));
			DrawDiagram(new Rectangle(0,0,this.Width,this.Height));
		}

		public Diagram(Diagram prototype)
		{
			InitializeComponent();
			SetStyle(System.Windows.Forms.ControlStyles.UserPaint, true);
			SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			SetStatus(Status.Default);
			
			//Create a new renderer
			SetRender(new Render()); //Sets up current layers reference etc
			ClearDiagram();	
			Margin = new Margin();

			SuspendEvents = true;
			
			mDiagramSize = prototype.DiagramSize;
			mShowToolTips = prototype.ShowTooltips;
			mCheckBounds = prototype.CheckBounds;
			mMargin = prototype.Margin;
			Paged = prototype.Paged;
			WorkspaceColor = prototype.WorkspaceColor;

			SuspendEvents = false;
		}

		//Properties
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Provides access to the diagram's shapes collection.")]
		public virtual Elements Shapes
		{
			get
			{
				return mShapes;
			}
			set
			{
				//Remove any existing handlers
				if (mShapes != null)
				{
					mShapes.InsertElement -= new ElementsEventHandler(Element_Insert);
					mShapes.RemoveElement -= new ElementsEventHandler(Element_Remove);
					mShapes.Cleared -= new EventHandler(mShapes_Cleared);
				}

				if (value == null)
				{
					mShapes = new Elements(typeof(Shape),"Shape");
				}
				else
				{
					mShapes = value;
				}
				mShapes.InsertElement += new ElementsEventHandler(Element_Insert);
				mShapes.RemoveElement += new ElementsEventHandler(Element_Remove);
				mShapes.Cleared += new EventHandler(mShapes_Cleared);

				if (Shapes !=null && Lines != null && !Suspended) GetRenderList(Render.RenderRectangle);
				Invalidate();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Provides access to the diagram's lines collection.")]
		public virtual Elements Lines
		{
			get
			{
				return mLines;
			}
			set
			{
				//Remove any existing handlers
				if (mLines != null)
				{
					mLines.InsertElement -= new ElementsEventHandler(Element_Insert);
					mLines.RemoveElement -= new ElementsEventHandler(Element_Remove);
					mLines.Cleared -= new EventHandler(mLines_Cleared);
				}

				if (value == null)
				{
					mLines = new Elements(typeof(Line),"Line");
				}
				else
				{
					mLines = value;
				}
					
				mLines.InsertElement += new ElementsEventHandler(Element_Insert);
				mLines.RemoveElement += new ElementsEventHandler(Element_Remove);
				mLines.Cleared += new EventHandler(mLines_Cleared);

				if (!Suspended) GetRenderList(Render.RenderRectangle);
				Invalidate();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns the layers collection for this diagram.")]
		public virtual Layers Layers
		{
			get
			{
				return mLayers;
			}
			set
			{
				mLayers = (value == null) ? mLayers = new Layers() : value;
				Render.Layers = value;
				if (Shapes != null && Lines != null && !Suspended) GetRenderList(Render.RenderRectangle);

				Invalidate();
			}
		}
		
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns an instance of the render class used to draw the diagram.")]
		public virtual IRender Render
		{
			get
			{
				return mRender;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns an instance of the render class if the renderer implements IRenderPaged.")]
		public virtual IRenderPaged RenderPaged
		{
			get
			{
				if (mRender is IRenderPaged) return (IRenderPaged) mRender;
				return null;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Provides access to the class used to return diagram graphs.")]
		public virtual Navigation.Navigate Navigate
		{
			get
			{
				return mNavigate;
			}
			set
			{
				mNavigate = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns a list of the elements currently being rendered.")]
		public virtual RenderList RenderList
		{
			get
			{
				return mRenderList;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets or retrieves the current margin values.")]
		public virtual Margin Margin
		{
			get
			{
				return mMargin;
			}
			set
			{
				mMargin = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Provides access to the current elements under the mouse pointer.")]
		public virtual Animator Animator
		{
			get
			{
				return mAnimator;
			}
			set
			{
				mAnimator = value;
				if (value != null) mAnimator.SetDiagram(this);
			}
		}


		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Provides access to the current elements under the mouse pointer.")]
		public virtual MouseElements CurrentMouseElements
		{
			get
			{
				return mMouseElements;
			}
		}

		[Category("Layout"), RefreshProperties(RefreshProperties.All), Description("Sets or retrieves the size of the internal diagram.")]
		public virtual Size DiagramSize
		{
			get
			{
				return mDiagramSize;
			}
			set
			{
				if (! mDiagramSize.Equals(value))
				{
					mDiagramSize = value;
					CheckDiagramSize();
					SetResizeRectangles(); //Calls SetPagedSettings();
					Invalidate();
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Returns the container offset. This will always be an empty value for a diagram control.")]
		public PointF Offset
		{
			get
			{
				return new PointF(0,0);
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Returns the size of the container.")]
		public SizeF ContainerSize
		{
			get
			{
				return new SizeF(DiagramSize.Width,DiagramSize.Height);
			}
		}
		
		[Category("Behavior"), DefaultValue(true), Description("Determines whether the movement of elements is restricted to the bounds of the diagram.")]
		public virtual bool CheckBounds
		{
			get
			{
				return mCheckBounds;
			}
			set
			{
				mCheckBounds = value;
			}
		}

		[Category("Behavior"), DefaultValue(100), RefreshProperties(RefreshProperties.All), Description("Sets or retrieves the current zoom level as a percentage.")]
		public virtual float Zoom
		{
			get
			{
				return mRender.Zoom;
			}
			set
			{
				if (value != mRender.Zoom)
				{
					mRender.Zoom = value;
					CheckDiagramSize();
					SetScrollRectangles();
					SetPagedSettings();
					Invalidate();

					OnZoomChanged();
				}
			}
		}

		[Browsable(false), Description("Sets or retrieves the current page rectangle.")]
		protected virtual Rectangle PageRectangle
		{
			get
			{
				return mPageRect;
			}
		}

		[Browsable(false), Description("Sets or retrieves the current control rectangle.")]
		protected virtual Rectangle ControlRectangle
		{
			get
			{
				return mControlRect;
			}
		}

		[Category("Layout"), DefaultValue(false), RefreshProperties(RefreshProperties.All), Description("Determines whether the diagram is displayed in page format.")]
		public virtual bool Paged
		{
			get
			{
				if (RenderPaged == null)
				{
					return false;
				}
				else
				{
					return RenderPaged.Paged;
				}
			}
			set
			{
				if (mRender is IRenderPaged)
				{
					RenderPaged.Paged = value;
					CheckDiagramSize();
					SetResizeRectangles();
					Invalidate();
				}
			}
		}

		[Category("Appearance"), Description("Sets or gets the color used to draw the application workspace.")]
		public virtual Color WorkspaceColor
		{
			get
			{
				if (mRender is IRenderPaged)
				{
					return RenderPaged.WorkspaceColor;
				}
				else
				{
					return SystemColors.AppWorkspace;
				}
			}
			set
			{
				if (mRender is IRenderPaged)
				{
					RenderPaged.WorkspaceColor = value;
					Invalidate();
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Retrieves a boolean value determining whether render and draw operations are suspended.")]
		public virtual bool Suspended
		{
			get
			{
				return mSuspendCount > 0;
			}
		}

		[Browsable(false), Category("Behavior"),Description("Returns the current action status of the diagram.")]
		public virtual Status Status
		{
			get
			{
				return mStatus;
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Determines if the diagram shows tooltips for elements in the diagram.")]
		public virtual bool ShowTooltips
		{
			get
			{
				return mShowToolTips;
			}
			set
			{
				mShowToolTips = value;
			}
		}

		//Determines whether events are prevented from being raised by this class.
		[Category("Behavior"), DefaultValue(false), Description("Determines whether events are prevented from being raised by this class.")]
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

		//Sets up the routing (AStar) class instance
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Route Route
		{
			get
			{
				return mRoute;
			}
			set
			{
				mRoute = value;
			}
		}

		//Methods
		[Browsable(false), Description("Resets all collections and properties of the diagram to their defaults.")]
		public virtual void Clear()
		{
			ClearDiagram();
			Refresh();
		}

		[Browsable(false), Description("Loads a diagram from the filename provided")]
		public virtual void Open(string path)
		{
			SoapFormatter formatter = new SoapFormatter();
			formatter.AssemblyFormat =  System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			Stream stream = new FileStream(path,FileMode.Open);
			LoadDiagram(stream,formatter);
			stream.Close();
		}

		[Browsable(false), Description("Loads a diagram from the stream provided")]
		public virtual void Open(Stream stream)
		{
			SoapFormatter formatter = new SoapFormatter();
			formatter.AssemblyFormat =  System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			LoadDiagram(stream,formatter);
		}

		[Browsable(false), Description("Loads a diagram from the filename provided using the format specified")]
		public virtual void Open(string path, LoadFormat format)
		{
			Stream stream = new FileStream(path, FileMode.Open);

			//Load binary
			if (format == LoadFormat.Binary)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				LoadDiagram(stream, formatter);
			}
			//Load xml
			else if (format == LoadFormat.Xml)
			{
				SoapFormatter formatter = new SoapFormatter();
				LoadDiagram(stream, formatter);
			}

			stream.Close();
		}

		[Browsable(false), Description("Loads a diagram from the stream provided using the format specified")]
		public virtual void Open(Stream stream, LoadFormat format)
		{
			//Load binary
			if (format == LoadFormat.Binary)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				LoadDiagram(stream,formatter);
			}
				//Load xml
			else if (format == LoadFormat.Xml)
			{
				SoapFormatter formatter = new SoapFormatter();
				LoadDiagram(stream,formatter);
			}
		}

		[Browsable(false), Description("Saves a diagram in binary format to the filename provided")]
		public virtual void Save(string path)
		{
			SoapFormatter formatter = new SoapFormatter();
			formatter.AssemblyFormat =  System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			Stream stream = new FileStream(path,FileMode.Create);
			SaveDiagram(stream,formatter);
			stream.Close();
		}

		[Browsable(false), Description("Saves a diagram in binary format to the stream provided")]
		public virtual void Save(Stream stream)
		{
			SoapFormatter formatter = new SoapFormatter();
			formatter.AssemblyFormat =  System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
			SaveDiagram(stream,formatter);
		}

		[Browsable(false), Description("Saves a diagram in the format specified to the filename provided")]
		public virtual void Save(string path, SaveFormat format)
		{
			FileStream stream = null;

			//Create new filestream
			if (format != SaveFormat.Metafile)
			{
				stream = new FileStream(path, FileMode.Create);
			}

			//Save options
			if (format == SaveFormat.Binary)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				SaveDiagram(stream,formatter);
			}
			else if (format == SaveFormat.Xml)
			{
				SoapFormatter formatter = new SoapFormatter();
				formatter.AssemblyFormat =  System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
				SaveDiagram(stream,formatter);
			}
			else if (format == SaveFormat.Svg)
			{
				ExportToSvg(stream);
			}
			else if (format == SaveFormat.Metafile)
			{
				ExportToMetafile(null,path);
			}
			//Export to image
			else if (format == SaveFormat.Bmp)
			{
				ExportToPicture(stream, ImageFormat.Bmp);
			}
			else if (format == SaveFormat.Gif)
			{
				ExportToPicture(stream, ImageFormat.Gif);
			}
			else if (format == SaveFormat.Jpeg)
			{
				ExportToPicture(stream, ImageFormat.Jpeg);
			}
			else if (format == SaveFormat.Png)
			{
				ExportToPicture(stream, ImageFormat.Png);
			}

			//Close filestream
			if (format != SaveFormat.Metafile)
			{
				stream.Close();
			}
		}

		[Browsable(false), Description("Saves a diagram in the format specified to the stream provided")]
		public virtual void Save(Stream stream, SaveFormat format)
		{
			if (format == SaveFormat.Binary)
			{
				BinaryFormatter formatter = new BinaryFormatter();
				SaveDiagram(stream,formatter);
			}
			else if (format == SaveFormat.Xml)
			{
				SoapFormatter formatter = new SoapFormatter();
				formatter.AssemblyFormat = System.Runtime.Serialization.Formatters.FormatterAssemblyStyle.Simple;
				SaveDiagram(stream,formatter);
			}
			else if (format == SaveFormat.Svg)
			{
				ExportToSvg(stream);
			}
			else if (format == SaveFormat.Metafile)
			{
				if (! (stream is FileStream)) throw new Exception("Stream must be a filestream when saving to the Metafile format.");
				
				FileStream file = (FileStream) stream;
				ExportToMetafile(file,null);
			}
				//Export to image
			else if (format == SaveFormat.Bmp)
			{
				ExportToPicture(stream, ImageFormat.Bmp);
			}
			else if (format == SaveFormat.Gif)
			{
				ExportToPicture(stream, ImageFormat.Gif);
			}
			else if (format == SaveFormat.Jpeg)
			{
				ExportToPicture(stream, ImageFormat.Jpeg);
			}
			else if (format == SaveFormat.Png)
			{
				ExportToPicture(stream, ImageFormat.Png);
			}
		}

		[Browsable(false), Description("Determines whether the diagram contains this point.")]
		public bool Contains(PointF location)
		{
			return new RectangleF(new PointF(0,0),DiagramSize).Contains(location);
		}

		[Browsable(false), Description("Determines whether the diagram contains this point.")]
		public bool Contains(PointF location, bool transparent)
		{
			return new RectangleF(new PointF(0,0),DiagramSize).Contains(location);
		}

		public bool Contains(PointF location, Margin margin)
		{
			return new RectangleF(margin.Left, margin.Top, DiagramSize.Width - margin.Right - margin.Left,DiagramSize.Height - margin.Bottom - margin.Top).Contains(location);
		}

		[Description("Causes the control to be re-rendered and redrawn.")]
		public new void Invalidate()
		{
			InvalidateImplementation(new Rectangle());
		}

		[Description("Causes the control to be re-rendered and redrawn.")]
		public virtual void Invalidate(Rectangle rectangle)
		{
			InvalidateImplementation(rectangle);
		}

		[Description("Allows a custom renderer to be set for the diagram.")]
		public virtual void SetRender(object render)
		{
			if (render == null) throw new ArgumentNullException("render","Render may not be null");
			if (!(render is IRender)) throw new ArgumentException("Object must implement IRender.");
			mRender = (IRender) render;
			mRender.Layers = Layers;
			
			//Create a renderlist if the shapes and lines have been set
			if (Shapes != null && Lines != null && !Suspended) GetRenderList(Render.RenderRectangle);
			Invalidate();
		}

		[Description("Suspends draw operations for a model object.")]
		public virtual void Suspend()
		{
			mSuspendCount += 1;
		}

		[Description("Suspends draw operations for a model object.")]
		public virtual void Resume()
		{
			mSuspendCount -= 1;
		}

		[Description("Resumes draw operations for a model object, forced if necessary.")]
		public virtual void Resume(bool Force)
		{
			if (Force)
			{
				mSuspendCount = 0;
			}
			else
			{
				mSuspendCount -= 1;
			}
		}

		[Description("Returns a diagram point from a mouse point.")]
		public virtual PointF PointToDiagram(int x, int y)
		{
			return TranslatePoint(x, y);
		}

		[Description("Returns a diagram point from a mouse point.")]
		public virtual PointF PointToDiagram(MouseEventArgs e)
		{
			return TranslatePoint(e.X, e.Y);
		}

		[Description("Returns a mouse point from a diagram point.")]
		public virtual Point DiagramToPoint(float x, float y)
		{
			return TranslateDiagram(x, y);
		}

		[Description("Returns a mouse point from a diagram point.")]
		public virtual Point DiagramToPoint(PointF point)
		{
			return TranslateDiagram(point.X, point.Y);
		}

		[Description("Returns a diagram rectangle from a control rectangle.")]
		public virtual RectangleF RectangleToDiagram(RectangleF rect)
		{
			return TranslateRectangle(rect);
		}

		[Description("Returns the top most element from this control point.")]
		public virtual Element ElementFromPoint(int x, int y)
		{
			return GetElement(TranslatePoint(x, y),mRenderList);
		}

		[Description("Returns the top most element from this control point.")]
		public virtual Element ElementFromPoint(MouseEventArgs e)
		{
			return GetElement(TranslatePoint(e.X, e.Y),mRenderList);
		}

		[Description("Returns the top most element from this diagram point.")]
		public virtual Element ElementFromDiagram(float x, float y)
		{
			return GetElement(new PointF(x, y),mRenderList);
		}

		[Description("Returns the top most element from this diagram point.")]
		public virtual Element ElementFromDiagram(PointF location)
		{
			return GetElement(location,mRenderList);
		}

		[Description("Returns an origin from a screen location for the specified line.")]
		public virtual Origin OriginFromPoint(Line line,int x, int y)
		{
			return line.OriginFromLocation(TranslatePoint(x,y));
		}

		[Description("Returns an origin from a screen location for the specified line.")]
		public virtual Origin OriginFromPoint(Line line, MouseEventArgs e)
		{
			return line.OriginFromLocation(TranslatePoint(e.X, e.Y));
		}

		[Description("Returns an origin from a diagram location for the specified line.")]
		public virtual Origin OriginFromDiagram(Line line, float x, float y)
		{
			return line.OriginFromLocation(new PointF(x, y));
		}

		public virtual void ScrollToElement(Element element)
		{
			PointF diagramPoint = new PointF(element.Rectangle.X + (element.Rectangle.Width /2),element.Rectangle.Y + (element.Rectangle.Height /2));
			ScrollToPoint(diagramPoint);
		}

		public virtual void ScrollToPoint(PointF point)
		{
			int ix = Convert.ToInt32((point.X * Render.ScaleFactor) - (Width / 2));
			int iy = Convert.ToInt32((point.Y * Render.ScaleFactor) - (Height / 2));

			//Offset for paging
			if (Paged) 
			{
				ix += RenderPaged.PagedOffset.X;
				iy += RenderPaged.PagedOffset.Y;
			}

			if (ix < 0) ix = 0;
			if (iy < 0) iy = 0;

			AutoScrollPosition = new Point(ix, iy);
		}

		//Scrolls the control to the diagram point specified
		public virtual void ScrollToPoint(float x, float y)
		{
			ScrollToPoint(new PointF(x, y));
		}

		[Description("Sets a shapes reference directly without an event handlers.")]
		protected virtual void SetShapes(Elements shapes)
		{
			mShapes = shapes;
		}

		[Description("Sets a lines reference directly without any event handlers.")]
		protected virtual void SetLines(Elements lines)
		{
			mLines = lines;
		}

		[Description("Sets the layers reference directly without any event handlers.")]
		protected virtual void SetLayers(Layers layers)
		{
			mLayers = layers;
			mRender.Layers = layers;
		}

		[Description("Sets the current renderlist.")]
		protected virtual void SetRenderList(RenderList renderList)
		{
			mRenderList = renderList;
			Render.Elements = renderList;
		}

		[Description("Sets the current diagram status")]
		protected internal virtual void SetStatus(Status status)
		{
			mStatus = status;
		}

		[Description("Sets the current mouse elements")]
		protected virtual void SetMouseElements(Diagram.MouseElements elements)
		{
			mMouseElements = elements;
		}

		//Event methods
		[Description("Raises the Serialize event.")]
		protected virtual void OnSerialize(IFormatter formatter, SurrogateSelector selector)
		{
			if (! SuspendEvents && Serialize != null) Serialize(this, new SerializationEventArgs(formatter,selector));
		}

		[Description("Raises the Deserialize event.")]
		protected virtual void OnDeserialize(IFormatter formatter, SurrogateSelector selector)
		{
			if (! SuspendEvents && Deserialize != null) Deserialize(this, new SerializationEventArgs(formatter,selector));
		}

		[Description("Raises the Serialize event.")]
		protected virtual void OnSerializeComplete(IFormatter formatter, SurrogateSelector selector)
		{
			if (! SuspendEvents && SerializeComplete != null) SerializeComplete(this, new SerializationEventArgs(formatter,selector));
		}

		[Description("Raises the DeserializeComplete event.")]
		protected virtual void OnDeserializeComplete(object graph, IFormatter formatter, SurrogateSelector selector)
		{
			if (! SuspendEvents && DeserializeComplete != null) DeserializeComplete(this, new SerializationCompleteEventArgs(graph,formatter,selector));
		}
		
		[Description("Raises the ElementMouseDown event.")]
		protected virtual void OnElementMouseDown(Element element,MouseEventArgs e)
		{
			if (! SuspendEvents && ElementMouseDown != null) ElementMouseDown(element,e);
		}

		[Description("Raises the ElementMouseUp event.")]
		protected virtual void OnElementMouseUp(Element element,MouseEventArgs e)
		{
			if (! SuspendEvents && ElementMouseUp != null) ElementMouseUp(element,e);
		}
		
		[Description("Raises the ElementClick event.")]
		protected virtual void OnElementClick(Element element)
		{
			if (! SuspendEvents && ElementClick != null) ElementClick(element,EventArgs.Empty);
		}

		[Description("Raises the ElementDoubleClick event.")]
		protected virtual void OnElementDoubleClick(Element element)
		{
			if (! SuspendEvents && ElementDoubleClick != null) ElementDoubleClick(element,EventArgs.Empty);
		}

		[Description("Raises the ElementEnter event.")]
		protected virtual void OnElementEnter(Element element)
		{
			if (! SuspendEvents && ElementEnter != null) ElementEnter(element,EventArgs.Empty);
		}

		[Description("Raises the ElementLeave event.")]
		protected virtual void OnElementLeave(Element element)
		{
			if (! SuspendEvents && ElementLeave != null) ElementLeave(element,EventArgs.Empty);
		}

		[Description("Raises the DiagramMouseDown event.")]
		protected virtual void OnDiagramMouseDown(MouseEventArgs e)
		{
			if (! SuspendEvents && DiagramMouseDown != null) DiagramMouseDown(this,e);
		}

		[Description("Raises the DiagramMouseUp event.")]
		protected virtual void OnDiagramMouseUp(MouseEventArgs e)
		{
			if (! SuspendEvents && DiagramMouseUp != null) DiagramMouseUp(this,e);
		}
		
		[Description("Raises the DiagramClick event.")]
		protected virtual void OnDiagramClick()
		{
			if (! SuspendEvents && DiagramClick != null) DiagramClick(this,EventArgs.Empty);
		}

		[Description("Raises the DiagramDoubleClick event.")]
		protected virtual void OnDiagramDoubleClick()
		{
			if (! SuspendEvents && DiagramDoubleClick != null) DiagramDoubleClick(this,EventArgs.Empty);
		}

		[Description("Raises the ElementInserted event.")]
		protected virtual void OnElementInserted(Element element)
		{
			if (! SuspendEvents && ElementInserted != null) ElementInserted(this,new ElementsEventArgs(element.Key,element));
		}

		[Description("Raises the ElementRemoved event.")]
		protected virtual void OnElementRemoved(Element element)
		{
			if (! SuspendEvents && ElementRemoved != null) ElementRemoved(this,new ElementsEventArgs(element.Key,element));
		}

		[Description("Raises the ElementRemoved event.")]
		protected virtual void OnElementInvalid(Element element)
		{
			if (! SuspendEvents && ElementInvalid != null) ElementInvalid(this,new ElementEventArgs(element));
		}
		
		[Description("Raises the ZoomChanged event.")]
		protected virtual void OnZoomChanged()
		{
			if (! SuspendEvents && ZoomChanged != null) ZoomChanged(this,EventArgs.Empty);
		}

		[Description("Raises the PagedChanged event.")]
		protected virtual void OnPagedChanged()
		{
			if (! SuspendEvents && PagedChanged != null) PagedChanged(this,EventArgs.Empty);
		}

		[Description("Raises the PagedChanged event.")]
		protected virtual void OnScroll()
		{
			if (! SuspendEvents && Scroll != null) Scroll(this,EventArgs.Empty);
		}
		
		#endregion

		#region Overrides

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		public override Color BackColor
		{
			get
			{
				return mRender.BackColor;
			}
			set
			{
				base.BackColor = value;
				mRender.BackColor = value;
				Invalidate();
			}
		}

		[Description("Forces the control to render and paint itself and it's child controls.")]
		public override void Refresh()
		{
			if (Suspended) return;
			GetRenderList(Render.RenderRectangle);
			mRender.RenderDiagram(Render.RenderRectangle);
			base.Refresh();
		}

		[Description("Gets or sets the minimum size of the auto-scroll."), RefreshProperties(RefreshProperties.All)]
		public new Size AutoScrollMinSize
		{
			get
			{
				return base.AutoScrollMinSize;
			}
			set
			{
				if (! value.Equals(base.AutoScrollMinSize)) SetPagedSettings();
			}
		}

		[Description("Gets or sets a value indicating whether the container will allow the user to scroll to any controls placed outside of its visible boundaries.")]
		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				if (value != base.AutoScroll)
				{
					base.AutoScroll = value;
				}
			}
		}
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new System.Drawing.Image BackgroundImage
		{
			get
			{
				return mRender.BackgroundImage;
			}
			set
			{
				mRender.BackgroundImage = value;

				mRender.RenderDiagram(mPageRect);
				DrawDiagram(mControlRect);
			}
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
            //Skip processing buttons after initial button
            if (mMouseElements.StartButton != MouseButtons.None && mMouseElements.StartButton != e.Button)
            {
                base.OnMouseDown(e);
                return;
            }

            SetStatus(Status.Updating);
            mMouseElements.MouseStartElement = ElementFromPoint(e);
            mMouseElements.StartButton = e.Button;

            //Check for origin 
            if (mMouseElements.MouseStartElement is Line)
            {
                Line line = (Line)mMouseElements.MouseStartElement;
                mMouseElements.MouseStartOrigin = OriginFromPoint(line, e);
            }

            if (mMouseElements.MouseStartElement != null)
            {
                OnElementMouseDown(mMouseElements.MouseStartElement, e);
            }
            else
            {
                OnDiagramMouseDown(e);
            }

			base.OnMouseDown (e);
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
            if (mMouseElements.StartButton != MouseButtons.None && mMouseElements.StartButton != e.Button)
            {
                base.OnMouseMove(e);
                return;
            }

			Element element = ElementFromPoint(e);
			if (element != null)
			{
				if (element != mMouseElements.MouseMoveElement) 
				{
                    if (element != null && mMouseElements.MouseMoveElement != null) OnElementLeave(mMouseElements.MouseMoveElement);
					mMouseElements.MouseMoveElement = element;
					OnElementEnter(element);
				}
			}
			else
			{
				if (element == null && mMouseElements.MouseMoveElement != null) 
				{
					mMouseElements.MouseMoveElement = null;
					OnElementLeave(mMouseElements.MouseMoveElement);
				}
			}
			
			base.OnMouseMove (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
            if (mMouseElements.StartButton != MouseButtons.None && mMouseElements.StartButton != e.Button)
            {
                base.OnMouseUp(e);
                return;
            }

            if (mMouseElements.MouseStartElement != null && mMouseElements.StartButton == e.Button)
            {
                OnElementMouseUp(mMouseElements.MouseStartElement, e);
            }
            else
            {
                OnDiagramMouseUp(e);
            }

            //Reset all mouse element references
            mMouseElements.Clear();
            SetStatus(Status.Default);

			base.OnMouseUp (e);
		}

		protected override void OnClick(EventArgs e)
		{
			if (mMouseElements.MouseStartElement != null && mMouseElements.MouseStartElement == mMouseElements.MouseMoveElement) 
			{
				OnElementClick(mMouseElements.MouseStartElement);
			}
			else
			{
				OnDiagramClick();
			}

			base.OnClick (e);
		}

		protected override void OnDoubleClick(EventArgs e)
		{
			//Commented out second clause due to non firing of event with high volume of elements
			if (mMouseElements.MouseStartElement != null) // && mMouseElements.MouseStartElement == mMouseElements.MouseMoveElement) 
			{
				OnElementDoubleClick(mMouseElements.MouseStartElement);
			}
			else
			{
				OnDiagramDoubleClick();
			}

			base.OnDoubleClick (e);
		}

		protected override void OnLayout(System.Windows.Forms.LayoutEventArgs levent)
		{
			base.OnLayout(levent);
			
			CheckDiagramSize();
			SetResizeRectangles();
			
			DrawDiagram(mControlRect);
		}

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e)
		{
			SetScrollRectangles();
			DrawDiagram(e.ClipRectangle);
			base.OnPaint(e);
		}

		//Do not call base implementation
		protected override void OnPaintBackground(System.Windows.Forms.PaintEventArgs pevent)
		{
			DrawDiagram(pevent.ClipRectangle);
		}

		//Detect end of acroll
		protected override void WndProc(ref Message m)
		{
			//Check for scroll message, with end scroll w param
			if (m.Msg == WM_VSCROLL || m.Msg == WM_HSCROLL)
			{
				if (m.WParam.ToInt32() == SB_ENDSCROLL)
				{
					//Get a sorted list and rerender and draw
					Refresh();
				}
			}
			base.WndProc (ref m);
		}

		
		#endregion

		#region Component Designer generated code

		private void InitializeComponent()
		{
			// 
			// Diagram
			// 
			this.Name = "Diagram";
		}

		#endregion

		#region Events

		//Occurs when an element is added to the elements collection
		private void Element_Insert(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;

			//Set the layer if not already set
			//Deserialized elements have layer information set
			if (element.Layer == null)
			{
				Layer layer = Layers.CurrentLayer;
				element.SetLayer(layer);

				//Add to the default level
				string key = layer.Elements.CreateKey();
				layer.Elements.SetModifiable(true);
				layer.Elements.Add(key, element); 
				layer.Elements.SetModifiable(false);

				//Set the layer key 
				element.SetLayerKey(key);

				//Set the element z order
				element.mZOrder = layer.Elements.Count;
				
				if (element is IUserInteractive)
				{
					IUserInteractive interactive = (IUserInteractive) element;
					if ((interactive.Interaction & UserInteraction.BringToFront) == UserInteraction.BringToFront) layer.Elements.BringToFront(element);
				}
			}

			//Set the container
			element.SetContainer(this);

			//Set handlers
			element.ElementInvalid +=new EventHandler(Element_ElementInvalid);

			//Draw the path if a line
			if (element is Line) 
			{
				Line line = (Line) element;
				
				//If a connector and is not auto routed and is not being deserialized then calculate points
				if (element is Connector)
				{
					Connector connector = (Connector) element;
					if (connector.Points == null) connector.CalculateRoute();
				}

				line.DrawPath();
			}

			//Set up route class if group
			if (element is Group)
			{
				Group group = (Group) element;
				group.Route = new Route();
				group.Route.Container = group;

				//Set the level for all the shapes in the group
				foreach (Element child in group.Shapes.Values)
				{
					child.SetLayer(Layers.CurrentLayer);
				}

				//Set the level for all the lines in the group
				foreach (Element child in group.Lines.Values)
				{
					child.SetLayer(Layers.CurrentLayer);
				}
			}

			//Set any containers for child elements
			if (element is IPortContainer)
			{
				IPortContainer container = (IPortContainer) element;
				
				foreach (Port port in container.Ports.Values)
				{
					port.SetContainer(this);
					port.SetLayer(element.Layer);
					if (port.Location.IsEmpty) container.LocatePort(port);
				}
			}

			//Set container and layers for children of compelx shape
			if (element is ComplexShape)
			{
				ComplexShape complex = (ComplexShape) element;

				foreach (Element child in complex.Children.Values)
				{
					child.SetContainer(this);
					child.SetLayer(Layers.CurrentLayer);
				}
			}

			//Set the height of the table
			if (element is Table)
			{
				Table table = (Table) element;
				table.SetHeight();
			}

			//Flag the routing terrain to be reformed
			if (Route != null) Route.Reform();

			//Re-render and redraw
			if (!Suspended) GetRenderList(Render.RenderRectangle);
			Invalidate();

			//Raise the ElementInserted event
			OnElementInserted(element);
		}

		//Occurs when an element is removed from the elements collection
		private void Element_Remove(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;

			//Remove handlers
			element.ElementInvalid -=new EventHandler(Element_ElementInvalid);

			//Remove the element from the level it is in
			element.Layer.Elements.SetModifiable(true);
			element.Layer.Elements.Remove(element.LayerKey);
			element.Layer.Elements.SetModifiable(false);

			if (element is Shape) ResetLines((Shape) element);

			//Remove origin handlers
			if (element is Line)
			{
				Line line = (Line) element;
				line.Start.RemoveHandlers();
				line.End.RemoveHandlers();
			}

			//Flag the routing terrain to be reformed
			if (Route != null) Route.Reform();

			//Re-render and redraw
			if (!Suspended) GetRenderList(Render.RenderRectangle);
			Invalidate();

			//Raise the ElementRemovedEvent
			OnElementRemoved(element);
		}

		//Occurs when an element raises an invalid event
		private void Element_ElementInvalid(object sender, EventArgs e)
		{
			Invalidate();
			OnElementInvalid((Element) sender);
		}

		//Remove all Shape references from layers and rerender
		private void mShapes_Cleared(object sender, EventArgs e)
		{
			Suspend();

			//Undo line references
			foreach (Line line in Lines.Values)
			{
				if (line.Start.DockedElement != null && line.Start.DockedElement is Shape) ResetLines((Shape) line.Start.DockedElement);
				if (line.End.DockedElement != null && line.End.DockedElement is Shape) ResetLines((Shape) line.End.DockedElement);
			}

			foreach (Layer layer in Layers)
			{
				layer.RemoveShapes();
			}

			//Flag the routing terrain to be reformed
			if (Route != null) Route.Reform();

			Resume();
			Refresh();
		}

		//Remove all Line references from layers and rerender
		private void mLines_Cleared(object sender, EventArgs e)
		{
			Suspend();
			foreach (Layer layer in Layers)
			{
				layer.RemoveLines();
			}
			Resume();
			Refresh();
		}

		#endregion

		#region Implementation

		public virtual object Clone()
		{
			return new Diagram(this);
		}

		private void ClearDiagram()
		{
			SuspendEvents = true;

			//Set default layers collection, need for shapes and lines
			Layers = new Layers();
			mRenderList = new RenderList();

			//Set up shapes and lines, needed for navigation
			Shapes = new Elements(typeof(Shape),"Shape");
			Lines = new Elements(typeof(Line),"Line");
			
			mNavigate = new Navigation.Navigate(this);
			Route = new Route();
			Route.Container = this;
			
			SuspendEvents = false;
		}

		private void SaveDiagram(Stream fs,IFormatter formatter)
		{
			SurrogateSelector selector = new SurrogateSelector();

			try 
			{
				selector.AddSurrogate(typeof(Diagram),new StreamingContext(StreamingContextStates.All), new DiagramSerialize());
				
				//Raise the Serialize event and allow subclasses to add their own surrogates
				OnSerialize(formatter,selector);
				
				formatter.SurrogateSelector = selector;
				formatter.Serialize(fs, this);
			}
			catch (Exception ex) 
			{
				if (ex.InnerException == null)
				{
					throw ex;
				}
				else
				{
					throw ex.InnerException;
				}
			}
			finally 
			{
				
			}

			OnSerializeComplete(formatter,selector);
		}

		private void LoadDiagram(Stream fs, IFormatter formatter)
		{
			Diagram diagram;
			SurrogateSelector selector = new SurrogateSelector();
			DiagramSerialize surrogate = new DiagramSerialize();

			try 
			{
				selector.AddSurrogate(typeof(Diagram),new StreamingContext(StreamingContextStates.All), surrogate);

				//Raise the deserialize event and allow subclasses to add/change the surrogate to their own surrogate
				OnDeserialize(formatter,selector);

				formatter.SurrogateSelector = selector;
				formatter.Binder = Component.Instance.DefaultBinder;
			
				diagram = (Diagram) formatter.Deserialize(fs);
			}
			catch (Exception ex) 
			{
				if (ex.InnerException == null)
				{
					throw ex;
				}
				else
				{
					throw ex.InnerException;
				}
			}
			finally 
			{
				
			}

			surrogate = Serialization.Serialize.GetSurrogate(diagram,selector);
			if (surrogate == null) throw new Exception("A deserialization surrogate could not be found.");

			//Update this object using the surrogate and the diagram
			surrogate.UpdateObjectReferences();

			SuspendEvents = true;
			Suspend();
			
			//Copy settings from deserialized object
			DiagramSize = diagram.DiagramSize;
			Zoom = diagram.Zoom;
			ShowTooltips = diagram.ShowTooltips;
			CheckBounds = diagram.CheckBounds;
			Paged = diagram.Paged;
			WorkspaceColor = diagram.WorkspaceColor;
			
			//Copy all layers across
			Layers.Clear();
			foreach (Layer layer in surrogate.Layers)
			{
				Layers.Add(layer);
			}
			Layers.CurrentLayer = surrogate.Layers.CurrentLayer;

			//Copy shapes and lines accross
			Shapes.Clear();
			foreach (Shape shape in surrogate.Shapes.Values)
			{
				Shapes.Add(shape.Key,shape);
			}

			Lines.Clear();
			foreach (Line line in surrogate.Lines.Values)
			{
				Lines.Add(line.Key,line);
			}

			mNavigate = new Navigation.Navigate(this);
			Route = new Route();
			Route.Container = this;
			Margin = new Margin(); //##margin needs to be serialized/deserialized

			//Raise the deserialize complete event
			OnDeserializeComplete(diagram, formatter, selector);

			Resume();
			SuspendEvents = false;

			Refresh();
		}
		
		private void InvalidateImplementation(Rectangle rectangle)
		{
			//Exit if suspended
			if (Suspended) return;

			//Render and draw
			if (Render != null)
			{
				if (rectangle.IsEmpty) rectangle = Render.RenderRectangle;
				mRender.RenderDiagram(rectangle);
				DrawDiagram(mControlRect);
			}
		}

		protected void DrawDiagram(Rectangle clipRectangle)
		{
			//if (Suspended) return;
			if (mRender == null) return;
			if (mRender.Bitmap == null) return;
			if (clipRectangle.IsEmpty) return;

			RectangleF sourceRectF = new RectangleF();
	
			//Set up the source rectangle from the rendered backbuffer
			sourceRectF.X = mPageRect.X + clipRectangle.X - mRender.RenderRectangle.X;
			sourceRectF.Y = mPageRect.Y + clipRectangle.Y - mRender.RenderRectangle.Y;
			sourceRectF.Width = clipRectangle.Width;
			sourceRectF.Height = clipRectangle.Height;

			Graphics graphics = base.CreateGraphics();

			try
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
	
				//Draw image unscaled a lot faster then drawimage, so try use
				if (sourceRectF.X == 0 && sourceRectF.Y == 0 && sourceRectF.Width == mControlRect.Width && sourceRectF.Height == mControlRect.Height)
				{
					graphics.DrawImageUnscaled(mRender.Bitmap, new Point(0,0));
				}
				else
				{
					graphics.DrawImage(mRender.Bitmap, clipRectangle.X, clipRectangle.Y, sourceRectF, GraphicsUnit.Pixel); //##in use during hectic movement
				}

				graphics.Dispose();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error drawing diagram " + ex.ToString());
			}
			finally
			{
				if (graphics != null) graphics.Dispose();
			}
		}
		
		private void CheckDiagramSize()
		{
			if (mRender == null) return;
			int width = mDiagramSize.Width;
			int height = mDiagramSize.Height;

			//If page mode, make sure autoscroll size is diagram size
			if (Paged)  
			{
				//Scale the width and height depending on the zoom
				width = Convert.ToInt32(width * Render.ScaleFactor);
				height = Convert.ToInt32(height * Render.ScaleFactor);
				
				//Add 20 pixels on each side of the diagram to give paged look
				width = Convert.ToInt32(width + Component.Instance.PagePadding.Width);
				height = Convert.ToInt32(height + Component.Instance.PagePadding.Height);
			}
			//Make diagram size at least control size
			else
			{
				if (width < this.Width) width = this.Width;
				if (height < this.Height) height = this.Height;
			
				DiagramSize = Size.Round(new SizeF(width, height));

				//Scale down by zoom for autoscroll
				width = Convert.ToInt32(width * Zoom / 100);
				height = Convert.ToInt32(height * Zoom / 100);
			}
			
			//Call base sub to prevent recursive loop
			if (base.AutoScroll) base.AutoScrollMinSize = new Size(width, height);			
		}

		private void SetResizeRectangles()
		{
			if (mRender == null) return;

			//Sets up the diagram size in the renderer
			Render.DiagramSize = DiagramSize;
			
			//Set up workspace size
			SetPagedSettings();

			Rectangle renderRect;

			int x = 0;
			int y = 0;
			int width = 0;
			int height = 0;

			//Get the scroll offsets as positive values
			//Catch errors in vs 2005 beta 2
			try
			{
				x = Math.Abs(this.DisplayRectangle.X);
				y = Math.Abs(this.DisplayRectangle.Y);
			}
			catch
			{
				
			}
			
			width = this.Width;
			height = this.Height;

			//check for resize
			if (Paged || (width > mRender.RenderRectangle.Width || height > mRender.RenderRectangle.Height))
			{
				//Calculate the zoom width and height
				int scaleWidth = Convert.ToInt32(mDiagramSize.Width * Render.ScaleFactor);
				int scaleHeight = Convert.ToInt32(mDiagramSize.Height * Render.ScaleFactor);

				//Add page offsets
				if (Paged)
				{
					scaleWidth += Convert.ToInt32(Component.Instance.PagePadding.Width); 
					scaleHeight += Convert.ToInt32(Component.Instance.PagePadding.Height);
				}

				if (x + width > scaleWidth) width -= x + width - scaleWidth;
				if (y + height > scaleHeight) height -= y + height - scaleHeight;

				//Adjust for zoomed off areas
				if (width * Render.ScaleFactor < this.Width) width = Convert.ToInt32(this.Width * Render.ZoomFactor);
				if (height * Render.ScaleFactor < this.Height) height = Convert.ToInt32(this.Height * Render.ZoomFactor);

				//Set the new draw and render rectangles
				renderRect = new Rectangle(x, y, width, height);

				//Select the new render lists
				if (!Suspended) 
				{
					GetRenderList(renderRect);
					mRender.RenderDiagram(renderRect);
				}
			}

			mControlRect = new Rectangle(0, 0, this.Width, this.Height);
		}

		internal bool SetScrollRectangles()
		{
			//During deserialization the DisplayRectangle property may throw an error
			//under .net 2.0. Check the parent instead of raising an exception.
			if (Parent == null) return false;
			
			Rectangle renderRect;

			int x = 0;
			int y = 0;
			int width = 0;
			int height = 0;

			bool returnFlag = false;

			x = Math.Abs(this.DisplayRectangle.X);
			y = Math.Abs(this.DisplayRectangle.Y);
			width = this.Width;
			height = this.Height;

			//Adjust for scrollbars
			//if (this.VScroll) width -= vScrollWidth;
			//if (this.HScroll) height -= hScrollWidth;
			
			Rectangle displayRect = new Rectangle(x, y, width, height);

			//check for scroll by comparing last control display rectangle to the current one
			if (x != mPageRect.X || y != mPageRect.Y)
			{
				returnFlag = true;

				//Raise the scroll event
				OnScroll();

				//Set up a new render rectangle if the current render rectangle doesnt contain the display rect
				//Use the last rendered rectangle to check against
				if (! (mRender.RenderRectangle.Contains(displayRect)))
				{
					//if we are scrolling positive on the x or y axis then double that axis display width render
					if (x > mPageRect.X) width *= 3;
					if (y > mPageRect.Y) height *= 3;

					//If we scroll negative on an axis, pull the render back by x by the display width and double
					if (x < mPageRect.X)
					{
						x -= width;
						width *= 3;
						if (x < 0)
						{
							width += x;
							x = 0;
						}
					}
					if (y < mPageRect.Y)
					{
						y -= height;
						height *= 3;
						if (y < 0)
						{
							height += y;
							y = 0;
						}
					}

					//Calculate the zoom width and height
					int scaleWidth = Convert.ToInt32(mDiagramSize.Width * Render.ScaleFactor);
					int scaleHeight = Convert.ToInt32(mDiagramSize.Height * Render.ScaleFactor);

					//Add page offsets
					if (Paged)
					{
						scaleWidth += Convert.ToInt32(Component.Instance.PagePadding.Width); 
						scaleHeight += Convert.ToInt32(Component.Instance.PagePadding.Height);
					}

					if (x + width > scaleWidth) width -= x + width - scaleWidth;
					if (y + height > scaleHeight) height -= y + height - scaleHeight;

					//Adjust for zoomed off areas
					if (width * Render.ScaleFactor < this.Width) width = Convert.ToInt32(this.Width * Render.ZoomFactor);
					if (height * Render.ScaleFactor < this.Height) height = Convert.ToInt32(this.Height * Render.ZoomFactor);

					//Set the new draw and render rectangles
					renderRect = new Rectangle(x, y, width, height);

					//Select the new render lists
					if (Component.Instance.SmoothScrolling)
					{
						CreateScrollRenderList(renderRect, true);
					}
					else
					{
						CreateRenderList(renderRect, true);
					}

					Render.Elements = RenderList;
					mRender.RenderDiagram(renderRect);
				}
			}

			//Set the page rectangle to the display area for refreshes
			mPageRect = displayRect;
			mControlRect = new Rectangle(0, 0, width, height);

			return returnFlag;
		}

		//Create a render list for shapes within a rectangle
		protected internal void GetRenderList(RectangleF rect)
		{
			CreateRenderList(rect, true);
			Render.Elements = RenderList;
		}

		public virtual void CreateRenderList(RectangleF rect, bool sort)
		{
			RectangleF zoomRect = TranslateRectangle(rect);
			RenderList renderList = RenderList;
			renderList.Clear();

			//Check for intersections with the zoomed rectangle;
			foreach (Shape shape in mShapes.Values)
			{
				if (shape.Selected || shape.Rectangle.IntersectsWith(zoomRect))
				{
					renderList.Add(shape);
				}
			}
			foreach (Line line in mLines.Values)
			{
				if (line.Selected || line.Rectangle.IntersectsWith(zoomRect))
				{
					renderList.Add(line);
				}
			}
			if (sort) renderList.Sort();
		}

		private void CreateScrollRenderList(RectangleF rect, bool sort)
		{
			RectangleF zoomRect = TranslateRectangle(rect);

			//Get a local reference and clear the renderlist
			RenderList newList = new RenderList();
			Hashtable additional = new Hashtable();

			//Check all elements from the previous render that intersect with the new rectangle
			foreach (Element element in mRenderList)
			{
				if (element.Rectangle.IntersectsWith(zoomRect))
				{
					if (element is Line)
					{
						Line line = (Line) element;

						//Add connected shapes to the list of possible additions
						if (line.Start.DockedElement != null && !additional.ContainsKey(line.Start.DockedElement.Key)) additional.Add(line.Start.DockedElement.Key, line.Start.DockedElement);
						if (line.End.DockedElement != null && !additional.ContainsKey(line.End.DockedElement.Key)) additional.Add(line.End.DockedElement.Key, line.End.DockedElement);

						newList.Add(element);
					}
					else if (element is Shape && !additional.ContainsKey(element.Key))
					{
						newList.Add(element);
					}
				}
			}

			//Add additional shapes from connected lines
			foreach (Shape shape in additional.Values)
			{
				if (shape.Rectangle.IntersectsWith(zoomRect))
				{
					newList.Add(shape);
				}
			}

			if (sort) newList.Sort();
			SetRenderList(newList);
		}

		private PointF TranslatePoint(Point point)
		{
			return TranslatePoint(point.X,point.Y);
		}

		//Returns a diagram point from the screen
		private PointF TranslatePoint(int x, int y)
		{
			float zoom = mRender.ZoomFactor;
			PointF pageOffset = new PointF(0,0);

			if (Paged) pageOffset = RenderPaged.PagedOffset;

			return new PointF(((x - DisplayRectangle.X - pageOffset.X ) * zoom), ((y - DisplayRectangle.Y - pageOffset.Y) * zoom));
		}

		//Returns a control point from a diagram point
		private Point TranslateDiagram(float x, float y)
		{
			float zoom = mRender.ZoomFactor;
			PointF pageOffset = new PointF(0,0);
			if (Paged) pageOffset = RenderPaged.PagedOffset;

			return Point.Round(new PointF((x / zoom) + DisplayRectangle.X + pageOffset.X, (y / zoom) + DisplayRectangle.Y + pageOffset.Y));
		}

		//Zooms a standard rectangle
		private RectangleF TranslateRectangle(RectangleF rect)
		{
			float zoom = mRender.ZoomFactor;
			PointF pageOffset = new PointF(0,0);

			if (Paged) pageOffset = RenderPaged.PagedOffset;
		
			return new RectangleF((rect.X - pageOffset.X) * zoom, (rect.Y - pageOffset.Y) * zoom, rect.Width * zoom, rect.Height * zoom);
		}

		//Returns the top element from a diagram point
		private Element GetElement(PointF location, RenderList renderlist)
		{
			Element bestElement = null;
			Port bestPort = null;
			Layer currentLayer = Layers.CurrentLayer;

			if (!currentLayer.Visible) return null;

			foreach (Element element in renderlist)
			{
				if (element.Layer == currentLayer)
				{
					//Selection Handles always beat anything else
					if (element is ISelectable)
					{
						ISelectable selectable = (ISelectable) element;

						if (selectable.Selected && element.Handles != null) 
						{
							PointF local = new PointF(location.X - element.Rectangle.X, location.Y - element.Rectangle.Y);

							foreach (Handle handle in element.Handles)
							{
								if (handle.Path.IsVisible(local)) return element;
							}
						}
					}
				
					//Check for ports
					if (element is IPortContainer)
					{		
						IPortContainer container = (IPortContainer) element;
						foreach (Port port in container.Ports.Values)
						{
							if (port.Visible && port.Contains(location))
							{
								if (bestElement == null || element.ZOrder < bestElement.ZOrder) 
								{
									bestElement = element;	
									bestPort = port;
								}
							}
						}
					}

					//Check element by calling Contains method which is overriden in most classes
					if (element.Visible && element.Contains(location))
					{
						if (bestElement == null) bestElement = element;
						if (element.ZOrder < bestElement.ZOrder) 
						{
							bestElement = element;
							bestPort = null;
						}
					}
				}
			}

			//Get child element from container, calling this function recursively
			if (bestElement is IContainer)
			{
				IExpandable expand = null;
				
				if (bestElement is IExpandable) expand = (IExpandable) bestElement;
				
				if (expand == null || expand.Expanded)
				{
					IContainer container = (IContainer) bestElement;
					PointF containerLocation = location;
					Element bestContainerElement;

					containerLocation.X -= container.Offset.X;
					containerLocation.Y -= container.Offset.Y;
					
					bestContainerElement = GetElement(containerLocation,container.RenderList);
					if (bestContainerElement != null) return bestContainerElement;
				}
			}

			if (bestPort == null)
			{
				return bestElement;
			}
			else
			{
				return bestPort;
			}
		}

		private void ResetLines(Shape shape)
		{
			Suspend();

			//Loop through each line and create a remove list
			foreach (Line line in Lines.Values)
			{
				if (line.Start.DockedElement != null && line.Start.DockedElement == shape) line.Start.Location = line.FirstPoint;
				if (line.End.DockedElement != null && line.End.DockedElement == shape) line.End.Location = line.LastPoint;
			}

			Resume();
		}

		private void SetPagedSettings()
		{
			if (Paged) 
			{
				if (AutoScroll)
				{
					int width;
					int height;

					width = (AutoScrollMinSize.Width < Width) ? Width : AutoScrollMinSize.Width;
					height = (AutoScrollMinSize.Height < Height) ? Height : AutoScrollMinSize.Height;

					RenderPaged.WorkspaceSize = new Size(width,height);
				}
				else
				{
					RenderPaged.WorkspaceSize = Size;
				}

				//Set up paged size
				RenderPaged.PagedSize = new SizeF(DiagramSize.Width * Render.ScaleFactor,DiagramSize.Height * Render.ScaleFactor);
				
				//Set up paged offset
				int offsetX = Convert.ToInt32((RenderPaged.WorkspaceSize.Width - RenderPaged.PagedSize.Width) / 2);
				int offsetY = Convert.ToInt32((RenderPaged.WorkspaceSize.Height - RenderPaged.PagedSize.Height) / 2);

				if (offsetX < 0) offsetX = 0;
				if (offsetY < 0) offsetY = 0;

				RenderPaged.PagedOffset = new Point(offsetX,offsetY);

				OnPagedChanged();
			}
		}

		//Save the diagram to disk or to a stream
		protected virtual void ExportToSvg(Stream stream)
		{
			SvgDocument svg = new SvgDocument();
			svg.AddDiagram(this);
			svg.Save(stream);
		}

		protected virtual void ExportToMetafile(FileStream stream, string path)
		{
			Metafile metafile = new Metafile();
			metafile.AddDiagram(this);

			if (stream != null)
			{
				metafile.Save(stream);
			}
			else
			{
				metafile.Save(path);
			}
		}

		protected virtual void ExportToPicture(Stream stream, ImageFormat format)
		{
			//Set the renderlists to the whole diagram
			Rectangle previous = Render.RenderRectangle;
			float zoom = Render.Zoom;

			Rectangle renderRect = new Rectangle(new Point(0,0),DiagramSize);
			GetRenderList(renderRect);

			//Get the bounds of the renderlist
			if (Component.Instance.ClipExport)
			{
				renderRect = Rectangle.Round(RenderList.GetBounds());
				renderRect.Inflate(20,20);

				if (renderRect.X < 0) renderRect.X = 0;
				if (renderRect.Y < 0) renderRect.Y = 0;
			}

			//Set the render rectangle
			Render.Zoom = 100;
			Render.RenderRectangle = renderRect;
			Render.RenderDiagram(renderRect);
			
			Render.Bitmap.Save(stream, format);

			//Reset the render
			Render.RenderRectangle = previous;
			GetRenderList(previous);
			Render.Zoom = zoom;
			Render.RenderDiagram(previous);
		}

		protected void SaveStatus()
		{
			mSaveStatus = mStatus;
		}

		protected void RestoreStatus()
		{
			SetStatus(mSaveStatus);
		}

		#endregion
	}
}
