using System;
using System.Xml;
using System.Drawing;

namespace Crainiate.Diagramming.Svg
{
	public abstract class Formatter
	{
		private XmlNode mNode;

		public virtual XmlNode Node
		{
			get
			{
				return mNode;
			}
		}

		protected virtual void SetNode(XmlNode node)
		{
			mNode = node;
		}
		
		public virtual void Reset()
		{
			mNode = null;
		}

		public abstract void WriteElement(SvgDocument document, Element element);

        protected PointF OffsetPoint(PointF a, PointF b)
        {
            return new PointF(a.X + b.X, a.Y + b.Y);
        }
		
	}
}
