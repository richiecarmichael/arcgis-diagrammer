using System;
using System.Xml;
using System.Text;

namespace Crainiate.Diagramming.Svg
{
	public class ComplexFormatter: SolidFormatter
	{
		private XmlElement mGroupElement;

		public virtual XmlElement GroupElement
		{
			get
			{
				return mGroupElement;
			}
		}

		public override void WriteElement(SvgDocument document, Element element)
		{
			base.WriteElement(document, element);

			XmlNode node = Node;
			
			ComplexShape complex = (ComplexShape) element;

			//Add a group for the complex shape
			XmlElement newElement = null;

			StringBuilder builder = new StringBuilder();
			builder.Append("translate(");
			builder.Append(XmlConvert.ToString(complex.X));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(complex.Y));
			builder.Append(")");

			newElement = document.CreateElement("g");
			newElement.SetAttribute("id", complex.Key + "Children");
			newElement.SetAttribute("transform", builder.ToString());

			document.ContainerNode.AppendChild(newElement);

			//Set the element as the temporary container node
			XmlNode temp = document.ContainerNode;
			string tempKey = document.ContainerKey;

			document.ContainerNode = newElement;
			document.ContainerKey = complex.Key;

			//Add each child as an element
			foreach (SolidElement solid in complex.Children.Values)
			{
				document.AddElement(solid);
			}

			document.ContainerNode = temp;
			document.ContainerKey = tempKey;

			//Write the ports
			
			//Restore the XmlElement
			SetNode(node);
			
			//Set the xml element for the group
			mGroupElement = newElement;
		}
	}
}
