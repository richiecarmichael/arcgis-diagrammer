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
	public class Marker
	{
		//Property variables
		private MarkerBase mMarkerBase;

		//Working variables
		private Definition mDefinition;

		#region Interface 

		//Constructors
		//Create a new graphicspath holder
		public Marker(MarkerBase marker)
		{
			MarkerBase = marker;
		}

		//Gets or sets the internal graphicspath object.
		public MarkerBase MarkerBase
		{
			get
			{
				return mMarkerBase;
			}
			set
			{
				mMarkerBase = value;
			}
		}

		//Extracts a polygon from a valid path.")]
		public virtual string ExtractMarker()
		{
			return ExtractMarkerImplementation(mMarkerBase, 0, false);
		}

		public virtual string ExtractMarker(float rotation, bool invert)
		{
			return ExtractMarkerImplementation(mMarkerBase, rotation, invert);
		}

		#endregion

		#region  Implementation 

		private string ExtractMarkerImplementation(MarkerBase marker, float rotation, bool invert)
		{
			GraphicsPath path = marker.GetPath();

			Matrix translateMatrix = new Matrix();
			RectangleF rectF = new RectangleF();

			rectF = path.GetBounds();

			translateMatrix.Translate(-rectF.X, -rectF.Y);
			path.Transform(translateMatrix);
			translateMatrix.Dispose();

			StringBuilder def = new StringBuilder();

			def.Append("<marker id=\"\" viewbox=\"0 0 ");

			//Add viewbox data
			def.Append(XmlConvert.ToString(Math.Round(rectF.Width, 2)));
			def.Append(" ");
			def.Append(XmlConvert.ToString(Math.Round(rectF.Height, 2)));
			def.Append("\" ");
			
			//Add ref x and y
			if (!invert)
			{
				def.Append("refX=\"");
				def.Append(XmlConvert.ToString(Math.Round(rectF.Width, 2)));
				def.Append("\" ");
			}
			def.Append("refY=\"");
			def.Append(XmlConvert.ToString(Math.Round(rectF.Height/2, 2)));
			def.Append("\" ");
			
			//Units
			def.Append("markerUnits=\"strokeWidth\" ");

			//markerWidth and Height
			def.Append("markerWidth=\"");
			def.Append(XmlConvert.ToString(Math.Round(rectF.Width, 2)));
			def.Append("\" ");
			def.Append("markerHeight=\"");
			def.Append(XmlConvert.ToString(Math.Round(rectF.Height, 2)));
			def.Append("\" ");

			//Orient
			def.Append("orient=\"auto\" ");

			//Stroke-width
			def.Append("stroke=\"");
			def.Append(Style.GetCompatibleColor(marker.BorderColor));
			def.Append("\" ");

			//Stroke-width
			def.Append("stroke-width=\"");
			def.Append(XmlConvert.ToString(Math.Round(marker.BorderWidth, 2)));
			def.Append("\" ");

			//Fill
			def.Append("fill=\"");
			
			if (marker.DrawBackground)
			{
				def.Append(Style.GetCompatibleColor(marker.BackColor));
			}
			else
			{
				def.Append("white");
			}
			def.Append("\" ");
			
			def.Append(">");

			//Append path
			if (mDefinition == null)
			{
				mDefinition = new Definition(marker.GetPath());
				mDefinition.IncludeId = false;
			}
			else
			{
				mDefinition.Path = marker.GetPath();
			}

			mDefinition.Rotate(rotation);
			def.Append(mDefinition.ExtractPath());

			//Close tag
			def.Append("</marker>");

			return def.ToString();
		}

		#endregion

	}

}