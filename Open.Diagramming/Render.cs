
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Reflection;

namespace Crainiate.Diagramming
{
	public delegate void RenderEventHandler(object sender,RenderEventArgs e);

	public class Render: IRender, IRenderDesign, IRenderPaged, ICloneable 
	{
		//Property variables
		private GraphicsPath mDecorationPath;
		private RenderList mElementRenderList;
		private RenderList mActionRenderList;
		private RenderList mHighlightRenderList;
		private Layers mLayers;
		private System.Drawing.Image mBackgroundImage = null;
		
		private bool mAlphaCorrection = true;
		private bool mDrawGrid = false;
		private bool mDrawSelections = false;
		private bool mDrawDecorations = true;
		private bool mDecorationFill = true;
		private int mSuspendCount;
		private bool mLocked;
		private float mZoomPerc = 100;

		private Rectangle mRenderRectangle;
		private Rectangle mSelectionRectangle;
		private Size mWorkspaceSize;
		private SizeF mDiagramSize;
		
		private SizeF mPageLineSize;
		private bool mPaged = false;
		private bool mDrawPageLines = false;
		private Point mPagedOffset = new Point();
		private SizeF mPagedSize = new SizeF();

		private Color mBackcolor = SystemColors.Window;
		private Color mWorkspacecolor = SystemColors.AppWorkspace;

		private CompositingMode mCompositingMode = CompositingMode.SourceOver;
		private CompositingQuality mCompositingQuality = CompositingQuality.AssumeLinear;
		private PixelOffsetMode mPixelOffsetMode = PixelOffsetMode.Default;
		private ActionStyle mActionStyle;
		
		private Color mGridColor = Color.Silver;
		private Size mGridSize = new Size(20, 20);
		private GridStyle mGridStyle = GridStyle.Complex;

		private string mFeedback = null;
		private Point mFeedbackLocation = new Point();

		private Matrix mTransform;
		private int mRenderTime;

		private RectangleF mVector;

		//Working Variables
		private float mZoomFactor = 1;
		private float mScaleFactor = 1;

		private byte mWorldOpacity = 100;

		private Bitmap mGraphicsStateBitmap; //Stores a copy of the current back buffer
		private Bitmap mGridBitmap; //Stores a render of the current grid
		private Bitmap mBitmap; //Stores the current render back buffer

		private Layer mCurrentLayer = null;

		#region Interface

		//Events
		public event RenderEventHandler PreRender;
		public event RenderEventHandler PostRender;

		//Constructor
		public Render()
		{
			ActionStyle = Component.Instance.DefaultActionStyle;
		}

		//Properties
		//Sets or retrieves the value of the alpha correction property.
		public virtual bool AlphaCorrection
		{
			get
			{
				return mAlphaCorrection;
			}
			set
			{
				mAlphaCorrection = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		public virtual ActionStyle ActionStyle
		{
			get
			{
				return mActionStyle;
			}
			set
			{
				mActionStyle = value;
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
				DisposeGridBitmap();
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
				DisposeGridBitmap();
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
				DisposeGridBitmap();
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
				DisposeGridBitmap();
			}
		}

		//Sets a decoration path such as the type used to render drag drop operations.
		public virtual GraphicsPath DecorationPath
		{
			get
			{
				return mDecorationPath;
			}
			set
			{
				mDecorationPath = value;
			}
		}

		//Sets or retrieves the color used to render the grid.
		public virtual Color GridColor
		{
			get
			{
				return mGridColor;
			}
			set
			{
				mGridColor = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Sets or retrieves the size object used to determine the grid spacing.
		public virtual Size GridSize
		{
			get
			{
				return mGridSize;
			}
			set
			{
				mGridSize = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Sets or retrieves the size object used to determine the grid spacing.
		public virtual GridStyle GridStyle
		{
			get
			{
				return mGridStyle;
			}
			set
			{
				mGridStyle = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Determines whether the grid is displayed
		public virtual bool DrawGrid
		{
			get
			{
				return mDrawGrid;
			}
			set
			{
				mDrawGrid = value;
				DisposeGraphicsStateBitmap();
				DisposeGridBitmap();
			}
		}

		//Determines whether the grid is displayed
		public virtual bool DrawSelections
		{
			get
			{
				return mDrawSelections;
			}
			set
			{
				mDrawSelections = value;
			}
		}

		//Determines whether the grid is displayed
		public virtual bool DrawDecorations
		{
			get
			{
				return mDrawDecorations;
			}
			set
			{
				mDrawDecorations = value;
			}
		}

		//Returns the current transform matrix
		public virtual Matrix Transform
		{
			get
			{
				return (Matrix) mTransform.Clone();
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
		
		//Sets or gets the list of elements to be rendered
		public virtual RenderList Actions
		{
			get
			{
				return mActionRenderList;
			}
			set
			{
				mActionRenderList = value;
			}
		}

		//Sets or gets the list of elements to be rendered
		public virtual RenderList Highlights
		{
			get
			{
				return mHighlightRenderList;
			}
			set
			{
				mHighlightRenderList = value;
			}
		}

		//Returns whether the render engine is suspended")]
		public virtual bool Suspended
		{
			get
			{
				return mSuspendCount > 0;
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

		//Sets or retrieves the rectangle used to draw a selection decoration.
		public virtual Rectangle SelectionRectangle
		{
			get
			{
				return mSelectionRectangle;
			}
			set
			{
				if (value.Right < value.Left)
				{
					value = new Rectangle(value.X + value.Width, value.Y, value.Width * -1, value.Height);
				}
				if (value.Bottom < value.Top)
				{
					value = new Rectangle(value.X, value.Y + value.Height, value.Width, value.Height * -1);
				}
				mSelectionRectangle = value;
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

		//Sets or gets the total size of the diagram that is being rendered
		public virtual SizeF PageLineSize
		{
			get
			{
				return mPageLineSize;
			}
			set
			{
				if (! mPageLineSize.Equals(value))
				{
					mPageLineSize = value;
					DisposeGraphicsStateBitmap();
				}
			}
		}

		//Determines whether page lines are displayed
		public virtual bool DrawPageLines
		{
			get
			{
				return mDrawPageLines;
			}
			set
			{
				mDrawPageLines = value;
				DisposeGraphicsStateBitmap();
			}
		}

		public virtual bool Paged
		{
			get
			{
				return mPaged;
			}
			set
			{
				mPaged = value;
				DisposeGraphicsStateBitmap();
			}
		}

		public virtual Point PagedOffset
		{
			get
			{
				return mPagedOffset;
			}
			set
			{
				mPagedOffset = value;
				DisposeGraphicsStateBitmap();
			}
		}

		public virtual SizeF PagedSize
		{
			get
			{
				return mPagedSize;
			}
			set
			{
				mPagedSize = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or gets the parameters for a design vector line
		public virtual RectangleF Vector
		{
			get
			{
				return mVector;
			}
			set
			{
				mVector = value;
			}
		}

		//Sets or retrieves the color used to render the background
		public virtual Color WorkspaceColor
		{
			get
			{
				return mWorkspacecolor;
			}
			set
			{
				mWorkspacecolor = value;
				DisposeGraphicsStateBitmap();
			}
		}

		//Sets or gets the overall workspace rectangle for a paged render
		public virtual Size WorkspaceSize
		{
			get
			{
				return mWorkspaceSize;
			}
			set
			{
				mWorkspaceSize = value;
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
					DisposeGridBitmap();
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

		public virtual string Feedback
		{
			get
			{
				return mFeedback;
			}
			set
			{
				mFeedback = value;
			}
		}

		public virtual Point FeedbackLocation
		{
			get
			{
				return mFeedbackLocation;
			}
			set
			{
				mFeedbackLocation = value;
			}
		}

		public virtual int RenderTime
		{
			get
			{
				return mRenderTime;
			}
		}

		//Methods

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

		//Draws the diagram area contained in the specified rectangle
		public virtual void RenderDiagram(Rectangle renderRectangle)
		{
			DateTime start = DateTime.Now;
			RenderDiagramImplementation(renderRectangle);
			TimeSpan time = DateTime.Now - start;
			
			mRenderTime = time.Milliseconds;
			mRenderRectangle = renderRectangle;
		}

		//Suspends all render operations.
		public virtual void Suspend()
		{
			mSuspendCount += 1;
		}

		//Resumes all render operations.
		public virtual void Resume()
		{
			mSuspendCount -= 1;
		}

		//Resumes all render operations.
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

		//Adjusts color Layers using width and opacity
		public virtual Color AdjustColor(Color color, float width,float opacity)
		{
			return AdjustColorImplementation(color,width,opacity);
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

		public virtual object Clone()
		{
			object objNew = Activator.CreateInstance(GetType());
			FieldInfo[] fields = GetType().GetFields();

			int intI = 0;

			foreach (FieldInfo fieldInfo in fields)
			{
				fields[intI].SetValue(objNew, fieldInfo.GetValue(this));
				intI += 1;
			}

			return objNew;
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

		//Clears memory and resets grid bitmap
		protected void DisposeGridBitmap()
		{
			try
			{
				if (! (mGridBitmap == null))
				{
					mGridBitmap.Dispose();
					mGridBitmap = null;
				}
			}
			catch {}
		}

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
					DisposeBufferBitmap();
					mBitmap = new Bitmap(Convert.ToInt32(renderRectangle.Width) , Convert.ToInt32(renderRectangle.Height), PixelFormat.Format32bppPArgb);

					//Get a graphics handle from the new back buffer
					graphics = Graphics.FromImage(mBitmap);
					
					//Set up the page transforms and draw the workspace
					if (Paged) 
					{
						graphics.TranslateTransform(-renderRectangle.X,-renderRectangle.Y);
						RenderPage(graphics,mPagedOffset,mPagedSize);
						
						//Translate the offset
						if (! mPagedOffset.IsEmpty)
						{
							//Add a region so that only the page gets drawn
							graphics.Clip = new Region(new RectangleF(mPagedOffset,mPagedSize));
						}
						graphics.TranslateTransform(renderRectangle.X,renderRectangle.Y);
					}

					//Draw grid over background image
					if (DrawGrid)
					{
						float gridWidth = mGridSize.Width * mScaleFactor;
						float gridHeight = mGridSize.Height * mScaleFactor;

						float gridOffsetX = gridWidth-(mPagedOffset.X % gridWidth);
						float gridOffsetY = gridHeight-(mPagedOffset.Y % gridHeight);

						//If there is no grid map, then create it
						if (mGridBitmap == null)
						{
							Graphics gridGraphics = null;

							//Background image will have been pre-sized
							//If there is one then gridmap becomes size of background image
							//else gridmap becomes 10 times gridsize
							if (mBackgroundImage == null)
							{
								mGridBitmap = new Bitmap(Convert.ToInt32(gridWidth * 10), Convert.ToInt32(gridHeight * 10), PixelFormat.Format32bppPArgb);
							}
							else
							{
								mGridBitmap = new Bitmap(mBackgroundImage.Width, mBackgroundImage.Height, PixelFormat.Format32bppPArgb);
							}

							//Get the graphics object from the gridmap, clear it or paint it with the background
							gridGraphics = Graphics.FromImage(mGridBitmap);
							if (mBackgroundImage == null)
							{
								gridGraphics.Clear(mBackcolor);
							}
							else
							{
								gridGraphics.DrawImageUnscaled(mBackgroundImage, new Point(0, 0));
							}

							RenderGrid(gridGraphics, 0, 0, gridWidth, gridHeight, mGridBitmap.Width, mGridBitmap.Height);
						}
						
						//Set up grid texture rectangle
						RectangleF textureRectangle = renderRectangle;

						//The grid is shifted to compensate for the page offset and therefore the renderrectangle needs to be increased slightly to make up for the shift
						if (Paged) textureRectangle = new RectangleF(renderRectangle.X,renderRectangle.Y,renderRectangle.Width+gridWidth,renderRectangle.Height+gridHeight);
						
						//Adjust grid texture position for paged offset and render rectangle
						graphics.TranslateTransform(-renderRectangle.X, -renderRectangle.Y);
						if (Paged) graphics.TranslateTransform(-gridOffsetX,-gridOffsetY);

						TextureBrush brush = new TextureBrush(mGridBitmap);
						graphics.FillRectangle(brush, textureRectangle);
						
						//Reset transform
						if (Paged) graphics.TranslateTransform(gridOffsetX,gridOffsetY);
						graphics.TranslateTransform(renderRectangle.X, renderRectangle.Y);
					}
					else
					{
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
					}
					mGraphicsStateBitmap = (Bitmap) mBitmap.Clone();
				}
				else
				{
					DisposeBufferBitmap();
					mBitmap = (Bitmap) mGraphicsStateBitmap.Clone();
					graphics = Graphics.FromImage(mBitmap);
				}

				//Set up the transform matrix
				mTransform = new Matrix();

				//Set up the page transform
				//Translate the offset
				if (Paged && !mPagedOffset.IsEmpty)
				{
					mTransform.Translate(mPagedOffset.X,mPagedOffset.Y);

					//Disable page clipping
					graphics.ResetClip();
				}

				//Set the scale and world transformation
				if (mZoomPerc != 100) mTransform.Scale(mScaleFactor, mScaleFactor);
				mTransform.Translate(-renderRectangle.X * mZoomFactor, -renderRectangle.Y * mZoomFactor);
				
				//Apply transform matrix
				graphics.Transform = mTransform;

				//Draw page outlines
				if (DrawPageLines && !PageLineSize.IsEmpty && !Locked) RenderPageLines(graphics,PageLineSize, DiagramSize);
				
				//Set the drawing options
				graphics.CompositingMode = mCompositingMode;
				graphics.CompositingQuality = mCompositingQuality;
				graphics.PixelOffsetMode = mPixelOffsetMode;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error getting render graphics" + ex.ToString());

			}
			return graphics;
		}

		private void RenderDiagramImplementation(Rectangle renderRectangle)
		{
			if (! Suspended)
			{
				if (! renderRectangle.Equals(mRenderRectangle)) DisposeGraphicsStateBitmap();
				
				Graphics graphics = CreateGraphics(renderRectangle);
				if (graphics==null) return;			

				OnPreRender(graphics);

				//Render the elements if the renderer isnt locked
				if (!mLocked) RenderDiagramElements(graphics);					
				
				//Render the decorations
				if (mDrawDecorations) RenderDecorations(graphics, renderRectangle);

				OnPostRender(graphics);

				graphics.Dispose();
			}
		}

		//Loop through layers and elements and render
		public virtual void RenderDiagramElements(Graphics graphics)
		{
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
								GraphicsState graphicsState = graphics.Save();
								Matrix matrix = graphics.Transform;
								matrix.Translate(element.Rectangle.X+layer.ShadowOffset.X ,element.Rectangle.Y+layer.ShadowOffset.Y);

								//Shadow is not rotated
								graphics.Transform = matrix;
								graphics.SmoothingMode = element.SmoothingMode;
								element.RenderShadow(graphics,this);
								graphics.Restore(graphicsState);
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
							Matrix matrix = graphics.Transform;

							matrix.Translate(element.Rectangle.X,element.Rectangle.Y);
							
							//Set up rotation and other transforms
							if (element is ITransformable)
							{
								ITransformable rotatable = (ITransformable) element;
								PointF center = new PointF(element.Rectangle.Width / 2, element.Rectangle.Height /2);
								matrix.RotateAt(rotatable.Rotation, center);
							}

							//Apply transform, mode, and render element
							graphics.Transform = matrix;
							graphics.SmoothingMode = element.SmoothingMode;
							element.Render(graphics,this);

							graphics.Restore(graphicsState);
						}
					}

					//Render selections
					if (DrawSelections)
					{
						foreach (Element element in mElementRenderList)
						{
							if (element is ISelectable && element.Visible)
							{
								ISelectable selectable = (ISelectable) element;
								
								if (element.Layer == layer && selectable.Selected && selectable.DrawSelected)
								{
									PointF transform;

									//Calculate the transform
									if (element is ITransformable)
									{
										ITransformable transformable = (ITransformable) element;
										transform = new PointF(transformable.TransformRectangle.X, transformable.TransformRectangle.Y);
									}
									else
									{
										transform = new PointF(element.Rectangle.X,element.Rectangle.Y);
									}
									
									//Apply and render
									graphics.TranslateTransform(transform.X, transform.Y);
									graphics.SmoothingMode = element.SmoothingMode;
									element.RenderSelection(graphics,this,this);
									graphics.TranslateTransform(-transform.X, -transform.Y);
								}
							}
						}
					}
				}
			}
			
			//Reset current layer
			mCurrentLayer = null;
		}

		protected virtual void RenderDecorations(Graphics graphics, Rectangle renderRectangle)
		{
			GraphicsState state = graphics.Save();
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			//Undo any clipping
			if (Paged) graphics.ResetClip();

			//Draw any action paths
			if (Actions != null)
			{
				foreach (Element element in Actions)
				{
					if (element.Visible)
					{
						GraphicsState graphicsState = graphics.Save();
						Matrix matrix = graphics.Transform;
						
						PointF translate = element.Rectangle.Location;

						//Translate actions for contained elements
						if (element.Container != null)
						{
							translate.X += element.Container.Offset.X;
							translate.Y += element.Container.Offset.Y;
						}
					
						//Translate to the required position
						matrix.Translate(translate.X,translate.Y);

						//Set up rotation
						if (element is ITransformable)
						{
							ITransformable rotatable = (ITransformable) element;
							PointF center = new PointF(element.Rectangle.Width / 2, element.Rectangle.Height /2);
							
							matrix.RotateAt(rotatable.Rotation, center);
						}

						//Apply transform
						graphics.Transform = matrix;

						element.RenderAction(graphics,this,this);
						
						//Restore graphics state
						graphics.Restore(graphicsState);
					}
				}
			}

			//Draw any highlights
			if (Highlights != null)
			{
				foreach (Element element in Highlights)
				{
					GraphicsState graphicsState = graphics.Save();
					Matrix matrix = graphics.Transform;

					PointF translate = element.Rectangle.Location;

					//Translate actions for contained elements
					if (element.Container != null)
					{
						translate.X += element.Container.Offset.X;
						translate.Y += element.Container.Offset.Y;
					}
					
					//Translate to the required position
					matrix.Translate(translate.X,translate.Y);
					
					//Set up rotation
					if (element is ITransformable)
					{
						ITransformable rotatable = (ITransformable) element;
						PointF center = new PointF(element.Rectangle.Width / 2, element.Rectangle.Height /2);
							
						matrix.RotateAt(rotatable.Rotation, center);
					}

					//Apply transform
					graphics.Transform = matrix;

					element.RenderHighlight(graphics,this,this);

					//Restore graphics state
					graphics.Restore(graphicsState);
				}
			}

			//Draw any decorations
			if (DecorationPath != null)
			{
				graphics.FillPath(Component.Instance.HighlightBrush,DecorationPath);
				graphics.DrawPath(Component.Instance.HighlightPen,DecorationPath);
			}

			//Reset transformation for non scaled transformations and translate for feedback and selection rectangle
			graphics.ResetTransform();
			graphics.TranslateTransform(-renderRectangle.X, -renderRectangle.Y);

			//Draw the vector if required
			if (Vector.Width != 0 || Vector.Height != 0)
			{
				PointF start = new PointF(Vector.Left, Vector.Top);
				PointF end = new PointF(Vector.Right, Vector.Bottom);

				graphics.DrawLine(Component.Instance.VectorPen, start, end);
			}
				
			//Draw any feedback if required
			if (Feedback != null && ! FeedbackLocation.IsEmpty)
			{
				graphics.SmoothingMode = SmoothingMode.None;

				Pen pen = new Pen(Color.Black);
				SolidBrush brush = new SolidBrush(Color.FromArgb(224,SystemColors.Info));
				SolidBrush textBrush = new SolidBrush(SystemColors.InfoText);
				SizeF size = graphics.MeasureString(Feedback,Component.Instance.DefaultFont);
				SizeF padding = new SizeF(2*ZoomFactor,1*ZoomFactor);
				RectangleF rectangle = new RectangleF(0,0,size.Width,size.Height);
				rectangle.Inflate(padding);

              	graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;

				graphics.TranslateTransform(FeedbackLocation.X,FeedbackLocation.Y);	
				graphics.FillRectangle(brush,rectangle); 
				graphics.DrawRectangle(pen,rectangle.X,rectangle.Y,rectangle.Width,rectangle.Height );
				graphics.DrawString(Feedback,Component.Instance.DefaultFont,textBrush,1*ZoomFactor,1*ZoomFactor);
				graphics.TranslateTransform(-FeedbackLocation.X,-FeedbackLocation.Y);
			}

			//Draw the selection rectangle if required
            if (!SelectionRectangle.IsEmpty && !SelectionRectangle.IsEmpty)
			{
                    graphics.SmoothingMode = SmoothingMode.None;

                    graphics.FillRectangle(Component.Instance.SelectionFillBrush, SelectionRectangle);
                    graphics.DrawRectangle(Component.Instance.SelectionPen , SelectionRectangle);
			}

			graphics.Restore(state);
		}
		
		protected virtual void RenderGrid(Graphics graphics, float startX, float startY, float gridWidth, float gridHeight, float totalWidth, float totalHeight)
		{
			Color gridColor;

			float x = 0;
			float y = 0;

			graphics.CompositingQuality = CompositingQuality.HighSpeed;
			graphics.SmoothingMode = SmoothingMode.None;

			//Set the grid color intensity
			if (mAlphaCorrection)
			{
				graphics.CompositingMode = CompositingMode.SourceOver;
				int intensity = Convert.ToInt32(128 * mScaleFactor);
				if (intensity > 255) intensity = 255;
				if (intensity < 20) intensity = 20;
				gridColor = Color.FromArgb(intensity, mGridColor.R, mGridColor.G, mGridColor.B);
			}
			else
			{
				graphics.CompositingMode = CompositingMode.SourceCopy;
				gridColor = mGridColor;
			}

			if (mGridStyle == GridStyle.Pixel)
			{
				for (x = startX; x <= totalWidth - 1; x = x + gridWidth)
				{
					for (y = startX; y <= totalHeight - 1; y = y + gridHeight)
					{
						mGridBitmap.SetPixel(Convert.ToInt32(x), Convert.ToInt32(y), gridColor);
					}
				}
			}
			else
			{
				Pen pen = new Pen(gridColor);
				bool swap = false;

				if (mGridStyle == GridStyle.Dot)
				{
					pen.DashStyle = DashStyle.Dot;
				}
				else if (mGridStyle == GridStyle.DashDot)
				{
					pen.DashStyle = DashStyle.DashDot;
				}
				else if (mGridStyle == GridStyle.DashDotDot)
				{
					pen.DashStyle = DashStyle.DashDotDot;
				}
				else if (mGridStyle == GridStyle.Dash)
				{
					pen.DashStyle = DashStyle.Dash;
				}

				//Draw vertical grid lines
				swap = true;
				for (x = startX; x <= totalWidth - 1; x = x + gridWidth)
				{
					//Swap styles if complex
					if (mGridStyle == GridStyle.Complex)
					{
						pen.DashStyle = (swap) ? DashStyle.Dot : DashStyle.Dash;
						swap = !swap;
					}
					graphics.DrawLine(pen, x, 0, x, totalHeight);
					
				}
				
				//Draw horizontal grid lines
				swap = true;
				for (y = startX; y <= totalHeight - 1; y = y + gridHeight)
				{
					//Swap styles if complex
					if (mGridStyle == GridStyle.Complex)
					{
						pen.DashStyle = (swap) ? DashStyle.Dot : DashStyle.Dash;
						swap = !swap;
					}
					graphics.DrawLine(pen, 0, y, totalWidth, y);
				}
				pen.Dispose();
			}
		}

		//Renders workspace and page outline
		protected virtual void RenderPage(Graphics graphics,Point pageOffset,SizeF pageSize)
		{
			graphics.Clear(WorkspaceColor);

			//Draw border
			Pen pen = new Pen(Color.FromArgb(66,65,66));
			RectangleF border = new RectangleF(pageOffset,pageSize);
			border.Inflate(1,1);
			graphics.DrawRectangle(pen,border.X,border.Y,border.Width,border.Height);
		}

		//Render lines to show page outlines
		protected virtual void RenderPageLines(Graphics graphics,SizeF pageSize,SizeF DiagramSize)
		{
			Pen pen = Component.Instance.PageOutlinePen;

			//Draw vertical lines
			for (float x = pageSize.Width; x < DiagramSize.Width; x += pageSize.Width)
			{
				graphics.DrawLine(pen,x,0,x,DiagramSize.Height);
			}

			//Draw horizontal lines
			for (float y = pageSize.Height; y < DiagramSize.Height; y += pageSize.Height )
			{
				graphics.DrawLine(pen,0,y,DiagramSize.Width,y);
			}
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

			//Half the intensity if locked ie is an action
			if (Locked) intensity = Convert.ToInt32(intensity * 0.75);
			
			if (intensity > 255) intensity = 255;
			if (intensity < 20) intensity = 20;

			return Color.FromArgb(intensity, color.R, color.G, color.B);
		}

#endregion

	}
}
