using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Image: ISerializable, ICloneable
	{
		//Property variables
		private Element mParent;
		private PointF mLocation;
		private InterpolationMode mInterpolationMode = InterpolationMode.Default;

		//Working variables
		private string mPath;
		private string mResource;
		private string mAssembly;
		private bool mSuspendEvents;
		private System.Drawing.Image mImage;
		
		#region Interface

		//Events
		public event EventHandler ImageInvalid;

		//Constructors
		public Image()
		{
			Location = new PointF(0,0);
		}

		//Loads an image form a path
		public Image(string path)
		{
			SuspendEvents = true;
			Location = new PointF(0,0);
			mPath = path;
			SetImage(Component.Instance.GetBitmap(path));
			SuspendEvents = false;
		}

		//Loads an image from an assembly
		public Image(string resourcename,string assembly)
		{
			SuspendEvents = true;
			Location = new PointF(0,0);
			mResource = resourcename;
			mAssembly = assembly;
			
			SetImage(Component.Instance.GetResource(resourcename,assembly));
			SuspendEvents = false;
		}

		//Loads an image from an existing GDI+ image
		public Image(System.Drawing.Image image)
		{
			SuspendEvents = true;
			Location = new PointF(0,0);
			SetImage(image);
			SuspendEvents = false;
		}

		public Image(Image prototype)
		{
			mLocation = prototype.Location;
			mResource = prototype.Resource;
			mPath = prototype.Path;
		}
		
		protected Image(SerializationInfo info, StreamingContext context)
		{
			SuspendEvents = true;
			
			Location = Serialization.Serialize.GetPointF(info.GetString("Location"));
			InterpolationMode = (InterpolationMode) Enum.Parse(typeof(InterpolationMode), info.GetString("InterpolationMode"));
			
			if (Serialize.Contains(info,"Path")) mPath = info.GetString("Path");
			if (Serialize.Contains(info,"Resource"))mResource = info.GetString("Resource");
			if (Serialize.Contains(info,"Assembly"))mAssembly = info.GetString("Assembly");
			
			//Load image
			if (mPath != null) SetImage(Component.Instance.GetBitmap(mPath));
			if (mResource != null) SetImage(Component.Instance.GetResource(mResource,mAssembly));
			
			SuspendEvents = false;
		}

		//Properties
		public virtual PointF Location
		{
			get
			{
				return mLocation;
			}
			set
			{
				if (!mLocation.Equals(value))
				{
					mLocation = value;
					OnImageInvalid();
				}
			}
		}

		public virtual string Path
		{
			get
			{
				return mPath;
			}
		}

		public virtual string Resource
		{
			get
			{
				return mResource;
			}
		}
		
		public virtual string Assembly
		{
			get
			{
				return mAssembly;
			}
		}

		protected virtual bool SuspendEvents
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

		//Returns the internal image bitmap
		public virtual System.Drawing.Image Bitmap
		{
			get
			{
				return mImage;
			}
		}

		public virtual Element Parent
		{
			get
			{
				return mParent;
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

		//Methods
		public virtual void Render(Graphics graphics,IRender render)
		{
			if (Bitmap != null)
			{
				Size size = Bitmap.Size;
				Point location = Point.Round(Location);

				graphics.TranslateTransform(Location.X,Location.Y);
				graphics.InterpolationMode = InterpolationMode;
				graphics.DrawImage(Bitmap,location.X,location.Y,size.Width,size.Height);
				graphics.TranslateTransform(-Location.X,-Location.Y);
			}
		}

		protected internal void SetParent(Element parent)
		{
			mParent = parent;
		}

		protected virtual void SetImage(System.Drawing.Image image)
		{
			mImage = image;
		}

		//Raises the element invalid event.
		protected virtual void OnImageInvalid()
		{
			if (!(mSuspendEvents) && ImageInvalid!=null) ImageInvalid(this,new EventArgs());
		}

		#endregion

		#region Implementation

		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("Location",Serialization.Serialize.AddPointF(Location));
			info.AddValue("InterpolationMode",Convert.ToInt32(InterpolationMode).ToString());
			
			if (Path != null) info.AddValue("Path",Path);
			if (Resource != null) info.AddValue("Resource",Resource);
			if (Assembly != null) info.AddValue("Assembly",Assembly);
		}

		//Returns a clone of this object
		public virtual object Clone()
		{
			return new Image(this);
		}

		#endregion
	}
}
