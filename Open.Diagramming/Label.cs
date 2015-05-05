using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Label: ISerializable, ICloneable
	{
		//Property variables
		private Element mParent;

		private Font mFont;
		
		private string mText;
		private Color mColor;
		private byte mOpacity;
		
		private PointF mOffset;
		private bool mVisible = true;

		//Working variables
		private bool mSuspendEvents;
		
		#region Interface

		//Events
		public event EventHandler LabelInvalid;

		//Constructors
		public Label()
		{
			SuspendEvents = true;
			Color = Color.Black;
			Opacity = Component.Instance.DefaultOpacity;
			SuspendEvents = false;
		}

		public Label(string text)
		{
			SuspendEvents = true;
			Color = Color.Black;
			Opacity = Component.Instance.DefaultOpacity;
			Text = text;
			SuspendEvents = false;
		}

		public Label(Label prototype)
		{
			mText = prototype.Text;
			mColor = prototype.Color;
			mOpacity = prototype.Opacity;
			mVisible = prototype.Visible;
			mOffset = prototype.Offset;

			//Check if font, alignment and line alignment are defaults, else clone
			mFont = (prototype.Font == Component.Instance.DefaultFont) ? null : (Font) prototype.Font.Clone();
		}
		
		//Creates a new element from the supplied XML.
		protected Label(SerializationInfo info, StreamingContext context)
		{
			SuspendEvents = true;
			
			Text = info.GetString("Text");
			Offset = Serialization.Serialize.GetPointF(info.GetString("Offset"));
			Opacity = info.GetByte("Opacity");
			Color = Color.FromArgb(Convert.ToInt32(info.GetString("Color")));
			Visible = info.GetBoolean("Visible");
			
			//Only set if exists eg is not default
			if (Serialize.Contains(info,"Font")) SetFont(Serialize.GetFont(info.GetString("Font")));
		
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
				mText = value;
				OnLabelInvalid();
			}
		}

		//The color used to draw the text.
		public virtual Color Color
		{
			get
			{
				return mColor;
			}
			set
			{
				mColor = value;
				OnLabelInvalid();
			}
		}

		public virtual PointF Offset
		{
			get
			{
				return mOffset;
			}
			set
			{
				if (!mOffset.Equals(value))
				{
					mOffset = value;
					OnLabelInvalid();
				}
			}
		}

		//Defines the percentage opacity for this annotation.
		public virtual byte Opacity
		{
			get
			{
				return mOpacity;
			}
			set
			{
				if (mOpacity != value)
				{
					mOpacity = value;
					OnLabelInvalid();
				}
			}
		}
		
		//Indicates whether the shape is currently visible and rendered during drawing operations.
		public virtual bool Visible
		{
			get
			{
				return mVisible;
			}
			set
			{
				if (mVisible != value)
				{
					mVisible = value;
					OnLabelInvalid();
				}
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
				OnLabelInvalid();
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
				OnLabelInvalid();
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
				OnLabelInvalid();
			}
		}

		public virtual bool Bold
		{
			get
			{
				return Font.Bold;
			}
			set
			{
				FontStyle = Component.Instance.GetFontStyle(value,Italic,Underline,Strikeout);
			}
		}

		public virtual bool Italic
		{
			get
			{
				return Font.Italic;
			}
			set
			{
				FontStyle = Component.Instance.GetFontStyle(Bold,value,Underline,Strikeout);
			}
		}

		public virtual bool Underline
		{
			get
			{
				return Font.Underline;
			}
			set
			{
				FontStyle = Component.Instance.GetFontStyle(Bold,Italic,value,Strikeout);
			}
		}

		public virtual bool Strikeout
		{
			get
			{
				return Font.Strikeout;
			}
			set
			{
				FontStyle = Component.Instance.GetFontStyle(Bold,Italic,Underline,value);
			}
		}

		public virtual Element Parent
		{
			get
			{
				return mParent;
			}
		}

		//Determines whether events are prevented from being raised by this class.
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

		public Font Font
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont;
				return mFont;
			}
		}
		
		//Methods
		public virtual void Render(Graphics graphics,IRender render)
		{
			//Translate by offset
			if (Visible)
			{
				graphics.TranslateTransform(Offset.X,Offset.Y);

				SolidBrush brush = new SolidBrush(render.AdjustColor(Color,0,Opacity));
				graphics.DrawString(Text,Font,brush,0,0);

				graphics.TranslateTransform(-Offset.X,-Offset.Y);
			}
		}

		public virtual void RenderAction(Graphics graphics,IRender render)
		{
			Render(graphics,render);
		}

		//Methods
		protected internal void SetParent(Element parent)
		{
			mParent = parent;
		}

		//Raises the element invalid event.
		protected virtual void OnLabelInvalid()
		{
			if (!(mSuspendEvents) && LabelInvalid!=null) LabelInvalid(this,new EventArgs());
		}

		//Sets the internal font directly
		protected internal virtual void SetFont(Font font)
		{
			mFont = font;
		}

		#endregion

		#region Implementation
		
		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("Text",Text);
			info.AddValue("Offset",Serialization.Serialize.AddPointF(Offset));
			info.AddValue("Opacity",Opacity);
			info.AddValue("Color",Color.ToArgb().ToString());			
			info.AddValue("Visible",Visible);
			
			//Only add if not the defaults eg memeber varaible not null
			if (mFont != null) info.AddValue("Font",Serialize.AddFont(mFont));
		}

		//Returns a clone of this object
		public virtual object Clone()
		{
			return new Label(this);
		}

		#endregion
	}

	
}
