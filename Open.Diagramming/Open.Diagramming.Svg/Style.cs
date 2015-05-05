using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Crainiate.Diagramming.Svg
{
	public class Style
	{
		private SolidElement mSolid;
		private Line mLine;
		private Curve mCurve;
		private TextLabel mLabel;

		private string mLinearGradient;
		private string mStyle;
		private string mClipId;

		static char[] hexDigits = {'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};

		#region Interface 

		public Style()
		{

		}

		public Style(Line line)
		{
			mLine = line;
			mStyle = StyleFromLineImplementation(line);
		}

		public Style(Curve curve)
		{
			mCurve = curve;
			mStyle = StyleFromCurveImplementation(curve);
		}

		public Style(SolidElement solid)
		{
			mSolid = solid;
			mStyle = StyleFromShapeImplementation(solid, "");
		}

		public Style(SolidElement solid, string clipId)
		{
			mSolid = solid;
			mClipId = clipId;
			mStyle = StyleFromShapeImplementation(solid, clipId);
		}

		public Style(TextLabel label)
		{
			mLabel = label;
			mStyle = StyleFromLabelImplementation(label, "");
		}

		//Sets or gets the shape used to create this style
		public virtual SolidElement SolidElement
		{
			get
			{
				return mSolid;
			}
			set
			{
				mSolid = value;
				mStyle = StyleFromShapeImplementation(value, mClipId);
			}
		}

		//Sets or gets the Line used to create this style
		public virtual Line Line
		{
			get
			{
				return mLine;
			}
			set
			{
				mLine = value;
				mStyle = StyleFromLineImplementation(value);
			}
		}

		//Sets or gets the Label used to create this style
		public virtual TextLabel Label
		{
			get
			{
				return mLabel;
			}
			set
			{
				mLabel = value;
				mStyle = StyleFromLabelImplementation(value, mClipId);
			}
		}

		//Sets or gets the lineargradient, if applicable
		public virtual string LinearGradient
		{
			get
			{
				return mLinearGradient;
			}
		}

		//Return the style definition from a shape
		public virtual string GetStyle()
		{
			return mStyle;
		}

		//Gets or sets the clipping path id
		public virtual string ClipPathId
		{
			get
			{
				return mClipId;
			}
			set
			{
				mClipId = value;
			}
		}

		#endregion

		#region  Implementation 

		private string StyleFromShapeImplementation(SolidElement solid, string clipId)
		{
			StringBuilder style = new StringBuilder();

			//Sort out the fill
			style.Append("fill:");
			
			if (!solid.DrawBackground)
			{
				style.Append("none;");
			}
			else
			{
				if (solid.DrawGradient && solid.BackColor != solid.GradientColor)
				{
					mLinearGradient = ExtractLinearGradient(solid.GradientMode, solid.BackColor, solid.GradientColor);
					style.Append("url(#none);");
				}
				else
				{
					mLinearGradient = "";
					style.Append(GetCompatibleColor(solid.BackColor));
					style.Append(";");
				}

				if (solid.Opacity < 100)
				{
					style.Append("fill-opacity:0.");
					style.Append(solid.Opacity.ToString());
					style.Append(";");
				}
			}

			//Sort out the stroke, ignore custom pens and use defaults
			style.Append("stroke:");
			style.Append(GetCompatibleColor(solid.BorderColor));
			style.Append(";");
			style.Append("stroke-width:");
			style.Append(solid.BorderWidth);
			style.Append(";");
			style.Append(GetDashStyle(mSolid.BorderStyle, mSolid.BorderWidth));

			//Add clipping path id if required
			if (clipId != "")
			{
				style.Append("clip-path:url(#");
				style.Append(clipId);
				style.Append(");");
			}

			return style.ToString();
		}

		private string StyleFromLineImplementation(Line line)
		{
			StringBuilder style = new StringBuilder();
			string colorString = GetCompatibleColor(line.BorderColor);

			style.Append("stroke:");
			style.Append(colorString);
			style.Append(";");
			style.Append("stroke-width:");
			style.Append(line.BorderWidth);
			style.Append(";");
			style.Append(GetDashStyle(line.BorderStyle, line.BorderWidth));
			style.Append("fill:none");
			style.Append(";");

			return style.ToString();
		}

		private string StyleFromCurveImplementation(Curve curve)
		{
			StringBuilder style = new StringBuilder();
			string colorString = GetCompatibleColor(curve.BorderColor);

			style.Append("stroke:");
			style.Append(colorString);
			style.Append(";");
			style.Append("stroke-width:");
			style.Append(curve.BorderWidth);
			style.Append(";");
			style.Append(GetDashStyle(curve.BorderStyle, curve.BorderWidth));
			style.Append("fill:none");
			style.Append(";");

			return style.ToString();
		}

		private string StyleFromLabelImplementation(TextLabel label, string clipId)
		{
			StringBuilder style = new StringBuilder();

			//Sort out the fill
			style.Append("fill:none;");

			//Sort out the stroke, ignore custom pens and use defaults
			//If mShape.CustomPen Is Nothing Then
			style.Append("stroke:");
			style.Append(GetCompatibleColor(label.Color));
			style.Append(";");
			style.Append("stroke-width:1;");

			return style.ToString();
		}

		public static string ExtractLinearGradient(LinearGradientMode gradientMode, Color backColor, Color gradientColor)
		{
			StringBuilder gradient = new StringBuilder();

			gradient.Append("<linearGradient id=\"\" gradientUnits=\"objectBoundingBox\" ");

			//Rotate if not horizontal gradient
			switch (gradientMode)
			{
				case System.Drawing.Drawing2D.LinearGradientMode.BackwardDiagonal:
					gradient.Append("gradientTransform=\"rotate(225)\"");
					break;
				case System.Drawing.Drawing2D.LinearGradientMode.ForwardDiagonal:
					gradient.Append("gradientTransform=\"rotate(45)\"");
					break;
				case System.Drawing.Drawing2D.LinearGradientMode.Vertical:
					gradient.Append("gradientTransform=\"rotate(90)\"");
				break;
			}

			gradient.Append(">");

			//Add the 2 stops - change for blends here
			gradient.Append("\r");
			gradient.Append("\t");
			gradient.Append("<stop offset=\"0%\" style=\"stop-color:");
			gradient.Append(GetCompatibleColor(backColor));
			gradient.Append("\"/>");
			gradient.Append("\r");
			gradient.Append("\t");
			gradient.Append("<stop offset=\"100%\" style=\"stop-color:");
			gradient.Append(GetCompatibleColor(gradientColor));
			gradient.Append("\"/>");

			gradient.Append("</linearGradient>");

			return gradient.ToString();
		}

		public static string GetCompatibleColor(Color color)
		{
			byte[] bytes = new byte[3];
			bytes[0] = color.R;
			bytes[1] = color.G;
			bytes[2] = color.B;
			char[] chars = new char[bytes.Length * 2];
			for (int i = 0; i < bytes.Length; i++) 
			{
				int b = bytes[i];
				chars[i * 2] = hexDigits[b >> 4];
				chars[i * 2 + 1] = hexDigits[b & 0xF];
			}
			return "#" + new string(chars);
		}

		public static string GetDashStyle(DashStyle dashStyle, float lineWidth)
		{
			if (dashStyle == DashStyle.Custom || dashStyle == DashStyle.Solid) return "";

			StringBuilder dash = new StringBuilder();
			string bigDash = XmlConvert.ToString(lineWidth * 3);
			string littleDash = XmlConvert.ToString(lineWidth);

			dash.Append("stroke-dasharray:");
			switch (dashStyle)
			{
				case DashStyle.Dash:
					dash.Append(bigDash);
					dash.Append(",");
					dash.Append(littleDash);
					break;

				case DashStyle.DashDot:
					dash.Append(bigDash);
					dash.Append(",");
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
					break;

				case DashStyle.DashDotDot:
					dash.Append(bigDash);
					dash.Append(",");
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
					break;

				case DashStyle.Dot:
					dash.Append(littleDash);
					dash.Append(",");
					dash.Append(littleDash);
				break;
			}
			dash.Append(";");

			return dash.ToString();
		}
	#endregion

	}
}