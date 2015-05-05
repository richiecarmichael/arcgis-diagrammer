using System;
using System.Drawing;
using System.Collections;

namespace Crainiate.Diagramming.Editing
{
	public class Tabs : CollectionBase
	{
		//Property variables
		private Tab mCurrentTab = null;
		private float mTabHeight;
		private Color mGradientColor;
		private Color mBackColor;
		private Color mForeColor;

		//Working Variables
		private bool mSuspendEvents;

		#region Interface

		public event EventHandler InsertTab;
		public event EventHandler RemoveTab;
		public event EventHandler TabsInvalid;

		public Tabs()
		{
			GradientColor = SystemColors.Control;
			BackColor = SystemColors.Control;
			ForeColor = Color.FromArgb(66,65,66);
			
			Tab Tab = new Tab(true);
			Tab.Name = "Default";
			Add(Tab);
			CurrentTab = Tab;
			TabHeight = 18;
		}

		public virtual float TabHeight
		{
			get
			{
				return mTabHeight;
			}
			set
			{
				mTabHeight = value;
			}
		}

		//Collection indexers
		public virtual Tab this[int index]  
		{
			get  
			{
				return (Tab) List[index];
			}
			set  
			{
				List[index] = value;
			}
		}

		public virtual Tab this[string name]  
		{
			get  
			{
				foreach (Tab Tab in List)
				{
					if (Tab.Name == name) return Tab;
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
		
		//Sets or gets the current Tab
		public virtual Tab CurrentTab
		{
			get 
			{
				return mCurrentTab;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("CurrentTab","Tab cannot be null reference.");
				if (! this.Contains(value)) throw new ArgumentException("Tab not found in collection.","CurrentTab");
				mCurrentTab = value;
			}
		}

		public virtual Color GradientColor
		{
			get
			{
				return mGradientColor;
			}
			set
			{
				mGradientColor = value;
				OnTabsInvalid();
			}
		}

		public virtual Color BackColor
		{
			get
			{
				return mBackColor;
			}
			set
			{
				mBackColor = value;
				OnTabsInvalid();
			}
		}

		public virtual Color ForeColor
		{
			get
			{
				return mForeColor;
			}
			set
			{
				mForeColor = value;
				OnTabsInvalid();
			}
		}

		//Methods
		//Adds an Tab to the list 
		public virtual int Add(Tab value)  
		{
			int index = List.Add(value);
			OnInsert(index,value);
			return index;
		}

		//Inserts an element into the list
		public virtual void Insert(int index, Tab value)  
		{
			List.Insert(index, value);
			OnInsert(index,value);
		}

		//Removes an Tab from the list
		public virtual void Remove(Tab value )  
		{
			if (value.Default) throw new TabsException("The default Tab cannot be removed.");
			Collapse(value,this["default"]);
		}

		//Returns true if list contains Tab
		public virtual bool Contains(Tab value)  
		{
			return List.Contains(value);
		}

		//Returns the index of an Tab
		public virtual int IndexOf(Tab value)  
		{
			return List.IndexOf(value);
		}

		//Methods
		//Moves all shape references to the target Tab and removes the source Tab
		public virtual void Clear()
		{
			List.Clear();
		}	

		public virtual void Collapse(Tab source, Tab target)
		{
			if (source.Default) throw new TabsException("Default Tab cannot be collapsed.");
			foreach (Element element in source.Elements)
			{
				target.Elements.Add(element.Key,element);
			}
			
			List.Remove(source);
			OnRemove(0,source);			
		}

		public virtual void MoveElement(Element element, Tab source, Tab target)
		{
			target.Elements.Add(element.Key,element);
			source.Elements.Remove(element.Key);
		}

		//Raises the InsertTab event
		//Original OnInsert method does not raise any events
		protected override void OnInsert(int index,object value)
		{
			if (value.GetType() != typeof(Tab)) throw new ArgumentException("Parameter value must be of type Tab.", "value");

			base.OnInsert(index,value);
			if (! mSuspendEvents && InsertTab!=null) InsertTab(value,new EventArgs());
		}

		//Raises the InsertTab event
		//Original OnRemove method does not raise any events
		protected override void OnRemove(int index,object value)
		{
			if (Count == 1) throw new TabsException("Collection must contain at least one Tab");
			base.OnRemove(index,value);
			if (! mSuspendEvents && RemoveTab!=null) RemoveTab(value,new EventArgs());
		}

		protected virtual void OnTabsInvalid()
		{
			if (! mSuspendEvents && TabsInvalid!=null) TabsInvalid(this,new EventArgs());
		}

		#endregion
	}
}