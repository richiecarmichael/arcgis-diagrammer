using System;
using System.Drawing;

namespace Crainiate.Diagramming.Editing
{
	public class Tab
	{
		//Property variables
		private Elements mElements;
		
		private bool mVisible = true;
		private string mName;
		private bool mDefault;
		private RectangleF mRectangle;
		private bool mPressed;
		private bool mButtonPressed;
		private bool mButtonEnabled;
		private ButtonStyle mButtonStyle;
		private RectangleF mButtonRectangle;
		private float mScroll;

		//Working variables
		private bool mSuspendEvents;

		#region Interface

		//Events
		public event EventHandler TabInvalid;

		//Constructor
		public Tab()
		{
			mElements = new Elements(typeof(Element),"Element");
			mElements.SetModifiable(false);
		}

		protected internal Tab(bool defaultTab)
		{
			mElements = new Elements(typeof(Element),"Element");
			mElements.SetModifiable(false);
			mDefault = defaultTab;
		}

		//Properties
		//Determines if Tab is visible
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
					OnTabInvalid();
				}
			}
		}

		//Sets or gets the name of this Tab
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

		//Returns the elements in this Tab
		public virtual Elements Elements
		{
			get
			{
				return mElements;
			}
			set
			{
				mElements = value;
				OnTabInvalid();
			}
		}

		//Returns whether this Tab is the default Tab.
		public bool Default
		{
			get
			{
				return mDefault;
			}
		}

		//Gets or sets the offset from the top of the palette
		public virtual RectangleF Rectangle
		{
			get
			{
				return mRectangle;
			}
		}

		public virtual RectangleF ButtonRectangle
		{
			get
			{
				return mButtonRectangle;
			}
		}

		internal bool Pressed
		{
			get
			{
				return mPressed;
			}
			set
			{
				mPressed = value;
			}
		}

		internal bool ButtonPressed
		{
			get
			{
				return mButtonPressed;
			}
			set
			{
				mButtonPressed = value;
			}
		}

		internal bool ButtonEnabled
		{
			get
			{
				return mButtonEnabled;
			}
			set
			{
				mButtonEnabled = value;
			}
		}

		internal ButtonStyle ButtonStyle
		{
			get
			{
				return mButtonStyle;
			}
			set
			{
				mButtonStyle = value;
			}
		}

		internal float Scroll
		{
			get
			{
				return mScroll;
			}
			set
			{
				mScroll = value;
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

		//Methods
		protected internal virtual void SetRectangle(RectangleF value)
		{
			mRectangle = value;
		}

		protected internal virtual void SetButtonRectangle(RectangleF value)
		{
			mButtonRectangle = value;
		}

		protected virtual void OnTabInvalid()
		{
			if (TabInvalid !=null && !mSuspendEvents) TabInvalid(this,new EventArgs());
		}

		#endregion
	}
}
