using System;
using System.Xml;
using System.Drawing;

namespace Crainiate.Diagramming.Svg
{
	public class LinkFormatter: LineFormatter
	{
		public override void WriteElement(SvgDocument document, Element element)
		{
            base.WriteElement(document, element);

			Link link = (Link) element;

            //Add link text
            if (link.Label != null) AddLinkText(document, link);
		}

        private void AddLinkText(SvgDocument document, Link link)
        {
            if (link.Label.Text.Trim() == "") return;

            Style style = new Style();

            //Set up text object
            PointF location = link.GetLabelLocation();
            location = OffsetPoint(location, link.Label.Offset);
            location = OffsetPoint(location, link.Rectangle.Location);

            Text text = new Text();
            text.Label = link.Label;
            text.LayoutRectangle = new RectangleF(location, new SizeF());

            //Get style
            string classId = null;
            classId = document.AddClass(text.GetStyle());

            //Create fragment and add to document
            XmlDocumentFragment frag = null;
            XmlNode newElementNode = null;

            frag = document.CreateDocumentFragment();
            frag.InnerXml = text.ExtractText(0, 0, link.Key + "Text");
            frag.FirstChild.Attributes.GetNamedItem("class").InnerText = classId;
            newElementNode = document.ContainerNode.AppendChild(frag);
        }


	}
}
