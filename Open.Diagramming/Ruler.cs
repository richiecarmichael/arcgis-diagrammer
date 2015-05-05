using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Data;
using System.Windows.Forms;

namespace Crainiate.Diagramming.Editing
{
	[ToolboxBitmap(typeof(EditingResources), "Resource.ruler.bmp")]
	public class Ruler : System.Windows.Forms.Control, IMessageFilter	
	{
		//Property variables
		private Bitmap mRenderBitmap = null;
		private float mMajor;
		private float mMinor;
		private float mMid;
		private float mStart;
		private float mPadding;
		private RulerUnit mUnit;
		private Font mFont;
		private bool mMouseTracking;
		private float mZoom;
		private RulerOrientation mOrientation;
		private float mMargin;
		private RulerBorderStyle mBorderStyle;
		private Color mGradientColor;
		
		private bool mDrawGuides;
		private RectangleF[] mGuides;

		private Diagram mDiagram;

		//Working variables
		private System.ComponentModel.Container components = null;
		private float mScaleFactor;
		private bool mSuspended;
		private PointF mUnitScaleFactor;

		private const int WM_MOUSEMOVE = 0x0200;

		#region Interface

		//Constructors
		public Ruler()
		{
			InitializeComponent();
			Component.Instance.GetLicense(typeof(Ruler), this);
			mFont = new Font("Microsoft Sans Serif",7.0F);
			mUnit = RulerUnit.Pixel;
			mMajor = 100;
			mMinor = 10;
			mMid = 50;
			mStart = 0;
			mZoom = 100;
			mScaleFactor = 1;
			mUnitScaleFactor = new PointF(1,1);
			mOrientation = RulerOrientation.Top;
			mBorderStyle = RulerBorderStyle.Edge;
			mDrawGuides = true;
			mGradientColor = BackColor;
		}

		//Properties
		//Major divisions
		[Category("Behaviour"), Description("Sets or retrieves the major increment in units for the ruler.")]
		public virtual float Major
		{
			get
			{
				return mMajor;
			}
			set
			{
				if (value != mMajor)
				{
					mMajor = value;
					Refresh();
				}
			}
		}

		//Minor divisions
		[Category("Behaviour"), Description("Sets or retrieves the minor increment in units for the ruler.")]
		public virtual float Minor
		{
			get
			{
				return mMinor;
			}
			set
			{
				if (value != mMinor)
				{
					mMinor = value;
					Refresh();
				}
			}
		}

		//Middle divisions
		[Category("Behaviour"), Description("Sets or retrieves the middle increment in units for the ruler.")]
		public virtual float Mid
		{
			get
			{
				return mMid;
			}
			set
			{
				if (value != mMid)
				{
					mMid = value;
					Refresh();
				}
			}
		}

		//Start number
		[Category("Behaviour"), DefaultValue(0), Description("Sets or gets the starting value in units for the ruler.")]
		public virtual float Start
		{
			get
			{
				return mStart;
			}
			set
			{
				if (value != mStart)
				{
					mStart = value;
					Refresh();
				}
			}
		}

		//Padding offset before start of measured area
		[Category("Appearance"), DefaultValue(0F), Description("Sets the distance a ruler beings from the start of the control.")]
		public virtual float Padding
		{
			get
			{
				return mPadding;
			}
			set
			{
				if (value != mPadding)
				{
					mPadding = value;
					Refresh();
				}
			}
		}

		[Category("Appearance"), DefaultValue(0F), Description("Sets the distance from the padding to the origin of the ruler.")]
		public virtual float Margin
		{
			get
			{
				return mMargin;
			}
			set
			{
				if (value != mMargin)
				{
					mMargin = value;
					Refresh();
				}
			}
		}

		[Category("Behaviour"),DefaultValue(RulerUnit.Pixel), Description("Sets or retrieves units used for measurement for the ruler.")]
		public virtual RulerUnit Units
		{
			get
			{
				return mUnit;
			}
			set
			{
				mUnit = value;
				SetDefaultUnitValues();
				SetUnitScaleFactors();
				Refresh();
			}
		}

		[Category("Appearance"),DefaultValue(RulerBorderStyle.None), Description("Sets or retrieves a value determining how the ruler border is drawn.")]
		public virtual RulerBorderStyle BorderStyle
		{
			get
			{
				return mBorderStyle;
			}
			set
			{
				mBorderStyle = value;
				Refresh();
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to render the background gradient.")]
		public virtual Color GradientColor
		{
			get
			{
				return mGradientColor;
			}
			set
			{
				mGradientColor = value;
				Refresh();
			}
		}

		[Category("Behaviour"),DefaultValue(false), Description("Determines whether the mouse position is tracked on the ruler.")]
		public virtual bool MouseTracking
		{
			get
			{
				return mMouseTracking;
			}
			set
			{
				if (mMouseTracking != value)
				{
					mMouseTracking = value;
					if (mMouseTracking)
					{
						Application.AddMessageFilter(this);
					}
					else
					{
						Application.RemoveMessageFilter(this);
					}
				}
			}
		}

		[Category("Behaviour"),DefaultValue(true), Description("When enabled draws shape tracking highlights on the ruler.")]
		public virtual bool DrawGuides
		{
			get
			{
				return mDrawGuides;
			}
			set
			{
				if (mDrawGuides != value)
				{
					mDrawGuides = value;
					Refresh();
				}
			}
		}
		
		[Category("Behavior"), DefaultValue(100F), Description("Sets or retrieves the current zoom level as a percentage.")]
		public virtual float Zoom
		{
			get
			{
				return mZoom;
			}
			set
			{
				if (value != mZoom)
				{
					mZoom = value;
					mScaleFactor = Convert.ToSingle(value / 100);
					Refresh();
				}
			}
		}

		[Category("Behavior"), DefaultValue(RulerOrientation.Top), Description("Determines the orientation of the ruler.")]
		public virtual RulerOrientation Orientation
		{
			get
			{
				return mOrientation;
			}
			set
			{
				if (mOrientation != value)
				{
					mOrientation = value;
					Refresh();
				}
			}
		}

		[Browsable(false), Category("Data"), Description("Retrieves a boolean value determining whether render and draw operations are suspended.")]
		public virtual bool Suspended
		{
			get
			{
				return mSuspended;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Sets or gets the diagram this ruler is measuring.")]
		public virtual Diagram Diagram
		{
			get
			{
				return mDiagram;
			}
			set
			{
				if (mDiagram != null)
				{
					mDiagram.ZoomChanged -=new EventHandler(Diagram_ZoomChanged);
					mDiagram.PagedChanged -= new EventHandler(Diagram_PagedChanged);
					mDiagram.Scroll -= new EventHandler(Diagram_Scroll);
		
					if (mDiagram is Model)
					{
						Model model = (Model) mDiagram;
			
						model.Moving -= new UserActionEventHandler(Model_UserAction);
						model.Scaling -= new UserActionEventHandler(Model_UserAction);
						model.Rotating -= new UserActionEventHandler(Model_UserAction);
						model.UpdateActions -= new UserActionEventHandler(Model_UpdateActions);
					}
				}
				
				mDiagram = value;
				
				if (mDiagram != null)
				{
					mDiagram.ZoomChanged +=new EventHandler(Diagram_ZoomChanged);
					mDiagram.PagedChanged += new EventHandler(Diagram_PagedChanged);
					mDiagram.Scroll += new EventHandler(Diagram_Scroll);
		
					if (mDiagram is Model)
					{
						Model model = (Model) mDiagram;
			
						model.Moving += new UserActionEventHandler(Model_UserAction);
						model.Scaling += new UserActionEventHandler(Model_UserAction);
						model.Rotating += new UserActionEventHandler(Model_UserAction);
						model.UpdateActions += new UserActionEventHandler(Model_UpdateActions);
					}
				}
			}
		}

		[Browsable(false), Category("Data"), Description("Returns an array of rectangles describing the areas on the ruler to be highlighted.")]
		protected virtual RectangleF[] Guides
		{
			get
			{
				return mGuides;
			}
		}

		//Methods
		[Description("Suspends draw operations for the ruler.")]
		public virtual void Suspend()
		{
			mSuspended = true;
		}

		[Description("Resumes draw operations for the ruler.")]
		public virtual void Resume()
		{
			mSuspended = false;
		}

		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if( components != null )
					components.Dispose();
			}
			base.Dispose( disposing );
		}

		protected virtual void SetGuides(RectangleF[] rectangles)
		{
			mGuides = rectangles;
		}

		#endregion

		#region Component Designer generated code
		
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
		}
		
		#endregion

		#region Events

		private void Diagram_ZoomChanged(object sender, EventArgs e)
		{
			Zoom = Diagram.Zoom;
		}

		private void Diagram_PagedChanged(object sender, EventArgs e)
		{
			if (Orientation == RulerOrientation.Top)
			{
				Margin = Diagram.RenderPaged.PagedOffset.X;
			}
			else
			{
				Margin = Diagram.RenderPaged.PagedOffset.Y;					
			}
		}
		
		private void Diagram_Scroll(object sender, EventArgs e)
		{
			Refresh();
		}

		private void Model_UserAction(object sender, UserActionEventArgs e)
		{
			if (DrawGuides)
			{
				CalculateGuides(e.Actions);
				Refresh();
			}
		}

		private void Model_UpdateActions(object sender, UserActionEventArgs e)
		{
			SetGuides(null);
			Refresh();
		}


		#endregion

		#region Overrides

		public override Color BackColor
		{
			get
			{
				return base.BackColor;
			}
			set
			{
				base.BackColor = value;
				Refresh();
			}
		}

		public override Color ForeColor
		{
			get
			{
				return base.ForeColor;
			}
			set
			{
				base.ForeColor = value;
				Refresh();
			}
		}
		
		protected override void OnPaint(PaintEventArgs pe)
		{
			if (mRenderBitmap == null) return;

			pe.Graphics.DrawImageUnscaled(mRenderBitmap,new Point(0,0));	

			//Draw the mouse movement indicator
			if (MouseTracking)
			{
				//get mouse position in screen co-ordinates and convert to control
				Point mousePoint = PointToClient(Control.MousePosition);
				
				if (Orientation == RulerOrientation.Top)
				{
					if (mousePoint.X >= Padding && mousePoint.X < Width) 
					{
						pe.Graphics.DrawLine(Component.Instance.HighlightPen,mousePoint.X,0,mousePoint.X,Height);
					}
				}
				else
				{
					if (mousePoint.Y >= Padding && mousePoint.Y < Height) 
					{
						pe.Graphics.DrawLine(Component.Instance.HighlightPen,0,mousePoint.Y,Width,mousePoint.Y);
					}
				}
			}

			base.OnPaint(pe);
		}

		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			if (mRenderBitmap == null) return;

			pevent.Graphics.DrawImageUnscaled(mRenderBitmap,new Point(0,0));
		}

		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);
			
			if (DisplayRectangle.Width <=0 || DisplayRectangle.Height <= 0) return;

			mRenderBitmap = new Bitmap(DisplayRectangle.Width,DisplayRectangle.Height,PixelFormat.Format32bppPArgb);
			Refresh();
		}

		public override void Refresh()
		{
			UpdateBuffer();
			base.Refresh();
		}
		
		#endregion

		#region Implementation

		public bool PreFilterMessage(ref Message m)
		{
			if (m.Msg == WM_MOUSEMOVE)
			{
				base.Refresh();
			}
			return false;
		}

		private void UpdateFromDiagram()
		{
			Suspend();
			Zoom = mDiagram.Zoom;
			
			if (Orientation == RulerOrientation.Top)
			{
				Margin = mDiagram.RenderPaged.PagedOffset.X;
			}
			else
			{
				Margin = mDiagram.RenderPaged.PagedOffset.Y;					
			}

			Resume();
			Refresh();
		}

		private void UpdateBuffer()
		{
			if (mRenderBitmap == null) return;
			if (Suspended) return;

			Graphics graphics = Graphics.FromImage(mRenderBitmap);
			graphics.Clear(BackColor);

			if (Orientation == RulerOrientation.Top)
			{
				RenderRuler(graphics);
			}
			else
			{
				RenderRulerVertical(graphics);
			}
			graphics.Dispose();
		}

		private void RenderRuler(Graphics graphics)
		{
			float position;
			float endPosition;
			
			float increment = Minor * mScaleFactor * mUnitScaleFactor.X; //Get the scaled ruler increments

			float third = Convert.ToSingle(Height * 0.7); //The size of the marks on the ruler
			float twoThirds = Convert.ToSingle(Height * 0.3);
			float full = Convert.ToSingle(Height * 0.1);
			float height = Height;
			float onePixel = 1;

			Pen pen = new Pen(ForeColor,-1);
			SolidBrush brush = new SolidBrush(ForeColor);
			
			//Set up a gradient brush size that does not contain zeros
			Size size = Size;
			if (size.Width == 0) size.Width = 1;
			if (size.Height == 0) size.Height = 1;

			//Draw background
			LinearGradientBrush gradientBrush = new LinearGradientBrush(new RectangleF(new PointF(0,0),size),BackColor,GradientColor,System.Drawing.Drawing2D.LinearGradientMode.Vertical);
			graphics.FillRectangle(gradientBrush, new RectangleF(new PointF(0,0),size));

			//Calculate end width
			endPosition = Width / mScaleFactor / mUnitScaleFactor.X;

			//Translate for scroll, add onto the width if scrolled 
			if (mDiagram != null)
			{
				graphics.TranslateTransform(mDiagram.AutoScrollPosition.X,0);
				endPosition -= mDiagram.AutoScrollPosition.X; //Scroll position is a negative value
			}
			
			//Translate for padding. Padding is always in pixels
			graphics.TranslateTransform(Padding,0);
			
			//Translate for margin. Margin is always in pixels
			graphics.TranslateTransform(mMargin,0);

			//Translate back in perfect multiples of the minor increment.
			graphics.TranslateTransform(-Convert.ToInt32(mMargin / increment) * increment,0);

			//Set the initial position value
			int temp = Convert.ToInt32((-mMargin / mScaleFactor / mUnitScaleFactor.X) / Minor);
			position = temp * Minor;
			
			//Draw a minor, major or mid mark, incrementing by minor
			while (position < endPosition)
			{
				if (position % Major == 0)
				{	
					//Draw full line
					graphics.DrawLine(pen,0,height,0,full);

					//Draw string
					string number = position.ToString();
					if (position == 0) number += "  " + Abbreviate(Units);

					graphics.DrawString(number,mFont,brush,0,- onePixel);
				}
				else if (position % Mid == 0)
				{
					graphics.DrawLine(pen,0,height,0,twoThirds);
				}
				else
				{
					graphics.DrawLine(pen,0,height,0,third);
				}

				//Translate by the minor increment and increment position
				graphics.TranslateTransform(increment,0);
				position += Minor;
			}

			graphics.ResetTransform();
			if (mDiagram != null) graphics.TranslateTransform(mDiagram.AutoScrollPosition.X,0);
			
			//Draw highlights
			if (DrawGuides && mGuides != null)
			{
				brush = new SolidBrush(Color.FromArgb(16,Component.Instance.HighlightBrush.Color));
				Pen pen2 = new Pen(Color.FromArgb(255,Component.Instance.HighlightPen.Color), 1);
							
				foreach (RectangleF rect in Guides)
				{
					RectangleF fill = new RectangleF((rect.X * mScaleFactor) + Margin + Padding, 0, (rect.Width * mScaleFactor), Height);

					graphics.FillRectangle(brush, fill);
					graphics.DrawLine(pen2, fill.X, 0, fill.X, height);
					graphics.DrawLine(pen2, fill.Right, 0, fill.Right, height);
				}
			}

			graphics.ResetTransform();

			//Clear padding area + 1 (to overwrite 0 line)
			RectangleF paddingRect = new RectangleF(0,0,Padding+1,Height);
			graphics.FillRectangle(gradientBrush,paddingRect);

			//Draw padding seperator
			graphics.DrawLine(pen,Padding-1,0,Padding-1,Height);

			//Draw border
			if (mBorderStyle != RulerBorderStyle.None)
			{
				//Draw bottom edge
				graphics.DrawLine(pen,0,Height-1,Width-1,Height-1);

				//Draw other sides
				if (mBorderStyle == RulerBorderStyle.Full)
				{
					graphics.DrawLine(pen,0,0,0,Height-1);
					graphics.DrawLine(pen,Width-1,0,Width-1,Height-1);
					graphics.DrawLine(pen,0,0,Width-1,0);
				}
			}
		}

		private void RenderRulerVertical(Graphics graphics)
		{
			float position;
			float endPosition;

			float increment = Minor * mScaleFactor * mUnitScaleFactor.Y; //Get the scaled ruler increments

			float third = Convert.ToSingle(Width-(Width * 0.2));
			float twoThirds = Convert.ToSingle(Width-(Width * 0.6));
			float full = Convert.ToSingle(Width-(Width * 0.9));
			float width = Width;
			float onePixel = 1;
			
			Pen pen = new Pen(ForeColor,-1);
			SolidBrush brush = new SolidBrush(ForeColor);

			//Set up a gradient brush size that does not contain zeros
			Size size = Size;
			if (size.Width == 0) size.Width = 1;
			if (size.Height == 0) size.Height = 1;

			//Draw background
			LinearGradientBrush gradientBrush = new LinearGradientBrush(new RectangleF(new PointF(0,0),size),BackColor,GradientColor,System.Drawing.Drawing2D.LinearGradientMode.Horizontal);
			graphics.FillRectangle(gradientBrush, new RectangleF(new PointF(0,0),size));

			//Calculate end height
			endPosition = Height / mScaleFactor / mUnitScaleFactor.Y;

			//Translate for scroll, add onto the height if scrolled
			if (mDiagram != null)
			{
				graphics.TranslateTransform(0,mDiagram.AutoScrollPosition.Y);
				endPosition -= mDiagram.AutoScrollPosition.Y; //Scroll position is a negative value
			}

			//Translate for padding. Padding is always in pixels
			graphics.TranslateTransform(0, Padding);
			
			//Translate for margin. Margin is always in pixels
			graphics.TranslateTransform(0, mMargin);

			//Translate back in perfect multiples of the minor increment.
			graphics.TranslateTransform(0, -Convert.ToInt32(mMargin / increment) * increment);

			//Set the initial position value
			position = (-Convert.ToInt32((mMargin / mScaleFactor / mUnitScaleFactor.Y) / Minor) * Minor);

			//Draw a minor, major or mid mark, incrementing by minor
			while (position < endPosition)
			{
				//Determine if major, middle or normal (minor mark)
				if (position % Major == 0)
				{	
					//Draw full line
					graphics.DrawLine(pen,full,0,Width,0);

					//Store the current translate
					Matrix transform = graphics.Transform;
					
					//Rotate graphics at position
					Matrix matrix = new Matrix();
					matrix.Translate(10, 0);
					matrix.Translate(transform.OffsetX,transform.OffsetY);
					matrix.RotateAt(90,new PointF(0,0));
					graphics.Transform = matrix;

					string number = position.ToString();
					if (position == 0) number += "  " + Abbreviate(Units);
					
					graphics.DrawString(number,mFont,brush,full,-onePixel);

					//Restore the transform
					graphics.Transform = transform;
				}
				else if (position % Mid == 0)
				{
					graphics.DrawLine(pen,twoThirds,0,Width,0);
				}
				else
				{
					graphics.DrawLine(pen,third,0,Width,0);
				}

				//Translate by the minor increment and increment position
				graphics.TranslateTransform(0, increment);
				position += Minor;
			}

			graphics.ResetTransform();
			if (mDiagram != null) graphics.TranslateTransform(0, mDiagram.AutoScrollPosition.Y);

			//Draw highlights
			if (DrawGuides && mGuides != null)
			{
				brush = new SolidBrush(Color.FromArgb(16,Component.Instance.HighlightBrush.Color));
				Pen pen2 = new Pen(Color.FromArgb(255,Component.Instance.HighlightPen.Color), 1);
							
				foreach (RectangleF rect in Guides)
				{
					RectangleF fill = new RectangleF(0, (rect.Y * mScaleFactor) + Margin + Padding, Width, (rect.Height * mScaleFactor));

					graphics.FillRectangle(brush, fill);
					graphics.DrawLine(pen2, 0, fill.Y, Width, fill.Y);
					graphics.DrawLine(pen2, 0, fill.Bottom, Width, fill.Bottom);
				}
			}

			graphics.ResetTransform();

			//Clear padding area + 1 (to overwrite 0 line)
			RectangleF paddingRect = new RectangleF(0,0,Width,Padding+1);
			graphics.FillRectangle(gradientBrush, paddingRect);

			//Draw padding seperator
			graphics.DrawLine(pen,0,Padding-1,Width,Padding-1);

			//Draw border
			if (mBorderStyle != RulerBorderStyle.None)
			{
				//Draw right edge
				graphics.DrawLine(pen,width-1,0,Width-1,Height-1);

				//Draw other sides
				if (mBorderStyle == RulerBorderStyle.Full)
				{
					graphics.DrawLine(pen,0,0,Width-1,0);
					graphics.DrawLine(pen,0,0,0,Height-1);
					graphics.DrawLine(pen,0,Height-1,Width-1,Height-1);
				}
			}
		}

		protected virtual void SetDefaultUnitValues()
		{
			switch (mUnit)
			{
				case RulerUnit.Pixel:
					mMajor = 100;
					mMinor = 10;
					mMid = 50;
					break;
				case RulerUnit.Point:
					mMajor = 72;
					mMinor = 6;
					mMid = 36;
					break;
				case RulerUnit.Inch:
					mMajor = 1;
					mMinor = 1F;
					mMid = 1F;
					break;
				case RulerUnit.Millimeter:
					mMajor = 20;
					mMinor = 2;
					mMid = 5;
					break;
				case RulerUnit.Document:
					mMajor = 200;
					mMinor = 20;
					mMid = 100;
					break;
				default:
					mMajor = 100;
					mMinor = 10;
					mMid = 50;
					break;
			}
		}

		private void SetUnitScaleFactors()
		{
			Graphics graphics = Component.Instance.CreateGraphics();
			graphics.PageUnit = ConvertUnit(Units);
			mUnitScaleFactor = Crainiate.Diagramming.Units.CalculateUnitFactors(graphics);
			graphics.Dispose();
		}

		private GraphicsUnit ConvertUnit(RulerUnit rulerUnit)
		{
			return (GraphicsUnit) Enum.Parse(typeof(GraphicsUnit),rulerUnit.ToString());
		}

		public static string Abbreviate(RulerUnit unit)
		{
			switch (unit)
			{
				case RulerUnit.Pixel:
					return "px";
				case RulerUnit.Display:
					return "ds";
				case RulerUnit.Document:
					return "dc";
				case RulerUnit.Inch:
					return "in";
				case RulerUnit.Millimeter:
					return "mm";
				case RulerUnit.Point:
					return "pt";
				default:
					return "";
			}
		}
	
		private void CalculateGuides(RenderList actions)
		{
			ArrayList list = new ArrayList();
			
			//Loop through actions and add rectangles
			foreach (Element element in actions)
			{   
				if (element is Shape && element.Visible)
				{
					ArrayList newList = new ArrayList();

					//Get the rectangle from element
					RectangleF newRect = element.Rectangle;

					//If a solid element, get transform rectangle
					if (element is SolidElement)
					{
						SolidElement solid = (SolidElement) element;
						newRect = solid.TransformRectangle;
					}

					//Offset rectangle according to container
					newRect.X += element.Container.Offset.X;
					newRect.Y += element.Container.Offset.Y;

					bool combine = false;
				
					//Loop through the existing rectangles and see if they intersect
					foreach (RectangleF rect in list)
					{
						//Determine if rectangles can be combined
						if (Orientation == RulerOrientation.Top)
						{
							combine = rect.Contains(new PointF(newRect.X, rect.Y )) || rect.Contains(new PointF(newRect.Right, rect.Y ));
						}
						else
						{
							combine = rect.Contains(new PointF(rect.X, newRect.Y)) || rect.Contains(new PointF(rect.X, newRect.Bottom));
						}

						//Add combined rectangle rectangle together, or add to new list if no intersection
						if (combine) 
						{
							newRect = CombineRectangle(newRect, rect); 
						}
						else
						{
							newList.Add(rect);
						}
					}
				
					//Add to new list if not combined
					newList.Add(newRect);
					list = newList;
				}
			}
			
			SetGuides((RectangleF[]) list.ToArray(typeof(RectangleF)));
		}

		private RectangleF CombineRectangle(RectangleF a,RectangleF b)
		{
			RectangleF c = new RectangleF();
			c.X = (a.Left < b.Left) ? a.Left : b.Left;
			c.Y = (a.Top < b.Top) ? a.Top : b.Top;
			c.Width = (a.Right > b.Right) ? a.Right - c.X : b.Right - c.X;
			c.Height = (a.Bottom > b.Bottom) ? a.Bottom - c.Y : b.Bottom - c.Y;
			return c;
		}

		#endregion

	}
}
