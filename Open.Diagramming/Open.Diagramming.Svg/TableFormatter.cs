using System;
using System.Xml;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Crainiate.Diagramming.Svg
{
	public class TableFormatter: SolidFormatter
	{
		private Definition mDefinition;

		public TableFormatter()
		{
			Reset();
		}

		public override void Reset()
		{
			base.Reset();
			if (mDefinition == null) mDefinition = new Definition(null);
		}

		public override void WriteElement(SvgDocument document, Element element)
		{
			Table table = (Table) element;

			//Get definition
			Definition definition = new Definition(table.GetPath());
			DefinitionId = document.AddDefinition(definition.ExtractDefinition(), "");

			//Add the shadow use only if background is drawn, layer has shadows, and is a subclass of solid
			if (table.DrawBackground && table.Layer.DrawShadows)
			{
				StringBuilder stringBuilder = new StringBuilder();
				double opacity = Math.Round(Convert.ToDouble(table.Opacity / 1000F), 2);

				stringBuilder.Append("fill:");
				stringBuilder.Append(ColorTranslator.ToHtml(table.Layer.ShadowColor));
				stringBuilder.Append(";fill-opacity:");
				stringBuilder.Append(opacity.ToString());
				stringBuilder.Append(";");

				ClassId = document.AddClass(stringBuilder.ToString(), "");

				document.AddUse(table.Key.ToString() + "Shadow", DefinitionId, ClassId, "", table.X + element.Layer.ShadowOffset.X, table.Y + element.Layer.ShadowOffset.Y);
			}
			
			//Set up the clip
			ClipId = document.AddClipPath(DefinitionId, 0, 0);

			//Add a group for the table shape
			XmlElement newElement = null;

			StringBuilder builder = new StringBuilder();
			builder.Append("translate(");
			builder.Append(XmlConvert.ToString(table.X));
			builder.Append(",");
			builder.Append(XmlConvert.ToString(table.Y));
			builder.Append(")");

			newElement = document.CreateElement("g");
			newElement.SetAttribute("id", table.Key + "Table");
			newElement.SetAttribute("transform", builder.ToString());

			builder = new StringBuilder();
			
			builder.Append("url(#");
			builder.Append(ClipId);
			builder.Append(")");

			newElement.SetAttribute("clip-path", builder.ToString());

			document.ContainerNode.AppendChild(newElement);

			//Set the element as the temporary container node
			XmlNode temp = document.ContainerNode;
			string tempKey = document.ContainerKey;

			document.ContainerNode = newElement;
			document.ContainerKey = table.Key;

			//Add each child as an element
			WriteHeading(document, table);
			
			if (table.Expanded)
			{
				int groupId = 1;
				int rowId = 1;
				float height = table.HeadingHeight + 1;

				foreach (TableRow row in table.Rows)
				{
					WriteTableRow(document, table, row, ref rowId, ref height);
				}

				foreach(TableGroup group in table.Groups)
				{
					WriteTableGroup(document, table, group, ref groupId, ref rowId, ref height);
				}
			}

			document.ContainerNode = temp;
			document.ContainerKey = tempKey;

			StringBuilder style = new StringBuilder();
			string colorString = Style.GetCompatibleColor(table.BorderColor);

			style.Append("stroke:");
			style.Append(colorString);
			style.Append(";");
			style.Append("stroke-width:");
			style.Append(table.BorderWidth);
			style.Append(";");
			style.Append(Style.GetDashStyle(table.BorderStyle, table.BorderWidth));
			style.Append("fill:none");
			style.Append(";");

			//Determine style
			ClassId = document.AddClass(style.ToString());

			//Add the use
			document.AddUse(table.Key.ToString(), DefinitionId, ClassId, "", table.X, table.Y);

			//Set the xml element
			SetNode(newElement);
		}

		protected virtual void WriteTableGroup(SvgDocument document, Table table, TableGroup group, ref int id, ref int rowId, ref float height)
		{
			//Add background
			mDefinition.Path = new GraphicsPath();
			mDefinition.Path.AddRectangle(new RectangleF(0, 0, table.Width, table.RowHeight));

			//Add the definition
			string defId = document.AddDefinition(mDefinition.ExtractRectangle());
			
			StringBuilder builder = new StringBuilder();
			builder.Append("fill:");
			builder.Append(Style.GetCompatibleColor(group.Backcolor));
			builder.Append(";fill-opacity:0.5;");

			//Add the class
			string classId = document.AddClass(builder.ToString());
			string key = "Group" + id.ToString();
			
			document.AddUse(key, defId, classId, "", 0, height);

			//Add the text
			Style style = new Style();
			Font font = null;

			//Add group text
			if (group.Text.Trim() != "")
			{
				string clipId = document.AddClipPath(defId, 0, height);
				
				//Add clipping to style if required
				style.ClipPathId = clipId;

				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.FormatFlags = StringFormatFlags.NoWrap;

				//Set up text object
				Text text = new Text();
				text.Format = format;	
				text.LayoutRectangle = new RectangleF(0, 0, table.Width - 20 - table.Indent - group.Indent, table.RowHeight);

				//Get style
				font = Component.Instance.GetFont(table.FontName, table.FontSize, table.FontStyle);
				classId = document.AddClass(text.GetStyle(font, table.Forecolor, clipId), "");

				//Create fragment and add to document
				XmlDocumentFragment frag = null;
				XmlNode newElementNode = null;

				frag = document.CreateDocumentFragment();
				frag.InnerXml = text.ExtractText(group.Text, font, table.Indent + group.Indent, height, key + "Text");
				frag.FirstChild.Attributes.GetNamedItem("class").InnerText = classId;
				newElementNode = document.ContainerNode.AppendChild(frag);
			}

			id++;
			height += table.RowHeight;

			if (group.Expanded)
			{
				//Draw child groups
				foreach (TableGroup child in group.Groups)
				{
					WriteTableGroup(document, table, child, ref id, ref rowId, ref height); 
				}

				//Draw child groups
				foreach (TableRow childRow in group.Rows)
				{
					WriteTableRow(document, table, childRow, ref rowId, ref height);
				}
			}
		}

		protected virtual void WriteTableRow(SvgDocument document, Table table, TableRow row, ref int id, ref float height)
		{
			//Add background
			mDefinition.Path = new GraphicsPath();
			mDefinition.Path.AddRectangle(new RectangleF(0, 0, row.Indent, table.RowHeight));

			//Add the definition
			string defId = document.AddDefinition(mDefinition.ExtractRectangle());
			
			StringBuilder builder = new StringBuilder();
			builder.Append("fill:");
			builder.Append(Style.GetCompatibleColor(row.Backcolor));
			builder.Append(";fill-opacity:0.5;");

			//Add the class
			string classId = document.AddClass(builder.ToString());
			string key = "Row" + id.ToString();
			
			document.AddUse(key, defId, classId, "", 0, height);

			//Add the text
			Style style = new Style();
			Font font = null;

			//Add row text
			if (row.Text.Trim() != "")
			{
				//Get a new clip path
				mDefinition.Path = new GraphicsPath();
				mDefinition.Path.AddRectangle(new RectangleF(0, 0, table.Width - row.Indent, table.RowHeight));

				//Add the definition
				defId = document.AddDefinition(mDefinition.ExtractRectangle());
				string clipId = document.AddClipPath(defId, row.Indent, height);
				
				//Add clipping to style if required
				style.ClipPathId = clipId;

				StringFormat format = new StringFormat();
				format.LineAlignment = StringAlignment.Center;
				format.FormatFlags = StringFormatFlags.NoWrap;

				//Set up text object
				Text text = new Text();
				text.Format = format;	
				text.LayoutRectangle = new RectangleF(0, 0, table.Width - 20 - table.Indent - row.Indent, table.RowHeight);

				//Get style
				font = Component.Instance.GetFont(table.FontName, table.FontSize, table.FontStyle);
				classId = document.AddClass(text.GetStyle(font, table.Forecolor, clipId), "");

				//Create fragment and add to document
				XmlDocumentFragment frag = null;
				XmlNode newElementNode = null;

				frag = document.CreateDocumentFragment();
				frag.InnerXml = text.ExtractText(row.Text, font, row.Indent, height, key + "Text");
				frag.FirstChild.Attributes.GetNamedItem("class").InnerText = classId;
				newElementNode = document.ContainerNode.AppendChild(frag);
			}

			id++;
			height += table.RowHeight;
		}

		protected virtual void WriteHeading(SvgDocument document, Table table)
		{
			//Add heading
			mDefinition.Path = new GraphicsPath();
			mDefinition.Path.AddRectangle(new RectangleF(0,0,table.Width, table.HeadingHeight));

			//Add the definition
			string defId = document.AddDefinition(mDefinition.ExtractRectangle());
			
			//Add the class
			string gradient = Style.ExtractLinearGradient(LinearGradientMode.Horizontal, table.GradientColor, table.BackColor);
			string classId = document.AddClass("fill:url(#none)", gradient);
			
			document.AddUse("Heading", defId, classId, "", 0 , 0);

			//Add the rest of the background
			mDefinition.Path = new GraphicsPath();
			mDefinition.Path.AddRectangle(new RectangleF(0, 0, table.Width, table.Height - table.HeadingHeight));

			//Add the definition
			defId = document.AddDefinition(mDefinition.ExtractRectangle());
			classId = document.AddClass("fill:" + Style.GetCompatibleColor(table.BackColor));

			document.AddUse("Fill", defId, classId, "", 0 , table.HeadingHeight);

			Style style = new Style();
			Font font = null;

			//Add Heading text
			if (table.Heading.Trim() != "")
			{
				//Add clipping to style if required
				style.ClipPathId = ClipId;

				//Set up text object
				Text text = new Text();

				//Get style
				font = Component.Instance.GetFont(table.FontName,table.FontSize,FontStyle.Bold);
				classId = document.AddClass(text.GetStyle(font, table.Forecolor, ClipId), "");

				//Create fragment and add to document
				XmlDocumentFragment frag = null;
				XmlNode newElementNode = null;

				frag = document.CreateDocumentFragment();
				frag.InnerXml = text.ExtractText(table.Heading, font, 8, 5, table.Key +"Heading");
				frag.FirstChild.Attributes.GetNamedItem("class").InnerText = classId;
				newElementNode = document.ContainerNode.AppendChild(frag);
			}

			//Add Heading text
			if (table.SubHeading.Trim() != "")
			{
				style = new Style();

				//Add clipping to style if required
				style.ClipPathId = ClipId;

				//Set up text object
				Text text = new Text();

				//Get style
				font = Component.Instance.GetFont(table.FontName,table.FontSize-1,FontStyle.Regular);
				classId = document.AddClass(text.GetStyle(font, table.Forecolor, ClipId), "");

				//Create fragment and add to document
				XmlDocumentFragment frag = null;
				XmlNode newElementNode = null;

				frag = document.CreateDocumentFragment();
				frag.InnerXml = text.ExtractText(table.SubHeading, font, 8, 20, table.Key +"Subheading");
				frag.FirstChild.Attributes.GetNamedItem("class").InnerText = classId;
				newElementNode = document.ContainerNode.AppendChild(frag);
			}
		}
	}
}
