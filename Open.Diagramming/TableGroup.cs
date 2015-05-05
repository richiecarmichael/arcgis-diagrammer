using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class TableGroup: TableItem, ISerializable, ICloneable
	{
		//Property variables
		private TableItems mGroups;
		private TableItems mRows;
		private bool mExpanded;

		//Working variables
		
		#region Interface
		
		//Events
		public event ExpandedChangedEventHandler ExpandedChanged;
		public event EventHandler HeightChanged;
						
		//Constructors
		public TableGroup()
		{
			Expanded = true;
			Rows = new TableItems(typeof(TableRow));
			Groups = new TableItems(typeof(TableGroup));
		}

		public TableGroup(TableGroup prototype): base(prototype)
		{
			mExpanded = prototype.Expanded;
			Rows = new TableItems(typeof(TableRow));
			Groups = new TableItems(typeof(TableGroup));
			Table.CopyRows(prototype.Rows,Rows);
			Table.CopyGroups(prototype.Groups, Groups);
		}

		protected TableGroup(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			
			Expanded = info.GetBoolean("Expanded");
			Rows = (TableItems) info.GetValue("Rows",typeof(TableItems));
			if (Serialize.Contains(info, "Groups",typeof(TableItems))) Groups = (TableItems) info.GetValue("Groups",typeof(TableItems));
			
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
				if (value == null) throw new ArgumentNullException("Groups may not be set to null.");
			
				if (mGroups != null)
				{
					mGroups.InsertItem-=new TableItemsEventHandler(TableItems_InsertItem);
					mGroups.RemoveItem-=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				mGroups = value;
				mGroups.InsertItem+=new TableItemsEventHandler(TableItems_InsertItem);
				mGroups.RemoveItem+=new TableItemsEventHandler(TableItems_RemoveItem);
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
				if (value == null) throw new ArgumentNullException();
				
				if (mRows != null)
				{
					mRows.InsertItem-=new TableItemsEventHandler(TableItems_InsertItem);
					mRows.RemoveItem-=new TableItemsEventHandler(TableItems_RemoveItem);
				}

				mRows = value;
				mRows.InsertItem+=new TableItemsEventHandler(TableItems_InsertItem);
				mRows.RemoveItem+=new TableItemsEventHandler(TableItems_RemoveItem);
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
					OnExpandedChanged(this, value);
				}
			}
		}

		protected internal virtual void SetTable()
		{
			foreach (TableGroup group in Groups)
			{
				group.SetTable(); //Calls this method recursively
			}
			foreach (TableRow row in Rows)
			{
				row.SetTable(Table);
			}	
		}

		//Methods
		protected virtual void OnExpandedChanged(object sender, bool expanded)
		{
			if (!SuspendEvents && ExpandedChanged != null) ExpandedChanged(sender, expanded);
		}

		protected virtual void OnHeightChanged(object sender, EventArgs e)
		{
			if (!SuspendEvents && HeightChanged != null) HeightChanged(sender, e);
		}

		#endregion

		#region Overrides

		public override void Render(Graphics graphics, IRender render)
		{
			byte opacity = 100;
			if (Table != null) opacity = Table.Opacity;

			//Draw Background
			SolidBrush brush = new SolidBrush(render.AdjustColor(Backcolor,1,opacity));
			brush.Color = Color.FromArgb(brush.Color.A /2, brush.Color);
		
			graphics.FillRectangle(brush,Rectangle);
			
			//Draw plus or minus rectangle
			SmoothingMode smoothing = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.HighQuality;
			RectangleF expander = new RectangleF(4 + Indent, Rectangle.Top + 4,11,11);
			LinearGradientBrush gradientBrush = new LinearGradientBrush(expander,Color.FromArgb(255,Color.White),Color.FromArgb(255,166,176,185),LinearGradientMode.Vertical);
			graphics.FillRectangle(gradientBrush,expander); // internal
			Pen pen = new Pen(Color.FromArgb(128,Color.Gray),1);
			graphics.DrawRectangle(pen,expander.X,expander.Y,expander.Width,expander.Height); //border
			pen.Color = Color.FromArgb(128,Color.Black);
			pen.Width = 2;
			graphics.DrawLine(pen,expander.X+2,expander.Y+5.5F,expander.X+9,expander.Y+5.5F); //minus
			if (!Expanded) graphics.DrawLine(pen,expander.X+5.5F,expander.Y+2,expander.X+5.5F,expander.Y+9); //plus
			graphics.SmoothingMode = smoothing;

			//Draw text
			RectangleF textRectangle = new RectangleF(20 + Indent,Rectangle.Top,Rectangle.Width - 20 - Indent,Rectangle.Height);
			brush = new SolidBrush(render.AdjustColor(Forecolor,1,opacity));
			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap;
			graphics.DrawString(Text,Component.Instance.GetFont(FontName,FontSize,FontStyle),brush,textRectangle,format);

			//Draw indent
			//brush = new SolidBrush(render.AdjustColor(Backcolor,1,opacity));
			//brush.Color = Color.FromArgb(brush.Color.A /2, brush.Color);
			//graphics.FillRectangle(brush,0,Rectangle.Top,Indent,Rectangle.Height);
		}

		public override void RenderSelection(Graphics graphics, IRender render)
		{
			SmoothingMode mode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.None;
			
			Pen pen = Component.Instance.SelectionPen;
			RectangleF rect = new RectangleF(Rectangle.X+2,Rectangle.Y,Rectangle.Width-4,Rectangle.Height);
			graphics.DrawRectangle(pen,rect.X,rect.Y,rect.Width,rect.Height);

			graphics.SmoothingMode = mode;
		}

		#endregion

		#region Events

		//Handles events for rows and groups
		private void TableItems_InsertItem(object sender, TableItemsEventArgs e)
		{
			//Check isnt same group
			if (e.Value == this) throw new TableItemsException("Cannot add a TableGroup to itself.");

			e.Value.SetTable(Table);
			e.Value.SetIndent(Indent + Table.Indent);
			e.Value.SetParent(this);
			e.Value.TableItemInvalid +=new EventHandler(TableItem_TableItemInvalid);

			if (e.Value is TableGroup)
			{
				TableGroup group = (TableGroup) e.Value;

				group.ExpandedChanged +=new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);
				group.HeightChanged +=new EventHandler(TableGroup_HeightChanged);
			}
			
			OnHeightChanged(this, EventArgs.Empty);
			OnTableItemInvalid();
		}
		
		private void TableItems_RemoveItem(object sender, TableItemsEventArgs e)
		{
			//Remove handlers
			if (e.Value != null)
			{
				e.Value.TableItemInvalid -=new EventHandler(TableItem_TableItemInvalid);

				if (e.Value is TableGroup)
				{
					TableGroup group = (TableGroup) e.Value;

					group.ExpandedChanged -=new ExpandedChangedEventHandler(TableGroup_ExpandedChanged);
					group.HeightChanged -=new EventHandler(TableGroup_HeightChanged);
				}
			}

			OnHeightChanged(this, EventArgs.Empty);
			OnTableItemInvalid();
		}

		private void TableItem_TableItemInvalid(object sender, EventArgs e)
		{
			OnTableItemInvalid();
		}

		private void TableGroup_ExpandedChanged(object sender, bool expanded)
		{
			OnExpandedChanged(sender, expanded);
		}

		private void TableGroup_HeightChanged(object sender, EventArgs e)
		{
			OnHeightChanged(sender, e);
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Expanded",Expanded);
			info.AddValue("Rows",Rows);
			info.AddValue("Groups",Groups);
			
			base.GetObjectData(info,context);
		}	
	
		//Implement cloning for this class
		public virtual object Clone()
		{
			return new TableGroup(this);
		}

		#endregion

	}
}
