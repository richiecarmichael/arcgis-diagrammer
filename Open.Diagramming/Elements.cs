using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void ElementsEventHandler(object sender, ElementsEventArgs e);
	
	//Strongly typed key value pair collection for elements
	[Serializable]
	public class Elements : DictionaryBase, ISerializable, IDeserializationCallback
	{
		//Property variables
		private bool mModifiable = true;
		private bool mSuspendEvents;
		private System.Type mBaseType;
		private string mTypeString;

		//Working variables
		private SerializationInfo mSavedInfo;

		#region Interface

		//Events
		public event ElementsEventHandler InsertElement;
		public event ElementsEventHandler RemoveElement;
		public event EventHandler Cleared;
	
		//Constructors
		public Elements()
		{
			mBaseType = typeof(Element);
			mTypeString = "Element";
		}

		public Elements(System.Type baseType,string typeString)
		{
			mBaseType = baseType;
			mTypeString = typeString;
		}

		protected Elements(SerializationInfo info, StreamingContext context)
		{
			mSavedInfo = info;
		}

		//Properties
		public virtual Element this[string key ]  
		{
			get  
			{
				Element element = Dictionary[key] as Element; 
				if (element == null) throw new ElementsException("Element with key " + key + " was not found in this collection.");

				return element;
			}
			set  
			{
				Dictionary[key] = value;
			}
		}

		public virtual ICollection Keys  
		{
			get  
			{
				return Dictionary.Keys;
			}
		}

		public virtual ICollection Values  
		{
			get  
			{
				return Dictionary.Values;
			}
		}

		public object SyncRoot 
		{ 
			get 
			{ 
				return Dictionary.SyncRoot; 
			} 
		}

		//Sets the base type
		protected internal virtual System.Type BaseType
		{
			get
			{
				return mBaseType;
			}
			set
			{
				mBaseType = value;
			}
		}

		//Sets the base type string
		protected virtual String TypeString
		{
			get
			{
				return mTypeString;
			}
			set
			{
				mTypeString = value;
			}
		}

		//Determines whether the collection is modifiable
		public virtual bool Modifiable
		{
			get
			{
				return mModifiable;
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

		//Methods
		public virtual void Add(string key, Element value )  
		{
			if (! mModifiable) throw new CollectionNotModifiableException("This collection is not modifiable.");
			if (value == null) throw new ArgumentNullException("value","Element parameter cannot be null reference.");
			if (key == null) throw new ArgumentNullException("key","Key may not be null.");
			if (key == "") throw new ArgumentException("Key may not be null string.","key");
			if (! mBaseType.IsInstanceOfType(value) && ! value.GetType().IsSubclassOf(mBaseType)) throw new ArgumentException("Items added to this collection must be of or inherit from type '" + mBaseType.ToString());

			//Key can be reset by removing and then readding to collection
			value.SetKey(key);
			Dictionary.Add(key, value );

			//Zordering now done outside the collection 
			//Since multiple collections can contain the same reference eg layer
			OnInsert(key,value);
		}

		public virtual void AddRange(Elements elements)
		{
			foreach (Element element in elements.Values)
			{
				Add(element.Key,element);
			}
		}

		public virtual bool Contains(string key)  
		{
			return Dictionary.Contains(key);
		}

		public virtual void Remove(string key)  
		{
			if (! mModifiable) throw new CollectionNotModifiableException("This collection is not modifiable.");
			
			Element element = null;

			//Try retrieve existing reference
			try
			{
				element = this[key];
			}
			catch
			{

			}
			if (element == null) return;
			
			Dictionary.Remove(key);
			OnRemove(key,element);
		}

		public virtual void Clear()
		{
			if (! mModifiable) throw new CollectionNotModifiableException("This collection is not modifiable.");
			Dictionary.Clear();
			OnClear();
		}

		//Sets whether this collection is modifiable
		protected internal virtual void SetModifiable(bool value)
		{
			mModifiable = value;
		}

		//Checks and raises the InsertElement event
		protected virtual void OnInsert(string key, Element value)  
		{
			if (! mSuspendEvents && InsertElement!=null) InsertElement(this,new ElementsEventArgs(key,value));
		}

		//Checks and raises the RemoveElement event
		protected virtual void OnRemove(string key, Element value )  
		{
			if (! mSuspendEvents && RemoveElement!=null) RemoveElement(this,new ElementsEventArgs(key,value));
		}

		//Raises the Clear event.
		protected override void OnClear()
		{
			base.OnClear();
			if (! mSuspendEvents && Cleared != null) Cleared(this, new EventArgs());
		}

		#endregion

		#region Implementation

		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			Object[] keys = new Object[base.Count];
			Object[] values = new Object[base.Count];
			
			base.InnerHashtable.Keys.CopyTo(keys,0);
			base.InnerHashtable.Values.CopyTo(values,0);

			info.AddValue("Keys",keys);
			info.AddValue("Values",values);
			info.AddValue("Modifiable",mModifiable);
			info.AddValue("BaseType",mBaseType.ToString());
			info.AddValue("TypeString",mTypeString);
		}

		//Use the OnDeserialization callback so that we have direct access to the objects
		public virtual void OnDeserialization(object sender)
		{
			// All objects in the stream have been deserialized.
			// We can now repopulate the collection object, one at a time
			// .Net serialization of hashtables works in the same way, there is no way to do a bulk copy

			Object[] keys = (Object[]) mSavedInfo.GetValue("Keys",typeof(Object));
			Object[] values = (Object[]) mSavedInfo.GetValue("Values",typeof(Object));

			//SuspendEvents = true;
			//SuspendOrder();

			mBaseType = System.Type.GetType(mSavedInfo.GetString("BaseType"));
			mTypeString = mSavedInfo.GetString("TypeString");

			for (int i=0; i<keys.Length; i++)
			{
				Add((string) keys[i],(Element) values[i]);
			}

			//Set modifiable after adding elements incase not modifiable
			mModifiable = mSavedInfo.GetBoolean("Modifiable");

			mSavedInfo = null;
			//SuspendEvents = false;
			//ResumeOrder();
		}

		//Creates a new key from the collection
		public string CreateKey()
		{
			if (Component.Instance.KeyCreateMode == KeyCreateMode.Normal)
			{
				int i = 0;

				for (i = this.Count; i >= 0; i--)
				{
					if (this.Contains((mTypeString + i.ToString()))) break;
				}

				if (i <= 0)
				{
					return mTypeString+"1";
				}
				else
				{
					while (this.Contains(mTypeString + i.ToString()))
					{
						i += 1;
					}
					return mTypeString + i.ToString();
				}
			}
			else
			{
				return Guid.NewGuid().ToString();
			}
		}

		#endregion
	}
}