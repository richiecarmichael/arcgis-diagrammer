using System;
using System.Xml;
using System.Drawing;
using Crainiate.Diagramming.Drawing2D;

namespace Crainiate.Diagramming.Svg
{
	public class ComplexLineFormatter: LineFormatter
	{
		public override void WriteElement(SvgDocument document, Element element)
		{
            base.WriteElement(document, element);

			ComplexLine line = (ComplexLine) element;

            //Add text
            //Render the segment items
            for (int i = 0; i < line.Points.Count - 1; i++)
            {
                Segment segment = line.Segments[i];
                PointF location = (PointF) line.Points[i];
                PointF reference = (PointF) line.Points[i + 1];

                if (segment.Label != null) AddSegmentText(document, i, line, segment, location, reference);
            }            
		}

        private void AddSegmentText(SvgDocument document, int index, ComplexLine line,Segment segment, PointF targetPoint, PointF referencePoint)
        {
            //Get midpoint of segment
            PointF location = new PointF(targetPoint.X + ((referencePoint.X - targetPoint.X) / 2), targetPoint.Y + ((referencePoint.Y - targetPoint.Y) / 2));

            if (segment.Label.Text.Trim() == "") return;

            Style style = new Style();

            //Set up text object
            location = OffsetPoint(location, segment.Label.Offset);
            //location = OffsetPoint(location, line.Rectangle.Location);

            Double rotation = Geometry.DegreesFromRadians(Geometry.GetAngle(targetPoint.X, targetPoint.Y, referencePoint.X, referencePoint.Y));

            Text text = new Text();
            text.Label = segment.Label;
            text.LayoutRectangle = new RectangleF(location, new SizeF());

            //Get style
            string classId = null;
            classId = document.AddClass(text.GetStyle());

            //Create fragment and add to document
            XmlDocumentFragment frag = null;
            XmlNode newElementNode = null;

            frag = document.CreateDocumentFragment();
            frag.InnerXml = text.ExtractText(0, 0, line.Key + index.ToString() + "Text", "rotate(" + rotation.ToString() + "," + location.X.ToString() + "," + location.Y.ToString() + ")");
            //frag.InnerXml = text.ExtractText(0, 0, line.Key + index.ToString() + "Text");
            frag.FirstChild.Attributes.GetNamedItem("class").InnerText = classId;
            newElementNode = document.ContainerNode.AppendChild(frag);
        }


	}
}
