using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public abstract class TableItem: ISerializable
	{
		//Property variables
		private string mText;
		private Font mFont;
		private Color mForecolor;
		private Color mBackcolor;
		private float mIndent;
		private object mTag;
		private Table mTable;
		private TableItem mParent;

		//Working variables
		private bool mSuspendEvents;
		private RectangleF mRectangle;
		
		#region Interface
		
		//Events
		public event EventHandler TableItemInvalid;
		
		//Constructors
		public TableItem()
		{
			Forecolor = Color.Black;
			Backcolor = Color.FromArgb(235, 235, 235);
		}

		public TableItem(TableItem prototype)
		{
			mText = prototype.Text;
			mFont = prototype.Font;
			mForecolor = prototype.Forecolor;
			mBackcolor = prototype.Backcolor;
			mIndent = prototype.Indent;
			mTag = prototype.Tag;
			mParent = prototype.Parent;
			mTable = prototype.Table;
		}

		protected TableItem(SerializationInfo info, StreamingContext context)
		{
			SuspendEvents = true;
			
			Text = info.GetString("Text");
			Forecolor = Color.FromArgb(Convert.ToInt32(info.GetString("Forecolor")));
			Backcolor = Color.FromArgb(Convert.ToInt32(info.GetString("Backcolor")));
			SetIndent(info.GetSingle("Indent"));
			if (Serialize.Contains(info,"Font")) SetFont(Serialize.GetFont(info.GetString("Font")));
			if (Serialize.Contains(info,"Tag")) Tag = info.GetValue("Tag",typeof(object));
			if (Serialize.Contains(info, "Parent", typeof(TableItem))) SetParent((TableItem) info.GetValue("Parent", typeof(TableItem)));
			if (Serialize.Contains(info, "Table", typeof(Table))) SetTable((Table) info.GetValue("Table", typeof(Table)));

			SuspendEvents = false;
		}

		//Properties
		public virtual string Text
		{
			get
			{
				return mText;
			}
			set
			{
				if (mText != value)
				{
					mText = value;
					OnTableItemInvalid();
				}
			}
		}

		public virtual Color Forecolor
		{
			get
			{
				return mForecolor;
			}
			set
			{
				mForecolor = value;
				OnTableItemInvalid();
			}
		}

		public virtual Color Backcolor
		{
			get
			{
				return mBackcolor;
			}
			set
			{
				mBackcolor = value;
				OnTableItemInvalid();
			}
		}

		public Font Font
		{
			get 
			{
				if (mFont == null) return Component.Instance.DefaultFont;
				return mFont;
			}
		}

		public virtual string FontName
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont.FontFamily.Name;
				return mFont.FontFamily.Name;
			}
			set
			{
				mFont = Component.Instance.GetFont(value,FontSize,FontStyle);
				OnTableItemInvalid();
			}
		}

		public virtual float FontSize
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont.Size;
				return mFont.Size;
			}
			set
			{
				mFont = Component.Instance.GetFont(FontName,value,FontStyle);
				OnTableItemInvalid();
			}
		}

		public virtual FontStyle FontStyle
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont.Style;
				return mFont.Style;
			}
			set
			{
				mFont = Component.Instance.GetFont(FontName,FontSize,value);
				OnTableItemInvalid();
			}
		}

		public virtual RectangleF Rectangle
		{
			get
			{
				return mRectangle;
			}
		}

		public virtual float Indent
		{
			get
			{
				return mIndent;
			}
		}

		//Sets or gets the tag for this object.
		public virtual object Tag
		{
			get
			{
				return mTag;
			}
			set
			{
				mTag = value;
			}
		}

		public virtual Table Table
		{
			get
			{
				return mTable;
			}
		}

		public virtual TableItem Parent
		{
			get
			{
				return mParent;
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
		//Renders the TableGroup onto the supplied graphics
		public virtual void Render(Graphics graphics,IRender render)
		{

		}

		public virtual void RenderSelection(Graphics graphics, IRender render)
		{

		}

		//Sets the internal font directly
		protected virtual void SetFont(Font font)
		{
			mFont = font;
		}

		protected internal virtual void SetRectangle(RectangleF rectangle)
		{
			mRectangle = rectangle;
		}

		protected internal virtual void SetIndent(float indent)
		{
			mIndent = indent;
		}

		//Sets the internal table reference
		protected internal virtual void SetTable(Table table)
		{
			mTable = table;
		}

		//Sets the internal table reference
		protected internal virtual void SetParent(TableItem parent)
		{
			mParent = parent;
		}

		//Raises the table group invalid event.
		protected virtual void OnTableItemInvalid()
		{
			if (!(mSuspendEvents) && TableItemInvalid !=null) TableItemInvalid(this,new EventArgs());
		}

		#endregion

		#region Implementation

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Text",Text);
			info.AddValue("Forecolor",Forecolor.ToArgb().ToString());
			info.AddValue("Backcolor",Backcolor.ToArgb().ToString());
			info.AddValue("Indent",Indent);
			if (mFont != null) info.AddValue("Font",Serialize.AddFont(mFont));

			info.AddValue("Parent",Parent);
			info.AddValue("Table",Table);
			
			Serialize.SerializeTag(info,Tag);
		}	
	
		#endregion

	}
}
