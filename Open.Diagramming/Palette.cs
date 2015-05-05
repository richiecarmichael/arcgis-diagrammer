using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;
using System.ComponentModel;
using Crainiate.Diagramming.Drawing2D;

//Required so that the resource can be found outside this namespace
namespace Crainiate.Diagramming
{
	internal class EditingResources
	{
	}
}

namespace Crainiate.Diagramming.Editing
{
	[ToolboxBitmap(typeof(EditingResources), "Resource.palette.bmp")]
	public class Palette : Diagram
	{
		//Properties
		private Size mItemSize;
		private SizeF mSpacing;
		private Tabs mTabs;
		private Margin mMargin;
		private PaletteStyle mPaletteStyle;
		
		//Working variables
		private HandleType mMouseHandle;
		private int mLastCols;
		private Timer mTimer;
		private ButtonStyle mCurrentStyle;

		#region Interface
		
		//Events

		public event EventHandler ArrangeElements;
		
		//Constructors
		public Palette() : base()
		{
			Suspend();
			
			SetRender(new PaletteRender());
			Margin = new Margin(10,10,10,10);
			Tabs = new Tabs();
			Spacing = new Size(20, 22);
			Render.Font = this.Font;
			BackColor  = SystemColors.Control;
			GradientColor = SystemColors.Control;
			BorderColor = Color.Black;
			ForeColor = Color.Black;
			FillColor = Color.White;
			ItemSize = new Size(18, 18);
			Style = PaletteStyle.Multiple;
			DrawScroll = true;

			Resume();
			
			//Set up scroll timer
			mTimer = new Timer();
			mTimer.Interval = 20;
			mTimer.Tick+=new EventHandler(mTimer_Tick);
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Category("Data"), Description("Returns the Tabs collection for the palette.")]
		public virtual Tabs Tabs
		{
			get
			{
				return mTabs;
			}
			set
			{
				if (mTabs != null)
				{
					mTabs.InsertTab -= new EventHandler(mTabs_TabsInvalid);
					mTabs.RemoveTab -= new EventHandler(mTabs_TabsInvalid);
					mTabs.TabsInvalid -= new EventHandler(mTabs_TabsInvalid);
				}
				
				mTabs = (value == null) ? mTabs = new Tabs() : value;
				Render.Tabs = value;
				mTabs.InsertTab += new EventHandler(mTabs_TabsInvalid);
				mTabs.RemoveTab += new EventHandler(mTabs_TabsInvalid);
				mTabs.TabsInvalid += new EventHandler(mTabs_TabsInvalid);
				
				if (Shapes != null && Lines != null && !Suspended) CreateRenderList(Render.RenderRectangle,true);

				Invalidate();
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets or retrieves the current margin values.")]
		public virtual Margin Margin
		{
			get
			{
				return mMargin;
			}
			set
			{
				mMargin = value;
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to render background gradient.")]
		public virtual Color GradientColor
		{
			get
			{
				return Render.GradientColor;
			}
			set
			{
				Render.GradientColor = value;
				Render.RenderDiagram(PageRectangle);
				DrawDiagram(ControlRectangle);
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to draw the borders of the items in the palette.")]
		public virtual Color BorderColor
		{
			get
			{
				return Render.BorderColor;
			}
			set
			{
				Render.BorderColor = value;
				Render.RenderDiagram(PageRectangle);
				DrawDiagram(ControlRectangle);
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to fill the background of the items contained in the palette.")]
		public virtual Color FillColor
		{
			get
			{
				return Render.FillColor;
			}
			set
			{
				Render.FillColor = value;
				Render.RenderDiagram(PageRectangle);
				DrawDiagram(ControlRectangle);
			}
		}

		[Description("Determines the size of the element when added to the palette.")]
		public virtual Size ItemSize
		{
			get
			{
				return mItemSize;
			}
			set
			{
				mItemSize = value;
			}
		}

		[Category("Appearance"), Description("Sets or the amount of spacing between items in the palette.")]
		public virtual Size Spacing
		{
			get
			{
				return Size.Round(mSpacing);
			}
			set
			{
				mSpacing = value;
				Arrange();
			}
		}

		[Category("Appearance"), DefaultValue(true), Description("Determines if the scroll buttons are drawn for the palette.")]
		public virtual bool DrawScroll
		{
			get
			{
				return Render.DrawScroll;
			}
			set
			{
				Render.DrawScroll = value;
			}
		}

		//Sets he comparer used to sort elements contained in the collection
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IComparer Comparer
		{
			get
			{
				return RenderList.Comparer;
			}
			set
			{
				if (value == null) throw new ArgumentNullException("Comparer","Comparer may not be null.");
				RenderList.Comparer = value;
			}
		}

		[Category("Behaviour"), DefaultValue(PaletteStyle.Multiple), Description("Determines whether the palette shows more than one pane at a time.")]
		public virtual PaletteStyle Style
		{
			get
			{
				return mPaletteStyle;
			}
			set
			{
				if (value != mPaletteStyle)
				{
					mPaletteStyle = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		//Methods
		[Description("Arranges the palette items using the internal margin and spacing values.")]
		public virtual void Arrange()
		{
			Suspend();
			CreateRenderList(new RectangleF(),true);
			ArrangeImplementation(mSpacing.Width, mSpacing.Height);
			Resume();
			Invalidate();
		}

		[Description("Creates a palette of shapes from the stencil provided.")]
		public virtual void AddStencil(Stencil stencil)
		{
			Suspend();
			CreateRenderList(new RectangleF(),true);
			AddStencilImplementation(stencil);
			ArrangeImplementation(50,50);
			Resume();
			Invalidate();
		}

		protected virtual void OnArrangeElements()
		{
			if (ArrangeElements != null) ArrangeElements(this,EventArgs.Empty);
		}

		#endregion

		#region Overrides

		protected override void OnFontChanged(EventArgs e)
		{
			base.OnFontChanged (e);
			Render.Font = Font;
			Render.RenderDiagram(PageRectangle);
			DrawDiagram(ControlRectangle);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			base.OnMouseDown (e);

			//Prepare the actionpath
			if (e.Button == MouseButtons.Left)
			{
				//See if we pressed a tab or a button
				Tab tab = GetMouseTab(e);

				if (tab != null)
				{
					//Check if tab or button
					if (tab.Rectangle.Contains(e.X,e.Y) && tab.Visible)
					{
						tab.Pressed = true;
					}
					else
					{
						if (DrawScroll && tab.ButtonEnabled)
						{
							tab.ButtonPressed = true;
							mCurrentStyle = tab.ButtonStyle;
							mTimer.Start();
						}
					}
					Invalidate();
				}
			}
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			if (e.Button == MouseButtons.Left)
			{
				//Set to unpressed
				foreach (Tab tab in Tabs)
				{
					if (tab.Pressed)
					{
						Tabs.CurrentTab = tab;
						tab.Pressed = false;
						CreateRenderList(Render.RenderRectangle,true);
						SetTabs();
						Arrange();
						CheckScrollButtons();
					}
					if (DrawScroll && tab.ButtonPressed)
					{
						mTimer.Stop();
						tab.ButtonPressed = false;
					}
				}
				if (Render.ScrollTab.ButtonPressed)
				{
					mTimer.Stop();
					Render.ScrollTab.ButtonPressed = false;
				}
				Invalidate();
			}

			base.OnMouseUp (e);
		}

		protected override void OnElementInserted(Element element)
		{
			Tabs.CurrentTab.Elements.SetModifiable(true);
			Tabs.CurrentTab.Elements.Add(element.Key,element);
			Tabs.CurrentTab.Elements.SetModifiable(false);

			//Make sure the element added is the right size
			if (element is Shape)
			{
				Shape shape = (Shape) element;

				shape.SuspendEvents = true;
				
				//Determines if shapes keep their aspect in the palette
				if (shape.KeepAspect == true)
				{
					shape.MinimumSize = new SizeF(1,1);
				}
				else
				{
					shape.MinimumSize = ItemSize;
				}
				
				shape.MaximumSize = ItemSize;
				
				//Set size taking current width and height ratio into consideration
				//The max/min size will determine if the ratio is taken into account
				if (shape.Width > shape.Height)
				{
					float ratio = ItemSize.Width / shape.Width;
					shape.Size = new SizeF(ItemSize.Width,shape.Height * ratio);
				}
				else
				{
					float ratio = ItemSize.Height / shape.Height;
					shape.Size = new SizeF(shape.Width * ratio, ItemSize.Height);
				}
				
				shape.AllowMove = false;
				shape.AllowScale = false;
				shape.Clip = false;

				shape.BorderColor = Color.FromArgb(66,65,66); //SystemColors.ControlDarkDark;
				shape.BackColor = Color.White;
				shape.SmoothingMode = SmoothingMode.HighQuality;

				if (shape.Label != null) shape.Label.Color = Color.FromArgb(66,65,66); //SystemColors.ControlDarkDark;

				shape.SuspendEvents = false;
			}	

			SetTabs();
			base.OnElementInserted(element);
		}

		protected override void OnElementRemoved(Element element)
		{
			Tab remove = null;

			//Loop through and remove from tabs
			if (element is Shape)
			{
				foreach (Tab tab in Tabs)
				{
					foreach (Element loop in tab.Elements.Values)
					{
						if (element == loop) 
						{
							remove = tab;
							break;
						}
					}
					if (remove != null) break;
				}
			}	

			//Remove if found
			if (remove != null) 
			{
				remove.Elements.SetModifiable(true);
				remove.Elements.Remove(element.Key);
				remove.Elements.SetModifiable(false);
			}

			SetTabs();
			base.OnElementRemoved (element);
		}

		//Clear element references out of tabs
		public override void Clear()
		{
			base.Clear();
			foreach (Tab tab in Tabs)
			{
				tab.Elements.SetModifiable(true);
				tab.Elements.Clear();
				tab.Elements.SetModifiable(false);
			}
		}

		public override bool AutoScroll
		{
			get
			{
				return base.AutoScroll;
			}
			set
			{
				base.AutoScroll = false;
			}
		}

		//Redraw because of gradient
		protected override void OnResize(EventArgs e)
		{
			base.OnResize (e);
			SetTabs();
			Render.RenderDiagram(ControlRectangle);
			DrawDiagram(ControlRectangle);
		}

		//Initial gradient draw
		protected override void OnLayout(LayoutEventArgs levent)
		{
			base.OnLayout (levent);
			SetTabs();
			Arrange();
			Render.RenderDiagram(ControlRectangle);
			DrawDiagram(ControlRectangle);
		}

		//Override diagram drag drop functionality
		protected override void OnDragEnter(DragEventArgs drgevent)
		{
            
			//Begin drag drop
			if (CurrentMouseElements.MouseStartElement != null) 
			{
				//Translate the drag point from screen co-ordinates to control co-ordinates
				PointF dragPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
				dragPoint = OffsetDrop(dragPoint,CurrentMouseElements.MouseStartElement.Rectangle);
				
				drgevent.Effect = DragDropEffects.Move;

				Render.Lock();
				Render.ActionPath = CurrentMouseElements.MouseStartElement.GetPath();

				//Move action path from origin to the mouse position
				Matrix matrix = new Matrix();
				matrix.Translate(dragPoint.X,dragPoint.Y);
				Render.ActionPath.Transform(matrix);
				matrix.Dispose();
			}
			
			base.OnDragEnter(drgevent);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			PointF mousePoint = new Point(drgevent.X,drgevent.Y);

			//Check left button only
			if (Render.ActionPath != null)
			{
				Suspend();

				//Translate the drag point from screen co-ordinates to control co-ordinates
				PointF dragPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
				dragPoint = OffsetDrop(dragPoint,CurrentMouseElements.MouseStartElement.Rectangle);

				//Move action path
				Render.ActionPath = CurrentMouseElements.MouseStartElement.GetPath();

				Matrix matrix = new Matrix();
				matrix.Translate(dragPoint.X,dragPoint.Y);
				Render.ActionPath.Transform(matrix);
				matrix.Dispose();
				
				Resume();
				Invalidate();
			}
			base.OnDragOver(drgevent);
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			Render.ActionPath = null;
			Render.Unlock();
			Invalidate();

			base.OnDragDrop(drgevent);
		}

		protected override void OnDragLeave(EventArgs e)
		{
			Render.ActionPath = null;
			Render.Unlock();
			Invalidate();

			base.OnDragLeave(e);
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public new PaletteRender Render
		{
			get
			{
				return (PaletteRender) base.Render;
			}
			set
			{
				if (value == null) throw new ArgumentException("Value may not be null","value");
				base.SetRender(value);	
			}
		}
		
		[Description("Zoom is always 100 percent for a palette control.")]
		public override float Zoom
		{
			get
			{
				return base.Zoom;
			}
			set
			{
				base.Zoom = 100;
			}
		}

		public override void CreateRenderList(RectangleF rect,bool sort)
		{
			RenderList renderList = RenderList;
			renderList.Clear();

			//Add all items from the current tab
			if (Tabs != null)
			{
				foreach (Element element in Tabs.CurrentTab.Elements.Values)
				{
					renderList.Add(element);	
				}
		
				if (sort) renderList.Sort();
			}
		}

		#endregion

		#region Events

		private void mTabs_TabsInvalid(object sender, EventArgs e)
		{
			SetTabs();
			Invalidate();
		}

		private void mTimer_Tick(object sender, EventArgs e)
		{
			CheckScrollButtons();
			ScrollTab(mCurrentStyle);
		}

		#endregion

		#region Implementation

		private void SetTabs()
		{
			float offset = 1;
			float width = this.Width-2;
			int count = 0;
			Tab previousTab = null;
			Tab lastTab = null;

			foreach (Tab tab in Tabs)
			{
				count+=1;
				tab.ButtonStyle = ButtonStyle.None;
				tab.SetButtonRectangle(new RectangleF());

				//Check to see if must reduce width for scroll
				if (DrawScroll && (Tabs.CurrentTab == previousTab || Tabs.CurrentTab == tab))
				{
					tab.SetRectangle(new RectangleF(1,offset,width-18,Tabs.TabHeight));
					if (Tabs.CurrentTab == tab) tab.ButtonStyle = ButtonStyle.Up;
					if (Tabs.CurrentTab == previousTab) tab.ButtonStyle = ButtonStyle.Down;
					tab.SetButtonRectangle(new RectangleF(tab.Rectangle.Right+1,tab.Rectangle.Top,17,tab.Rectangle.Height));
				}
				else
				{
					tab.SetRectangle(new RectangleF(1,offset,width,Tabs.TabHeight));
				}
				previousTab = tab;

				//Check if tabs must move to bottom
				if (Tabs.CurrentTab == tab)
				{
					offset = (Height - (Tabs.Count - count) * (Tabs.TabHeight + 1));
				}
				else
				{
					offset += Tabs.TabHeight + 1;	
				}

				//Store last tab
				lastTab = tab;
			}
			
			//work out scroll button tab at the bottom if required
			if (DrawScroll && lastTab == Tabs.CurrentTab)
			{
				Render.ScrollTab.Visible = true;
				Render.ScrollTab.SetRectangle(new RectangleF(1,Height - Tabs.TabHeight -1,width-19,Tabs.TabHeight));
				Render.ScrollTab.SetButtonRectangle(new RectangleF(width-16,Height - Tabs.TabHeight -1,17,Tabs.TabHeight));
			}
			else
			{
				Render.ScrollTab.Visible = false;
			}

		}

		private void ArrangeImplementation(float spacingWidth, float spacingHeight)
		{
			float totalwidth = DisplayRectangle.Width - Margin.Left - Margin.Right;
            
			float width = Margin.Left;
			float height = Margin.Top + Tabs.CurrentTab.Scroll;
			Element lastElement = null;
			PointF lastLocation = new PointF();

			//Check if must include tab in height
			if (Tabs.CurrentTab.Visible) height += Tabs.CurrentTab.Rectangle.Bottom;

			Suspend();

			//Arrange each node according to the order of the renderlist
			foreach (Element element in RenderList)
			{
				//Set the location
				if (lastElement != null) 
				{
					if (lastElement is SolidElement)
					{
						SolidElement solid = (SolidElement) lastElement;
						solid.Location = lastLocation;
					}
				}

				lastElement = element;
				lastLocation = new PointF(width,height);

				height += spacingHeight;

				SetLabel(lastElement);
			}
			 
			//Set the final item
			if (lastElement != null)
			{	
				if (lastElement is SolidElement)
				{
					SolidElement solid = (SolidElement) lastElement;
					solid.Location = lastLocation;
				}
				SetLabel(lastElement);
			}

			//Save the number of columns
			mLastCols = (int) System.Math.Floor((DisplayRectangle.Width - Margin.Left - Margin.Right) / Spacing.Width);
			
			//Enable disable scroll buttons
			CheckScrollButtons();

			Resume();
			Invalidate();
		}

		private void AddStencilImplementation(Stencil stencil)
		{
			Suspend();
			foreach(StencilItem item in stencil.Values)
			{
				Shape shape = new Shape(item);
				shape.Label = new TextLabel(item.Key);
				
				Shapes.Add(Shapes.CreateKey(),shape);
			}
			Resume();
		}

		private void SetLabel(Element element)
		{
			if (element is Shape)
			{
				Shape shape = (Shape) element;
				if (shape.Label != null)
				{
					TextLabel label = (TextLabel) shape.Label;
					
					label.Offset = new PointF(ItemSize.Width + 18, 0F);
					label.Size = new SizeF(Math.Abs(Width - ItemSize.Width - shape.Width) , ItemSize.Height);
					label.LineAlignment = StringAlignment.Center;
					label.Alignment = StringAlignment.Near;
					label.FontSize = 8;
				}
			}
		}

		private Tab GetMouseTab(MouseEventArgs e)
		{
			foreach (Tab tab in Tabs)
			{
				if (tab.Rectangle.Contains(e.X,e.Y)) return tab;
				if (tab.ButtonStyle != ButtonStyle.None && tab.ButtonRectangle.Contains(e.X,e.Y)) return tab;
			}
			if (Render.ScrollTab.Visible && Render.ScrollTab.ButtonRectangle.Contains(e.X,e.Y))
			{
				return Render.ScrollTab;
			}
			return null;
		}

		private void OffsetRenderlist(float offset)
		{
			Suspend();
			foreach (Element element in RenderList)
			{
				if (element is Shape)
				{
					Shape shape = (Shape) element;
					shape.SuspendEvents = true;
					shape.Location = new PointF(shape.X,shape.Y+offset);
					shape.SuspendEvents = false;
				}
			}
			Resume();
		}

		private void CheckScrollButtons()
		{
			Tab next = GetNextTab();
			Tab tab = Tabs.CurrentTab;

			//check for current tab button enabled
			tab.ButtonEnabled = (tab.Scroll < 0);

			//Loop through and check if any element greater in height than next tab
			next.ButtonEnabled = false;
			foreach (Element element in Tabs.CurrentTab.Elements.Values)
			{
				if (element.Rectangle.Bottom > next.Rectangle.Top)
				{	
					next.ButtonEnabled = true;
					break;
				}
			}

			//Check if button must be disabled and the timer stopped
			if (tab.ButtonPressed && !tab.ButtonEnabled)
			{
				mTimer.Stop();
				tab.ButtonPressed = false;
			}
			if (next.ButtonPressed && !next.ButtonEnabled) 
			{
				mTimer.Stop();
				next.ButtonPressed = false;
			}
		}

		private void ScrollTab(ButtonStyle style)
		{
			Tab tab = Tabs.CurrentTab;
			Tab next = GetNextTab();
			
			//Scroll
			if (style == ButtonStyle.Up && tab.ButtonEnabled)
			{
				tab.Scroll += Tabs.TabHeight;
				OffsetRenderlist(Tabs.TabHeight);
			}
			else if (style == ButtonStyle.Down && next.ButtonEnabled)
			{
				tab.Scroll -= Tabs.TabHeight;
				OffsetRenderlist(-Tabs.TabHeight);
			}
			CheckScrollButtons();
			Invalidate();
		}

		private Tab GetNextTab()
		{
			Tab previous = null;

			foreach (Tab tab in Tabs)
			{
				if (Tabs.CurrentTab == previous) return tab ;
				previous = tab;
			}
			return Render.ScrollTab;
		}

		private PointF OffsetDrop(PointF point,RectangleF rectangle)
		{
			PointF offset = new PointF(point.X - (rectangle.Width / 2), point.Y -  (rectangle.Height / 2));
			
			//Adjust for scale
			return PointToDiagram(Convert.ToInt32(offset.X),Convert.ToInt32(offset.Y));
		}

		#endregion
	}
}
