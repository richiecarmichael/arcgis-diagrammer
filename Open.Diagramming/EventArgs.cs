using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public class LocationChangedEventArgs : EventArgs 
	{
		private PointF mLocation;

		//Constructor
		public LocationChangedEventArgs(PointF location)
		{
			mLocation = location;
		}

		//Properties
		public PointF Location
		{
			set
			{
				mLocation = value;
			}
			get
			{
				return mLocation;
			}
		}
	}

	public class SizeChangedEventArgs : EventArgs 
	{
		private RectangleF mRectangle;

		//Contructor
		public SizeChangedEventArgs(RectangleF rect)
		{
			mRectangle = rect;
		}
		
		//Properties
		public RectangleF Rectangle
		{
			set
			{
				mRectangle = value;
			}
			get
			{
				return mRectangle;
			}
		}
	}

	public class RotationChangedEventArgs : EventArgs 
	{
		private float mRotation;

		//Constructor
		public RotationChangedEventArgs(float degrees)
		{
			mRotation = degrees;
		}

		//Properties
		public float Rotation
		{
			set
			{
				mRotation = value;
			}
			get
			{
				return mRotation;
			}
		}
	}

	public class ElementEventArgs: EventArgs
	{
		private Element mValue;

		public ElementEventArgs(Element value)
		{
			mValue = value;
		}
		public Element Value
		{
			get
			{
				return mValue;
			}
		}
	}

	public class ElementsEventArgs: EventArgs
	{

		private string mKey;
		private Element mValue;

		public ElementsEventArgs(string key,Element value)
		{
			mKey = key;
			mValue = value;
		}

		public string Key
		{
			get
			{
				return mKey;
			}
		}

		public Element Value
		{
			get
			{
				return mValue;
			}
		}
	}

	public class TableItemsEventArgs: EventArgs
	{
		private TableItem mValue;

		public TableItemsEventArgs(TableItem value)
		{
			mValue = value;
		}

		public TableItem Value
		{
			get
			{
				return mValue;
			}
		}
	}

	public class SegmentsEventArgs: EventArgs
	{
		private Segment mValue;

		public SegmentsEventArgs(Segment value)
		{
			mValue = value;
		}

		public Segment Value
		{
			get
			{
				return mValue;
			}
		}
	}

	public class RenderEventArgs: EventArgs
	{
		private Graphics mGraphics;

		//Constructor
		public RenderEventArgs(Graphics graphics)
		{
			mGraphics = graphics;
		}

		public Graphics Graphics
		{
			get
			{
				return mGraphics;
			}
		}
	}

	public class SerializationEventArgs: EventArgs
	{
		private IFormatter mFormatter;
		private SurrogateSelector mSelector;

		public SerializationEventArgs(IFormatter formatter, SurrogateSelector selector)
		{
			mFormatter = formatter;
			mSelector = selector;
		}

		public IFormatter Formatter
		{
			get
			{
				return mFormatter;
			}
		}

		public SurrogateSelector Selector
		{
			get
			{
				return mSelector;
			}
		}
	}

	public class SerializationCompleteEventArgs: SerializationEventArgs
	{
		private object mGraph;
		
		public SerializationCompleteEventArgs(object graph, IFormatter formatter, SurrogateSelector selector): base(formatter,selector)
		{
			mGraph = graph;
		}

		public object Graph
		{
			get
			{
				return mGraph;
			}
		}
	}

	public class DrawShapeEventArgs: EventArgs
	{
		private GraphicsPath mGraphicsPath;
		private float mWidth;
		private float mHeight;

		//Constructor
		public DrawShapeEventArgs(GraphicsPath path, float width, float height)
		{
			mGraphicsPath = path;
			mWidth = width;
			mHeight = height;
		}

		public GraphicsPath Path
		{
			get
			{
				return mGraphicsPath;
			}
		}

		public float Width
		{
			get
			{
				return mWidth;
			}
		}

		public float Height
		{
			get
			{
				return mHeight;
			}
		}
	}

	public class UserActionEventArgs: EventArgs
	{
		private RenderList mActions;

		public UserActionEventArgs(RenderList actions)
		{
			mActions = actions;
		}
		public RenderList Actions
		{
			get
			{
				return mActions;
			}
		}
	}

}


