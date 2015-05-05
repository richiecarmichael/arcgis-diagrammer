using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void SegmentsEventHandler(object sender, SegmentsEventArgs e);
	
	[Serializable]
	public class Segments: CollectionBase, ISerializable, IDeserializationCallback
	{
		//Working Variables
		private bool mSuspendEvents;
		private SerializationInfo mSavedInfo;

		#region Interface

		public event SegmentsEventHandler InsertItem;
		public event SegmentsEventHandler RemoveItem;
		public event EventHandler Clear;

		public Segments()
		{
		}

		protected Segments(SerializationInfo info, StreamingContext context)
		{
			mSavedInfo = info;
		}

		//Collection indexers
		public virtual Segment this[int index]  
		{
			get  
			{
				return (Segment) List[index];
			}
			set  
			{
				List[index] = value;
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

		//Adds an Segment to the list 
		protected internal virtual int Add(Segment value)  
		{
			if (value == null) throw new ArgumentNullException("Segment parameter cannot be null reference.","value");
			return List.Add(value);
		}

		//Inserts an elelemnt into the list
		protected internal virtual void Insert(int index, Segment value)  
		{
			if (value == null) throw new ArgumentNullException("Segment parameter cannot be null reference.","value");
			List.Insert(index, value);
		}

		//Removes an Segment from the list
		protected internal virtual void Remove(Segment value )  
		{
			List.Remove(value);
		}

		//Returns true if list contains Segment
		public virtual bool Contains(Segment value)  
		{
			return List.Contains(value);
		}

		//Returns the index of an Segment
		public virtual int IndexOf(Segment value)  
		{
			return List.IndexOf(value);
		}

		//Raises the InsertItem event
		//Original OnInsert method does not raise any events
		protected override void OnInsert(int index,object value)
		{
			base.OnInsert(index,value);
			if (! mSuspendEvents && InsertItem!=null) InsertItem(value,new SegmentsEventArgs((Segment) value));
		}

		//Raises the RemoveItem event
		//Original OnRemove method does not raise any events
		protected override void OnRemove(int index,object value)
		{
			base.OnRemove(index,value);
			if (! mSuspendEvents && RemoveItem!=null) RemoveItem(value,new SegmentsEventArgs((Segment) value));
		}

		//Raises the Clear event.
		protected virtual void OnClear()
		{
			base.OnClear();
			if (! mSuspendEvents && Clear!=null) Clear(this,new EventArgs());
		}

		#endregion
		
		#region Implementation

		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			Object[] values = new Object[base.Count];

			base.InnerList.CopyTo(values,0);
			info.AddValue("Values",values);
		}

		//Use the OnDeserialization callback so that we have direct access to the objects
		public virtual void OnDeserialization(object sender)
		{
			// All objects in the stream have been deserialized.
			// We can now populate the inner collection
			Object[] values = (Object[]) mSavedInfo.GetValue("Values",typeof(Object));

			SuspendEvents = true;

			for (int i=0; i<values.Length; i++)
			{
				Add((Segment) values[i]);
			}
			
			mSavedInfo = null;
			SuspendEvents = false;
		}

		#endregion
	}
}