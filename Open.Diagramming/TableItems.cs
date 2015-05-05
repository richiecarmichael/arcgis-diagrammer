using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void TableItemsEventHandler(object sender, TableItemsEventArgs e);

	[Serializable]
	public class TableItems : CollectionBase, ISerializable, IDeserializationCallback
	{
		//Working Variables
		private bool mSuspendEvents;
		private SerializationInfo mSavedInfo;
		private System.Type mBaseType;

		#region Interface

		public event TableItemsEventHandler InsertItem;
		public event TableItemsEventHandler RemoveItem;
		public event EventHandler ClearList;

		public TableItems()
		{
			mBaseType = typeof(TableItem);
		}

		public TableItems(System.Type baseType)
		{
			mBaseType = baseType;
		}

		protected TableItems(SerializationInfo info, StreamingContext context)
		{
			mBaseType = typeof(TableItem);
			mSavedInfo = info;
		}
		
		//Collection indexers
		public virtual TableItem this[int index]  
		{
			get  
			{
				return (TableItem) List[index];
			}
			set  
			{
				List[index] = value;
			}
		}

		public virtual void Clear()
		{
			List.Clear();
			if (! mSuspendEvents && ClearList!=null) ClearList(this,EventArgs.Empty);
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

		//Adds an TableItem to the list 
		public virtual int Add(TableItem value)  
		{
			if (value == null) throw new ArgumentNullException("TableItem parameter cannot be null reference.","value");
			if (! mBaseType.IsInstanceOfType(value) && ! value.GetType().IsSubclassOf(mBaseType)) throw new ArgumentException("Items added to this collection must be of or inherit from type '" + mBaseType.ToString());
			
			int index= List.Add(value);
			if (! mSuspendEvents && InsertItem!=null) InsertItem(this, new TableItemsEventArgs((TableItem) value));

			return index;
		}

		//Inserts an elelemnt into the list
		public virtual void Insert(int index, TableItem value)  
		{
			if (value == null) throw new ArgumentNullException("TableItem parameter cannot be null reference.","value");
			if (! mBaseType.IsInstanceOfType(value) && ! value.GetType().IsSubclassOf(mBaseType)) throw new ArgumentException("Items added to this collection must be of or inherit from type '" + mBaseType.ToString());
			List.Insert(index, value);

			if (! mSuspendEvents && InsertItem!=null) InsertItem(this, new TableItemsEventArgs((TableItem) value));
		}

		//Removes an TableItem from the list
		public virtual void Remove(TableItem value )  
		{
			List.Remove(value);

			if (! mSuspendEvents && RemoveItem!=null) RemoveItem(this, new TableItemsEventArgs((TableItem) value));
		}

		//Returns true if list contains TableItem
		public virtual bool Contains(TableItem value)  
		{
			return List.Contains(value);
		}

		//Returns the index of an TableItem
		public virtual int IndexOf(TableItem value)  
		{
			return List.IndexOf(value);
		}

		//Sets or gets the base type for the collection
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

		#endregion
		
		#region Implementation

		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			Object[] values = new Object[base.Count];
			base.InnerList.CopyTo(values,0);
			
			info.AddValue("BaseType",BaseType);
			info.AddValue("Values",values);
		}

		//Use the OnDeserialization callback so that we have direct access to the objects
		public virtual void OnDeserialization(object sender)
		{
			// All objects in the stream have been deserialized.
			// We can now populate the inner arraylist

			SuspendEvents = true;
			mBaseType = (Type) mSavedInfo.GetValue("BaseType",typeof(Type));		
		
			Object[] values = (Object[]) mSavedInfo.GetValue("Values",typeof(Object));
			for (int i=0; i<values.Length; i++)
			{
				Add((TableItem) values[i]);
			}

			mSavedInfo = null;
			SuspendEvents = false;
		}

		#endregion
	}
}