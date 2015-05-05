using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Crainiate.Diagramming;

namespace Crainiate.Diagramming.Editing
{
	public class PaletteRender: IRender
	{
		//Property variables
		private Color mBackcolor = SystemColors.Window;
		private Color mGradientcolor = Color.Silver;
		private Color mBordercolor = Color.Black;
		private Color mForecolor = Color.Black;
		private Color mFillcolor = Color.White;
		private LinearGradientMode mGradientMode = LinearGradientMode.ForwardDiagonal;
		private Diagram mDiagram;

		private CompositingMode mCompositingMode = CompositingMode.SourceOver;
		private CompositingQuality mCompositingQuality = CompositingQuality.AssumeLinear;
		private InterpolationMode mInterpolationMode = InterpolationMode.Default;
		private PixelOffsetMode mPixelOffsetMode = PixelOffsetMode.Default;
		private SmoothingMode mSmoothingMode = SmoothingMode.AntiAlias;

		private RenderList mElementRenderList;
		private Layers mLayers;
		private Tabs mTabs;
		private System.Drawing.Image mBackgroundImage = null;
		
		private bool mAlphaCorrection = true;

		private int mSuspendCount;
		private bool mLocked;
		private float mZoomPerc = 100;

		private Rectangle mRenderRectangle;
		private SizeF mDiagramSize;

		private Matrix mTransform;
		private GraphicsPath mActionPath;

		private bool mUpPressed;
		private bool mDownPressed;
		private Font mFont;

		private bool mDrawScroll;

		//Working Variables
		internal float mZoomFactor = 1;
		internal float mScaleFactor = 1;

		private byte mWorldOpacity = 100;
		private Bitmap mBitmap; //Stores the current render back buffer
		private Bitmap mGraphicsStateBitmap; //Stores a copy of the current back buffer

		private Layer mCurrentLayer = null;
		private Tab mScrollTab;
	
		#region Interface
		
		//Events
		public event RenderEventHandler PreRender;
		public event RenderEventHandler PostRender;

		//Constructors
		public PaletteRender()
		{
			mScrollTab = new Tab();
			mScrollTab.Visible = false;
			mScrollTab.ButtonStyle = ButtonStyle.Down;
		}

		//Gets or sets the diagram reference for this renderer
		public virtual Diagram Diagram
		{
			get
			{
				return mDiagram;
			}
			set
			{
				mDiagram = value;
			}
		}

		public virtual Font Font
		{
			get
			{
				return mFont;
			}
			set
			{
				mFont = value;
			}
		}

		//Sets or retrieves the color used to render the background
		public virtual Color BackColor
		{
			get
			{
				return mBackcolor;
			}
			set
			{
				mBackcolor = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or retrieves the color used to render the background gradient
		public virtual Color GradientColor
		{
			get
			{
				return mGradientcolor;
			}
			set
			{
				mGradientcolor = value;
				DisposeGraphicsStateBitmap();
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
					DisposeGraphicsStateBitmap();
				}
			}
		}

		//Sets or retrieves the color used to render the background gradient
		public virtual Color FillColor
		{
			get
			{
				return mFillcolor;
			}
			set
			{
				mFillcolor = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or retrieves the color used to render the background gradient
		public virtual Color BorderColor
		{
			get
			{
				return mBordercolor;
			}
			set
			{
				mBordercolor = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or retrieves the color used to render text in the palette
		public virtual Color ForeColor
		{
			get
			{
				return mForecolor;
			}
			set
			{
				mForecolor = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Specifies the whether objects are drawn with alpha blending
		public virtual CompositingMode CompositingMode
		{
			get
			{
				return mCompositingMode;
			}
			set
			{
				mCompositingMode = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Specifies how objects are compositied when drawn together
		public virtual CompositingQuality CompositingQuality
		{
			get
			{
				return mCompositingQuality;
			}
			set
			{
				mCompositingQuality = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Specifies how data is interpolated between endpoints.
		public virtual InterpolationMode InterpolationMode
		{
			get
			{
				return mInterpolationMode;
			}
			set
			{
				mInterpolationMode = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Gets or set a value specifying how pixels are offset during rendering.")]
		public virtual PixelOffsetMode PixelOffsetMode
		{
			get
			{
				return mPixelOffsetMode;
			}
			set
			{
				mPixelOffsetMode = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or gets the list of elements to be rendered
		public virtual RenderList Elements
		{
			get
			{
				return mElementRenderList;
			}
			set
			{
				mElementRenderList = value;
			}
		}

		//Gets or sets the actionpath
		public virtual GraphicsPath ActionPath
		{
			get
			{
				return mActionPath;
			}
			set
			{
				mActionPath = value;
			}
		}

		//Sets or gets the list of elements to be rendered
		public virtual Layers Layers
		{
			get
			{
				return mLayers;
			}
			set
			{
				mLayers = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or gets the list of elements to be rendered
		public virtual Tabs Tabs
		{
			get
			{
				return mTabs;
			}
			set
			{
				mTabs = value;
				DisposeGraphicsStateBitmap();
			}
		}
		
		//Sets or retrieves the rectangle defining the area currently in view.
		public virtual Rectangle RenderRectangle
		{
			get
			{
				return mRenderRectangle;
			}
			set
			{
				mRenderRectangle = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or gets the total size of the diagram that is being rendered
		public virtual SizeF DiagramSize
		{
			get
			{
				return mDiagramSize;
			}
			set
			{
				mDiagramSize = value;
				DisposeGraphicsStateBitmap();
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
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or retrieves the current zoom Layer as a percentage.")]
		public virtual float Zoom
		{
			get
			{
				return mZoomPerc;
			}
			set
			{
				if (value > 0 && value != mZoomPerc)
				{
					mZoomPerc = value;
					mZoomFactor = Convert.ToSingle(100 / value);
					mScaleFactor = Convert.ToSingle(value / 100);
					DisposeGraphicsStateBitmap();
				}
			}
		}

		public virtual bool DrawScroll
		{
			get
			{
				return mDrawScroll;
			}
			set
			{
				mDrawScroll = value;
			}
		}

		//Retrieves the scaling factor for this render
		public virtual float ScaleFactor
		{
			get
			{
				return mScaleFactor;
			}
		}

		//Retrieves the zooming factor for this render
		public virtual float ZoomFactor
		{
			get
			{
				return mZoomFactor;
			}
		}

		//Returns the layer that is currently being rendered, or null if not rendering
		public virtual Layer CurrentLayer
		{
			get
			{
				return mCurrentLayer;
			}
		}

		//Retrieves the backbuffer bitmap
		public virtual Bitmap Bitmap
		{
			get
			{
				return mBitmap;
			}
		}

		//Returns whether the renderer is currently locked.")]
		public virtual bool Locked
		{
			get
			{
				return mLocked;
			}
		}

		public virtual bool UpPressed
		{
			get
			{
				return mUpPressed;
			}
			set
			{
				mUpPressed = value;
			}
		}		

		public virtual bool DownPressed
		{
			get
			{
				return mDownPressed;
			}
			set
			{
				mDownPressed = value;
			}
		}

		//Sets or retrieves the background image for the diagram.
		public virtual System.Drawing.Image BackgroundImage
		{
			get
			{
				return mBackgroundImage;
			}
			set
			{
				mBackgroundImage = value;
				DisposeGraphicsStateBitmap();
			}
		}

		internal Tab ScrollTab
		{
			get
			{
				return mScrollTab;
			}
		}

		//Locks the render class.
		public virtual void Lock()
		{
			mGraphicsStateBitmap = (Bitmap) mBitmap.Clone();
			mLocked = true;
		}

		//Unlocks the render class.")]
		public virtual void Unlock()
		{
			DisposeGraphicsStateBitmap();
			mLocked = false;
		}

		//Adjusts color Layers using width and opacity
		public virtual Color AdjustColor(Color color, float width,float opacity)
		{
			return AdjustColorImplementation(color,width,opacity);
		}

		//Draws the diagram area contained in the specified rectangle
		public virtual void RenderDiagram(Rectangle renderRectangle)
		{
			RenderDiagramImplementation(renderRectangle);
			mRenderRectangle = renderRectangle;
		}

		//Raises the PreRender event
		protected virtual void OnPreRender(Graphics graphics)
		{
			if (PreRender != null) PreRender(this, new RenderEventArgs(graphics));
		}

		//Raises the PostRender event
		protected virtual void OnPostRender(Graphics graphics)
		{
			if (PostRender != null) PostRender(this, new RenderEventArgs(graphics));
		}

		#endregion

		#region Implementation

		//sets up the internal graphics object from a bitmap back buffer
		public Graphics CreateGraphics(Rectangle renderRectangle)
		{
			if (renderRectangle.Width == 0 || renderRectangle.Height == 0) return null;

			Graphics graphics = null;

			try
			{
				if (mGraphicsStateBitmap == null)
				{
					//Set up a new bitmap, can throw errors when in use eg whilst scrolling
					//Unlike other renderers, palette render creates the entire diagram buffer
					//This is to simplify the gradient rendering process
					DisposeBufferBitmap();
					mBitmap = new Bitmap(Convert.ToInt32(DiagramSize.Width),Convert.ToInt32(DiagramSize.Height),PixelFormat.Format32bppPArgb);

					//Get a graphics handle from the new back buffer
					graphics = Graphics.FromImage(mBitmap);
					graphics.Clear(BackColor);

					//Draw the background gradient
					LinearGradientBrush gradient = new LinearGradientBrush(new RectangleF(new PointF(0,0),DiagramSize),BackColor,GradientColor,GradientMode);
					graphics.FillRectangle(gradient,new RectangleF(new PointF(0,0),renderRectangle.Size));
				}
				else
				{
					DisposeBufferBitmap();
					mBitmap = (Bitmap) mGraphicsStateBitmap.Clone();
					graphics = Graphics.FromImage(mBitmap);
				}

				//Set up the transform matrix
				mTransform = new Matrix();

				//Set the scale and world transformation
				if (mZoomPerc != 100) mTransform.Scale(mScaleFactor, mScaleFactor);
				
				//Apply transform matrix
				graphics.Transform = mTransform;

				//Set the drawing options
				graphics.CompositingMode = mCompositingMode;
				graphics.CompositingQuality = mCompositingQuality;
				graphics.InterpolationMode = mInterpolationMode;
				graphics.PixelOffsetMode = mPixelOffsetMode;
				graphics.SmoothingMode = mSmoothingMode;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error getting render graphics" + ex.ToString());

			}
			return graphics;
		}

		private void RenderDiagramImplementation(Rectangle renderRectangle)
		{
			Graphics graphics = CreateGraphics(renderRectangle);
			if (graphics==null) return;					

			OnPreRender(graphics);

			//Render the elements if the renderer isnt locked
			if (!mLocked) RenderDiagramElements(graphics);					
			if (ActionPath != null) RenderDiagramAction(graphics);
			OnPostRender(graphics);

			graphics.Dispose();
		}

		//Loop through layers and elements and render
		public virtual void RenderDiagramElements(Graphics graphics)
		{
			bool isup = false;
			bool isdown = false;
			RectangleF region = new RectangleF(0,0,DiagramSize.Width,DiagramSize.Height);
			
			foreach (Tab tab in Tabs)
			{
				if (tab.Visible) 
				{
					//Determine if an up or down arrow should be drawn
					if (isup)
					{
						isdown = true; //set down
						isup = false; //reset up
						region.Height = tab.Rectangle.Top - region.Top;
					}
					//Reset down
					if (isdown) isdown = false;

					//Set up
					if (Tabs.CurrentTab == tab)
					{
						isup = true;
						region.Y = tab.Rectangle.Bottom+1;
					}

					RenderTab(graphics,tab);
				}
				
				if (tab.ButtonStyle != ButtonStyle.None && DrawScroll)
				{
					RenderButton(graphics, tab.ButtonStyle, tab.ButtonRectangle, tab.ButtonPressed, tab.ButtonEnabled);
				}
			}

			///Draw final button
			if (ScrollTab.Visible && DrawScroll)
			{
				RenderButton(graphics,ButtonStyle.Down,ScrollTab.ButtonRectangle,ScrollTab.ButtonPressed,ScrollTab.ButtonEnabled);
			}

			//Set up the region to confine the elements drawn
			graphics.Clip = new Region(region);

			foreach (Layer layer in mLayers)
			{
				if (layer.Visible) 
				{
					mWorldOpacity = layer.Opacity;
					mCurrentLayer = layer;
					
					//Render shadows
					if (layer.DrawShadows)
					{
						foreach (Element element in mElementRenderList)
						{
							if (element.Layer == layer && element.DrawShadow && element.Visible)
							{
								graphics.TranslateTransform(element.Rectangle.X+layer.ShadowOffset.X ,element.Rectangle.Y+layer.ShadowOffset.Y);
								graphics.SmoothingMode = element.SmoothingMode;
								element.RenderShadow(graphics,this);
								graphics.TranslateTransform(-element.Rectangle.X-layer.ShadowOffset.X,-element.Rectangle.Y-layer.ShadowOffset.Y);
							}
						}
					}

					//Draw each element by checking if it is renderable and calling the render method
					foreach (Element element in mElementRenderList)
					{
						if (element.Layer == layer && element.Visible)
						{
							//Draw shapes
							GraphicsState graphicsState = graphics.Save();

							graphics.TranslateTransform(element.Rectangle.X,element.Rectangle.Y);
							graphics.SmoothingMode = element.SmoothingMode;
							element.Render(graphics,this);
							
							graphics.Restore(graphicsState);
						}
					}
				}
			}
			
			//Reset current layer
			mCurrentLayer = null;
			graphics.ResetClip();
		}

		//Render the action path 
		protected virtual void RenderDiagramAction(Graphics graphics)
		{
			Region region = graphics.Clip;

			graphics.ResetClip();
			SolidBrush brush = new SolidBrush(Color.FromArgb(128,FillColor));
			Pen pen = new Pen(Color.FromArgb(128,BorderColor));

			graphics.FillPath(brush,ActionPath);
			graphics.DrawPath(pen,ActionPath);

			graphics.Clip = region;
		}

		//Adjusts for opacity and alpha scale blending
		//Will reduce the opacity of a line with width * scale < 1
		private Color AdjustColorImplementation(Color color, float width,float opacity)
		{
			float widthScale = width * mScaleFactor;
			
			if (widthScale == 0 || widthScale > 1) widthScale =1;
									
			//original alpha x width scaled x local opacity x Layer opacity
			int intensity = Convert.ToInt32(color.A * widthScale * opacity * mWorldOpacity / 10000);

			if (intensity > 255) intensity = 255;
			if (intensity < 20) intensity = 20;

			return Color.FromArgb(intensity, color.R, color.G, color.B);
		}

		//Clears memory and resets graphicsstate bitmap
		protected void DisposeBufferBitmap()
		{
			try
			{
				if (! (mBitmap == null))
				{
					mBitmap.Dispose();
					mBitmap = null;
				}
			}
			catch {}
		}

		//Clears memory and resets graphicsstate bitmap
		protected void DisposeGraphicsStateBitmap()
		{
			try
			{
				if (! (mGraphicsStateBitmap == null))
				{
					mGraphicsStateBitmap.Dispose();
					mGraphicsStateBitmap = null;
				}
			}
			catch {}
		}

		private void RenderTab(Graphics graphics, Tab tab)
		{
			Pen pen;

			//Setup background and text rectangle
			RectangleF rect = tab.Rectangle;
			RectangleF innerRect = tab.Rectangle;
			innerRect.Inflate(-1,-1);

			if (rect.IsEmpty) return;

			//Draw the gradient background
			LinearGradientBrush gradient = new LinearGradientBrush(rect,Tabs.BackColor,Tabs.GradientColor,LinearGradientMode.Vertical);
			graphics.FillRectangle(gradient,rect);

			//Draw left + top
			pen = (tab.Pressed) ? SystemPens.ControlDark : SystemPens.ControlLightLight;
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Bottom),new PointF(rect.Left,rect.Top));
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Top),new PointF(rect.Right,rect.Top));

			//Draw right bottom
			pen = (tab.Pressed) ? SystemPens.ControlLightLight : SystemPens.ControlDark;
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Top),new PointF(rect.Right,rect.Bottom));
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Bottom),new PointF(rect.Left,rect.Bottom));

			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;

			//Offset by one if pressed
			if (tab.Pressed) innerRect.Offset(1,1);

			SolidBrush brush = new SolidBrush(Tabs.ForeColor);
			graphics.DrawString(tab.Name,mFont,brush,innerRect,format);
		}

		private void RenderButton(Graphics graphics, ButtonStyle style, RectangleF rect, bool pressed, bool enabled)
		{
			Pen pen;

			if (rect.IsEmpty) return;

			//Draw a pale white underlay
			LinearGradientBrush gradient = new LinearGradientBrush(rect,Tabs.BackColor,Tabs.GradientColor,LinearGradientMode.Vertical);
			graphics.FillRectangle(gradient,rect);

			pen = (pressed) ? SystemPens.ControlDark : SystemPens.ControlLightLight;
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Bottom),new PointF(rect.Left,rect.Top));
			graphics.DrawLine(pen,new PointF(rect.Left,rect.Top),new PointF(rect.Right,rect.Top));

			//Draw right bottom
			pen = (pressed) ? SystemPens.ControlLightLight : SystemPens.ControlDark;
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Top),new PointF(rect.Right,rect.Bottom));
			graphics.DrawLine(pen,new PointF(rect.Right,rect.Bottom),new PointF(rect.Left,rect.Bottom));

			//Draw up or down arrow
			GraphicsPath path = new GraphicsPath();
			Matrix matrix = new Matrix();
			
			if (style == ButtonStyle.Up)
			{
				path.AddLine(4,0,0,4);
				path.AddLine(0,4,8,4);
				matrix.Translate(rect.X+4,rect.Y+6);
			}
			else
			{
				path.AddLine(0,0,3,3);
				path.AddLine(3,3,6,0);
				matrix.Translate(rect.X+6,rect.Y+8);
			}
			path.CloseFigure();			

			//Translate to correct position on button
			if (pressed) matrix.Translate(1,1);
			path.Transform(matrix);

			Brush systemBrush = (enabled) ? new SolidBrush(Color.FromArgb(66,65,66)) : new SolidBrush(Color.FromArgb(192,SystemColors.ControlDark));

			graphics.SmoothingMode = SmoothingMode.Default;
			graphics.FillPath(systemBrush,path);
		}

		#endregion

	}
}
