using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	sealed public class Component: IDisposable	
	{
		//Singleton instance
		public static readonly Component Instance = new Component();
		
		//Property variables
		private StringFormat mStringFormat;
		private StringFormatFlags mStringFormatFlags;
		private Font mFont;
		private Pen mSelectionPen;
		private Pen mSelectionStartPen;
		private Pen mSelectionEndPen;
		private Pen mSelectionRotatePen;
		private Pen mExpandPen;
		private Pen mDefaultPen;
		private SolidBrush mSelectionBrush;
		private SolidBrush mSelectionStartBrush;
		private SolidBrush mSelectionEndBrush;
		private SolidBrush mSelectionFillBrush;
		private SolidBrush mSelectionRotateBrush;
		private SolidBrush mExpandBrush;
		private Pen mSelectionHatchPen;
		private Pen mActionPen;
		private SolidBrush mActionBrush;
		private Pen mHighlightPen;
		private SolidBrush mHighlightBrush;
		private Pen mVectorPen;
		private Pen mPageOutlinePen;
		private byte mDefaultOpacity = 100;
		private float mDefaultBorderWidth = 1;
		private int mDragScrollInterval = 500;
		private int mDragScrollAmount = 100;
		private PointF mDefaultShadowOffset = new PointF(5, 5);
		private Color mDefaultShadowColor = Color.FromArgb(48, 0, 0, 0);
		private GraphicsPath mDefaultHandlePath;
		private GraphicsPath mDefaultLargeHandlePath;
		private StencilItem mDefaultStencilItem;
		private SizeF mDefaultSize = new SizeF(100, 60);
		private SizeF mPagePadding = new SizeF(40,40);
		private bool mHideActions = false;
		private ActionStyle mDefaultActionStyle = ActionStyle.Default;
		private float mRoundingRadius = 6;
		private KeyCreateMode mKeyCreateMode = KeyCreateMode.Normal;
		private bool mSmoothScrolling;
		private bool mClipExport = true;

		private Hashtable mFonts; 
		private Hashtable mBitmaps;
		private Hashtable mResources;
		private Hashtable mStencils;
		private Hashtable mCursors;
		private Hashtable mResourceCursors;

		private Units mUnits;

		private License mLicense;
		private SerializationBinder mBinder;

		private Handle mDefaultHandle;

		#region Interface

		//Constructor
		private Component() {}

		//properties
		public bool HideActions
		{
			get
			{
				return mHideActions;
			}
			set
			{
				mHideActions = value;
			}
		}

		public ActionStyle DefaultActionStyle
		{
			get
			{
				return mDefaultActionStyle;
			}
			set
			{
				mDefaultActionStyle = value;
			}
		}

		//Gets or sets the default string format
		//Can be set to eg automatically provide right to left
		public StringFormat DefaultStringFormat
		{
			get
			{
				if (mStringFormat == null) CreateDefaultStringFormat();
				return mStringFormat;
			}
			set
			{
				mStringFormat = value;
			}
		}

		public StringFormatFlags DefaultStringFormatFlags
		{
			get
			{
				return mStringFormatFlags;
			}
			set
			{
				mStringFormatFlags = value;
			}
		}

		public Font DefaultFont
		{
			get
			{
				if (mFont == null) CreateDefaultFont();
				return mFont;
			}
			set
			{
				mFont = value;
			}
		}

		public Handle DefaultHandle
		{
			get
			{
				if (mDefaultHandle == null) CreateDefaultHandle();
				return mDefaultHandle;
			}
		}

		public Units Units
		{
			get
			{
				if (mUnits == null) CreateDefaultUnits();
				return mUnits;
			}
			set
			{
				mUnits = value;
			}
		}

		public SerializationBinder DefaultBinder
		{
			get
			{
				if (mBinder == null) CreateDefaultBinder();
				return mBinder;
			}
			set
			{
				mBinder = value;
			}
		}

		public StencilItem DefaultStencilItem
		{
			get
			{
				if (mDefaultStencilItem  == null) CreateDefaultStencilItem();
				return mDefaultStencilItem;
			}
			set
			{
				mDefaultStencilItem = value;
			}
		}

		public byte DefaultOpacity
		{
			get
			{
				return mDefaultOpacity;
			}
			set
			{
				mDefaultOpacity = value;
			}
		}

		public float DefaultBorderWidth
		{
			get
			{
				return mDefaultBorderWidth;
			}
			set
			{
				mDefaultBorderWidth = value;
			}
		}

		public int DragScrollInterval
		{
			get
			{
				return mDragScrollInterval;
			}
			set
			{
				mDragScrollInterval = value;
			}
		}

		public int DragScrollAmount
		{
			get
			{
				return mDragScrollAmount;
			}
			set
			{
				mDragScrollAmount = value;
			}
		}

		public float RoundingRadius
		{
			get
			{
				return mRoundingRadius;
			}
			set
			{
				mRoundingRadius = value;
			}
		}

		public Pen DefaultPen
		{
			get
			{
				if (mDefaultPen == null) CreateDefaultPen();
				return mDefaultPen;
			}
			set
			{
				mDefaultPen = value;
			}
		}

		public Pen SelectionPen
		{
			get
			{
				if (mSelectionPen == null) CreateSelectionPen();
				return mSelectionPen;
			}
			set
			{
				mSelectionPen = value;
			}
		}

		public Pen SelectionStartPen
		{
			get
			{
				if (mSelectionStartPen == null) CreateSelectionStartPen();
				return mSelectionStartPen;
			}
			set
			{
				mSelectionStartPen = value;
			}
		}

		public Pen SelectionEndPen
		{
			get
			{
				if (mSelectionEndPen == null) CreateSelectionEndPen();
				return mSelectionEndPen;
			}
			set
			{
				mSelectionEndPen = value;
			}
		}

		public Pen SelectionRotatePen
		{
			get
			{
				if (mSelectionRotatePen == null) CreateSelectionRotatePen();
				return mSelectionRotatePen;
			}
			set
			{
				mSelectionRotatePen = value;
			}
		}

		public Pen ExpandPen
		{
			get
			{
				if (mExpandPen == null) CreateExpandPen();
				return mExpandPen;
			}
			set
			{
				mExpandPen = value;
			}
		}

		public Color DefaultShadowColor
		{
			get
			{
				return mDefaultShadowColor;
			}
			set
			{
				mDefaultShadowColor = value;
			}
		}

		public PointF DefaultShadowOffset
		{
			get
			{
				return mDefaultShadowOffset;
			}
			set
			{
				mDefaultShadowOffset = value;
			}
		}

		public SizeF DefaultSize
		{
			get
			{
				return mDefaultSize;
			}
			set
			{
				mDefaultSize = value;
			}
		}

		public GraphicsPath DefaultHandlePath
		{
			get
			{
				if (mDefaultHandlePath == null) CreateDefaultHandlePath();
				return mDefaultHandlePath;
			}
			set
			{
				mDefaultHandlePath = value;
			}
		}

		public GraphicsPath DefaultLargeHandlePath
		{
			get
			{
				if (mDefaultLargeHandlePath == null) CreateDefaultLargeHandlePath();
				return mDefaultLargeHandlePath;
			}
			set
			{
				mDefaultLargeHandlePath = value;
			}
		}

		public SolidBrush SelectionBrush
		{
			get
			{
				if (mSelectionBrush == null) CreateSelectionBrush();
				return mSelectionBrush;
			}
			set
			{
				mSelectionBrush = value;
			}
		}

		public SolidBrush SelectionStartBrush
		{
			get
			{
				if (mSelectionStartBrush == null) CreateSelectionStartBrush();
				return mSelectionStartBrush;
			}
			set
			{
				mSelectionStartBrush = value;
			}
		}

		public SolidBrush SelectionEndBrush
		{
			get
			{
				if (mSelectionEndBrush == null) CreateSelectionEndBrush();
				return mSelectionEndBrush;
			}
			set
			{
				mSelectionEndBrush = value;
			}
		}

		public SolidBrush SelectionRotateBrush
		{
			get
			{
				if (mSelectionRotateBrush == null) CreateSelectionRotateBrush();
				return mSelectionRotateBrush;
			}
			set
			{
				mSelectionRotateBrush = value;
			}
		}

		public SolidBrush ExpandBrush
		{
			get
			{
				if (mExpandBrush == null) CreateExpandBrush();
				return mExpandBrush;
			}
			set
			{
				mExpandBrush = value;
			}
		}

		public Pen ActionPen
		{
			get
			{
				if (mActionPen == null) CreateActionPen();
				return mActionPen;
			}
			set
			{
				mActionPen = value;
			}
		}

		public SolidBrush ActionBrush
		{
			get
			{
				if (mActionBrush == null) CreateActionBrush();
				return mActionBrush;
			}
			set
			{
				mActionBrush = value;
			}
		}

		public Pen HighlightPen
		{
			get
			{
				if (mHighlightPen == null) CreateHighlightPen();
				return mHighlightPen;
			}
			set
			{
				mHighlightPen = value;
			}
		}

		public SolidBrush HighlightBrush
		{
			get
			{
				if (mHighlightBrush == null) CreateHighlightBrush();
				return mHighlightBrush;
			}
			set
			{
				mHighlightBrush = value;
			}
		}

		public Pen VectorPen
		{
			get
			{
				if (mVectorPen == null) CreateVectorPen();
				return mVectorPen;
			}
			set
			{
				mVectorPen = value;
			}
		}

		public Pen PageOutlinePen
		{
			get
			{
				if (mPageOutlinePen == null) CreatePageOutlinePen();
				return mPageOutlinePen;
			}
			set
			{
				mPageOutlinePen = value;
			}
		}

		public SolidBrush SelectionFillBrush
		{
			get
			{
				if (mSelectionFillBrush == null) CreateSelectionFillBrush();
				return mSelectionFillBrush;
			}
			set
			{
				mSelectionFillBrush = value;
			}
		}

		public Pen SelectionHatchPen
		{
			get
			{
				if (mSelectionHatchPen == null) CreateSelectionHatchPen();
				return mSelectionHatchPen;
			}
			set
			{
				mSelectionHatchPen = value;
			}
		}

		public SizeF PagePadding
		{
			get
			{
				return mPagePadding;
			}
			set
			{
				mPagePadding = value;
			}
		}

		public Hashtable Fonts
		{
			get
			{
				return mFonts;
			}
		}

		public Hashtable Bitmaps
		{
			get
			{
				return mBitmaps;
			}
		}

		public Hashtable Resources
		{
			get
			{
				return mResources;
			}
		}

		public Hashtable Stencils
		{
			get
			{
				return mStencils;
			}
		}

		public Hashtable Cursors
		{
			get
			{
				return mCursors;
			}
			set
			{
				mCursors = value;
			}
		}

		public KeyCreateMode KeyCreateMode
		{
			get
			{
				return mKeyCreateMode;
			}
			set
			{
				mKeyCreateMode = value;
			}
		}

		public bool SmoothScrolling
		{
			get
			{
				return mSmoothScrolling;
			}
			set
			{
				mSmoothScrolling = value;
			}
		}

		public bool ClipExport
		{
			get
			{
				return mClipExport;
			}
			set
			{
				mClipExport = value;
			}
		}

		//Methods
        public static void Preload(string assemblyName)
        {
            Assembly.LoadWithPartialName(assemblyName);
        }

		public License GetLicense(System.Type type, object instance)
		{
			if (mLicense == null) LicenseManager.Validate(type, instance);
			return mLicense;
		}

		public Cursor GetCursor(HandleType type)
		{
			if (mCursors == null) CreateCursors();
			return mCursors[type] as Cursor;
		}

		public Cursor LoadCursor(string resource)
		{
			if (mResourceCursors == null) mResourceCursors = new Hashtable();
			
			if (mResourceCursors.ContainsKey(resource))
			{
				return (Cursor) mResourceCursors[resource];
			}
			else
			{
				Cursor cursor = new Cursor(typeof(Crainiate.Diagramming.Component), resource);
				mResourceCursors.Add(resource, cursor);
				
				return cursor;
			}
		}

		public Graphics CreateGraphics()
		{
			Bitmap temp = new Bitmap(1,1,System.Drawing.Imaging.PixelFormat.Format32bppPArgb);
			return Graphics.FromImage(temp);
		}

		public Stencil GetStencil(Type stencilType)
		{
			if (mStencils == null) mStencils = new Hashtable();

			if (mStencils.ContainsKey(stencilType))
			{
				return (Stencil) mStencils[stencilType];
			}
			else
			{
				object stencil = Activator.CreateInstance(stencilType);
				mStencils.Add(stencilType,stencil);
				return (Stencil) stencil;
			}
		}
		
		public Font GetFont(string name,float size,FontStyle fontStyle)
		{
			Font font;
			string key = name + size.ToString() + Convert.ToInt32(fontStyle).ToString();
			
			if (mFonts == null) mFonts = new Hashtable();
			
			if (mFonts.ContainsKey(key))
			{
				return (Font) mFonts[key];
			}
			else
			{
				font = new Font(name,size,fontStyle);
				mFonts.Add(key,font);
				return font;
			}
		}

		public FontStyle GetFontStyle(bool bold, bool italic, bool underline, bool strikeout)
		{
			FontStyle style = FontStyle.Regular;

			if (bold) style  = style | FontStyle.Bold;
			if (italic) style = style | FontStyle.Italic;
			if (underline) style = style | FontStyle.Underline;
			if (strikeout) style = style| FontStyle.Strikeout;
			
			return style;
		}

		//Returns a bitmap from the internal resource file
		public Bitmap GetResource(string name,string assembly)
		{
			Bitmap bitmap;
			string key = name+assembly;

			if (mResources == null) mResources = new Hashtable();				

			if (mResources.ContainsKey(key))
			{
				return (Bitmap) mResources[key];
			}
			else
			{
				//Load from assembly
				if (assembly == "")
				{
					bitmap = new Bitmap(typeof(Component),name);
				}
				else
				{
                    Type type = GetAnyAssemblyType(assembly);

                    if (type == null) throw new ApplicationException("Assembly type " + assembly + " does not contain resource " + name + ". Make sure the resource is an embedded resource and the reference is included.");

					bitmap = new Bitmap(type, name);
				}
				mResources.Add(key,bitmap);
				return bitmap;
			}
		}

		//Returns a resource from disk
		public Bitmap GetBitmap(string path)
		{
			Bitmap bitmap;

			if (mBitmaps == null) mBitmaps = new Hashtable();

			if (mBitmaps.ContainsKey(path)) 
			{
				return (Bitmap) mBitmaps[path];
			}
			else
			{
				//Check if must load image from uri 
				if (path.Substring(0,7) == "http://" || path.Substring(0,8) == "https://")
				{
					try
					{
						System.IO.Stream imageStream = new System.Net.WebClient().OpenRead(path);
						bitmap = (Bitmap) System.Drawing.Image.FromStream(imageStream);
					}
					catch
					{
						throw new ComponentException("An image could not be loaded from the URI provided: " + path);
					}
				}
				else
				{
					try
					{
						bitmap = new Bitmap(path);
					}
					catch
					{
						throw new ComponentException("An image could not be loaded from the path specified: " + path);
					}
				}
				mBitmaps.Add(path,bitmap);
				return bitmap;
			}
		}	
	
		public void Dispose()
		{
			if (mLicense != null) 
			{
				mLicense.Dispose();
				mLicense = null;
			}
		}

		#endregion

		#region Implementation

		private void CreateDefaultStencilItem()
		{
			mDefaultStencilItem = new StencilItem(); 
			mDefaultStencilItem.Redraw = true;
		}

		private void CreateDefaultStringFormat()
		{
			mStringFormat = new StringFormat();
			mStringFormat.Alignment = StringAlignment.Center;
			mStringFormat.LineAlignment = StringAlignment.Center;
		}

		private void CreateDefaultHandlePath()
		{
			mDefaultHandlePath = new GraphicsPath();
			mDefaultHandlePath.AddEllipse(0,0,6,6);
			//mDefaultHandlePath.AddRectangle(new Rectangle(0,0,6,6));
		}

		private void CreateDefaultLargeHandlePath()
		{
			mDefaultLargeHandlePath = new GraphicsPath();
			
			mDefaultLargeHandlePath.AddArc(0, 3, 3, 6, 90, 180);
			mDefaultLargeHandlePath.AddArc(10, 3, 3, 6, 270, 180);
			mDefaultLargeHandlePath.CloseFigure();

			Geometry.MovePathToOrigin(mDefaultLargeHandlePath);
		}

		private void CreateDefaultFont()
		{
			mFont = new Font("Microsoft Sans Serif",8.25F);
		}

		private void CreateDefaultHandle()
		{
			mDefaultHandle = new Handle();
			mDefaultHandle.Type = HandleType.Move;
		}

		private void CreateDefaultUnits()
		{
			mUnits = new Units();
		}

		private void CreateDefaultBinder()
		{
			mBinder = new DiagramBinder();
		}
		private void CreateSelectionPen()
		{
			mSelectionPen = new Pen(Color.FromArgb(160,Color.FromArgb(10,36,106)),-1);
		}

		private void CreateDefaultPen()
		{
			mDefaultPen = new Pen(Color.Black);
			mDefaultPen.Width = 2;
		}

		private void CreateSelectionBrush()
		{
			mSelectionBrush = new SolidBrush(Color.FromArgb(24,Color.FromArgb(10,36,106)));
		}

		private void CreateSelectionStartPen()
		{
			mSelectionStartPen = new Pen(Color.FromArgb(192,Color.Green),-1);
		}

		private void CreateSelectionStartBrush()
		{
			mSelectionStartBrush = new SolidBrush(Color.FromArgb(32,Color.Green));
		}

		private void CreateSelectionEndPen()
		{
			mSelectionEndPen = new Pen(Color.FromArgb(192,Color.Red),-1);
		}

		private void CreateSelectionRotatePen()
		{
			mSelectionRotatePen = new Pen(Color.FromArgb(160,Color.FromArgb(10,36,106)),-1);
		}

		private void CreateExpandPen()
		{
			mExpandPen = new Pen(Color.FromArgb(160,Color.Black),-1);
		}

		private void CreateSelectionEndBrush()
		{
			mSelectionEndBrush = new SolidBrush(Color.FromArgb(32,Color.Red));
		}

		private void CreateSelectionFillBrush()
		{
			mSelectionFillBrush = new SolidBrush(Color.FromArgb(12,Color.FromArgb(10,36,106)));
		}

		private void CreateSelectionRotateBrush()
		{
			mSelectionRotateBrush = new SolidBrush(Color.FromArgb(24,Color.FromArgb(10,36,106)));
		}

		private void CreateExpandBrush()
		{
			mExpandBrush = new SolidBrush(Color.White);
		}

		private void CreateSelectionHatchPen()
		{
			HatchBrush brush = new HatchBrush(HatchStyle.Percent25, Color.FromArgb(48,Color.FromArgb(10,36,106)),Color.FromArgb(8,Color.FromArgb(10,36,106)));
			mSelectionHatchPen = new Pen(brush,6);
		}

		private void CreateActionPen()
		{
			mActionPen = new Pen(Color.FromArgb(128,Color.FromArgb(10,36,106)),1);
		}

		private void CreateActionBrush()
		{
			mActionBrush = new SolidBrush(Color.FromArgb(32,Color.FromArgb(10,36,106)));
		}

		private void CreateHighlightPen()
		{
			mHighlightPen = new Pen(Color.FromArgb(64,Color.FromArgb(10,36,106)),5);
			mHighlightPen.DashStyle = DashStyle.Dash;
			mHighlightPen.Alignment = PenAlignment.Center;
		}

		private void CreateVectorPen()
		{
			mVectorPen = new Pen(Color.FromArgb(128,Color.FromArgb(10,36,106)),1);
			mVectorPen.DashStyle = DashStyle.Dash;
			mVectorPen.Alignment = PenAlignment.Center;
		}

		private void CreateHighlightBrush()
		{
			mHighlightBrush = new SolidBrush(Color.FromArgb(32,Color.FromArgb(10,36,106)));
		}

		private void CreatePageOutlinePen()
		{
			mPageOutlinePen = new Pen(Color.FromArgb(128,Color.Teal),1);
			mPageOutlinePen.DashStyle = DashStyle.Dot;
		}

		private void CreateCursors()
		{
			mCursors = new Hashtable();

			mCursors.Add(HandleType.Move, System.Windows.Forms.Cursors.SizeAll);
			mCursors.Add(HandleType.Bottom, System.Windows.Forms.Cursors.SizeNS);
			mCursors.Add(HandleType.BottomLeft, System.Windows.Forms.Cursors.SizeNESW);
			mCursors.Add(HandleType.BottomRight, System.Windows.Forms.Cursors.SizeNWSE);
			mCursors.Add(HandleType.Left, System.Windows.Forms.Cursors.SizeWE);
			mCursors.Add(HandleType.Right, System.Windows.Forms.Cursors.SizeWE);
			mCursors.Add(HandleType.Top, System.Windows.Forms.Cursors.SizeNS);
			mCursors.Add(HandleType.TopLeft, System.Windows.Forms.Cursors.SizeNWSE);
			mCursors.Add(HandleType.TopRight, System.Windows.Forms.Cursors.SizeNESW);
			mCursors.Add(HandleType.Origin, System.Windows.Forms.Cursors.PanNorth);
			mCursors.Add(HandleType.LeftRight, System.Windows.Forms.Cursors.SizeWE);
			mCursors.Add(HandleType.UpDown, System.Windows.Forms.Cursors.SizeNS);
			mCursors.Add(HandleType.Rotate, LoadCursor("Resource.rotate.cur"));
			mCursors.Add(HandleType.Expand, System.Windows.Forms.Cursors.PanEast);
			mCursors.Add(HandleType.None, System.Windows.Forms.Cursors.Arrow);
		}

        private Type GetAnyAssemblyType(string typeName)
        {
            Assembly[] assemblyArray = AppDomain.CurrentDomain.GetAssemblies();

            foreach (Assembly assembly in assemblyArray)
            {
                if (!assembly.FullName.StartsWith("mscorlib") && !assembly.FullName.StartsWith("System"))
                {
                    Type type = assembly.GetType(typeName);
                    if (type != null) return type;
                }
            }

            return null;
        }

		#endregion
	}
}

