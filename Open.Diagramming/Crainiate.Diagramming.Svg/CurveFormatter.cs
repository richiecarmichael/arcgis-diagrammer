using System;
using System.Xml;

namespace Crainiate.Diagramming.Svg
{
	public class CurveFormatter: Formatter
	{
		//Working variables
		private Marker _marker;
		private Curvedline _curvedline;

		public override void WriteElement(SvgDocument document, Element element)
		{
			Curve curve = (Curve) element;
			
			Style style = new Style(curve);

			XmlNode newNode = null;
			XmlDocumentFragment fragment = null;

			string classId = null;

			//Add the line
			_curvedline.Curve = curve;
			fragment = document.CreateDocumentFragment();
			fragment.InnerXml = _curvedline.ExtractCurve();

			newNode = document.ContainerNode.AppendChild(fragment);

			//Determine style
			classId = document.AddClass(style.GetStyle(), "");
			newNode.Attributes.GetNamedItem("class").InnerText = classId;
			
			//Reset the key if in a container
			if (document.ContainerKey != null && document.ContainerKey != string.Empty) 
			{
				newNode.Attributes.GetNamedItem("id").Value  = document.ContainerKey + "," + curve.Key;
			}

			//Check for start marker
			if (curve.Start.Marker != null)
			{
				_marker.MarkerBase = curve.Start.Marker;

				//Check for a definition or add a new one
				string defId = document.AddDefinition(_marker.ExtractMarker(-90, true));
				
				XmlElement newElement = (XmlElement) newNode;
				newElement.SetAttribute("marker-start","url(#" + defId + ")");
			}

			//Check for end marker
			if (curve.End.Marker != null)
			{
				_marker.MarkerBase = curve.End.Marker;

				//Check for a definition or add a new one
				string defId = document.AddDefinition(_marker.ExtractMarker(90, false));
				
				XmlElement newElement = (XmlElement) newNode;
				newElement.SetAttribute("marker-end","url(#" + defId + ")");
			}
		}

		public override void Reset()
		{
			base.Reset();
			if (_marker == null) _marker = new Marker(null);
			if (_curvedline == null) _curvedline = new Curvedline(null);
		}
	}
}
