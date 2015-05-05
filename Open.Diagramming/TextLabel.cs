using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class TextLabel: Label, ISerializable, ICloneable
	{
		private SizeF mSize;
		private StringFormat mStringFormat;
		
		#region Interface

		//Constructors
		public TextLabel()
		{
			
		}

		public TextLabel(string text): base(text)
		{
			
		}

		public TextLabel(TextLabel prototype): base(prototype)
		{
			mSize = prototype.Size;
			mStringFormat = prototype.StringFormat;
		}

		//Deserializes info into a new solid element
		protected TextLabel(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			
			Size = Serialization.Serialize.GetSizeF(info.GetString("Size"));
			if (Serialize.Contains(info,"StringFormat")) SetFormat(Serialize.GetStringFormat(info.GetString("StringFormat")));

			SuspendEvents = false;	
		}

		//Properties
		public virtual SizeF Size
		{
			get
			{
				return mSize;
			}
			set
			{
				if (!mSize.Equals(value))
				{
					mSize = value;
					OnLabelInvalid();
				}
			}
		}

		public virtual StringAlignment Alignment
		{
			get
			{
				if (mStringFormat == null) return Component.Instance.DefaultStringFormat.Alignment; 
				return mStringFormat.Alignment;
			}
			set
			{
				CheckStringFormatDefault();
				mStringFormat.Alignment = value;
				OnLabelInvalid();
			}
		}

		public virtual StringAlignment LineAlignment
		{
			get
			{
				if (mStringFormat == null) return Component.Instance.DefaultStringFormat.LineAlignment; 
				return mStringFormat.LineAlignment;
			}
			set
			{
				CheckStringFormatDefault();	
				mStringFormat.LineAlignment = value;
				OnLabelInvalid();
			}
		}

		public virtual StringTrimming Trimming
		{
			get
			{
				if (mStringFormat == null) return Component.Instance.DefaultStringFormat.Trimming; 
				return mStringFormat.Trimming;
			}
			set
			{
				CheckStringFormatDefault();				
				mStringFormat.Trimming = value;
				OnLabelInvalid();
			}
		}

		protected virtual StringFormat StringFormat
		{
			get
			{
				if (mStringFormat == null) return Component.Instance.DefaultStringFormat; 
				return mStringFormat;
			}
		}

		//Methods
		public virtual void Reset()
		{
			mSize = new SizeF();
			Offset = new PointF();
			mStringFormat = null;
			OnLabelInvalid();
		}

		public virtual StringFormat GetStringFormat()
		{
			return (StringFormat) StringFormat.Clone();
		}

		//Sets the internal string format directly
		protected internal virtual void SetFormat(StringFormat format)
		{
			mStringFormat = format;
		}

		#endregion

		#region Overrides

		public virtual void Render(Graphics graphics, RectangleF layout, IRender render)
		{
			//If Offset and Size specified then replace layout
			if (Visible)
			{
				if (!Offset.IsEmpty) layout = new RectangleF(Offset,layout.Size);
				if (!Size.IsEmpty) layout = new RectangleF(layout.Location,Size);

				SolidBrush brush = new SolidBrush(render.AdjustColor(Color,0,Opacity));
				graphics.DrawString(Text,Font,brush,layout,StringFormat);
			}

			//Render the layout rectangle for testing
			//Pen pen = new Pen(Color.Red);
			//graphics.DrawRectangle(pen,layout.X,layout.Y,layout.Width,layout.Height);
		}

		public virtual void RenderAction(Graphics graphics, RectangleF layout, IRender render)
		{
			Render(graphics,layout,render);
		}

		public override object Clone()
		{
			return new TextLabel(this);
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Size",Serialization.Serialize.AddSizeF(Size));
			if (mStringFormat != null) info.AddValue("StringFormat",Serialize.AddStringFormat(mStringFormat));
			
			base.GetObjectData(info,context);
		}

		//Clones the default string format
		private void CheckStringFormatDefault()
		{
			if (mStringFormat == null) mStringFormat = (StringFormat) Component.Instance.DefaultStringFormat.Clone(); 
		}

		#endregion
	}
}
