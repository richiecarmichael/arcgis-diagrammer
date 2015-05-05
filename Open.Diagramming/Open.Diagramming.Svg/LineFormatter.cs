using System;
using System.Xml;

namespace Crainiate.Diagramming.Svg
{
	public class LineFormatter: Formatter
	{
		//Working variables
		private Marker mMarker;
		private Polyline mPolyLine;

		public override void WriteElement(SvgDocument document, Element element)
		{
			Line line = (Line) element;
			
			Style style = new Style(line);

			XmlNode newNode = null;
			XmlDocumentFragment fragment = null;

			string classId = null;

			//Add the line
			mPolyLine.Line = line;
			fragment = document.CreateDocumentFragment();
			fragment.InnerXml = mPolyLine.ExtractPolyline();

			newNode = document.ContainerNode.AppendChild(fragment);

			//Determine style
			classId = document.AddClass(style.GetStyle(), "");
			newNode.Attributes.GetNamedItem("class").InnerText = classId;
			
			//Reset the key if in a container
			if (document.ContainerKey != null && document.ContainerKey != string.Empty) 
			{
				newNode.Attributes.GetNamedItem("id").Value  = document.ContainerKey + "," + line.Key;
			}

			//Check for start marker
			if (line.Start.Marker != null)
			{
				mMarker.MarkerBase = line.Start.Marker;

				//Check for a definition or add a new one
				string defId = document.AddDefinition(mMarker.ExtractMarker(-90, true));
				
				XmlElement newElement = (XmlElement) newNode;
				newElement.SetAttribute("marker-start","url(#" + defId + ")");
			}

			//Check for end marker
			if (line.End.Marker != null)
			{
				mMarker.MarkerBase = line.End.Marker;

				//Check for a definition or add a new one
				string defId = document.AddDefinition(mMarker.ExtractMarker(90, false));
				
				XmlElement newElement = (XmlElement) newNode;
				newElement.SetAttribute("marker-end","url(#" + defId + ")");
			}

			//Set the xml element
			SetNode(newNode);
		}

		public override void Reset()
		{
			base.Reset();
			if (mMarker == null) mMarker = new Marker(null);
			if (mPolyLine == null) mPolyLine = new Polyline(null);
		}
	}
}
