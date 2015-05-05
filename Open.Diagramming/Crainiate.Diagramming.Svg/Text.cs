using System;
using System.Collections;
using System.Drawing;
using System.ComponentModel;
using System.Xml;
using System.Text;

namespace Crainiate.Diagramming.Svg
{
	public class Text
	{
		private Label mLabel;
		private RectangleF[] mRectangles;
		private RectangleF mLayoutRectangle;
		private StringFormat mFormat; 

		#region  Interface 

		public Text()
		{
			mFormat = new StringFormat();
		}

		public Text(Label label)
		{
			mFormat = new StringFormat();
			Label = label;
		}

		//Sets the internal label object
		public virtual Label Label
		{
			get
			{
				return mLabel;
			}
			set
			{
				mLabel = value;
			}
		}

		public virtual RectangleF LayoutRectangle
		{
			get
			{
				return mLayoutRectangle;
			}
			set
			{
				mLayoutRectangle = value;
			}
		}

		public virtual StringFormat Format
		{
			get
			{
				return mFormat;
			}
			set
			{
				mFormat = value;
			}
		}

		//Returns the text element for the internal label object
		public virtual string ExtractText(float x, float y, string id)
		{
			if (mLabel == null) throw new TextException("Text cannot be extracted using this method without a valid Label reference.");
			MeasureCharacters();
			return ExtractTextImplementation(mLabel.Text, mLabel.Font,  x, y, id, "");
		}

        //Returns the text element for the internal label object
        public virtual string ExtractText(float x, float y, string id, string transform)
        {
            if (mLabel == null) throw new TextException("Text cannot be extracted using this method without a valid Label reference.");
            MeasureCharacters();
            return ExtractTextImplementation(mLabel.Text, mLabel.Font, x, y, id, transform);
        }

		//Returns the text element for the internal label object
		public virtual string ExtractText(string text, Font font, float x, float y, string id)
		{
			MeasureCharactersImplementation(text, font, mFormat);
			return ExtractTextImplementation(text, font,  x, y, id, "");
		}

		//Return the style definition from a shape
		public virtual string GetStyle()
		{
			if (mLabel == null) throw new TextException("Style cannot be extracted using this method without a valid Label reference.");
			return ExtractStyleImplementation(mLabel.Font, mLabel.Color, "");
		}

		//Return the style definition with clipping path from a shape
		public virtual string GetStyle(string ClipPathId)
		{
			if (mLabel == null) throw new TextException("Style cannot be extracted using this method without a valid Label reference.");
			return ExtractStyleImplementation(mLabel.Font, mLabel.Color, ClipPathId);
		}

		//Return the style definition from a shape
		public virtual string GetStyle(Font font, Color color)
		{
			return ExtractStyleImplementation(font, color, "");
		}

		//Return the style definition with clipping path from a shape
		public virtual string GetStyle(Font font, Color color, string ClipPathId)
		{
			return ExtractStyleImplementation(font, color, ClipPathId);
		}

		public virtual void MeasureCharacters()
		{
			if (mLabel == null) throw new TextException("Characters cannot be measured using this method without a valid Label reference.");
			
			if (mLabel is TextLabel)
			{
				TextLabel textLabel = (TextLabel) Label;
				mFormat = textLabel.GetStringFormat();
			}
			MeasureCharactersImplementation(mLabel.Text, mLabel.Font, mFormat);
		}

		public virtual void Reset()
		{
			mLabel = null;
			mLayoutRectangle = new RectangleF();
		}

		#endregion

		#region  Implementation 

		private void SetLabel(Label label)
		{
			mLabel = label;
		}

		private void MeasureCharactersImplementation(string text, Font font, StringFormat format)
		{
			Graphics graphics = Component.Instance.CreateGraphics();
			Region[] regions = null;
			CharacterRange[] ranges = new CharacterRange[1];

			//Get the text
			int i = 0;
			int count = text.Length;
			if (text == "") text = ".";

			if (mLayoutRectangle.Size.IsEmpty)
			{
				SizeF size = graphics.MeasureString(text, font);
				size.Width += 10;
				size.Height += 10;
				mLayoutRectangle = new RectangleF(mLayoutRectangle.Location, size);
			}
			
			//Measure each character
			mRectangles = new RectangleF[count+1];
			for (i = 0; i < count; i++)
			{
				ranges[0].First = i;
				ranges[0].Length = 1;
				format.SetMeasurableCharacterRanges(ranges);
				regions = graphics.MeasureCharacterRanges(text, font, mLayoutRectangle, format);
				mRectangles[i + 1] = regions[0].GetBounds(graphics);
			}

			//Reset the first rectangle
			mRectangles[0] = mRectangles[1];
			mRectangles[0].Width = 1;
			mRectangles[0].X -= 1;

			graphics.Dispose();
		}

		private string ExtractTextImplementation(string text, Font font, float x, float y, string key, string transform)
		{
			if (text == "") return "";

			int i = 0;
			float top = 0F;
			char[] arrText = text.ToCharArray();
			StringBuilder builder = new StringBuilder();

			int ascent = font.FontFamily.GetCellAscent(font.Style);

			// 14.484375 = 16.0 * 1854 / 2048
			float ascentPixel =	font.Size * ascent / font.FontFamily.GetEmHeight(font.Style);

			builder.Append("<text id=\"");
			builder.Append(key);
			builder.Append("\" class=\"\"");
            if (transform.Length > 0)
            {
                builder.Append(" transform=\"");
                builder.Append(transform);
                builder.Append("\"");
            }
            builder.Append(">");
			builder.Append("\r");
			builder.Append("\t");

			for (i = 0; i <= arrText.GetUpperBound(0); i++)
			{
				//Exit if top is less than last top (measure character reverts Y to 0 when clipped)
				//Line return comes in at empty
				if (top > mRectangles[i + 1].Top && i > 0 && !mRectangles[i + 1].IsEmpty) break;

				if (top < mRectangles[i + 1].Top || i == 0)
				{
					//Close previous tspan tag
					if (i > 0)
					{
						builder.Append("</tspan>");
					}

					top = mRectangles[i + 1].Top;
					builder.Append("<tspan x=\"");
					builder.Append(XmlConvert.ToString((mRectangles[i + 1].Left + x)));
					builder.Append("\" y=\"");
					builder.Append(XmlConvert.ToString((mRectangles[i + 1].Top + Convert.ToInt32(ascentPixel) + y)));
					builder.Append("\">");
				}

				//Check here for reserved chacters like <,>,&
				string strChar = arrText[i].ToString();

				if (strChar == "<") 
				{
					strChar = "&lt;";
				}
				else if (strChar == ">")
				{
					strChar = "&gt;";
				}
				else if (strChar == "&")
				{
					strChar = "&amp;";
				}
				else if (strChar == "\"")
				{
					strChar = "&quot;";
				}

				builder.Append(strChar);
			}

			builder.Append("</tspan>");
			builder.Append("</text>");

			return builder.ToString();
		}

		private string ExtractStyleImplementation(Font font, Color color, string clipPathId)
		{
			StringBuilder builder = new StringBuilder();
			
			builder.Append("text-rendering:auto;");

			builder.Append("font-family:'");
			builder.Append(font.FontFamily.Name.ToString());
			builder.Append("';");			

			if (font.Bold) builder.Append("font-weight:bold;");
			if (font.Italic) builder.Append("font-style:italic;");

			builder.Append("font-size:");
			builder.Append(XmlConvert.ToString(font.SizeInPoints));
			builder.Append("pt;color:");
			builder.Append(Style.GetCompatibleColor(color));
			builder.Append(";");

			//Add the clip path id if required
			if (clipPathId != "")
			{
				builder.Append("clip-path:url(#");
				builder.Append(clipPathId);
				builder.Append(");");
			}

			return builder.ToString();
		}

		#endregion
	}
}