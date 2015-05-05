using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Layers : CollectionBase, ISerializable, IDeserializationCallback
	{

		//Property variables
		private Layer mCurrentLayer = null;

		//Working Variables
		private bool mSuspendEvents;
		private SerializationInfo mSavedInfo;

		#region Interface

		public event EventHandler InsertLayer;
		public event EventHandler RemoveLayer;
		public event EventHandler ClearLayers;

		public Layers()
		{
			Layer layer = new Layer(true);
			layer.Name = "Default";
			Add(layer);
			CurrentLayer = layer;
		}

		protected Layers(SerializationInfo info, StreamingContext context)
		{
			mSavedInfo = info;
		}
		
		//Collection indexers
		public virtual Layer this[int index]  
		{
			get  
			{
				return (Layer) List[index];
			}
			set  
			{
				List[index] = value;
			}
		}

		public virtual Layer this[string name]  
		{
			get  
			{
				foreach (Layer layer in List)
				{
					if (layer.Name == name) return layer;
				}
				return null;
			}
		}
		
		//Determines whether events are suspended inside the control
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

		//Adds an Layer to the list 
		public virtual int Add(Layer value)  
		{
			return List.Add(value);
		}

		//Inserts an elelemnt into the list
		public virtual void Insert(int index, Layer value)  
		{
			List.Insert(index, value);
		}

		//Removes an Layer from the list
		public virtual void Remove(Layer value )  
		{
			if (value.Default) throw new LayerException("The default layer cannot be removed.");
			Collapse(value,this["default"]);
		}

		//Returns true if list contains Layer
		public virtual bool Contains(Layer value)  
		{
			return List.Contains(value);
		}

		//Returns the index of an Layer
		public virtual int IndexOf(Layer value)  
		{
			return List.IndexOf(value);
		}

		//Sets or gets the current Layer
		public virtual Layer CurrentLayer
		{
			get 
			{
				return mCurrentLayer;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("CurrentLayer","Layer cannot be null reference.");
				if (! this.Contains(value)) throw new ArgumentException("Layer not found in collection.","CurrentLayer");
				mCurrentLayer = value;
			}
		}
		
		//Methods
		//Moves all shape references to the target layer and removes the source layer
		public virtual void Collapse(Layer source, Layer target)
		{
			if (source.Default) throw new LayerException("Default layer cannot be collapsed.");
			foreach (Element element in source.Elements)
			{
				target.Elements.Add(element.Key,element);
			}
			List.Remove(source);
		}

		public virtual void MoveElement(Element element, Layer source, Layer target)
		{
            target.Elements.SetModifiable(true);
            source.Elements.SetModifiable(true);

			target.Elements.Add(element.Key,element);
			source.Elements.Remove(element.Key);
            element.SetLayer(target);

            target.Elements.SetModifiable(false);
            source.Elements.SetModifiable(false);
		}

		public virtual void Clear()
		{
			List.Clear();
		}

		//Raises the InsertLayer event
		//Original OnInsert method does not raise any events
		protected override void OnInsert(int index,object value)
		{
			if (value.GetType() != typeof(Layer)) throw new ArgumentException("Parameter value must be of type Layer.", "value");

			base.OnInsert(index,value);
			if (! mSuspendEvents && InsertLayer!=null) InsertLayer(value,new EventArgs());
		}

		//Raises the InsertLayer event
		//Original OnRemove method does not raise any events
		protected override void OnRemove(int index,object value)
		{
			if (Count == 1) throw new LayerException("Collection must contain at least one layer");
			base.OnRemove(index,value);
			if (! mSuspendEvents && RemoveLayer!=null) RemoveLayer(value,new EventArgs());
		}

		//Raises the Clear event.
		protected virtual void OnClear()
		{
			base.OnClear();
			if (! mSuspendEvents && ClearLayers !=null) ClearLayers(this,new EventArgs());
		}

		#endregion
		
		#region Implementation

		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			Object[] values = new Object[base.Count];

			base.InnerList.CopyTo(values,0);
			info.AddValue("Values",values);
			info.AddValue("CurrentLayer",CurrentLayer);
		}

		//Use the OnDeserialization callback so that we have direct access to the objects
		public virtual void OnDeserialization(object sender)
		{
			// All objects in the stream have been deserialized.
			// We can now populate the inner arraylist

			Object[] values = (Object[]) mSavedInfo.GetValue("Values",typeof(Object));

			SuspendEvents = true;

			for (int i=0; i<values.Length; i++)
			{
				Add((Layer) values[i]);
			}
			
			CurrentLayer = (Layer) mSavedInfo.GetValue("CurrentLayer",typeof(Layer));

			mSavedInfo = null;
			SuspendEvents = false;
		}

		#endregion
	}
}