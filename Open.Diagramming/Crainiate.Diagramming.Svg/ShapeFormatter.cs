using System;
using System.Xml;
using System.Text;

namespace Crainiate.Diagramming.Svg
{
	public class ShapeFormatter: SolidFormatter
	{
		public override void WriteElement(SvgDocument document, Element element)
		{
			base.WriteElement(document, element);
			
			XmlNode node = Node;

			Shape shape = (Shape) element;
			
			//Set the element as the temporary container node
			XmlNode temp = document.ContainerNode;
			string tempKey = document.ContainerKey;

			document.ContainerKey = shape.Key + "Ports";

			//Add each child as an element
			foreach (Port port in shape.Ports.Values)
			{
				if (port.Visible) document.AddElement(port);
			}

			document.ContainerKey = tempKey;

			//Restore shape element
			SetNode(node);
		}
	}
}
