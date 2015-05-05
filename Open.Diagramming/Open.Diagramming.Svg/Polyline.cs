using System;
using System.Collections;
using System.Data;
using System.ComponentModel;
using System.Drawing;
using System.Xml;

namespace Crainiate.Diagramming.Svg
{
	public class Polyline
	{
		//Property variables
		private Line mLine;
		
		#region  Interface 

		//Create a new polyline class from a line
		public Polyline(Line line)
		{
			Line = line;
		}

		//Sets or gets the line used to extract the polyline
		public virtual Line Line
		{
			get
			{
				return mLine;
			}
			set
			{
				mLine = value;
			}
		}

		//Extracts a polyline definition for this ERM line
		public virtual string ExtractPolyline()
		{
			return ExtractPolylineImplementation();
		}

		#endregion

		#region  Implementation 

		private string ExtractPolylineImplementation()
		{
			System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();

			stringBuilder.Append("<polyline id=\"");
			stringBuilder.Append(mLine.Key);
			stringBuilder.Append("\" class=\"\" points=\"");

			foreach (PointF point in mLine.Points)
			{
                stringBuilder.Append(XmlConvert.ToString(point.X));
				stringBuilder.Append(",");
				stringBuilder.Append(XmlConvert.ToString(point.Y));
				stringBuilder.Append(" ");
			}

			stringBuilder.Append("\" ");

			//Close tag
			stringBuilder.Append("/>");

			return stringBuilder.ToString();
		}

		#endregion
	}
}