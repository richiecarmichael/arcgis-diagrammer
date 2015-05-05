using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Crainiate.Diagramming;

namespace Crainiate.Diagramming.Printing
{
	public class PrintRender: IRender
	{
		//Property variables
		private Color mBackcolor = SystemColors.Window;
		private CompositingMode mCompositingMode = CompositingMode.SourceOver;
		private CompositingQuality mCompositingQuality = CompositingQuality.AssumeLinear;
		private InterpolationMode mInterpolationMode = InterpolationMode.Default;
		private PixelOffsetMode mPixelOffsetMode = PixelOffsetMode.Default;
		private SmoothingMode mSmoothingMode = SmoothingMode.AntiAlias;
		private Diagram mDiagram;
		private bool mSelectedOnly;

		private RenderList mElementRenderList;
		private Layers mLayers;
		private System.Drawing.Image mBackgroundImage = null;
		
		private bool mAlphaCorrection = false;

		private int mSuspendCount;
		private bool mLocked;
		private float mZoomPerc = 100;

		private Rectangle mRenderRectangle;
		private SizeF mDiagramSize;

		private Matrix mTransform;

		private bool mDrawShadows = false;
		private bool mDrawBackground = true;
		private bool mGreyscale = false;

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
		public PrintRender()
		{
		}

		public virtual bool SelectedOnly
		{
			get
			{
				return mSelectedOnly;
			}
			set
			{
				mSelectedOnly = value;
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

		public virtual bool DrawBackground 
		{
			get
			{
				return mDrawBackground;
			}
			set
			{
				mDrawBackground = value;
			}
		}

		public virtual bool DrawShadows
		{
			get
			{
				return mDrawShadows;
			}
			set
			{
				mDrawShadows = value;
			}
		}

		public virtual bool Greyscale
		{
			get
			{
				return mGreyscale;
			}
			set
			{
				mGreyscale = value;
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
			throw new PrintRenderException("RenderDiagram cannot be called directly in the print renderer.");
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

		//Loop through layers and elements and render
		public virtual void RenderDiagramElements(Graphics graphics)
		{
			OnPreRender(graphics);

			//Set graphics
			graphics.InterpolationMode = InterpolationMode;
			graphics.CompositingMode = CompositingMode;
			graphics.CompositingQuality = CompositingQuality;
			graphics.SmoothingMode = SmoothingMode;
			graphics.PixelOffsetMode = PixelOffsetMode;

			bool drawBackgroundTemp = false;

			foreach (Layer layer in mLayers)
			{
				if (layer.Visible) 
				{
					mWorldOpacity = layer.Opacity;
					mCurrentLayer = layer;

					//Draw each element by checking if it is renderable and calling the render method
					if (DrawShadows && layer.DrawShadows)
					{
						foreach (Element element in mElementRenderList)
						{
							if (element.Layer == layer && element.Visible)
							{
								if (SelectForPrint(element))
								{
									//Draw shapes
									GraphicsState graphicsState = graphics.Save();

									graphics.TranslateTransform(element.Rectangle.X,element.Rectangle.Y);
									element.RenderShadow(graphics,this);
									graphics.Restore(graphicsState);
								}
							}
						}
					}
					
					//Draw each element by checking if it is renderable and calling the render method
					foreach (Element element in mElementRenderList)
					{
						if (element.Layer == layer && element.Visible)
						{
							if (SelectForPrint(element))
							{
								//Draw shapes
								GraphicsState graphicsState = graphics.Save();
								SolidElement solid = null;

								if (element is SolidElement)
								{
									solid = (SolidElement) element;	
									drawBackgroundTemp = solid.DrawBackground;
									solid.mDrawBackground = DrawBackground;								
								}

								graphics.TranslateTransform(element.Rectangle.X,element.Rectangle.Y);
								element.Render(graphics,this);
								graphics.Restore(graphicsState);

								if (element is SolidElement) solid.mDrawBackground = drawBackgroundTemp;
							}
						}
					}
				}
			}
			
			//Reset current layer
			mCurrentLayer = null;

			OnPostRender(graphics);
		}

		//Adjusts for opacity and alpha scale blending
		//Will reduce the opacity of a line with width * scale < 1
		private Color AdjustColorImplementation(Color color, float width,float opacity)
		{
			if (CompositingMode == CompositingMode.SourceCopy) return Color.FromArgb(255,color);

			float widthScale = width * mScaleFactor;
			
			if (widthScale == 0 || widthScale > 1) widthScale =1;
									
			//original alpha x width scaled x local opacity x Layer opacity
			int intensity = Convert.ToInt32(color.A * widthScale * opacity * mWorldOpacity / 10000);

			if (intensity > 255) intensity = 255;
			if (intensity < 20) intensity = 20;

			if (Greyscale)
			{
				int average = (Convert.ToInt32(color.R) + Convert.ToInt32(color.G) + Convert.ToInt32(color.B)) / 3;
				return Color.FromArgb(intensity,average,average,average);
			}

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

		protected virtual bool SelectForPrint(Element element)
		{
			//Print all elements if not selected only
			if (!SelectedOnly) return true;

			//Check lines and docked elements
			if (element is Line)
			{
				Line line = (Line) element;
				
				//Return if line selected
				if (line.Selected) return true;

				//Return if docked shape is selected
				if (line.Start.DockedElement != null && SelectForPrint(line.Start.DockedElement)) return true;
				if (line.End.DockedElement != null && SelectForPrint(line.End.DockedElement)) return true;
			}

			//Check for other Iselectable items
			if (element is ISelectable)
			{
				ISelectable select = (ISelectable) element;
				return select.Selected;
			}


			return false;
		}

		#endregion

	}
}
