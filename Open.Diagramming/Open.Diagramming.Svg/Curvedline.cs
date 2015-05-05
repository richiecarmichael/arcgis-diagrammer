using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Crainiate.Diagramming.Svg
{
	public class Curvedline
	{
		private Curve mCurve;

		#region  Interface 

		//Create a new polycurve class from an ERM curve
		public Curvedline(Curve curve)
		{
			Curve = curve;
		}

		//Sets or gets the curve used to extract the curve
		public virtual Curve Curve
		{
			get
			{
				return mCurve;
			}
			set
			{
				mCurve = value;
			}
		}

		//Extracts a polycurve definition for this ERM curve
		public virtual string ExtractCurve()
		{
			return ExtractCurveImplementation();
		}

		#endregion

		#region  Implementation 

		private string ExtractCurveImplementation()
		{
			StringBuilder stringBuilder = new System.Text.StringBuilder();
			GraphicsPath path = mCurve.GetPath();
			
			//Translate path to the location of the curve
			Matrix matrix = new Matrix();
			matrix.Translate(mCurve.Rectangle.X, mCurve.Rectangle.Y);
			path.Transform(matrix);
			
			PathData pathData = path.PathData;

			int i = 0;

			byte bytType;
			int cCount = 0;

			string strX = null;
			string strY = null;

			stringBuilder.Append("<path id=\"");
			stringBuilder.Append(mCurve.Key.ToString());
			stringBuilder.Append("\" class=\"\" d=\"");

			//We'll use the same path logic as definitions using the
			//mcurve.GraphicsPath.PathPoints and type
			for (i = 0; i <= pathData.Points.GetUpperBound(0); i++)
			{
				bytType = pathData.Types[i];
				strX = XmlConvert.ToString(Math.Round(pathData.Points[i].X, 2));
				strY = XmlConvert.ToString(Math.Round(pathData.Points[i].Y, 2));

				if (bytType == 0) //Moveto
				{
					stringBuilder.Append("M ");
					stringBuilder.Append(strX);
					stringBuilder.Append(",");
					stringBuilder.Append(strY);
					stringBuilder.Append(" ");
					cCount = 0;
				}
				else
				{
					if (bytType == 1) //Curveto
					{
						stringBuilder.Append("L ");
						stringBuilder.Append(strX);
						stringBuilder.Append(",");
						stringBuilder.Append(strY);
						stringBuilder.Append(" ");
						cCount = 0;
					}
					else
					{
						//Curve, in groups of 3
						if (cCount == 0) stringBuilder.Append("C ");
						stringBuilder.Append(strX);
						stringBuilder.Append(",");
						stringBuilder.Append(strY);
						stringBuilder.Append(" ");
						cCount += 1;
						if (cCount == 3) cCount = 0;
					}
				}
			}

			stringBuilder.Append("\"/>");

			return stringBuilder.ToString();
		}
		#endregion
	}
}