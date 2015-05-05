using System;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Collections;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void ExpandedChangedEventHandler(object sender, bool expanded);
	
	[Serializable]
	public class Table: Shape, IExpandable, IMouseEvents, ISerializable, ICloneable
	{
		//Property variables
		private string mHeading;
		private string mSubHeading;
		private TableItems mGroups;
		private TableItems mRows;
		private float mHeadingHeight;
		private bool mExpanded;
		private bool mDrawExpand;
		private SizeF mExpandedSize;
		private SizeF mContractedSize;
		private float mGroupHeight;
		private float mRowHeight;
		private float mRowIndent;
		private Font mFont;
		private Color mForecolor;
		private TableItem mSelectedItem;
		private bool mDrawSelectedItem;

		//Working variables
		private GraphicsPath mExpandPath;
		
		#region Interface
		
		//Events
		public event ExpandedChangedEventHandler ExpandedChanged;
		public event EventHandler SelectedItemChanged;
		
		//Constructors
		public Table()
		{
			SuspendEvents = true;
			
			Groups = new TableItems(typeof(TableGroup));
			Rows = new TableItems(typeof(TableRow));
			HeadingHeight = 40;
			Heading = "Heading";
			SubHeading = "Sub Heading";
			GroupHeight = 20;
			RowHeight = 20;
			Indent = 16;
			ExpandedSize = Size;
			ContractedSize = new SizeF(Size.Width,CalculateHeight());
			Expanded = true;
			DrawExpand = false;
			Forecolor = Color.Black;
			GradientColor = SystemColors.Control;
			DrawSelectedItem = true;

			SuspendEvents = false;
		}

		public Table(Table prototype): base(prototype)
		{
			mHeadingHeight = prototype.HeadingHeight;
			mHeading = prototype.Heading;
			mSubHeading = prototype.SubHeading;
			mGroupHeight = prototype.GroupHeight;
			mRowHeight = prototype.RowHeight;
			mRowIndent = prototype.Indent;
			mExpanded = prototype.Expanded;
			mDrawExpand = prototype.DrawExpand;
			mForecolor = prototype.Forecolor;
			GradientColor = prototype.GradientColor;
			mDrawSelectedItem = prototype.DrawSelectedItem;
			mFont = prototype.Font;

			ContractedSize = prototype.ContractedSize;
			ExpandedSize = prototype.ExpandedSize;
			
			Groups = new TableItems(typeof(TableGroup));
			Rows = new TableItems(typeof(TableRow));

			Table.CopyGroups(prototype.Groups,Groups);
			Table.CopyRows(prototype.Rows,Rows);
		}

		protected Table(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;

			//Set up rows and groups
			Groups = (TableItems) info.GetValue("Groups",typeof(TableItems));
			Rows = (TableItems) info.GetValue("Rows",typeof(TableItems));
			SelectedItem = (TableItem) info.GetValue("SelectedItem",typeof(TableItem));

			Heading = info.GetString("Heading");
			SubHeading = info.GetString("SubHeading");
			HeadingHeight = info.GetSingle("HeadingHeight");
			DrawExpand = info.GetBoolean("DrawExpand");
			ExpandedSize = Serialize.GetSizeF(info.GetString("ExpandedSize"));
			ContractedSize = Serialize.GetSizeF(info.GetString("ContractedSize"));
			mExpanded = info.GetBoolean("Expanded");
			GroupHeight = info.GetSingle("GroupHeight");
			RowHeight = info.GetSingle("RowHeight");
			Indent = info.GetSingle("Indent");
			SetFont(Serialize.GetFont(info.GetString("Font")));
			Forecolor = Color.FromArgb(Convert.ToInt32(info.GetString("Forecolor")));
			DrawSelectedItem = info.GetBoolean("DrawSelectedItem");
			
			SuspendEvents = false;
		}

		//Properties
		public virtual TableItems Groups
		{
			get
			{
				return mGroups;
			}
			set
			{
				if (mGroups != null)
				{
					mGroups.InsertItem -=new TableItemsEventHandler(TableItems_InsertItem);
					mGroups.RemoveItem -=new TableItemsEventHandler(TableItems_RemoveItem);
				}
				
				mGroups = value;
				
				if (mGroups != null)
				{
					mGroups.InsertItem +=new TableItemsEventHandler(TableItems_InsertItem);
					mGroups.RemoveItem +=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				OnElementInvalid();
			}
		}

		public virtual TableItems Rows
		{
			get
			{
				return mRows;
			}
			set
			{
				if (mRows != null)
				{
					mRows.InsertItem-=new TableItemsEventHandler(TableItems_InsertItem);
					mRows.RemoveItem-=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				mRows = value;

				if (mRows != null)
				{
					mRows.InsertItem+=new TableItemsEventHandler(TableItems_InsertItem);
					mRows.RemoveItem+=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				OnElementInvalid();
			}
		}

		public virtual Color Forecolor
		{
			get
			{
				return mForecolor;
			}
			set
			{
				mForecolor = value;
				OnElementInvalid();
			}
		}

		public virtual string Heading
		{
			get
			{
				return mHeading;
			}
			set
			{
				if (mHeading != value)
				{
					mHeading = value;
					OnElementInvalid();
				}
			}
		}
		public virtual string SubHeading
		{
			get
			{
				return mSubHeading;
			}
			set
			{
				if (mSubHeading != value)
				{
					mSubHeading = value;
					OnElementInvalid();
				}
			}
		}

		public virtual string FontName
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont.FontFamily.Name;
				return mFont.FontFamily.Name;
			}
			set
			{
				mFont = Component.Instance.GetFont(value,FontSize,FontStyle);
				OnElementInvalid();
			}
		}

		public virtual float FontSize
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont.Size;
				return mFont.Size;
			}
			set
			{
				mFont = Component.Instance.GetFont(FontName,value,FontStyle);
				OnElementInvalid();
			}
		}

		public virtual FontStyle FontStyle
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont.Style;
				return mFont.Style;
			}
			set
			{
				mFont = Component.Instance.GetFont(FontName,FontSize,value);
				OnElementInvalid();
			}
		}

		public Font Font
		{
			get
			{
				if (mFont == null) return Component.Instance.DefaultFont;
				return mFont;
			}
		}

		public virtual float HeadingHeight
		{
			get
			{
				return mHeadingHeight;
			}
			set
			{
				if (mHeadingHeight != value)
				{
					mHeadingHeight = value;
				}
			}
		}

		public virtual float GroupHeight
		{
			get
			{
				return mGroupHeight;
			}
			set
			{
				if (mGroupHeight != value)
				{
					mGroupHeight = value;
				}
			}
		}
		
		public virtual float RowHeight
		{
			get
			{
				return mRowHeight;
			}
			set
			{
				if (mRowHeight != value)
				{
					mRowHeight = value;
				}
			}
		}

		public virtual float Indent
		{
			get
			{
				return mRowIndent;
			}
			set
			{
				if (mRowIndent != value)
				{
					mRowIndent = value;
					OnElementInvalid();
				}
			}
		}

		public virtual bool Expanded
		{
			get
			{
				return mExpanded;
			}
			set
			{
				if (mExpanded != value)
				{
					mExpanded = value;
					
					//Adjust size of group
					if (mExpanded)
					{
						ContractedSize = Size;
						Size = ExpandedSize;
					}
					else
					{
						ExpandedSize = Size;
						Size = ContractedSize;
					}
					OnExpandedChanged();
				}
			}
		}

		public virtual bool DrawExpand
		{
			get
			{
				return mDrawExpand;
			}
			set
			{
				if (mDrawExpand != value)
				{
					mDrawExpand = value;
					OnElementInvalid();
				}
			}
		}

		//Returns the expander path
		public virtual GraphicsPath Expander
		{
			get
			{
				return mExpandPath;
			}
		}

		public virtual SizeF ExpandedSize
		{
			get
			{
				return mExpandedSize;
			}
			set
			{
				if (!mExpandedSize.Equals(value))
				{
					mExpandedSize = value;
					if (MaximumSize.Width < mExpandedSize.Width) MaximumSize = new SizeF(mExpandedSize.Width,MaximumSize.Height);
					if (MaximumSize.Height < mExpandedSize.Height) MaximumSize = new SizeF(MaximumSize.Width, mExpandedSize.Height);
				}
			}
		}

		public virtual SizeF ContractedSize
		{
			get
			{
				return mContractedSize;
			}
			set
			{
				if (!mContractedSize.Equals(value))
				{
					mContractedSize = value;
					if (MinimumSize.Width > mContractedSize.Width) MinimumSize = new SizeF(mContractedSize.Width,MinimumSize.Height);
					if (MinimumSize.Height > mContractedSize.Height) MinimumSize = new SizeF(MinimumSize.Width, mContractedSize.Height);
				}
			}
		}

		public virtual TableItem SelectedItem
		{
			get
			{
				return mSelectedItem;
			}
			set
			{
				if (mSelectedItem != value)
				{
					mSelectedItem = value;
					OnSelectedItemChanged();
				}

				OnElementInvalid();
			}
		}

		public virtual bool DrawSelectedItem
		{
			get
			{
				return mDrawSelectedItem;
			}
			set
			{
				mDrawSelectedItem = value;
				OnElementInvalid();
			}
		}

		//Methods
		public virtual void SetHeight()
		{
            float height = CalculateHeight();

            //Only set the height if the table is expanded
            if (Expanded)
            {
                Height = height;
            }
            else
            {
                ExpandedSize = new SizeF(ExpandedSize.Width, height);
            }
		}
		
		public virtual bool OnMouseDown(System.Windows.Forms.MouseEventArgs e)
		{
			//Only consider left button
			if (e.Button != MouseButtons.Left) return false;

			Diagram diagram = GetDiagram();
			PointF location = diagram.PointToDiagram(e);
			PointF local = PointToElement(location);

			TableItem old = SelectedItem;
			SelectedItem = GetTableItemFromLocation(local);
			if (SelectedItem != old && Selected) return true;
			
			//Check to see if any groups can be expanded
			TableGroup group = GetTableGroupExpander(Groups, local);
			if (group != null)
			{
				group.Expanded = ! group.Expanded;
				return true;
			}

			return false;
		}

		public virtual bool OnMouseMove(System.Windows.Forms.MouseEventArgs e)
		{
			return false;
		}

		public virtual bool OnMouseUp(System.Windows.Forms.MouseEventArgs e)
		{
			
			return false;
		}

		//Returns the table item from a mouse point
		public virtual TableItem GetTableItem(System.Windows.Forms.MouseEventArgs e)
		{
			Diagram diagram = GetDiagram();
			PointF location = diagram.PointToDiagram(e);
			PointF local = PointToElement(location);
			return GetTableItemFromLocation(local);
		}

		//Returns a table item from a local point
		public virtual TableItem GetTableItem(PointF location)
		{
			return GetTableItemFromLocation(location);
		}

		public virtual PointF GetItemPosition(TableItem item)
		{
			float height = HeadingHeight + 2;

			//Add top level groups
			foreach (TableGroup group in Groups)
			{
				if (group == item) return new PointF(0, height);
				if (GetGroupHeight(item, group, ref height)) return new PointF(0, height);
			}
			
			//Add top level rows
			foreach (TableRow row in Rows)
			{
				if (row == item) return new PointF(0, height);
				height += RowHeight;
			}
			
			return Point.Empty;
		}

		//Methods
		//Sets the internal font directly
		protected internal virtual void SetFont(Font font)
		{
			mFont = font;
		}

		protected virtual void OnExpandedChanged()
		{
			if (!SuspendEvents && ExpandedChanged != null) ExpandedChanged(this,Expanded);
		}

		protected virtual void OnSelectedItemChanged()
		{
			if (!SuspendEvents && SelectedItemChanged != null) SelectedItemChanged(this,EventArgs.Empty);
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Table(this);
		}

		protected internal override bool SuspendEvents
		{
			get
			{
				return base.SuspendEvents;
			}
			set
			{
				base.SuspendEvents = value;

				//Suspend events in each group and row
				if (Groups != null)
				{
					foreach (TableGroup group in Groups)
					{
						group.SuspendEvents = value;
					
						foreach (TableRow row in group.Rows)
						{
							row.SuspendEvents = value;
						}
					}
				}
				
				//Suspend each top level row
				if (Rows != null)
				{
					foreach (TableRow row in Rows)
					{
						row.SuspendEvents = value;
					}
				}
			}
		}


		protected internal override void Render(Graphics graphics, IRender render)
		{
			RenderTable(graphics,render);
			RenderExpand(graphics,render);

			//Render the ports
			if (Ports != null)
			{
				foreach (Port port in Ports.Values)
				{
					graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
					port.SuspendValidation();
					port.Render(graphics,render);
					port.ResumeValidation();
					graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
				}
			}
		}

		//Implement a base rendering of an element selection
		protected internal override void RenderAction(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			if (renderDesign.ActionStyle == ActionStyle.Default)
			{
				RenderTable(graphics,render);
	
				//Render the ports
				if (Ports != null)
				{
					foreach (Port port in Ports.Values)
					{
						if (port.Visible)
						{
							graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
							port.SuspendValidation();
							port.Render(graphics,render);
							port.ResumeValidation();
							graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
						}
					}
				}
			}
			else
			{
				base.RenderAction(graphics,render,renderDesign);
			}
		}

		public override Handle Handle(PointF location)
		{
			//Check expander
			PointF local = PointToElement(location);
			if (Expander != null && Expander.IsVisible(local)) return new Handle(HandleType.Arrow);

			//Check groups
			if (GetTableGroupExpander(Groups, local) != null) return new Handle(HandleType.Arrow);

			return base.Handle(location);
		}

		#endregion

		#region Events

		//Fired when a group or top level row is added to the table
		private void TableItems_InsertItem(object sender, TableItemsEventArgs e)
		{
			TableItem item = e.Value;

			//Set common values and event handlers for row or group
			item.SetTable(this);
			item.Backcolor = GradientColor;

			//If is a row then set the indent
			if (item is TableRow) item.SetIndent(Indent);

			if (item is TableGroup)
			{
				TableGroup group = (TableGroup) item;
				group.HeightChanged +=new EventHandler(TableGroup_HeightChanged);
				group.ExpandedChanged += new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);

				//Set all the table references correctly
				group.SetTable();
			}

			item.TableItemInvalid +=new EventHandler(TableItems_TableItemInvalid);

			SetHeight();

			//Make sure diagram is redrawn
			OnElementInvalid();
		}
		
		private void TableItems_RemoveItem(object sender, TableItemsEventArgs e)
		{
			TableItem item = e.Value;

			//Remove handlers
			if (item is TableGroup)
			{
				TableGroup group = (TableGroup) item;
				group.HeightChanged -=new EventHandler(TableGroup_HeightChanged);
				group.ExpandedChanged -= new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);
			}
			item.TableItemInvalid -=new EventHandler(TableItems_TableItemInvalid);

			SetHeight();
			OnElementInvalid();
		}

		//Fired when a group or toplevel row becomes invalid
		private void TableItems_TableItemInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		private void TableGroup_HeightChanged(object sender, EventArgs e)
		{
			SetHeight();
		}

		private void TableGroup_ExpandedChanged(object sender, bool Expanded)
		{
			SetHeight();
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Heading",Heading);
			info.AddValue("SubHeading",SubHeading);
			info.AddValue("HeadingHeight",HeadingHeight);
			info.AddValue("Expanded",Expanded);
			info.AddValue("DrawExpand",DrawExpand);
			info.AddValue("ExpandedSize",Serialize.AddSizeF(ExpandedSize));
			info.AddValue("ContractedSize",Serialize.AddSizeF(ContractedSize));
			info.AddValue("GroupHeight",GroupHeight);
			info.AddValue("RowHeight",RowHeight);
			info.AddValue("Indent",Indent);
			info.AddValue("Font",Serialize.AddFont(Font));
			info.AddValue("Forecolor",Forecolor.ToArgb().ToString());
			info.AddValue("Groups",Groups);
			info.AddValue("Rows",Rows);
			info.AddValue("SelectedItem",SelectedItem);
			info.AddValue("DrawSelectedItem",DrawSelectedItem);
			
			base.GetObjectData(info,context);
		}	
	
		//Copy all rows from source to target via clone
		public static void CopyRows(TableItems source,TableItems target)
		{
			foreach (TableRow row in source)
			{
				target.Add((TableRow) row.Clone());
			}
		}

		//Copy all rows from source to target via clone
		public static void CopyGroups(TableItems source,TableItems target)
		{
			foreach (TableGroup group in source)
			{
				target.Add((TableGroup) group.Clone());
			}
		}

		private void RenderTable(Graphics graphics, IRender render)
		{
			GraphicsPath path = GetPathInternal();
			if (path == null) return;

			//Draw background
			Color backColor = render.AdjustColor(BackColor,0,Opacity);
			Color gradientColor = render.AdjustColor(GradientColor,0,Opacity);
			SolidBrush brush = new SolidBrush(backColor);
			graphics.FillPath(brush,path);

			Region current = graphics.Clip;
			Region region = new Region(GetPathInternal());
			graphics.SetClip(region,CombineMode.Intersect);

			//Draw Heading
			RectangleF headingRect = new RectangleF(0,0,Width,HeadingHeight);
			LinearGradientBrush gradient = new LinearGradientBrush(headingRect,gradientColor,backColor,LinearGradientMode.Horizontal);
			graphics.FillRectangle(gradient,headingRect);
			
			//Draw Heading text
			brush.Color = render.AdjustColor(Forecolor,1,Opacity);
			graphics.DrawString(Heading,Component.Instance.GetFont(FontName,FontSize,FontStyle.Bold),brush,8,5);
			graphics.DrawString(SubHeading,Component.Instance.GetFont(FontName,FontSize-1,FontStyle.Regular),brush,8,20);			
			
			if (Expanded)
			{
				float iHeight = HeadingHeight;

				//Draw the top level rows (if any)
				if (Rows.Count > 0)
				{
					brush.Color = GradientColor;
					graphics.FillRectangle(brush,0,iHeight,Indent,1);
					iHeight+=1;
			
					RenderTableRows(graphics,render,Rows,ref iHeight);
				}
			
				if (Groups.Count > 0)
				{
					foreach (TableGroup tableGroup in Groups)
					{
						iHeight+=1;
						tableGroup.SuspendEvents = true;

						tableGroup.SetRectangle(new RectangleF(0,iHeight,Width,RowHeight));
						tableGroup.Render(graphics,render);
						iHeight+=RowHeight;

						tableGroup.SuspendEvents = false;

						if (tableGroup.Groups.Count > 0 && tableGroup.Expanded) RenderTableGroups(graphics, render, tableGroup.Groups, ref iHeight);
						if (tableGroup.Rows.Count > 0 && tableGroup.Expanded) RenderTableRows(graphics, render, tableGroup.Rows, ref iHeight);
					}
				}

				//Render highlight (if any)
				if (DrawSelectedItem && SelectedItem != null) SelectedItem.RenderSelection(graphics,render);
			}

			graphics.Clip = current;

			//Draw outline
			Pen pen;
			if (CustomPen == null)
			{
				pen = new Pen(BorderColor,BorderWidth);
				pen.DashStyle = BorderStyle;
	
				//Check if winforms renderer and adjust color as required
				pen.Color = render.AdjustColor(BorderColor,BorderWidth,Opacity);
			}
			else	
			{
				pen = CustomPen;
			}
			graphics.DrawPath(pen,path);
		}

		private void RenderTableGroups(Graphics graphics, IRender render, TableItems groups,ref float iHeight)
		{
			foreach (TableGroup tableGroup in groups)
			{
				tableGroup.SuspendEvents = true;

				tableGroup.SetRectangle(new RectangleF(0, iHeight, Width, GroupHeight));
				tableGroup.Render(graphics,render);
				iHeight+=GroupHeight;

				tableGroup.SuspendEvents = false;

				//Render groups and rows recursively
				if (tableGroup.Groups.Count > 0 && tableGroup.Expanded) RenderTableGroups(graphics, render, tableGroup.Groups, ref iHeight);
				if (tableGroup.Rows.Count > 0 && tableGroup.Expanded) RenderTableRows(graphics, render, tableGroup.Rows, ref iHeight);
			}
		}

		private void RenderTableRows(Graphics graphics, IRender render,TableItems rows,ref float iHeight)
		{
			foreach (TableRow tableRow in rows)
			{
				tableRow.SuspendEvents = true;

				tableRow.SetRectangle(new RectangleF(0,iHeight,Width,RowHeight));
				tableRow.Render(graphics,render);
				iHeight+=RowHeight;

				tableRow.SuspendEvents = false;
			}
		}

		private void RenderExpand(Graphics graphics,IRender render)
		{
			//Obtain a reference to IExpandable interface
			IExpandable expand = (IExpandable) this;
			
			if (!expand.DrawExpand) return;

			//Draw the expander
			Pen pen = new Pen(Color.FromArgb(128,Color.Gray),1);
			SolidBrush brush = new SolidBrush(Color.White);
			
			//Set up the expand path
			mExpandPath = new GraphicsPath();
			mExpandPath.AddEllipse(Width-20,5,14,14);

			SmoothingMode smoothing = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			graphics.FillPath(brush,mExpandPath); //Fill Circle
			graphics.DrawPath(pen,mExpandPath); //Outline
			
			pen.Color = Color.FromArgb(128,Color.Navy);
			pen.Width = 2;
			PointF[] points;

			if (expand.Expanded)
			{
				points = new PointF[] {new PointF(Width-16,11),new PointF(Width-13,8),new PointF(Width-10,11)};
			}
			else
			{
				points = new PointF[] {new PointF(Width-16,8),new PointF(Width-13,11),new PointF(Width-10,8)};
			}
			graphics.DrawLines(pen,points);
			points[0].Y += 5;
			points[1].Y += 5;
			points[2].Y += 5;
			graphics.DrawLines(pen,points);
			graphics.SmoothingMode = smoothing;
		}

		private Diagram GetDiagram()
		{
			if (Container is Diagram) 
			{
				return (Diagram) Container;
			}
			else
			{
				Group group = (Group) Container;
				return (Diagram) group.Container;
			}
		}

		private TableGroup GetTableGroupExpander(TableItems groups, PointF local)
		{
			foreach (TableGroup group in groups)
			{
				RectangleF expander = new RectangleF(group.Indent + 4, group.Rectangle.Top + 4, 11, 11);
				if (expander.Contains(local)) return group;

				//Sub groups
				TableGroup result = GetTableGroupExpander(group.Groups, local);
				if (result != null) return result;
			}

			return null;
		}

		protected virtual float CalculateHeight()
		{
			float height = HeadingHeight;

			//Add top level groups
			foreach (TableGroup group in Groups)
			{
				height += GetGroupHeight(group);
			}
			
			//Add top level rows
			foreach (TableRow row in Rows)
			{
				height += RowHeight;
			}
			
			//Add padding of 10 pixels
			height += 10;
			
			return height;
		}

		private float GetGroupHeight(TableGroup parent)
		{
			float height = 0;

			height += GroupHeight;
			
			if (parent.Expanded) 
			{
				//Add sub groups
				foreach (TableGroup subgroup in parent.Groups)
				{
					height += GetGroupHeight(subgroup);
				}

				//Add sub rows
				height += GetRowsHeight(parent);
			}

			return height;
		}

		private float GetRowsHeight(TableGroup parent)
		{
			float height = 0;

			//Add top level rows
			foreach (TableRow row in parent.Rows)
			{
				height += RowHeight;
			}

			return height;
		}

		protected virtual TableItem GetTableItemFromLocation(PointF local)
		{
			float currentHeight = HeadingHeight;

			//Check the top level rows (if any)
			if (Rows.Count > 0)
			{
				foreach (TableRow tableRow in Rows)
				{
					if (local.Y >= currentHeight && local.Y <= currentHeight + RowHeight) return tableRow;
					currentHeight += RowHeight;
				}
			}

			//Check the groups and each rows collection inside groups
			return GetGroupTableItem(Groups, local, ref currentHeight);
		}

		private TableItem GetGroupTableItem(TableItems groups, PointF local, ref float currentHeight)
		{
			//Check the groups and each rows collection inside groups
			foreach (TableGroup tableGroup in groups)
			{
				if (local.Y >= currentHeight && local.Y <= currentHeight + GroupHeight) return tableGroup;
				currentHeight += GroupHeight;

				if (tableGroup.Expanded)
				{
					TableItem item = GetGroupTableItem(tableGroup.Groups, local, ref currentHeight);
					if (item != null) return item;

					foreach (TableRow tableRow in tableGroup.Rows)
					{
						if (local.Y >= currentHeight && local.Y <= currentHeight + GroupHeight) return tableRow;
						currentHeight += RowHeight;
					}
				}
			}
			return null;
		}

		private bool GetGroupHeight(TableItem check, TableGroup parent, ref float height)
		{
			height += GroupHeight;
			
			if (parent.Expanded) 
			{
				//Add sub groups
				foreach (TableGroup subgroup in parent.Groups)
				{
					if (subgroup == check) return true;
					if (GetGroupHeight(check, subgroup, ref height)) return true;
				}

				//Add sub rows
				if (GetRowsHeight(check, parent, ref height)) return true;
			}

			return false;
		}

		private bool GetRowsHeight(TableItem check, TableGroup parent, ref float height)
		{
			//Add top level rows
			foreach (TableRow row in parent.Rows)
			{
				if (row == check) return true;
				height += RowHeight;
			}

			return false;
		}

		#endregion

	}
}
