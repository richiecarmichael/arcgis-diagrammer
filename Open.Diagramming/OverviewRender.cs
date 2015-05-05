using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Crainiate.Diagramming;

namespace Crainiate.Diagramming.Navigation
{
	public class OverviewRender: IRender
	{
		//Property variables
		private Color mBackcolor = SystemColors.Window;
		private CompositingMode mCompositingMode = CompositingMode.SourceOver;
		private CompositingQuality mCompositingQuality = CompositingQuality.AssumeLinear;
		private InterpolationMode mInterpolationMode = InterpolationMode.Default;
		private PixelOffsetMode mPixelOffsetMode = PixelOffsetMode.Default;
		private SmoothingMode mSmoothingMode = SmoothingMode.AntiAlias;
		private Diagram mDiagram;

		private RenderList mElementRenderList;
		private Layers mLayers;
		private System.Drawing.Image mBackgroundImage = null;
		
		private bool mAlphaCorrection = true;

		private int mSuspendCount;
		private bool mLocked;
		private float mZoomPerc = 100;

		private Rectangle mRenderRectangle;
		private SizeF mDiagramSize;

		private Matrix mTransform;

		//Working Variables
		internal float mZoomFactor = 1;
		internal float mScaleFactor = 1;

		private byte mWorldOpacity = 100;
		private Bitmap mBitmap; //Stores the current render back buffer

		private Layer mCurrentLayer = null;
	
		#region Interface
		
		//Events
		public event RenderEventHandler PreRender;
		public event RenderEventHandler PostRender;

		//Constructors
		public OverviewRender()
		{
		}

		public virtual bool AlphaCorrection
		{
			get
			{
				return mAlphaCorrection;
			}
			set
			{
				mAlphaCorrection = value;
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
			}
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
				}
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
			}
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
				//Set up a new bitmap, can throw errors when in use eg whilst scrolling
				DisposeBufferBitmap();
				mBitmap = new Bitmap(Convert.ToInt32(renderRectangle.Width) , Convert.ToInt32(renderRectangle.Height), PixelFormat.Format32bppPArgb);

				//Get a graphics handle from the new back buffer
				graphics = Graphics.FromImage(mBitmap);

				//No background image then just clear, else just background image
				if (mBackgroundImage == null)
				{
					graphics.Clear(mBackcolor);
				}
				else
				{
					graphics.TranslateTransform(renderRectangle.X, renderRectangle.Y);
					TextureBrush objBrush = new TextureBrush(mBackgroundImage);
					graphics.FillRectangle(objBrush, renderRectangle);
					graphics.TranslateTransform(renderRectangle.X, renderRectangle.Y);
				}
	
				//Set up the transform matrix
				mTransform = new Matrix();

				//Set the scale and world transformation
				if (mZoomPerc != 100) mTransform.Scale(mScaleFactor, mScaleFactor);
				mTransform.Translate(-renderRectangle.X * mZoomFactor, -renderRectangle.Y * mZoomFactor);
				
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
			RenderDiagramElements(graphics);					

			OnPostRender(graphics);

			graphics.Dispose();
		}

		//Loop through layers and elements and render
		public virtual void RenderDiagramElements(Graphics graphics)
		{
			foreach (Layer layer in Layers)
			{
				if (layer.Visible) 
				{
					mWorldOpacity = layer.Opacity;
					mCurrentLayer = layer;
					
					//Draw each element by checking if it is renderable and calling the render method
					RenderDiagramElementsLayer(graphics,mElementRenderList,layer);
				}
			}
			
			//Reset current layer
			mCurrentLayer = null;
		}

		protected virtual void RenderDiagramElementsLayer(Graphics graphics, RenderList renderlist, Layer layer)
		{
			foreach (Element element in renderlist)
			{
				if (element.Layer == layer && element.Visible)
				{
					//Draw shapes
					graphics.TranslateTransform(element.Rectangle.X,element.Rectangle.Y);
					RenderElement(graphics,element);

					//Render any elements in the container
					if (element is IContainer)
					{
						IContainer container = (IContainer) element;

						Region region = new Region(element.GetPathInternal());
						Region current = graphics.Clip;
						graphics.SetClip(region,CombineMode.Intersect);

						RenderDiagramElementsLayer(graphics,container.RenderList,layer);			

						//reset clip
						graphics.Clip = current;
					}

					graphics.TranslateTransform(-element.Rectangle.X,-element.Rectangle.Y);
				}
			}
		}

		protected virtual void RenderElement(Graphics graphics,Element element)
		{
			GraphicsPath path = element.GetPathInternal();
			if (path  == null) return;

			//Create a brush if no custom brush defined
			if (element is SolidElement)
			{
				SolidElement solid = (SolidElement) element;
				if (solid.DrawBackground)
				{
					if (solid.CustomBrush == null)
					{
						//Use a linear gradient brush if gradient requested
						if (solid.DrawGradient)
						{
							LinearGradientBrush brush;
							brush = new LinearGradientBrush(new RectangleF(0,0,solid.Rectangle.Width,solid.Rectangle.Height),AdjustColor(solid.BackColor,0,solid.Opacity),AdjustColor(solid.GradientColor,0,solid.Opacity),solid.GradientMode);
							if (solid.Blend != null) brush.Blend = solid.Blend;
							graphics.FillPath(brush, path);
						}
							//Draw normal solid brush
						else
						{
							SolidBrush brush;
							brush = new SolidBrush(solid.BackColor);
						
							//Check if winforms renderer and adjust color as required
							brush.Color = AdjustColor(solid.BackColor,0,solid.Opacity);
							graphics.FillPath(brush,path);
						}
					}
					else	
					{
						graphics.FillPath(solid.CustomBrush, path);
					}
				}
			}

			Pen pen = null;

			if (element.CustomPen == null)
			{
				pen = new Pen(element.BorderColor,element.BorderWidth);
				pen.DashStyle = element.BorderStyle;
	
				//Check if winforms renderer and adjust color as required
				pen.Color = AdjustColor(element.BorderColor,element.BorderWidth,element.Opacity);
			}
			else	
			{
				pen = element.CustomPen;
			}
			
			graphics.DrawPath(pen,path);
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

		#endregion

	}
}
