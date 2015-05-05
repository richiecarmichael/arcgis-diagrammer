using System;
using System.Collections;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class LayerElements : Elements
	{
		//Working variables
		private int mSuspendCount;

		#region Interface

		//Events
		public event EventHandler ChangeOrder;
	
		//Constructors
		public LayerElements(): base()
		{
		}

		public LayerElements(System.Type baseType,string typeString): base(baseType, typeString)
		{
		}

		protected LayerElements(SerializationInfo info, StreamingContext context): base(info, context)
		{
			
		}
		//Properties

		//Methods

		//Suspend Ordering
		public virtual void SuspendOrder()
		{
			mSuspendCount +=1;
		}

		//Resume ordering
		public virtual void ResumeOrder()
		{
			if (mSuspendCount > 0) mSuspendCount -=1;
		}

		//Forces ordering to resume
		public virtual void ResumeOrder(bool force)
		{
			mSuspendCount = 0;
		}

		public virtual bool OrderSuspended
		{
			get
			{
				return (mSuspendCount > 0);
			}
		}

		//Brings a Element to the front of the zorder.
		public virtual void BringToFront(Element element)
		{
			if (mSuspendCount > 0)	return;
						
			foreach (Element loop in Values)
			{
				if (loop.mZOrder < element.mZOrder) loop.mZOrder += 1;
			}

			element.mZOrder = 0;
			OnChangeOrder();
		}

		//Raises the ChangeOrder event.
		protected virtual void OnChangeOrder()
		{
			if (! SuspendEvents && ChangeOrder != null) ChangeOrder(this,new EventArgs());
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(info,context);
		}

		public virtual void SetOrder(Element element, int order)
		{
			//Get existing zorder of element
			int existingOrder = element.ZOrder;

			//Check that zorder is within bounds of collection
			if (order >= this.Count) throw new ArgumentException("Order value must be less than the number of elements.","order");
			if (order < 0) throw new ArgumentException("Order value cannot be less than zero.","value");
			if (existingOrder == order) return;
			
			//Loop through internal collection
			foreach (Element loop in this.Values)
			{
				if (loop == element)
				{
					loop.mZOrder = order;
				}
				else
				{
					if (existingOrder < order)
					{
						if (loop.mZOrder <= order && loop.mZOrder > existingOrder)
						{
							loop.mZOrder -= 1;
						}
					}
					else
					{
						if (loop.mZOrder >=order && loop.ZOrder < existingOrder)
						{
							loop.mZOrder += 1;
						}
					}
				}
			}
			OnChangeOrder();
		}

		#endregion
	}
}