using System;
using System.Collections;
using System.Drawing;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Layer : ISerializable ,ICloneable
	{
		//Property variables
		private LayerElements mElements;
				
		private byte mOpacity = 100;
		private bool mVisible = true;
		private string mName;

		private bool mDrawShadows = true;
		private bool mSoftShadows = true;
		private PointF mShadowOffset;
		private Color mShadowColor;
		private bool mDefault;

		//Working variables
		private bool mSuspendEvents;

		#region Interface

		//Events
		public event EventHandler LayerInvalid;

		//Constructor
		public Layer()
		{
			mElements = new LayerElements(typeof(Element),"Element");
			mElements.SetModifiable(false);
			mShadowOffset = Component.Instance.DefaultShadowOffset;
			mShadowColor = Component.Instance.DefaultShadowColor;
		}

		protected internal Layer(bool defaultLayer)
		{
			mElements = new LayerElements(typeof(Element),"Element");
			mElements.SetModifiable(false);
			mDefault = defaultLayer;
			mShadowOffset = Component.Instance.DefaultShadowOffset;
			mShadowColor = Component.Instance.DefaultShadowColor;
		}

		public Layer(Layer prototype)
		{
			mElements = new LayerElements(typeof(Element),"Element");
			mElements.SetModifiable(false);

			mOpacity = prototype.Opacity;
			mVisible = prototype.Visible;
			mName = prototype.Name;
			mDrawShadows = prototype.DrawShadows;
			mShadowOffset = prototype.ShadowOffset;
			mShadowColor = prototype.ShadowColor;
			mSoftShadows = prototype.SoftShadows;
		}
		
		//Creates a new element from the supplied XML.
		protected Layer(SerializationInfo info, StreamingContext context)
		{
			SuspendEvents = true;
			
			Opacity = info.GetByte("Opacity");
			Visible = info.GetBoolean("Visible");
			Name = info.GetString("Name");
			DrawShadows = info.GetBoolean("DrawShadows");
			ShadowOffset = Serialize.GetPointF(info.GetString("ShadowOffset"));
			ShadowColor = Color.FromArgb(Convert.ToInt32(info.GetString("ShadowColor")));
			SoftShadows = info.GetBoolean("SoftShadows");

			Elements = (LayerElements) info.GetValue("Elements",typeof(Elements));

			SuspendEvents = false;
		}

		//Properties

		//Sets the opacity of this Layer
		public virtual byte Opacity
		{
			get
			{
				return mOpacity;
			}
			set
			{
				if (value != mOpacity)
				{
					mOpacity = value;
					OnLayerInvalid();
				}
			}
		}

		//Determines if Layer is visible
		public virtual bool Visible
		{	
			get
			{
				return mVisible;
			}
			set
			{
				if (value != mVisible)
				{
					mVisible = value;
					OnLayerInvalid();
				}
			}
		}

		//Sets or gets the name of this Layer
		public virtual string Name
		{
			get
			{
				return mName;
			}
			set
			{
				mName = value;
			}
		}

		//Returns the elements in this layer
		public virtual LayerElements Elements
		{
			get
			{
				return mElements;
			}
			set
			{
				mElements = value;
				OnLayerInvalid();
			}
		}

		//Sets or retrieves a boolean value determining if the diagram renders shadows.
		public virtual bool DrawShadows
		{
			get
			{
				return mDrawShadows;
			}
			set
			{
				if (mDrawShadows != value)
				{
					mDrawShadows = value;
					OnLayerInvalid();
				}
			}
		}

		//Sets or retrieves a point used to offset the shadows in the diagram
		public virtual PointF ShadowOffset
		{
			get
			{
				return mShadowOffset;
			}
			set
			{
				if (! mShadowOffset.Equals(value))
				{
					mShadowOffset = value;
					OnLayerInvalid();
				}
			}
		}

		//Sets or retrieves the color used to render the shadows.")]
		public virtual Color ShadowColor
		{
			get
			{
				return mShadowColor;
			}
			set
			{
				mShadowColor = value;
				OnLayerInvalid();
			}
		}

		//Sets or retrieves a boolean value determining if the diagram renders shadows with an additional penumbra.
		public virtual bool SoftShadows
		{
			get
			{
				return mSoftShadows;
			}
			set
			{
				if (mSoftShadows != value)
				{
					mSoftShadows = value;
					OnLayerInvalid();
				}
			}
		}

		//Returns whether thsi layer is the default layer.
		public bool Default
		{
			get
			{
				return mDefault;
			}
		}

		public virtual void RemoveShapes()
		{
			RemoveLayerElements(true,false);
		}

		public virtual void RemoveLines()
		{
			RemoveLayerElements(false,true);
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

		//Methods
		protected virtual void OnLayerInvalid()
		{
			if (LayerInvalid !=null && !mSuspendEvents) LayerInvalid(this,new EventArgs());
		}

		#endregion

		#region Implementation

		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("Opacity",Opacity);
			info.AddValue("Visible",Visible);
			info.AddValue("Name",Name);
			info.AddValue("DrawShadows",DrawShadows);
			info.AddValue("ShadowOffset",Serialize.AddPointF(ShadowOffset));
			info.AddValue("ShadowColor",ShadowColor.ToArgb().ToString());
			info.AddValue("SoftShadows",SoftShadows);

			info.AddValue("Elements",Elements);
		}

		public virtual object Clone()
		{
			return new Layer(this);
		}

		private void RemoveLayerElements(bool shapes, bool lines)
		{
			ArrayList keys = new ArrayList();

			//Determine the keys to be removed
			//Must use the dictionery entry object to retrieve the key
			foreach (DictionaryEntry de in Elements)
			{
				Element element = (Element)de.Value;

				if (shapes && element is Shape) keys.Add(de.Key);
				if (lines && element is Line) keys.Add(de.Key);
			}
			
			//Remove keys from collection
			Elements.SetModifiable(true);
			foreach (string key in keys)
			{
				Elements.Remove(key);
			}
			Elements.SetModifiable(false);
		}

		#endregion
	}
}
