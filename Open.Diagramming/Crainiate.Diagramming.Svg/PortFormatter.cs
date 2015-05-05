using System;
using System.Xml;
using System.Text;

namespace Crainiate.Diagramming.Svg
{
	public class PortFormatter: SolidFormatter
	{
		public override void WriteElement(SvgDocument document, Element element)
		{
			base.WriteElement(document, element);

			//Reset the transform to the port transform
			XmlElement xmlelement = (XmlElement) Node;
			Port port = element as Port;

			xmlelement.SetAttribute("x", (port.X + port.Offset.X).ToString());
			xmlelement.SetAttribute("y", (port.Y + port.Offset.Y).ToString());
		}
	}
}
