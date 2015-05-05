using System;
using System.Drawing;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Origin : ISerializable
	{
		//Property variables
		private PointF mLocation;
		private Shape mShape;
		private string mShapeKey;
		private Port mPort;
		private string mPortKey;

		private MarkerBase mMarker;
		private bool mDocked;
		private bool mAllowMove;
		private bool mSuspendEvents;

		private Line mLine;

		private LocationChangedEventHandler mLocationChangedEventHandler;

		#region Interface

		public event EventHandler OriginInvalid;
		public event EventHandler DockChanged;

		//Constructors
		public Origin()
		{
			SuspendEvents = true;
			AllowMove = true;
			SuspendEvents = false;
		}

		public Origin(PointF location)
		{	
			SuspendEvents = true;
			Location = location;
			AllowMove = true;
			SuspendEvents = false;
		}

		public Origin(Shape shape)
		{
			SuspendEvents = true;
			Shape = shape;
			AllowMove = true;
			SuspendEvents = false;
		}

		public Origin(Port port)
		{
			SuspendEvents = true;
			Port = port;
			AllowMove = true;
			SuspendEvents = false;
		}

		//Deserializes info into a new origin
		protected Origin(SerializationInfo info, StreamingContext context)
		{
			Location = Serialize.GetPointF(info.GetString("Location"));
			Marker = (MarkerBase) info.GetValue("Marker",typeof(MarkerBase));
			AllowMove = info.GetBoolean("AllowMove");
			
			if (Serialize.Contains(info,"Shape",typeof(Shape))) Shape = (Shape) info.GetValue("Shape",typeof(Shape));
			if (Serialize.Contains(info,"Port",typeof(Port))) Port = (Port) info.GetValue("Port",typeof(Port));
		}

		//Properties
		//Sets a location as the origin
		public virtual PointF Location
		{
			get
			{
				return mLocation;
			}
			set
			{
				if (! mLocation.Equals(value))
				{
					RemoveHandlers();

					//Store if the dock has changed
					bool changed = (mShape != null || mPort != null);

					mLocation = value;
					mShape = null;
					mShapeKey = null;
					mPort = null;
					mPortKey = null;
					mDocked = false;
					OnOriginInvalid();

					//Check to see if must fire dock changed event
					if (changed) OnDockChanged();
				}
			}
		}

		//Sets a shape as the origin
		public virtual Shape Shape
		{
			get
			{
				return mShape;
			}
			set
			{
				if (mShape != value)
				{
					RemoveHandlers();

					mShape = value;
					mShapeKey = null;
					mPort = null;
					mPortKey = null;
					mLocation = new PointF();
					mDocked = true;

					mShape.LocationChanged += new LocationChangedEventHandler(Element_LocationChanged);
					mShape.SizeChanged += new SizeChangedEventHandler(Element_SizeChanged);
					mShape.RotationChanged += new RotationChangedEventHandler(Element_RotationChanged);

					OnOriginInvalid();
					OnDockChanged();
				}
			}
		}

		//Sets a shape key as the origin
		protected internal virtual string ShapeKey
		{
			get
			{
				return mShapeKey;
			}
			set
			{
				if (mShapeKey != value)
				{
					mShapeKey = value;
					mShape = null;
					mPort = null;
					mPortKey = null;
					mLocation = new PointF();

					OnOriginInvalid();
				}
			}
		}

		//Sets a port as the origin
		public virtual Port Port
		{
			get
			{
				return mPort;
			}
			set
			{
				if (mPort != value)
				{
					RemoveHandlers();

					mPort = value;
					mPortKey = null;
					mShape = null;
					mShapeKey = null;
					mLocation = new PointF();
					mDocked = true;

					mPort.LocationChanged += new LocationChangedEventHandler(Element_LocationChanged);

					OnOriginInvalid();
					OnDockChanged();
				}
			}
		}

		//Sets a Port key as the origin
		protected internal virtual string PortKey
		{
			get
			{
				return mPortKey;
			}
			set
			{
				if (mPortKey != value)
				{
					mPortKey = value;
					mPort = null;
					mShape = null;
					mLocation = new PointF();
					OnOriginInvalid();
				}
			}
		}
		
		//sets the marker at the start of the line
		public virtual MarkerBase Marker
		{
			get
			{
				return mMarker;
			}
			set
			{
				if (mMarker != null) mMarker.ElementInvalid -=new EventHandler(Marker_ElementInvalid);

				mMarker = value;
				if (value != null) mMarker.ElementInvalid +=new EventHandler(Marker_ElementInvalid);
				
				OnOriginInvalid();
			}
		}

		//Determines whether the origin is docked to a shape or port
		public virtual bool Docked
		{
			get
			{
				return mDocked;
			}
		}

		//Returns the current directly docked shape or port docked shape
		public virtual Element DockedElement
		{
			get
			{
				//Can be a line or a shape
				if (Port != null) return (Element) Port.Parent;
				return Shape;
			}
		}

		//Indicates whether the origin can be moved at runtime
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

		//Returns the parent line for this origin
		public virtual Line Parent
		{
			get
			{
				return mLine;
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

		//Methods
		public virtual void Move(float dx, float dy)
		{
			Location = new PointF(Location.X+dx,Location.Y+dy);	
		}

		//Resolve all string key references
		public virtual void Resolve(Elements Shapes)
		{
			//Add check for speed
			if (ShapeKey == null) return;
			
			SuspendEvents = true;
			
			//Resolve keys
			Shape shape = (Shape) Shapes[ShapeKey];

			if (PortKey != null) 
			{
				Port = (Port) shape.Ports[PortKey];
				shape = null;
			}
			Shape = shape;

			//Reset to blank
			mShapeKey = null;
			mPortKey = null;

			SuspendEvents = false;
		}

		protected internal void SetLine(Line line)
		{
			mLine = line;
		}

		//Raises the OriginInvalid event
		protected virtual void OnOriginInvalid()
		{
			if (!SuspendEvents && OriginInvalid != null) OriginInvalid(this,EventArgs.Empty);
		}

		//Raises the DockChanged event
		protected virtual void OnDockChanged()
		{
			if (!SuspendEvents && DockChanged != null) DockChanged(this,EventArgs.Empty);
		}

		#endregion

		#region Events

		//Handles marker invalid events
		private void Marker_ElementInvalid(object sender, EventArgs e)
		{
			OnOriginInvalid();
		}

		private void Element_LocationChanged(object sender, LocationChangedEventArgs e)
		{
			OnOriginInvalid();
		}

		private void Element_SizeChanged(object sender, SizeChangedEventArgs e)
		{
			OnOriginInvalid();
		}

		private void Element_RotationChanged(object sender, RotationChangedEventArgs e)
		{
			OnOriginInvalid();
		}

		#endregion

		#region Implementation

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Location",Serialize.AddPointF(Location));
			info.AddValue("Marker",Marker);
			info.AddValue("AllowMove",AllowMove);
			
			if (mShape != null) info.AddValue("Shape",Shape);
			if (mPort != null) info.AddValue("Port",Port);
		}

		internal void RemoveHandlers()
		{
			if (mShape != null)
			{
				mShape.LocationChanged -=  new LocationChangedEventHandler(Element_LocationChanged);
				mShape.SizeChanged -= new SizeChangedEventHandler(Element_SizeChanged);
				mShape.RotationChanged -= new RotationChangedEventHandler(Element_RotationChanged);
			}

			if (mPort != null)
			{
				mPort.LocationChanged -= new LocationChangedEventHandler(Element_LocationChanged);
			}
		}

		#endregion

	}
}
