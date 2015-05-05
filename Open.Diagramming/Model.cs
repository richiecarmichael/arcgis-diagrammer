using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Windows.Forms;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Editing;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void UserActionEventHandler(object sender, UserActionEventArgs e);

	[ToolboxBitmap(typeof(Crainiate.Diagramming.Model), "Resource.model.bmp")]
	public class Model: Diagram, ICloneable
	{
		//Structs
		private struct ClipboardData
		{
			public Elements Elements; //stores cut or copied shapes
			public bool IsCopy;
		}

		//Properties
		private bool mAlignGrid;
		private bool mDragScroll = true;
		private bool mDragSelect = true;
		private UndoList mUndoList;
		private Runtime mRuntime;
		private Element mDragElement;
		
		//Working variables
		private Point mStartPoint;
		private Point mLastPoint;
		private bool mHandled;
		private Handle mMouseHandle;
		private Timer mTimer;
		private bool mSyncTimer;
		
		private Label mLabel;
		private ILabelEdit mLabelEdit;
		private ToolTip mTooltip;

		private const int WM_KEYDOWN = 0x100;
		private Model.ClipboardData mClipboard;
		private Element mActionElement;

		private Elements mSelectedElements;
		private Elements mSelectedShapes;
		private Elements mSelectedLines;

		#region Interface
		
		//Events
		[Category("Action"),Description("Occurs after user changes have been successfully applied to a model.")]
		public event UserActionEventHandler UpdateActions;

        [Category("Action"), Description("Occurs when user changes are cancelled without being applied to the model.")]
        public event UserActionEventHandler CancelActions;
		
        [Category("Action"),Description("Caused by the user moving one or more elements during a move action .")]
		public event UserActionEventHandler Moving;

		[Category("Action"),Description("Caused by the user scaling one or more elements during a scale action .")]
		public event UserActionEventHandler Scaling;

		[Category("Action"),Description("Caused by the user rotating one or more elements during a rotate action.")]
		public event UserActionEventHandler Rotating;

		[Category("Action"),Description("Occurs when elements contained in the diagram are selected or deselected.")]
		public event EventHandler SelectedChanged;

		[Category("Action"),Description("Occurs when the user begins a drag selection with the mouse.")]
		public event MouseEventHandler BeginDragSelect;

		[Category("Action"),Description("Occurs when the user ends a drag selection with the mouse.")]
		public event MouseEventHandler EndDragSelect;
        
        [Category("Action"), Description("Occurs when the user ends a drag selection with the mouse.")]
        public event EventHandler CancelDragSelect;
		
		//Constructors
		public Model() : base()
		{
			Suspend();

			UndoList = new UndoList();
			Runtime = new Runtime();

			RenderDesign.DrawSelections = true;
			RenderDesign.DrawGrid = true;
			RenderDesign.DrawPageLines = false;

			Resume();
		}

		public Model(Model prototype): base(prototype)
		{
			Suspend();

			UndoList = new UndoList();
			Runtime = new Runtime();
			
			RenderDesign.DrawSelections = prototype.DrawSelections;
			RenderDesign.DrawGrid = prototype.DrawGrid;
			RenderDesign.DrawPageLines = prototype.DrawPageLines;

			Resume();
		}
		
		//Properties
		[Category("Behavior"), DefaultValue(false), Description("A boolean value determining whether elements are aligned to the grid.")]
		public virtual bool AlignGrid
		{
			get
			{
				return mAlignGrid;
			}
			set
			{
				mAlignGrid = value;
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Determines whether the model is drawn with a grid.")]
		public virtual bool DrawGrid
		{
			get
			{
				return RenderDesign.DrawGrid;
			}
			set
			{
				if (DrawGrid != value)
				{
					RenderDesign.DrawGrid = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}


		[Category("Behavior"), DefaultValue(true), Description("Determines whether selection handles are shown for a model.")]
		public virtual bool DrawSelections
		{
			get
			{
				return RenderDesign.DrawSelections;
			}
			set
			{
				if (DrawSelections != value)
				{
					RenderDesign.DrawSelections = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		[Category("Behavior"), DefaultValue(false), Description("Determines whether page boundaries are drawn onto the model.")]
		public virtual bool DrawPageLines
		{
			get
			{
				return RenderDesign.DrawPageLines;
			}
			set
			{
				if (DrawPageLines != value)
				{
					RenderDesign.DrawPageLines = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the color used to render the grid.")]
		public virtual Color GridColor
		{
			get
			{
				return RenderDesign.GridColor;
			}
			set
			{
				RenderDesign.GridColor = value;
				Render.RenderDiagram(PageRectangle);
				DrawDiagram(ControlRectangle);
			}
		}

		[Category("Appearance"), Description("Sets or retrieves the size object used to determine the grid spacing.")]
		public virtual Size GridSize
		{
			get
			{
				return RenderDesign.GridSize;
			}
			set
			{
				if (!RenderDesign.GridSize.Equals(value))
				{
					if (value.Width < 1) value.Width = 1;
					if (value.Height < 1) value.Height = 1;

					RenderDesign.GridSize = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		[DefaultValue(GridStyle.Complex), Category("Appearance"), Description("Sets or retrieves the size object used to determine the grid spacing.")]
		public virtual GridStyle GridStyle
		{
			get
			{
				return RenderDesign.GridStyle;
			}
			set
			{
				if (RenderDesign.GridStyle != value)
				{
					RenderDesign.GridStyle = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Determines whether the model scrolls when an element is dragged outside the viewable area.")]
		public virtual bool DragScroll
		{
			get
			{
				return mDragScroll;
			}
			set
			{
				mDragScroll = value;
			}
		}

		[Category("Behavior"), DefaultValue(true), Description("Determines whether elements can be selected by dragging a selection rectangle around them.")]
		public virtual bool DragSelect
		{
			get
			{
				return mDragSelect;
			}
			set
			{
				mDragSelect = value;
			}
		}

		[Browsable(false)]
		public virtual Element DragElement
		{
			get
			{
				return mDragElement;
			}
			set
			{
				mDragElement = value;
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual IRenderDesign RenderDesign
		{
			get
			{
				return (IRenderDesign) Render;
			}
		}

		[Browsable(false), Category("Layout"), Description("Sets or retrieves the size of the page used to draw page lines.")]
		public virtual SizeF PageLineSize
		{
			get
			{
				return RenderDesign.PageLineSize;
			}
			set
			{
				if (! RenderDesign.PageLineSize.Equals(value))
				{
					RenderDesign.PageLineSize = value;
					Render.RenderDiagram(PageRectangle);
					DrawDiagram(ControlRectangle);
				}
			}
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual UndoList UndoList
		{
			get
			{
				return mUndoList;
			}
			set
			{
				if (value == null)
				{
					mUndoList = new UndoList();
				}
				else
				{
					mUndoList = value;
				}
			}
		}

		[Browsable(false),DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Runtime Runtime
		{
			get
			{
				return mRuntime;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("Runtime","Runtime property may not be null.");
				}
				else
				{
					mRuntime = value;
					Runtime.ActiveContainer = this;
					Runtime.SetDiagram(this);
				}
			}
		}

		//Methods
		[Description("Performs a command on the model depending on it's current state.")]
		public virtual void DoCommand(string command)
		{
			DoCommandImplementation(command, new PointF());
		}

		[Description("Performs a command on the model at the specified location depending on it's current state.")]
		public virtual void DoCommand(string command,Point location)
		{
			PointF diagramLocation = PointToDiagram(location.X,location.Y);
			DoCommandImplementation(command, diagramLocation);
		}

		[Description("Performs a command on the model at the specified location depending on it's current state.")]
		public virtual void DoCommand(string command,MouseEventArgs e)
		{
			PointF diagramLocation = PointToDiagram(e.X,e.Y);
			DoCommandImplementation(command, diagramLocation);
		}

		[Description("Performs a command on the model depending on it's current state.")]
		public virtual void DoCommand(string command, string value)
		{
			DoCommandImplementation(command, value);
		}

		[Description("Returns a boolean determining whether the given command can be performed on the model.")]
		public virtual bool GetCommand(string command)
		{
			return GetCommandImplementation(command);
		}

		[Description("Returns a string containing the possible values for the command provided.")]
		public virtual string GetCommandString(string command)
		{
			return GetCommandStringImplementation(command);
		}

		[Description("Returns all selected elements")]
		public virtual Elements SelectedElements()
		{
			if (mSelectedElements == null) GetSelectedElements();
			return mSelectedElements;
		}

		[Description("Returns all selected elements of the specified type")]
		public virtual Elements SelectedElements(System.Type type)
		{
			return GetSelectedElements(type);
		}

		[Description("Returns all selected shapes into a non-modiifable colelction.")]
		public virtual Elements SelectedShapes()
		{
			if (mSelectedShapes == null) GetSelectedElements();
			return mSelectedShapes;
		}

		[Description("Returns all selected lines into a non-modiifable colelction.")]
		public virtual Elements SelectedLines()
		{
			if (mSelectedLines == null) GetSelectedElements();
			return mSelectedLines;
		}

		[Description("Starts editing for a label contained in the model.")]
		public virtual ILabelEdit StartEdit(Label label)
		{
			if (label == null) throw new ArgumentNullException("Label may not be null.");
			if (label.Parent == null) throw new ModelException("Label cannot have null parent.");
			if (label.Parent.Container == null) throw new ModelException("Label Parent container cannot be null.");
			if (! (label.Parent is ILabelContainer)) throw new ModelException("Label Parent container must be an ILabelContainer object.");
			if (mLabelEdit != null) return mLabelEdit;

			//Set up control
			mLabelEdit = Runtime.CreateLabelEdit();
			if (! (mLabelEdit is System.Windows.Forms.Control)) throw new ModelException("Label editor does not inherit from System.Windows.Forms.Control.");

			System.Windows.Forms.Control labelEdit = (System.Windows.Forms.Control) mLabelEdit;

			this.Controls.Add(labelEdit);
			mLabel = label;

			//Set up location and size 
			Element parent = label.Parent;
			ILabelContainer container = (ILabelContainer) parent;
			PointF location = container.GetLabelLocation();
			SizeF size = container.GetLabelSize();

			//Offset the location for the container
			location = new PointF(location.X + parent.Rectangle.X + parent.Container.Offset.X,location.Y + parent.Rectangle.Y + parent.Container.Offset.Y);

			//Adjust the location and size to screen co-ordinates and pixels according to scale and units
			location = new PointF(location.X *  Render.ScaleFactor, location.Y * Render.ScaleFactor);
			location = new PointF(location.X + DisplayRectangle.X,location.Y + DisplayRectangle.Y);
			size = new SizeF(size.Width * Render.ScaleFactor, size.Height * Render.ScaleFactor);

			//Adjust the location if the model is paged
			if (Paged) location = new PointF(location.X + RenderPaged.PagedOffset.X, location.Y + RenderPaged.PagedOffset.Y);

			labelEdit.Location = Point.Round(location);
			labelEdit.Size = Size.Round(size);
			labelEdit.Font = label.Font;
			labelEdit.Text = label.Text;
			if (label is TextLabel)	mLabelEdit.StringFormat = ((TextLabel) label).GetStringFormat();
			labelEdit.ForeColor = label.Color;
			labelEdit.BackColor = Color.White;
			mLabelEdit.Zoom = Zoom;
			mLabelEdit.SendEnd();

			//Set autosize causing correct size
			if (parent is Link) mLabelEdit.AutoSize = Editing.AutoSizeMode.Horizontal;

			//Set event handlers
			mLabelEdit.Complete += new EventHandler(mLabelEdit_Complete);
			mLabelEdit.Cancel += new EventHandler(mLabelEdit_Complete);

			labelEdit.Visible = true;
			labelEdit.Focus();

			return mLabelEdit;
		}

		[Description("Adds a shape to the model")]
		public virtual Shape AddShape(PointF location)
		{
			return AddShapeImplementation("",location,null, new SizeF());
		}

		[Description("Adds a shape to the model")]
		public virtual Shape AddShape(PointF location, SizeF size)
		{
			return AddShapeImplementation("",location,null, size);
		}

		[Description("Adds a shape to the model")]
		public virtual Shape AddShape(string key,PointF location)
		{
			return AddShapeImplementation(key,location,null, new SizeF());					
		}

		[Description("Adds a shape to the model")]
		public virtual Shape AddShape(string key,PointF location,SizeF size)
		{
			return AddShapeImplementation(key,location,null, size);					
		}

		[Description("Adds a shape with the specified key and stencil to the model")]
		public virtual Shape AddShape(string key,PointF location,StencilItem stencil)
		{
			return AddShapeImplementation(key,location,stencil, new SizeF());					
		}

		[Description("Adds a shape with the specified key and stencil to the model")]
		public virtual Shape AddShape(string key,PointF location,SizeF size, StencilItem stencil)
		{
			return AddShapeImplementation(key,location,stencil, size);					
		}

		[Description("Adds a shape with the specified stencil to the model")]
		public virtual Shape AddShape(PointF location, StencilItem stencil)
		{
			return AddShapeImplementation(Shapes.CreateKey(),location,stencil, new SizeF());					
		}

		[Description("Adds a shape with the specified stencil to the model")]
		public virtual Shape AddShape(PointF location, SizeF size, StencilItem stencil)
		{
			return AddShapeImplementation(Shapes.CreateKey(),location,stencil, size);					
		}

		[Description("Adds a line to the model")]
		public virtual Line AddLine(PointF start, PointF end)
		{
			Line line = Runtime.CreateLine(start,end);
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds a line with the specified key to the model")]
		public virtual Line AddLine(string key, PointF start, PointF end)
		{
			Line line = Runtime.CreateLine(start,end);
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}

		[Description("Adds a line with the specified key to the model")]
		public virtual Line AddLine(Shape start, Shape end)
		{
			Line line = Runtime.CreateLine();
			line.Start.Shape = start;
			line.End.Shape = end;
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds a line with the specified key to the model")]
		public virtual Line AddLine(string key, Shape start, Shape end)
		{
			Line line = Runtime.CreateLine();
			line.Start.Shape = start;
			line.End.Shape = end;
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}
		
		[Description("Adds a line with the specified key to the model")]
		public virtual Line AddLine(Port start, Port end)
		{
			Line line = Runtime.CreateLine();
			line.Start.Port = start;
			line.End.Port = end;
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds a line with the specified key to the model")]
		public virtual Line AddLine(string key, Port start, Port end)
		{
			Line line = Runtime.CreateLine();
			line.Start.Port = start;
			line.End.Port = end;
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}

		[Description("Adds an connector to the model")]
		public virtual Connector AddConnector(PointF start, PointF end)
		{
			Connector line = Runtime.CreateConnector(start,end);
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(string key, PointF start, PointF end)
		{
			Connector line = Runtime.CreateConnector(start,end);
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}

		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(Shape start, Shape end)
		{
			Connector line = Runtime.CreateConnector();
			line.Start.Shape = start;
			line.End.Shape = end;
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(string key, Shape start, Shape end)
		{
			Connector line = Runtime.CreateConnector();
			line.Start.Shape = start;
			line.End.Shape = end;
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}
		
		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(Port start, Port end)
		{
			Connector line = Runtime.CreateConnector();
			line.Start.Port = start;
			line.End.Port = end;
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(string key, Port start, Port end)
		{
			Connector line = Runtime.CreateConnector();
			line.Start.Port = start;
			line.End.Port = end;
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}

		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(Port start, Shape end)
		{
			Connector line = Runtime.CreateConnector();
			line.Start.Port = start;
			line.End.Shape = end;
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds an connector with the specified key to the model")]
		public virtual Connector AddConnector(string key, Port start, Shape end)
		{
			Connector line = Runtime.CreateConnector();
			line.Start.Port = start;
			line.End.Shape = end;
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}

		[Description("Adds a recursive line to the model")]
		public virtual Connector AddRecursiveLine(Port start, Port end)
		{
			if (start.Parent != end.Parent) throw new ArgumentException("The start and end ports must belong to the same shape.");
			Connector line = Runtime.CreateConnector();
			line.Start.Port = start;
			line.End.Port = end;
			Runtime.ActiveContainer.Lines.Add(Runtime.ActiveContainer.Lines.CreateKey(),line);
			return line;
		}

		[Description("Adds a recursive line with the specified key to the model")]
		public virtual Connector AddRecursiveLine(string key, Port start, Port end)
		{
			if (start.Parent != end.Parent) throw new ArgumentException("The start and end ports must belong to the same shape.");

			Connector line = Runtime.CreateConnector();
			line.Start.Port = start;
			line.End.Port = end;
			Runtime.ActiveContainer.Lines.Add(key,line);
			return line;
		}

		[Description("Stops annotation editing for the current edited annotation.")]
		public virtual void StopEdit()
		{
			if (mLabelEdit != null)
			{
				if (!mLabelEdit.Cancelled) mLabel.Text = mLabelEdit.Text;
				CancelEdit();
			}
		}

		[Description("Cancels annotation editing for the current edited annotation without updating the annotation.")]
		public virtual void CancelEdit()
		{
			if (mLabelEdit != null)
			{
				//Remove event handlers
				mLabelEdit.Complete -= new EventHandler(mLabelEdit_Complete);
				mLabelEdit.Cancel -= new EventHandler(mLabelEdit_Complete);

				mLabelEdit.Visible = false;
				this.Controls.Remove(mLabelEdit as System.Windows.Forms.Control);
				mLabelEdit = null; 
			}
		}
		
		[Description("Raises the UpdateActions event.")]
		protected virtual void OnUpdateActions(RenderList actions)
		{
			if (! SuspendEvents && UpdateActions != null) UpdateActions(this, new UserActionEventArgs(actions));
		}

        [Description("Raises the CancelActions event.")]
        protected virtual void OnCancelActions(RenderList actions)
        {
            if (!SuspendEvents && CancelActions != null) CancelActions(this, new UserActionEventArgs(actions));
        }
		
		[Description("Raises the Moving event.")]
		protected virtual void OnMoving(RenderList actions)
		{
			if (! SuspendEvents && Moving != null) Moving(this, new UserActionEventArgs(actions));
		}

		[Description("Raises the Scaling event.")]
		protected virtual void OnScaling(RenderList actions)
		{
			if (! SuspendEvents && Scaling != null) Scaling(this, new UserActionEventArgs(actions));
		}

		[Description("Raises the Rotating event.")]
		protected virtual void OnRotating(RenderList actions)
		{
			if (! SuspendEvents && Rotating != null) Rotating(this, new UserActionEventArgs(actions));
		}

		[Description("Raises the selected changed event.")]
		protected virtual void OnSelectedChanged()
		{
			if (! SuspendEvents && SelectedChanged != null) SelectedChanged(this,EventArgs.Empty);
		}

		[Description("Raises the BeginDragSelect event.")]
		protected virtual void OnBeginDragSelect(MouseEventArgs e)
		{
			if (! SuspendEvents && BeginDragSelect != null) BeginDragSelect(this,e);
		}

		[Description("Raises the EndDragSelect event.")]
		protected virtual void OnEndDragSelect(MouseEventArgs e)
		{
			if (! SuspendEvents && EndDragSelect != null) EndDragSelect(this,e);
		}

        [Description("Raises the CancelDragSelect event.")]
        protected virtual void OnCancelDragSelect()
        {
            if (!SuspendEvents && CancelDragSelect != null) CancelDragSelect(this, EventArgs.Empty);
        }

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Model(this);
		}

		public override void Clear()
		{
			UndoList = new UndoList();
			base.Clear();
		}

		protected override void OnSerialize(IFormatter formatter, SurrogateSelector selector)
		{
			selector.AddSurrogate(typeof(Model),new StreamingContext(StreamingContextStates.All), new ModelSerialize());
			base.OnSerialize (formatter, selector);
		}

		protected override void OnDeserialize(System.Runtime.Serialization.IFormatter formatter, System.Runtime.Serialization.SurrogateSelector selector)
		{
			ModelSerialize surrogate = new ModelSerialize();
			selector.RemoveSurrogate(typeof(Diagram),new StreamingContext(StreamingContextStates.All));
			selector.AddSurrogate(typeof(Model),new StreamingContext(StreamingContextStates.All), surrogate);
			base.OnDeserialize (formatter, selector);
		}

		protected override void OnDeserializeComplete(object graph, IFormatter formatter, SurrogateSelector selector)
		{
			ModelSerialize surrogate = (ModelSerialize) Serialization.Serialize.GetSurrogate(graph,selector);
			Model model = (Model) graph;

			//Apply surrogate settings
			SuspendEvents = true;
			Suspend();

			Runtime = surrogate.Runtime;
			Runtime.ActiveContainer = this;

			AlignGrid= model.AlignGrid;
			DragScroll = model.DragScroll;
			DrawGrid = model.DrawGrid;
			DrawPageLines = model.DrawPageLines;
			DrawSelections = model.DrawSelections;
			GridColor = model.GridColor;
			GridSize = model.GridSize;
			GridStyle = model.GridStyle;
			PageLineSize = model.PageLineSize;
			Margin = model.Margin;

			Resume();
			SuspendEvents = false;

			base.OnDeserializeComplete (graph, formatter, selector);
		}

		public override void SetRender(object render)
		{
			if (! (render is IRender)) throw new ArgumentException("Render must implement IRender.");
			if (! (render is IRenderDesign)) throw new ArgumentException("Render must implement IRenderDesign.");
			base.SetRender (render);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			//Cancel any text editing
			StopEdit();

			//Selects the mouse elements
			base.OnMouseDown (e);

			//If the status has changed then the mouse up event has been triggered
			//eg by opening a context menu
			if (Status != Status.Updating) return;

            //Skip processing buttons after initial button
            if (CurrentMouseElements.StartButton != MouseButtons.None && CurrentMouseElements.StartButton != e.Button) return;

			Suspend();
			mStartPoint = new Point(e.X,e.Y);

			//Change the start location to a shape point if scaling on a grid align
			if (AlignGrid && mMouseHandle != null) 
			{
				if (mMouseHandle.Type != HandleType.Arrow && mMouseHandle.Type != HandleType.Move && mMouseHandle.Type != HandleType.Origin && mMouseHandle.Type != HandleType.LeftRight && mMouseHandle.Type != HandleType.UpDown && mMouseHandle.Type != HandleType.Expand)
				{
					SetScaleStartingPoint();
				}
			}

			mLastPoint = mStartPoint;
			mHandled = false;

			//Deselect all elements
			if (ModifierKeys != Keys.Control && (CurrentMouseElements.MouseStartElement == null || !CurrentMouseElements.MouseStartSelectable.Selected || CurrentMouseElements.MouseStartElement is Port)) SelectElements(false,this);
			
			//Select the new element
			if (CurrentMouseElements.MouseStartElement != null) 
			{
				//Select the shape and bring to front if in same layer
				if (CurrentMouseElements.MouseStartElement.Layer == Layers.CurrentLayer)
				{
					//Check if expander
					if (CurrentMouseElements.MouseStartElement is IExpandable)
					{
						Element element = CurrentMouseElements.MouseStartElement;
						IExpandable expand = (IExpandable) element;
						IContainer container = CurrentMouseElements.MouseStartElement.Container;
						PointF location = element.PointToElement(PointToDiagram(e));
	
						//Modify expanded property
						if (expand.DrawExpand && expand.Expander != null && expand.Expander.IsVisible(location)) 
						{
							mHandled = true;
					
							//Suspend to avoid multiple repaints
							Suspend();
							expand.Expanded = !expand.Expanded;
							Resume();

							Invalidate();
						}
					}
					
					//Check if handles mouse
					if (CurrentMouseElements.MouseStartElement is IMouseEvents) mHandled = ((IMouseEvents) CurrentMouseElements.MouseStartElement).OnMouseDown(e);				
					
					//Process if mouse has not been handled
					if (!mHandled)
					{
						//Select or invert
						if (ModifierKeys == Keys.Control)
						{
							CurrentMouseElements.MouseStartSelectable.Selected = !CurrentMouseElements.MouseStartSelectable.Selected;
						}
						else
						{
							CurrentMouseElements.MouseStartSelectable.Selected = true;
						}
						
						//Check if must come to front
						if (IsOrderable(CurrentMouseElements.MouseStartElement))
						{
							if (CurrentMouseElements.MouseStartElement is Shape) CurrentMouseElements.MouseStartElement.Layer.Elements.BringToFront(CurrentMouseElements.MouseStartElement);
							if (CurrentMouseElements.MouseStartElement is Line) CurrentMouseElements.MouseStartElement.Layer.Elements.BringToFront(CurrentMouseElements.MouseStartElement);
							CurrentMouseElements.MouseStartElement.Container.RenderList.Sort();

							//Redraw all connectors with jumps
							if (CurrentMouseElements.MouseStartElement is Connector) RedrawConnectors();
						}

						//Start a drag scroll timer if required
						if (DragScroll)
						{
							mTimer = new Timer();
							mTimer.Interval = Component.Instance.DragScrollInterval;
							mTimer.Tick += new EventHandler(mTimer_Tick);
							mTimer.Start();
						}
					}
				}
			}

			Resume();
			Invalidate();
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//Check if in dragscroll and if so exit
			if (mSyncTimer) return;

			Point mousePoint = new Point(e.X,e.Y);

			if (IsAlignable(CurrentMouseElements.MouseStartElement)) mousePoint = AlignMouseCoordinates(e);

			//Check if handles mouse
			if (!mHandled && CurrentMouseElements.MouseMoveElement is IMouseEvents) mHandled = ((IMouseEvents) CurrentMouseElements.MouseMoveElement).OnMouseMove(e);

            if (CurrentMouseElements.StartButton != MouseButtons.None && CurrentMouseElements.StartButton != e.Button)
            {
                base.OnMouseMove(e);
                return;
            }

			if (!mHandled)
			{
				//Set the cursor
				if (e.Button == MouseButtons.None)
				{
					if (CurrentMouseElements.MouseMoveElement == null)
					{
						Cursor = Component.Instance.GetCursor(HandleType.None);
						CancelTooltip();
					}
					else
					{
						if (CurrentMouseElements.MouseMoveElement.Layer == Layers.CurrentLayer)
						{
							mMouseHandle = CurrentMouseElements.MouseMoveElement.Handle(PointToDiagram(e));

							if (CurrentMouseElements.MouseMoveElement.Cursor != null && mMouseHandle.Type == HandleType.Move)
							{
								Cursor = CurrentMouseElements.MouseMoveElement.Cursor;
							}
							else
							{
								Cursor = Component.Instance.GetCursor(mMouseHandle.Type);
							}

							//Set the tooltip if it is null or has changed
							if (mTooltip == null || (mTooltip.GetToolTip(this) != CurrentMouseElements.MouseMoveElement.Tooltip)) SetTooltip(CurrentMouseElements.MouseMoveElement);
						}
					}
				}

				//Check left button only
                bool leftButton = (e.Button & MouseButtons.Left) == MouseButtons.Left;

				if (leftButton && CurrentMouseElements.StartButton == e.Button)
				{
					Suspend();

					IRenderDesign render = (IRenderDesign) Render;
					render.Highlights = new RenderList();

					float dx  = (mousePoint.X - mLastPoint.X) / Render.ScaleFactor;
					float dy  = (mousePoint.Y - mLastPoint.Y) / Render.ScaleFactor;

                    if (Runtime.InteractiveMode == InteractiveMode.Normal && leftButton)
					{
						if (CurrentMouseElements.MouseStartElement == null)
						{
							//Set up drag rectangle
							if (DragSelect && !mStartPoint.Equals(new Point(e.X,e.Y)))
							{
								//Set the decoration selection rectangle
								Rectangle select = Rectangle.Round(Geometry.CreateRectangle(mStartPoint,new Point(e.X,e.Y)));

								//Select the elements in the rectangle
								SelectElements(select);

								//Offset the rectangle by the scroll for rendering
								select.Offset(-DisplayRectangle.X,-DisplayRectangle.Y);

								//Raise the event and set in the render
								if (render.SelectionRectangle.IsEmpty) OnBeginDragSelect(e);
								render.SelectionRectangle = select;
							}
						}
						else
						{
							//If this is the first mouse move then create the action renderlist and lock the renderer
							if (!render.Locked)
							{
								RenderList actions = CreateActionRenderList(CurrentMouseElements.MouseStartElement.Container.RenderList);

								//If align to grid then set up positions of shapes
								//Reset dx,dy because last mouse point wasnt aligned
								if (AlignGrid && mMouseHandle.Type == HandleType.Move) 
								{
									AlignElementLocations(actions);
									dx=0; 
									dy=0;
								}
								
								//Hide the actioned elements and render before lock
								if (Component.Instance.HideActions)
								{
									HideActionElements(actions);
									Render.RenderDiagram(Render.RenderRectangle);
								}

								render.Lock();
								render.Actions = actions;
							}

							//Loop through and offset each element in the action renderlist
							if (mMouseHandle != null)
							{
								if (mMouseHandle.Type == HandleType.Move) 
								{
									OffsetElements(dx,dy);
									OnMoving(RenderDesign.Actions);
								}
								else if (mMouseHandle.Type == HandleType.Rotate)
								{
									RotateElements();
									OnRotating(RenderDesign.Actions);
								}
								else
								{
									ScaleElements();
									OnScaling(RenderDesign.Actions);
								}
							}
						}

						//Set any highlights
						if (CurrentMouseElements.MouseStartOrigin != null && CurrentMouseElements.MouseStartOrigin.AllowMove && CurrentMouseElements.MouseMoveElement != null)
						{
							//Check origin is dockable, containers are compatible etc
							if (IsModelDockable())
							{
								if (Runtime.CanDock(CurrentMouseElements)) render.Highlights.Add(CurrentMouseElements.MouseMoveElement);
							}
						}

						//Set any feedback 
						if (Runtime.Feedback && mMouseHandle != null) 
						{
							Runtime.SetFeedback(mMouseHandle.Type);
							RenderDesign.FeedbackLocation = new Point(e.X + 16 - DisplayRectangle.X, e.Y + 16 - DisplayRectangle.Y);
						}
					}

					//Add interactive items
					if (Runtime.InteractiveMode != InteractiveMode.Normal)
					{
						//Create interactive items
						if (!render.Locked)
						{
							render.Actions = CreateInteractiveRenderList(mousePoint);
							render.Lock(); //Not locked as interactive creates a new item every time
						}
						//Loop through and offset each element in the action renderlist
						else if (CurrentMouseElements.MouseStartElement != null)
						{
							if (mMouseHandle.Type == HandleType.Move) 
							{
								OffsetElements(dx,dy);
							}
							else
							{
								ScaleElements();
							}
						}

						//Set any highlights
						if (Runtime.InteractiveMode != InteractiveMode.AddShape && Runtime.InteractiveMode != InteractiveMode.AddComplexShape && CurrentMouseElements.MouseMoveElement != null)
						{
							//Check origin is dockable, containers are compatible etc
							if (IsModelDockable())
							{
								if (Runtime.CanDock(CurrentMouseElements)) render.Highlights.Add(CurrentMouseElements.MouseMoveElement);
							}
						}

						//Set any feedback 
						if (Runtime.Feedback)
						{
							Runtime.SetFeedback(mMouseHandle.Type);
							RenderDesign.FeedbackLocation = new Point(e.X+16-DisplayRectangle.X,e.Y+16-DisplayRectangle.Y);
						}
					}

					Resume();
					Invalidate();
				}
			}

			mLastPoint = mousePoint;
			base.OnMouseMove (e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			//Dispose of Dragscroll timer
			if (mTimer != null)
			{	
				mTimer.Stop();
				mTimer.Dispose();
				mTimer = null;
			}

			//Check if handles mouse
			if (!mHandled && CurrentMouseElements.MouseStartElement is IMouseEvents) mHandled = ((IMouseEvents) CurrentMouseElements.MouseStartElement).OnMouseUp(e);

            if (CurrentMouseElements.StartButton != MouseButtons.None && CurrentMouseElements.StartButton != e.Button)
            {
                base.OnMouseUp(e);
                return;
            }

			//Process left mouse button
			if (!mHandled)
			{
                //Set a mouse handle in case a mouse move has not been fired yet
                if (mMouseHandle == null) mMouseHandle = Component.Instance.DefaultHandle; 

                bool leftButton = (e.Button & MouseButtons.Left) == MouseButtons.Left;

				if (leftButton && CurrentMouseElements.StartButton == e.Button)
				{
					if (Runtime.InteractiveMode == InteractiveMode.Normal)
					{
						//Reset highlights
						RenderDesign.Highlights = new RenderList();
				
						if (CurrentMouseElements.MouseStartElement == null)
						{
							//Reset the selection rectangle decoration
							if (!RenderDesign.SelectionRectangle.IsEmpty)
							{
								RenderDesign.SelectionRectangle = new Rectangle();
								OnEndDragSelect(e);
							}
						}
						else
						{
							//Moves the elements according to the renderlist
							if (RenderDesign.Actions != null)
							{
								//Unhides the action elements
								if (Component.Instance.HideActions) ShowActionElements(RenderDesign.Actions);
								
								Suspend();
								UpdateElements();
								RedrawConnectors();
								Resume();
								Invalidate();

								OnUpdateActions(RenderDesign.Actions);
								RenderDesign.Actions = null;
								RenderDesign.Feedback = null;
								RenderDesign.FeedbackLocation = new Point();
								RenderDesign.Vector = new RectangleF();
								RenderDesign.Unlock();
							}
						}
					}

					if (Runtime.InteractiveMode != InteractiveMode.Normal)
					{
						//Reset highlights
						RenderDesign.Highlights = new RenderList();

						//Cant suspend as events are required
						UpdateInteractiveElements();

						OnUpdateActions(RenderDesign.Actions);
						RenderDesign.Actions = null;
						RenderDesign.Feedback = null;
						RenderDesign.FeedbackLocation = new Point();
						RenderDesign.Unlock();
					}
					Invalidate();
				}
			}

			mHandled = false;
			base.OnMouseUp (e);
		}

		protected override void OnDragOver(DragEventArgs drgevent)
		{
			SaveStatus();
			SetStatus(Status.DragOver);

			if (DragElement != null)
			{

				//Translate the drag point from screen co-ordinates to control co-ordinates
				Point controlPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
				
				//Get any container elements under the cursor
				Element element = ElementFromPoint(controlPoint.X, controlPoint.Y);

				//Reset highlights
				RenderDesign.Highlights = new RenderList();

				if (element != null && element is IContainer)
				{
					IContainer container = (IContainer) element;

					RenderDesign.Highlights.Add(element);
				}

				//Get the offset so that the shape is centered round the cursor
				PointF dropPoint = OffsetDrop(controlPoint,DragElement.Rectangle);

				//Reset the decoration path
				RenderDesign.DecorationPath = DragElement.GetPath();

				//Move the decoration path to the new position
				Matrix translate = new Matrix();
				translate.Translate(dropPoint.X, dropPoint.Y);
				RenderDesign.DecorationPath.Transform(translate);
				
				Invalidate();
			}

			base.OnDragOver(drgevent);
			RestoreStatus();
		}

		protected override void OnDragEnter(DragEventArgs drgevent)
		{
			SaveStatus();
			SetStatus(Status.DragEnter);

			//Get the type 
			Type type;

			foreach (String str in drgevent.Data.GetFormats())
			{
				//Try resolve the type, possibly across assemblies
				type = null;
				type = Serialization.Serialize.ResolveType(str);

				if (type !=null && type.IsSubclassOf(typeof(Element)))
				{
					drgevent.Effect = DragDropEffects.Copy;
					
					//Set up the drag element
					DragElement = (Element) drgevent.Data.GetData(type); //gets the data
					
					//Translate the drag point from screen co-ordinates to control co-ordinates
					PointF dragPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
					dragPoint = OffsetDrop(dragPoint,DragElement.Rectangle);

					//Reset the decoration path
					RenderDesign.DecorationPath = DragElement.GetPath();

					//Move the decoration path to the new position
					Matrix translate = new Matrix();
					translate.Translate(dragPoint.X, dragPoint.Y);
					RenderDesign.DecorationPath.Transform(translate);
					
					Invalidate();
					break;
				}
			}
			
			base.OnDragEnter(drgevent);
			RestoreStatus();
		}

		protected override void OnDragDrop(DragEventArgs drgevent)
		{
			SaveStatus();
			SetStatus(Status.DragDrop);

			if (DragElement != null)
			{
				Suspend();

				//Set the container
				IContainer container = this;

				//Translate the drag point from screen co-ordinates to control co-ordinates
				Point controlPoint = PointToClient(new Point(drgevent.X, drgevent.Y));
				
				//Get any container elements under the cursor
				Element element = ElementFromPoint(controlPoint.X, controlPoint.Y);
				if (element != null && element is IContainer) container = (IContainer) element;

				PointF dropPoint = OffsetDrop(controlPoint,DragElement.Rectangle);

				dropPoint.X -= container.Offset.X;
				dropPoint.Y -= container.Offset.Y;

				//Move any ports
				if (DragElement is IPortContainer)
				{
					IPortContainer portContainer = (IPortContainer) DragElement;
 
					//Store values before updating underlying rectangle
					float dx = dropPoint.X - DragElement.Rectangle.X;
					float dy = dropPoint.Y - DragElement.Rectangle.Y;
					
					//Move ports by change in x and y
					if (portContainer.Ports != null)
					{
						foreach(Port port in portContainer.Ports.Values)
						{
							port.SuspendValidation();
							port.Move(dx,dy);
							port.ResumeValidation();
						}
					}
				}

				//Change settings				
				DragElement.SetRectangle(dropPoint);
				if (DragElement is SolidElement)
				{
					SolidElement solid = (SolidElement) DragElement;
					solid.SetTransformRectangle(dropPoint);
				}

				DragElement.SetLayer(null);

				if (DragElement is Shape)
				{
					Shape shape = (Shape) DragElement;

					shape.AllowMove = true;
					shape.AllowScale = true;
					shape.MinimumSize = new SizeF(32, 32);
					shape.MaximumSize = new SizeF(320, 320);

					//Align to grid using common grid alignment
					if (AlignGrid)
					{
						RenderList list = new RenderList();
						list.Add(shape);
						AlignElementLocations(list);
					}

					//Add the element to lines or shapes
					container.Shapes.Add(container.Shapes.CreateKey(),DragElement);
				}

				//Clear the render path
				RenderDesign.DecorationPath = null;
				RenderDesign.Highlights = null;
	
				//Repaint
				Resume();
				Refresh();
			}
			
			base.OnDragDrop(drgevent);
			DragElement = null; //Set to null only after the event
			
			RestoreStatus();
		}

		protected override void OnDragLeave(EventArgs e)
		{
			base.OnDragLeave(e);

			DragElement = null;
			RenderDesign.DecorationPath = null;
			RenderDesign.Highlights = null;
			Invalidate();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			//Dont process messages if the text editor is active
			if (mLabelEdit != null) return false;

			if (msg.Msg == WM_KEYDOWN)
			{
				switch (keyData)
				{
					case Keys.Control | Keys.Z:
						
						if (Runtime.Undo) 
						{
							DoCommand("undo");
							return true;
						}
						break;
	
					case Keys.Control | Keys.Y:
				
						if (Runtime.Redo) 
						{
							DoCommand("redo");
							return true;
						}
						break;

					case Keys.Control | Keys.X:
				
						if (Runtime.Cut)
						{ 
							DoCommand("cut");
							return true;
						}
						break;

					case Keys.Control | Keys.C:
				
						if (Runtime.Copy)
						{
							DoCommand("copy");
							return true;
						}
						break;

					case Keys.Control | Keys.V:
				
						if (Runtime.Paste)
						{
							DoCommand("paste");
							return true;
						}
						break;

					case Keys.Control | Keys.A:

						UndoList.Suspend();
						Suspend();
						SelectElements(true,this);
						Resume();
						Refresh();
						UndoList.Resume();

						return true;
						break;

					case Keys.Control | Keys.PageDown:
						
						ZoomModel(true);

						return true;
						break;

					case Keys.Control | Keys.PageUp:
						
						ZoomModel(false);

						return true;
						break;

					case Keys.Delete:
				
						if (Runtime.Delete) 
						{
							DoCommand("delete");
							return true;
						}
						break;

                    case Keys.Escape:

                        CancelAction();
                        return true;
                        
                        break;
				}
			}

			return base.ProcessCmdKey (ref msg, keyData);
		}

		protected override void ExportToPicture(System.IO.Stream stream, System.Drawing.Imaging.ImageFormat format)
		{
			bool drawGrid = RenderDesign.DrawGrid;
			bool drawPageLines = RenderDesign.DrawPageLines;
			bool drawSelection = RenderDesign.DrawSelections;
			bool paged = RenderPaged.Paged;

			Suspend();
			
			RenderDesign.DrawGrid = false;
			RenderDesign.DrawPageLines = false;
			RenderDesign.DrawSelections = false;
			RenderPaged.Paged = false;
			
			base.ExportToPicture (stream, format);
			
			RenderDesign.DrawGrid = drawGrid;
			RenderDesign.DrawPageLines = drawPageLines;
			RenderDesign.DrawSelections = drawSelection;
			RenderPaged.Paged = paged;

			Resume();
			Invalidate();
		}

		protected override void OnElementInserted(Element element)
		{
			//Add a new undo point
			if (!UndoList.Suspended) 
			{
				SaveStatus();
				
				SetStatus(Status.UndoRedo);
				UndoList.Add(element,UndoAction.Add);
				
				RestoreStatus();
			}

			//Attach handler to detect if selected
			if (element is ISelectable)
			{
				ISelectable selectable = (ISelectable) element;
				selectable.SelectedChanged +=new EventHandler(selectable_SelectedChanged);
			}

			base.OnElementInserted (element);
		}

		protected override void OnElementRemoved(Element element)
		{
			//Remove handler to detect if selected
			if (element is ISelectable)
			{
				ISelectable selectable = (ISelectable) element;
				selectable.SelectedChanged -=new EventHandler(selectable_SelectedChanged);
			}

			if (!UndoList.Suspended) 
			{
				SaveStatus();
				
				SetStatus(Status.UndoRedo);
				UndoList.Add(element,UndoAction.Remove);

				RestoreStatus();
			}
			
			base.OnElementRemoved (element);
		}

		#endregion

		#region Events
		
		//Occurs when the timer ticks
		private void mTimer_Tick(object sender, EventArgs e)
		{
			mSyncTimer = true;
			CheckDragScroll();
			mSyncTimer = false;
		}

		private void mLabelEdit_Complete(object sender, EventArgs e)
		{
			StopEdit();
		}

		//Add or remove items from the selected list
		private void selectable_SelectedChanged(object sender, EventArgs e)
		{
			mSelectedElements = null;
			mSelectedShapes = null;
			mSelectedLines = null;
			OnSelectedChanged();
		}

		#endregion

		#region Implementation

		private Shape AddShapeImplementation(string key,PointF location, StencilItem stencil, SizeF size)
		{
			Shape shape = Runtime.CreateShape(location,size);
			shape.Location = location;
			
			//Set values if not provided
			if (key == null || key == "") key = Runtime.ActiveContainer.Shapes.CreateKey();
			if (stencil == null) stencil = Component.Instance.DefaultStencilItem;

			//Set size
			if (!size.IsEmpty) shape.Size = size;
			
			//Set shape's stencil
			shape.StencilItem = stencil;
			stencil.CopyTo(shape);

			//Add and return the new shape
			Runtime.ActiveContainer.Shapes.Add(key,shape);
			return shape;
		}
		
		//Sets all elements selected to true/false
		public void SelectElements(bool selection)
		{
			SelectElements(selection,this);
		}

		//Sets all elements selected to true/false
		public void SelectElements(bool selection,IContainer container)
		{
			Suspend();
			
			foreach (Shape shape in container.Shapes.Values)
			{
				shape.Selected = selection;
				if (shape is Group) SelectElements(selection,(IContainer) shape);
			}

			foreach (Line line in container.Lines.Values )
			{
				line.Selected = selection;
			}

			Resume();
			Invalidate();
		}

		//Selects elements from a rectangle
		public void SelectElements(Rectangle screenRect)
		{
			RectangleF modelRect = screenRect;
			modelRect.Offset(-DisplayRectangle.X,-DisplayRectangle.Y);
			modelRect = RectangleToDiagram(modelRect);
			
			foreach (Element element in RenderList)
			{
				if (element is ISelectable)
				{
					ISelectable selectable = (ISelectable) element;
					selectable.Selected = (element.Intersects(modelRect) && element.Layer == Layers.CurrentLayer);
				}
			}
		}

		//Sets the selected working elements
		private void GetSelectedElements()
		{
			mSelectedElements = new Elements();
			mSelectedShapes = new Elements(typeof(Shape),"Shape");
			mSelectedLines = new Elements(typeof(Line),"Line");

			foreach (Shape shape in Shapes.Values)
			{
				if (shape.Selected)
				{
					mSelectedElements.Add(shape.Key,shape);
					mSelectedShapes.Add(shape.Key,shape);
				}
			}

			foreach (Line line in Lines.Values)
			{
				if (line.Selected) 
				{
					mSelectedElements.Add(line.Key,line);
					mSelectedLines.Add(line.Key,line);
				}
			}

			mSelectedElements.SetModifiable(false);
			mSelectedShapes.SetModifiable(false);
			mSelectedLines.SetModifiable(false);
		}

		private Elements GetSelectedElements(System.Type type)
		{
			Elements elements = new Elements();

			foreach (Shape shape in Shapes.Values)
			{
				if (shape.Selected && (type.IsInstanceOfType(shape) || shape.GetType().IsSubclassOf(type))) elements.Add(shape.Key,shape);
			}

			foreach (Line line in Lines.Values)
			{
				if (line.Selected && (type.IsInstanceOfType(line) || line.GetType().IsSubclassOf(type))) elements.Add(line.Key,line);
			}

			elements.SetModifiable(false);
			return elements;
		}

		//Builds up a renderlist containing copies of the elements to be rendered in an action
		protected virtual RenderList CreateActionRenderList(RenderList renderList)
		{
			RenderList actions = new RenderList();
			Element newElement;
			Port newPort;
						
			foreach (Element element in renderList)
			{
				if (element is ISelectable && element.Layer == Layers.CurrentLayer)
				{
					//Add shapes and lines
					ISelectable selectable = (ISelectable) element;

					if (selectable.Selected) 
					{
						newElement = (Element) element.Clone();
						newElement.ActionElement = element;
						newElement.SetKey(element.Key);
						actions.Add(newElement);
					
						//Set the action shapes for the complex shape children
						if (element is ComplexShape)
						{
							ComplexShape complex = (ComplexShape) element;
							ComplexShape newComplex = (ComplexShape) newElement;
						
							foreach (SolidElement solid in newComplex.Children.Values)
							{
								solid.ActionElement = complex.Children[solid.Key];
							}
						}

						//Keep size of table
						if (element is Table)
						{
							Table table = (Table) element;
							Table newTable = (Table) newElement;

							newTable.Size = table.Size;
						}
					}
				}
			}

			//Connect any shapes and lines, adding any non-selected items as hidden actions
			ConnectInteractiveElements(actions);
				
			//Loop through and connect the lines
			foreach (Element element in actions)
			{
				if (element is Line)
				{
					Line line = (Line) element;
					if (line.Start.Shape != null) line.Start.Shape = (Shape) actions[line.Start.Shape.Key];
					if (line.End.Shape != null) line.End.Shape = (Shape) actions[line.End.Shape.Key];
				}
			}

			//Add any ports
			if (CurrentMouseElements.MouseStartElement is Port)
			{
				newPort = (Port) CurrentMouseElements.MouseStartElement.Clone();
				newPort.Validate = true;
				newPort.ActionElement = CurrentMouseElements.MouseStartElement;
				actions.Add(newPort);
			}

			return actions;
		}

		//Builds up a renderlist containing copies of the elements to be rendered in an action
		private RenderList CreateInteractiveRenderList(Point mousePoint)
		{
			RenderList actions = new RenderList();
			PointF start = PointToDiagram(mStartPoint.X,mStartPoint.Y);
			PointF end = PointToDiagram(mousePoint.X,mousePoint.Y);

			IContainer container = this;

			//Prepare a new mouse elements structure
			MouseElements mouseElements = new MouseElements(CurrentMouseElements);

			//Set the correct container
			if (CurrentMouseElements.MouseStartElement != null)
			{
				container = CurrentMouseElements.MouseStartElement.Container;
				start.X -= container.Offset.X;
				start.Y -= container.Offset.Y;
				end.X -= container.Offset.X;
				end.Y -= container.Offset.Y;
			}
			
			//Add a line interactively
			if (Runtime.InteractiveMode == InteractiveMode.AddLine)
			{
				//Create new line
				Line line = Runtime.CreateLine(start,end);
				
				//Set temporary container and layer
				line.SetLayer(Layers.CurrentLayer);
				
				//Set line container
				line.SetContainer(container);
				
				line.DrawPath();

				//Create action line
				Line newLine = (Line) line.Clone();
				newLine.ActionElement = line;
				actions.Add(newLine);

				mMouseHandle = new Handle(HandleType.Origin);
				mMouseHandle.CanDock = true;

				//Set up mouse elements
				mouseElements.MouseStartElement = line;
				mouseElements.MouseStartOrigin = line.End;
			}

			//Add an connector interactively
			if (Runtime.InteractiveMode == InteractiveMode.AddConnector)
			{
				//Create new line
				Connector line = Runtime.CreateConnector(start,end);

				//Set temporary container and layer
				line.SetLayer(Layers.CurrentLayer);
				
				//Set line container
				line.SetContainer(container);

				line.Avoid = true;
				line.CalculateRoute();
				line.DrawPath();

				//Create action line
				Connector newLine = (Connector) line.Clone();
				newLine.ActionElement = line;
				newLine.Avoid = true;
				actions.Add(newLine);

				mMouseHandle = new Handle(HandleType.Origin);
				mMouseHandle.CanDock = true;

				//Set up mouse elements
				mouseElements.MouseStartElement = line;
				mouseElements.MouseStartOrigin = line.End;
			}

			if (Runtime.InteractiveMode == InteractiveMode.AddComplexLine)
			{
				//Create new line
				ComplexLine line = Runtime.CreateComplexLine(start,end);

				//Set temporary container and layer
				line.SetLayer(Layers.CurrentLayer);
				
				//Set line container
				line.SetContainer(container);

				line.DrawPath();

				//Create action line
				Line newLine = (ComplexLine) line.Clone();
				newLine.ActionElement = line;
				actions.Add(newLine);

				mMouseHandle = new Handle(HandleType.Origin);
				mMouseHandle.CanDock = true;

				//Set up mouse elements
				mouseElements.MouseStartElement = line;
				mouseElements.MouseStartOrigin = line.End;
			}

			if (Runtime.InteractiveMode == InteractiveMode.AddCurve)
			{
				//Create new line
				Curve line = Runtime.CreateCurve(start,end);

				//Set temporary container and layer
				line.SetLayer(Layers.CurrentLayer);
				
				//Set line container
				line.SetContainer(container);

				line.DrawPath();

				//Create action line
				Line newLine = (Curve) line.Clone();
				newLine.ActionElement = line;
				actions.Add(newLine);

				mMouseHandle = new Handle(HandleType.Origin);
				mMouseHandle.CanDock = true;

				//Set up mouse elements
				mouseElements.MouseStartElement = line;
				mouseElements.MouseStartOrigin = line.End;
			}

			if (Runtime.InteractiveMode == InteractiveMode.AddShape)
			{
				SizeF size = new SizeF(end.X - start.X,end.Y - start.Y);
				Shape shape = Runtime.CreateShape(start,size);

				//Set temporary container and layer
				shape.SetLayer(Layers.CurrentLayer);
				
				//Set line container
				shape.SetContainer(container);

				//Create action shape
				Shape newShape = (Shape) shape.Clone();
				newShape.ActionElement = shape;
				actions.Add(newShape);

				mMouseHandle = new Handle(HandleType.BottomRight);
			}

			if (Runtime.InteractiveMode == InteractiveMode.AddComplexShape)
			{
				SizeF size = new SizeF(end.X - start.X,end.Y - start.Y);
				ComplexShape shape = Runtime.CreateComplexShape(start,size);

				//Set temporary container and layer
				shape.SetLayer(Layers.CurrentLayer);
				
				//Set line container
				shape.SetContainer(container);

				//Create action shape
				ComplexShape newShape = (ComplexShape) shape.Clone();
				newShape.ActionElement = shape;
				actions.Add(newShape);

				//Set the action shapes for the complex shape children
				foreach (SolidElement solid in newShape.Children.Values)
				{
					solid.ActionElement = shape.Children[solid.Key];
				}

				mMouseHandle = new Handle(HandleType.BottomRight);
			}

			//Check for interactive docking
			if (Runtime.InteractiveMode == InteractiveMode.AddLine || Runtime.InteractiveMode == InteractiveMode.AddConnector || Runtime.InteractiveMode == InteractiveMode.AddComplexLine || Runtime.InteractiveMode == InteractiveMode.AddCurve)
			{
				foreach (Element element in actions)
				{
					if (element is Line)
					{
						Line line = (Line) element;
						Line action = (Line) line.ActionElement;

						//Set up the elements
						mouseElements.InteractiveElement = line;
						mouseElements.InteractiveOrigin = line.Start;
						mouseElements.MouseMoveElement = CurrentMouseElements.MouseMoveElement;
						
						//Check if start is shape
						if (CurrentMouseElements.MouseMoveElement is Shape && Runtime.CanDock(mouseElements)) 
						{
							//line.Start.Shape = (Shape) CurrentMouseElements.MouseStartElement;
							action.Start.Shape = (Shape) CurrentMouseElements.MouseStartElement;
						}
				
						//Check if start is port
						if (CurrentMouseElements.MouseMoveElement is Port && Runtime.CanDock(mouseElements)) 
						{
							//line.Start.Port = (Port) CurrentMouseElements.MouseStartElement;
							action.Start.Port = (Port) CurrentMouseElements.MouseStartElement;
						}
					}
				}
			}

			ConnectInteractiveElements(actions);

			//Set mouse elements
			SetMouseElements(mouseElements);

			return actions;
		}

		//Connect any shapes and lines, adding any non-selected items as hidden actions
		private void ConnectInteractiveElements(RenderList actions)
		{
			//Set up origins for any lines
			RenderList hidden = new RenderList();

			foreach (Element element in actions)
			{
				if (element is Line)
				{
					Line line = (Line) element;
					Line actionLine = (Line) element.ActionElement;
					Shape newShape;
					Element newElement;
					IPortContainer ports;

					line.Start.SuspendEvents = true;
					line.End.SuspendEvents = true;

					//Add any shapes and shapes with ports that are connected to lines as invisible items
					if (actionLine.Start.Shape != null)
					{
						if (!actions.ContainsKey(actionLine.Start.Shape.Key))
						{
							newShape = (Shape) actionLine.Start.Shape.Clone();
							newShape.ActionElement = actionLine.Start.Shape;
							newShape.SetKey(actionLine.Start.Shape.Key);
							newShape.Visible = false;
							hidden.Add(newShape);

							line.Start.Shape = newShape;
						}
						else
						{
							line.Start.Shape = (Shape) actions[actionLine.Start.Shape.Key];
						}
					}
					else if (actionLine.Start.Port != null)
					{
						Element parent = (Element) actionLine.Start.Port.Parent;
						
						if (!actions.ContainsKey(parent.Key))
						{
							newElement = (Element) parent.Clone();
							newElement.ActionElement = parent;
							newElement.SetKey(parent.Key);
							newElement.Visible = false;
							hidden.Add(newElement);
							
							ports = (IPortContainer) newElement;
							line.Start.Port = (Port) ports.Ports[actionLine.Start.Port.Key];
						}
						else
						{
							ports = (IPortContainer) actions[parent.Key];
							line.Start.Port =(Port) ports.Ports[actionLine.Start.Port.Key];
						}
					}

					if (actionLine.End.Shape != null)
					{
						if (!actions.ContainsKey(actionLine.End.Shape.Key))
						{
							newShape = (Shape) actionLine.End.Shape.Clone();
							newShape.ActionElement = actionLine.End.Shape;
							newShape.SetKey(actionLine.End.Shape.Key);
							newShape.Visible = false;
							hidden.Add(newShape);
							line.End.Shape = newShape;
						}
						else
						{
							line.End.Shape = (Shape) actions[actionLine.End.Shape.Key];
						}
					}
					else if (actionLine.End.Port != null)
					{
						Element parent = (Element) actionLine.End.Port.Parent;

						if (!actions.ContainsKey(parent.Key))
						{
							newElement = (Element) parent.Clone();
							newElement.ActionElement = parent;
							newElement.SetKey(parent.Key);
							newElement.Visible = false;
							hidden.Add(newElement);
							
							ports = (IPortContainer) newElement;
							line.End.Port = (Port) ports.Ports[actionLine.End.Port.Key];
						}
						else
						{
							ports = (IPortContainer) actions[parent.Key];
							line.End.Port =(Port) ports.Ports[actionLine.End.Port.Key];
						}
					}

					line.Start.SuspendEvents = false;
					line.End.SuspendEvents = false;
				}
			}
			
			//Add any hidden shapes to the actions renderlist
			if (hidden.Count > 0) actions.AddRange(hidden);
		}

		//Loop through and offset the elements in the action renderlist
		protected virtual void OffsetElements(float dx,float dy)
		{
			//Check bounds of all items
			if (CurrentMouseElements.MouseStartElement == null) return;
			if (!CheckBounds(dx,dy,CurrentMouseElements.MouseStartElement.Container)) return;

			//Change position of each element
			foreach (Element element in RenderDesign.Actions)
			{
				if (element.Visible)
				{
					//Offset shapes
					if (element is Shape)
					{
						Shape shape = (Shape) element;
						if (shape.AllowMove) shape.Move(dx,dy);

						if (Route != null) Route.Reform();
					}

					//Offset ports
					if (element is Port)
					{
						Port port = (Port) element;
						Port actionPort = (Port) element.ActionElement;
						if (port.AllowMove) port.Move(actionPort.Location.X + dx,actionPort.Location.Y + dy);

						if (Route != null) Route.Reform();
					}

					//Offset line
					if (element is Line)
					{
						//Offset all segments in complex line
						if (element is ComplexLine)
						{
							ComplexLine complexLine = (ComplexLine) element;
							ComplexLine actionLine = (ComplexLine) element.ActionElement;
							Segment segment = null;
							Segment actionSegment = null;

							if (actionLine.AllowMove)
							{
								for (int i=0; i<complexLine.Segments.Count; i++)
								{
									segment = complexLine.Segments[i];
									actionSegment = actionLine.Segments[i];
								
									if (!actionSegment.Start.Docked)
									{
										segment.Start.Move(dx,dy);
									}
								}
								if (!actionSegment.End.Docked) segment.End.Move(dx,dy);
							}
						}
						else if (element is Curve)
						{
							Curve curve = (Curve) element;
							Curve actionCurve = (Curve) curve.ActionElement;
							if (actionCurve.AllowMove)
							{
								if (!actionCurve.Start.Docked || (actionCurve.Start.Shape != null && actionCurve.Start.Shape.Selected) || (actionCurve.Start.Port != null && ((ISelectable) actionCurve.Start.Port.Parent).Selected)) curve.Start.Move(dx,dy);
								if (!actionCurve.End.Docked || (actionCurve.End.Shape != null && actionCurve.End.Shape.Selected) || (actionCurve.End.Port != null && ((ISelectable) actionCurve.End.Port.Parent).Selected)) curve.End.Move(dx,dy);
							
								PointF[] newPoints = new PointF[actionCurve.ControlPoints.GetUpperBound(0)+1];
								for(int i=0; i<=actionCurve.ControlPoints.GetUpperBound(0); i++)
								{
									newPoints[i] = new PointF(curve.ControlPoints[i].X + dx,curve.ControlPoints[i].Y + dy);
								}
								curve.ControlPoints = newPoints;
							}
						}
						else if (element is Connector) 
						{
							Connector connector = (Connector) element;
							Connector actionConnector = (Connector) connector.ActionElement;							

							if (actionConnector.AllowMove && !actionConnector.Start.Docked && !actionConnector.End.Docked)
							{
								ArrayList newPoints = new ArrayList();

								foreach (PointF point in connector.Points)
								{
									newPoints.Add(new PointF(point.X + dx, point.Y + dy));
								}
								connector.SetPoints(newPoints);
								connector.DrawPath();
							}
						}
						else
						{
							Line line = (Line) element;
							Line actionLine = (Line) line.ActionElement;
							if (actionLine.AllowMove)
							{
								if (!actionLine.Start.Docked)  line.Start.Move(dx,dy);
								if (!actionLine.End.Docked) line.End.Move(dx,dy);
								line.DrawPath();
							}
						}
					}
					
					//Offset stand-alone port
					if (element is Port)
					{
						Port port = (Port) element;
						if (port.AllowMove) port.Move(dx,dy);
					}
				}
			}
		}

		//Loop through and scale the elements in the action renderlist
		protected virtual void ScaleElements()
		{
			//Calculate the percentage scale
			float dx = (mLastPoint.X - mStartPoint.X) * Render.ZoomFactor; //distance cursor has moved
			float dy = (mLastPoint.Y - mStartPoint.Y) * Render.ZoomFactor;
			float sx = 1; //scale
			float sy = 1; 
			float mx = 0; //movement as a result of scale
			float my = 0;

			foreach (Element element in RenderDesign.Actions)
			{
				if (element.Visible)
				{
					//Scale shapes
					if (element is Shape)
					{
						Shape shape = (Shape) element; //a clone of the original shape, contained in the list
						Shape actionshape = (Shape) shape.ActionElement; //the actual shape being moved

						if (actionshape.AllowScale)
						{
							if (Route != null) Route.Reform();

							PointF saveLocation = shape.Location;
							SizeF saveSize = shape.Size;
							
							//Reset the ports
							foreach (Port port in shape.Ports.Values)
							{
								Port actionPort = (Port) actionshape.Ports[port.Key];
								port.SuspendValidation();
								port.Location = actionPort.Location;
								port.ResumeValidation();
							}

							//Reset shape location and size
							shape.Location = actionshape.Location; //reset location
							shape.SetSize(actionshape.Size,actionshape.InternalRectangle); //reset to original size

							//Reset children of a complex shape
							if (shape is ComplexShape)
							{
								ComplexShape complex = (ComplexShape) shape;
						
								foreach (SolidElement solid in complex.Children.Values)
								{
									SolidElement actionSolid = (SolidElement) solid.ActionElement;
								
									solid.Location = actionSolid.Location; //reset location
									solid.SetSize(actionSolid.Size,actionSolid.InternalRectangle); //reset to original size
								}
							}

							//Scale Right x				
							if (mMouseHandle.Type == HandleType.TopRight || mMouseHandle.Type == HandleType.Right || mMouseHandle.Type == HandleType.BottomRight)
							{
								sx = ((dx) / shape.ActionElement.Rectangle.Width)+1;
							}
							//Scale Bottom y
							if (mMouseHandle.Type == HandleType.BottomLeft || mMouseHandle.Type == HandleType.Bottom || mMouseHandle.Type == HandleType.BottomRight)
							{
								sy = ((dy) /shape.ActionElement.Rectangle.Height)+1;
							}
							//Scale Left x
							if (mMouseHandle.Type == HandleType.TopLeft || mMouseHandle.Type == HandleType.Left || mMouseHandle.Type == HandleType.BottomLeft)
							{
								sx = ((-dx) / shape.ActionElement.Rectangle.Width)+1;
								mx = dx;
								if (shape.Rectangle.Width * sx < shape.MinimumSize.Width) mx = (shape.ActionElement.Rectangle.Width - shape.MinimumSize.Width);
								if (shape.Rectangle.Width * sx > shape.MaximumSize.Width) mx = (shape.ActionElement.Rectangle.Width - shape.MaximumSize.Width);
							}
							//Scale Top y
							if (mMouseHandle.Type == HandleType.TopLeft || mMouseHandle.Type == HandleType.Top || mMouseHandle.Type == HandleType.TopRight)
							{
								sy = ((-dy) /shape.ActionElement.Rectangle.Height)+1;
								my = dy;
								if (shape.Rectangle.Height * sy < shape.MinimumSize.Height) my = (shape.ActionElement.Rectangle.Height - shape.MinimumSize.Height);
								if (shape.Rectangle.Height * sy > shape.MaximumSize.Height) my = (shape.ActionElement.Rectangle.Height - shape.MaximumSize.Height);
							}
				
							shape.Scale(sx,sy,mx,my,ModifierKeys == Keys.Shift || shape.KeepAspect);

							//Restore shape bounds if not correct
							//if (!CheckBounds(element, element.Container)) 
							if (!CheckBounds(shape, shape.Container)) 
							{
								shape.Location = saveLocation;
								shape.Size = saveSize;
							}
						}
					}

					//Move line origins
					if (element is Line)
					{
						if (element is ComplexLine)
						{
							ComplexLine line = (ComplexLine) element;
							ComplexLine actionline = (ComplexLine) line.ActionElement;
							Segment segment;
							Segment actionSegment;
					
							if (mMouseHandle.Type == HandleType.Origin)
							{
								for (int i2 = 0; i2 < line.Segments.Count; i2++)
								{
									segment = line.Segments[i2];
									actionSegment = actionline.Segments[i2];

									if (actionSegment.Start == CurrentMouseElements.MouseStartOrigin && actionSegment.Start.AllowMove)
									{
										if (CheckBounds(actionSegment.Start.Location, dx, dy, actionline.Container))
										{
											segment.Start.SuspendEvents = true;
											segment.Start.Location = (PointF) actionline.Points[i2]; //Resets the location
											segment.Start.Move(dx,dy);
											segment.Start.SuspendEvents = false;
											line.DrawPath(); 
										}
										break;
									}

									if (actionSegment.End == CurrentMouseElements.MouseStartOrigin && actionSegment.End.AllowMove)
									{
										if (CheckBounds(actionSegment.End.Location, dx, dy, actionline.Container))
										{
											segment.End.SuspendEvents = true;
											segment.End.Location = (PointF) actionline.Points[i2+1]; //Resets the location
											segment.End.Move(dx,dy);
											segment.End.SuspendEvents = false;
											line.DrawPath(); 
										}
										break;
									}
								}
							}

							//Add the segment and reset the handle to an origin handle
							if (mMouseHandle.Type == HandleType.Expand)
							{
								//Find the segment
								ExpandHandle expand = (ExpandHandle) mMouseHandle;
								
								//Get origin locations
								PointF start = line.GetOriginLocation(expand.Segment.Start, expand.Segment.End);
								PointF end = line.GetOriginLocation(expand.Segment.End, expand.Segment.Start);
								
								Origin origin = new Origin(new PointF(start.X + ((end.X - start.X) / 2),start.Y + ((end.Y - start.Y) / 2)));
								Origin actionOrigin = new Origin(new PointF(origin.Location.X, origin.Location.Y));

								line.AddSegment(expand.Index + 1, origin );
								actionline.AddSegment(expand.Index + 1, actionOrigin);

								mMouseHandle = new Handle(HandleType.Origin);

								//Set up mouse elements
								MouseElements mouseElements = new MouseElements(CurrentMouseElements);
								mouseElements.MouseStartOrigin = actionOrigin;
								SetMouseElements(mouseElements);
								
							}
						}
						else if (element is Connector)
						{
							Connector line = (Connector) element;
							Connector actionLine = (Connector) element.ActionElement;
						
							//Move start or end of connector
							if (mMouseHandle.Type == HandleType.Origin)
							{			
								Origin origin = null;
								PointF point = new PointF();

								//Get the origin point
								if (actionLine.Start == CurrentMouseElements.MouseStartOrigin && actionLine.Start.AllowMove)
								{
									origin = line.Start;
									point = (PointF) actionLine.Points[0];
								}
								if (actionLine.End == CurrentMouseElements.MouseStartOrigin && actionLine.End.AllowMove)
								{
									origin = line.End;
									point = (PointF) actionLine.Points[actionLine.Points.Count-1];
								}

								if (origin != null)
								{
									if (CheckBounds(point, dx, dy, actionLine.Container))
									{
										//Offset the origin point
										origin.Location = new PointF(point.X + dx, point.Y + dy);
									
										//Set to shape if current mouse element is shape
										if (IsModelDockable() && Runtime.CanDock(CurrentMouseElements))
										{
											if (CurrentMouseElements.MouseMoveElement is Shape)
											{
												origin.Shape = (Shape) CurrentMouseElements.MouseMoveElement;
											}
											else if (CurrentMouseElements.MouseMoveElement is Port) 
											{
												origin.Port = (Port) CurrentMouseElements.MouseMoveElement;
											}
										}
											 
										line.CalculateRoute();
									}
								}
							}
							//Move a connector segment
							else if (mMouseHandle.Type == HandleType.UpDown || mMouseHandle.Type == HandleType.LeftRight)
							{
								ConnectorHandle handle = (ConnectorHandle) mMouseHandle;
								
								if (handle != null)
								{
									PointF start = (PointF) actionLine.Points[handle.Index -1];
									PointF end = (PointF) actionLine.Points[handle.Index];

									//Move the two segment points and place them back in the correct place
									if (mMouseHandle.Type == HandleType.UpDown)
									{
										if (CheckBounds(start, 0, dy, actionLine.Container) && CheckBounds(end, 0, dy, actionLine.Container))
										{
											start.Y += dy;
											end.Y += dy;

											//Update the line
											line.Points[handle.Index -1] = start;
											line.Points[handle.Index] = end;
										}
									}
									else if (mMouseHandle.Type == HandleType.LeftRight)
									{
										if (CheckBounds(end, dx, 0, actionLine.Container) && CheckBounds(end, dx, 0, actionLine.Container))
										{
											start.X += dx;
											end.X += dx;

											//Update the line
											line.Points[handle.Index -1] = start;
											line.Points[handle.Index] = end;
										}
									}
								}
							}
						}
						else if (element is Curve)
						{
							Curve curve = (Curve) element;
							Curve actionCurve = (Curve) curve.ActionElement;

							if (CurrentMouseElements.MouseStartOrigin == actionCurve.Start && actionCurve.Start.AllowMove)
							{
								if (CheckBounds(actionCurve.FirstPoint, dx, dy, actionCurve.Container))
								{								
									curve.Start.SuspendEvents = true;
									curve.Start.Location = actionCurve.FirstPoint; //Resets the location
									curve.Start.Move(dx,dy);
									curve.Start.SuspendEvents = false;
								}
							}
							else if (CurrentMouseElements.MouseStartOrigin == actionCurve.End && actionCurve.End.AllowMove)
							{
								if (CheckBounds(actionCurve.LastPoint, dx, dy, actionCurve.Container))
								{
									curve.End.SuspendEvents = true;
									curve.End.Location = actionCurve.LastPoint; //Resets the location
									curve.End.Move(dx,dy);
									curve.End.SuspendEvents = false;
								}
							}
							else
							{
								//Move control points
								int index = 0;
								foreach (PointF point in actionCurve.ControlPoints)
								{
									PointF location = new PointF(point.X - actionCurve.Rectangle.X - actionCurve.Container.Offset.X, point.Y - actionCurve.Rectangle.Y - actionCurve.Container.Offset.Y);

									if (mMouseHandle != null && mMouseHandle.Path != null && mMouseHandle.Path.IsVisible(location))
									{
										curve.ControlPoints[index] = new PointF(actionCurve.ControlPoints[index].X + dx, actionCurve.ControlPoints[index].Y + dy);
										break;
									}
									index ++;
								}
							}
						}
						else if (element is Line)
						{
							Line line = (Line) element;
							Line actionline = (Line) line.ActionElement;

							if (CurrentMouseElements.MouseStartOrigin == actionline.Start && actionline.Start.AllowMove)
							{
								if (CheckBounds(actionline.FirstPoint, dx, dy, actionline.Container))
								{
									line.Start.SuspendEvents = true;
									line.Start.Location = actionline.FirstPoint; //Resets the location
									line.Start.Move(dx,dy);
									line.Start.SuspendEvents = false;
								}
							}
							if (CurrentMouseElements.MouseStartOrigin == actionline.End && actionline.End.AllowMove)
							{
								if (CheckBounds(actionline.LastPoint, dx, dy, actionline.Container))
								{
									line.End.SuspendEvents = true;
									line.End.Location = actionline.LastPoint; //Resets the location
									line.End.Move(dx,dy);
									line.End.SuspendEvents = false;
								}
							}
						}

						//Update docking
						if (CurrentMouseElements.MouseMoveElement != null && IsModelDockable() && Runtime.CanDock(CurrentMouseElements))
						{
							Line line = (Line) element;
							Line actionline = (Line) line.ActionElement;

							if (CurrentMouseElements.MouseMoveElement is Shape)
							{
								if (CurrentMouseElements.MouseStartOrigin == actionline.Start && actionline.Start.AllowMove)
								{
									line.Start.SuspendEvents = true;
									line.Start.Shape = (Shape) CurrentMouseElements.MouseMoveElement;
									line.Start.SuspendEvents = false;
								}
								if (CurrentMouseElements.MouseStartOrigin == actionline.End && actionline.End.AllowMove)
								{
									line.End.SuspendEvents = true;
									line.End.Shape = (Shape) CurrentMouseElements.MouseMoveElement;
									line.End.SuspendEvents = false;
								}
							}
							else if (CurrentMouseElements.MouseMoveElement is Port)
							{
								if (CurrentMouseElements.MouseStartOrigin == actionline.Start && actionline.Start.AllowMove)
								{
									line.Start.SuspendEvents = true;
									line.Start.Port = (Port) CurrentMouseElements.MouseMoveElement;
									line.Start.SuspendEvents = false;
								}
								if (CurrentMouseElements.MouseStartOrigin == actionline.End && actionline.End.AllowMove)
								{
									line.End.SuspendEvents = true;
									line.End.Port = (Port) CurrentMouseElements.MouseMoveElement;
									line.End.SuspendEvents = false;
								}
							}
						}
						
						Line clone = (Line) element;
						clone.DrawPath(); //Update the action path
					}
				}
			}
		}

		//Loop through and rotate the elements in the action renderlist
		protected virtual void RotateElements()
		{
			//Calculate the angle between the start location and the current location
			float dx = (mLastPoint.X - mStartPoint.X) * Render.ZoomFactor; //distance cursor has moved
			float dy = (mLastPoint.Y - mStartPoint.Y) * Render.ZoomFactor;
			float degrees = Convert.ToInt32(Math.Atan2(dy,dx) * (180 / Math.PI)) + 90;

			if (Route != null) Route.Reform();

			//Function will return -180 to 180
			if (degrees < 0) degrees += 360;

			//Snap to orientations
			if (degrees > 355 || degrees < 5) degrees = 0;
			if (degrees > 85 && degrees < 95) degrees = 90;
			if (degrees > 175 && degrees < 185) degrees = 180;
			if (degrees > 265 && degrees < 275) degrees = 270;

			foreach (Element element in RenderDesign.Actions)
			{
				if (element.Visible)
				{
					//Rotate transformable
					if (element is ITransformable && element is Shape)
					{
						Shape shape = (Shape) element;
						if (shape.AllowRotate) shape.Rotation = degrees;
					}
				}
			}

			//Calculate the vector between the center of the rotated element and the mouse pointer
			if (CurrentMouseElements.MouseStartElement is Shape)
			{
				Shape shape = (Shape) CurrentMouseElements.MouseStartElement;
				PointF center = new PointF(shape.Center.X * Render.ScaleFactor, shape.Center.Y * Render.ScaleFactor);

				center.X += RenderPaged.PagedOffset.X;
				center.Y += RenderPaged.PagedOffset.Y;

				PointF client = PointToClient(MousePosition);
				client.X -= DisplayRectangle.X;
				client.Y -= DisplayRectangle.Y;
				
				RenderDesign.Vector = new RectangleF(center, new SizeF(client.X - center.X, client.Y - center.Y));			
			}
			
		}
		
		//Loop through and apply the locations of the elements in the action renderlist
		protected virtual void UpdateElements()
		{
			//Add all affected items to undo list by invalidating them
			if (!UndoList.Suspended) 
			{
				foreach (Element element in RenderDesign.Actions)
				{
					//Only include updated action elements
					if (element.Visible && element.ActionElement != null)
					{
						string description = "Scale ";
						if (mMouseHandle != null && mMouseHandle.Type == HandleType.Move) description = "Move ";
						
						description += (element.ActionElement is Shape) ? "Shape" : "Line";

						SaveStatus();

						SetStatus(Status.UndoRedo);
						UndoList.Add(element.ActionElement, UndoAction.Edit, description);

						RestoreStatus();
					}
				}
			}

			//Suspend undo/redo
			UndoList.Suspend();

			foreach (Element element in RenderDesign.Actions)
			{
				if (element.Visible)
				{
					element.SuspendEvents = true; 

					if (element is Shape)
					{
						Shape shape = (Shape) element;
						Shape actionShape = (Shape) element.ActionElement;						
					
						//Round values if appropriate
						if (Runtime.RoundPixels)
						{
							shape.Location = Point.Round(shape.Location);
							shape.Size = System.Drawing.Size.Round(shape.Size);
						}

						//Move and scale. Shape size property does not check equality
						if (!actionShape.Location.Equals(shape.Location)) actionShape.Location = shape.Location; //new PointF(shape.X,shape.Y);
						if (!actionShape.Size.Equals(shape.Size)) actionShape.Size = shape.Size;

						//Update children of a complex shape
						if (shape is ComplexShape)
						{
							ComplexShape complex = (ComplexShape) shape;
						
							foreach (SolidElement solid in complex.Children.Values)
							{
								SolidElement actionSolid = (SolidElement) solid.ActionElement;
								actionSolid.SetPath(solid.GetPath());
								actionSolid.SetRectangle(solid.Location);
								actionSolid.SetTransformRectangle(solid.Location);
							}
						}
					}

					//Update rotation
					if (element is ITransformable)
					{
						ITransformable transform = (ITransformable) element;
						ITransformable actionTransform = (ITransformable) element.ActionElement;

						if (actionTransform.Rotation != transform.Rotation) actionTransform.Rotation = transform.Rotation;
					}

					if (element is Port)
					{
						Port port = (Port) element;
						Port actionPort = (Port) element.ActionElement;		
				
						//Move and scale. Port size property does not check equality
						if (!actionPort.Location.Equals(port.Location)) actionPort.Location = port.Location; //new PointF(port.X,port.Y);

						//Update the port percentage
						IPortContainer ports = (IPortContainer) actionPort.Parent;
						ports.GetPortPercentage(actionPort, actionPort.Location);
					}

					//Update the locations of the line origins
					if (element is Line)
					{
						Line clone = (Line) element;
						clone.Start.RemoveHandlers();
						clone.End.RemoveHandlers();

						//Undock any origins
						if (CurrentMouseElements.MouseStartOrigin != null && CurrentMouseElements.MouseStartOrigin.Docked && CurrentMouseElements.MouseStartOrigin.AllowMove)
						{
							Origin origin = CurrentMouseElements.MouseStartOrigin;
							if (origin == origin.Parent.Start) origin.Location = origin.Parent.FirstPoint;
							if (origin == origin.Parent.End) origin.Location = origin.Parent.LastPoint;
						}

						if (element is ComplexLine)
						{
							ComplexLine complexLine = (ComplexLine) element;
							ComplexLine actionLine = (ComplexLine) element.ActionElement;

							if (complexLine.Segments.Count == actionLine.Segments.Count)
							{
								Segment segment = null;
								Segment actionSegment = null;

								for (int i = 0; i < complexLine.Segments.Count; i++)
								{
									segment = complexLine.Segments[i];
									actionSegment = actionLine.Segments[i];
									if (!actionSegment.Start.Docked) actionSegment.Start.Location = segment.Start.Location;
								}
                                
								//Update end of last segment
								if (segment != null && actionSegment != null && !actionSegment.End.Docked) actionSegment.End.Location = segment.End.Location;
							
								actionLine.DrawPath();
                                actionLine.LocatePorts();
							}
						}
						else if (element is Curve)
						{
							Curve curve = (Curve) element;
							Curve actionCurve = (Curve) element.ActionElement;						

							if (!actionCurve.Start.Docked) actionCurve.Start.Location = curve.Start.Location;
							if (!actionCurve.End.Docked) actionCurve.End.Location = curve.End.Location;
						
							actionCurve.ControlPoints = curve.ControlPoints;
							actionCurve.DrawPath();
						}
						else if (element is Connector)
						{
							//Update connector oblong handle
							if (mMouseHandle.Type == HandleType.UpDown || mMouseHandle.Type == HandleType.LeftRight)
							{
								Connector connectorLine = (Connector) element;
								Connector actionLine = (Connector) element.ActionElement;
	
								//Get the two points of the segment
								ConnectorHandle handle = (ConnectorHandle) mMouseHandle;
								if (handle != null)
								{
									actionLine.Points[handle.Index-1] = (PointF) connectorLine.Points[handle.Index -1];
									actionLine.Points[handle.Index] = (PointF) connectorLine.Points[handle.Index];
									actionLine.RefinePoints();
									actionLine.DrawPath();
                                    actionLine.LocatePorts();
								}
							}
							//Update start or end of connector
							else if (mMouseHandle.Type == HandleType.Origin)
							{
								Connector connectorLine = (Connector) element;
								Connector actionLine = (Connector) element.ActionElement;

								actionLine.SetPoints(connectorLine.Points);
								actionLine.RefinePoints();

								//Set origins
								if (!actionLine.Start.Docked) actionLine.Start.Location = connectorLine.FirstPoint;
								if (!actionLine.End.Docked) actionLine.End.Location = connectorLine.LastPoint;

								actionLine.GetPortPercentages();
								actionLine.DrawPath();
								actionLine.LocatePorts();
							}
							//Move all points if connector is not connected
							else if (mMouseHandle.Type == HandleType.Move)
							{
								Connector connectorLine = (Connector) element;
								Connector actionLine = (Connector) element.ActionElement;
								
								if (actionLine.AllowMove && !actionLine.Start.Docked && !actionLine.End.Docked)
								{
									actionLine.Points.Clear();
									
									foreach (PointF point in connectorLine.Points)
									{
										actionLine.Points.Add(point);
									}

									actionLine.DrawPath();
                                    actionLine.LocatePorts();
								}
							}
						}
						else
						{
							Line line = (Line) element;
							Line actionLine = (Line) element.ActionElement;						

							//Round values if appropriate
							if (Runtime.RoundPixels)
							{
								line.Start.Location = Point.Round(line.Start.Location);
								line.End.Location = Point.Round(line.End.Location);
							}
						
							if (!actionLine.Start.Docked) actionLine.Start.Location = line.Start.Location;
							if (!actionLine.End.Docked) actionLine.End.Location = line.End.Location;
						
							actionLine.DrawPath();
						}
					}

					if (element is Port)
					{
						Port actionPort = (Port) element;
						Port port = (Port) actionPort.ActionElement;
					
						port.Location = actionPort.Location;
					}

					element.SuspendEvents = false;
				}
			}

			//Update the line docking
			if (CurrentMouseElements.MouseStartOrigin != null && CurrentMouseElements.MouseStartOrigin.AllowMove && CurrentMouseElements.MouseMoveElement != null && IsModelDockable() && Runtime.CanDock(CurrentMouseElements))
			{
				Line line = (Line) CurrentMouseElements.MouseStartElement;
				
				//Dock start to shape
				if (CurrentMouseElements.MouseStartOrigin == line.Start && CurrentMouseElements.MouseMoveElement is Shape)
				{
					line.Start.Shape = (Shape) CurrentMouseElements.MouseMoveElement;
				}
				//Dock end to shape
				if (CurrentMouseElements.MouseStartOrigin == line.End && CurrentMouseElements.MouseMoveElement is Shape)
				{
					line.End.Shape = (Shape) CurrentMouseElements.MouseMoveElement;
				}
				//Dock start to port
				if (CurrentMouseElements.MouseStartOrigin == line.Start && CurrentMouseElements.MouseMoveElement is Port)
				{
					line.Start.Port = (Port) CurrentMouseElements.MouseMoveElement;
				}
				//Dock end to port
				if (CurrentMouseElements.MouseStartOrigin == line.End && CurrentMouseElements.MouseMoveElement is Port)
				{
					line.End.Port = (Port) CurrentMouseElements.MouseMoveElement;
				}
			}

			//Remove all references to cloned objects
			foreach (Element element in RenderDesign.Actions)
			{
				element.ActionElement = null;
			}

			UndoList.Resume();
		}

        protected virtual void CancelAction()
        {
			//Reset highlights
			RenderDesign.Highlights = new RenderList();
	
			if (CurrentMouseElements.MouseStartElement == null)
			{
				//Reset the selection rectangle decoration
				if (!RenderDesign.SelectionRectangle.IsEmpty)
				{
					RenderDesign.SelectionRectangle = new Rectangle();
					OnCancelDragSelect();
				}
			}
			else
			{
				//Moves the elements according to the renderlist
				if (RenderDesign.Actions != null)
				{
					//Unhides the action elements
					if (Component.Instance.HideActions) ShowActionElements(RenderDesign.Actions);
					
					Invalidate();

					OnCancelActions(RenderDesign.Actions);
					RenderDesign.Actions = null;
					RenderDesign.Feedback = null;
					RenderDesign.FeedbackLocation = new Point();
					RenderDesign.Vector = new RectangleF();
					RenderDesign.Unlock();
				}
			}

            Cursor = Component.Instance.GetCursor(HandleType.None);
            CancelTooltip();
            Invalidate();
            mHandled = true;
        }
		private void HideActionElements(RenderList actions)
		{
			//Suspend undo/redo
			UndoList.Suspend();

			foreach (Element element in actions)
			{
				if (element.ActionElement.Visible && element.Visible) 
				{
					element.ActionElement.Visible = false;
				}
			}

			UndoList.Resume();
		}

		private void ShowActionElements(RenderList actions)
		{
			//Suspend undo/redo
			UndoList.Suspend();

			foreach (Element element in actions)
			{
				if (!element.ActionElement.Visible && element.Visible) 
				{
					element.ActionElement.Visible = true;
				}
			}

			UndoList.Resume();
		}

		private void UpdateInteractiveElements()
		{
			if (RenderDesign.Actions == null) return;

			IContainer container = this;

			//Add the elements to the shapes or lines collections and update as usual
			foreach (Element element in RenderDesign.Actions)
			{
				if (element.Visible)
				{
					//Reset temporary layer so that the element can be added normally
					element.ActionElement.SetLayer(null);

					//Determine the correct container from the starting object
					if (element is Line)
					{
						Line line = (Line) element;
						if (line.Start.DockedElement != null) container = line.Start.DockedElement.Container;

						if (Runtime.CanAdd(element)) container.Lines.Add(container.Lines.CreateKey(), element.ActionElement);	
					}
				
					if (element is Shape && Runtime.CanAdd(element)) container.Shapes.Add(container.Shapes.CreateKey(), element.ActionElement);
					
				}
			}

			UpdateElements();
		}

		private bool CheckBounds(float dx, float dy, IContainer container)
		{
			//Return pass if the diagram does not have boundary checking
			if (!base.CheckBounds) return true;

			foreach (Element element in RenderDesign.Actions)
			{
				if (element.Visible)
				{
					Element action = element.ActionElement;

					//If the original (action) is contained in the container
					if (container.Contains(new PointF(action.Rectangle.X + container.Offset.X, action.Rectangle.Y + container.Offset.Y),container.Margin) && container.Contains(new PointF(action.Rectangle.Right + container.Offset.X,action.Rectangle.Bottom + container.Offset.Y),container.Margin))
					{
						//Check top left
						if (! container.Contains(new PointF(element.Rectangle.X + dx + container.Offset.X, element.Rectangle.Y + dy + container.Offset.Y),container.Margin)) return false;
						
						//Check bottom right
						if (! container.Contains(new PointF(element.Rectangle.Right + dx + container.Offset.X,element.Rectangle.Bottom + dy + container.Offset.Y),container.Margin)) return false;
					}
				}
			}
			return true;
		}

		private bool CheckBounds(Element element, IContainer container)
		{
			if (element.Visible)
			{
				Element action = element.ActionElement;

				//If the original (action) is contained in the container
				if (container.Contains(new PointF(action.Rectangle.X + container.Offset.X, action.Rectangle.Y + container.Offset.Y),container.Margin) && container.Contains(new PointF(action.Rectangle.Right + container.Offset.X,action.Rectangle.Bottom + container.Offset.Y),container.Margin))
				{
					//Check top left
					if (!container.Contains(new PointF(element.Rectangle.X + container.Offset.X, element.Rectangle.Y + container.Offset.Y),container.Margin)) return false;
						
					//Check bottom right
					if (! container.Contains(new PointF(element.Rectangle.Right + container.Offset.X, element.Rectangle.Bottom + container.Offset.Y),container.Margin)) return false;
				}
			}

			return true;
		}

		private bool CheckBounds(PointF action, float dx, float dy, IContainer container)
		{
			//Return pass if the diagram does not have boundary checking
			if (!base.CheckBounds) return true;


			//If the original (action) is contained in the container
			if (container.Contains(new PointF(action.X + container.Offset.X, action.Y + container.Offset.Y), container.Margin))
			{
				//Check top left
				if (! container.Contains(new PointF(action.X + dx + container.Offset.X, action.Y + dy + container.Offset.Y),container.Margin)) return false;
			}
			
			return true;
		}

		//Return an aligned screen point
		protected virtual Point AlignMouseCoordinates(MouseEventArgs e)
		{
			Point point = new Point();
			SizeF gridSize = GridSize;
			Size zoomSize = new Size(Convert.ToInt32(gridSize.Width * Render.ScaleFactor),Convert.ToInt32(gridSize.Height * Render.ScaleFactor));
			
			if (zoomSize.Width < 1) zoomSize.Width = 1;
			if (zoomSize.Height < 1) zoomSize.Height = 1;

			point = Point.Round(new PointF(e.X / zoomSize.Width, e.Y / zoomSize.Height));
			point = new Point(point.X * zoomSize.Width,point.Y * zoomSize.Height);

			return point;
		}

		//Determines if an element is alignable
		private bool IsAlignable(Element element)
		{
			//Always true of align grid is on
			if (AlignGrid) return true;

			//If doesnt implement the IUserInteractive interface then false
			if (! (element is IUserInteractive)) return false;

			//Check the multiple flag enumeration to see if align to grid is set
			IUserInteractive interact = (IUserInteractive) element;
			return ((interact.Interaction & UserInteraction.AlignToGrid) == UserInteraction.AlignToGrid);
		}

		private bool IsOrderable(Element element)
		{
			//If doesnt implement the IUserInteractive interface then false
			if (! (element is IUserInteractive)) return false;

			//Check the multiple flag enumeration to see if alibring to front is set
			IUserInteractive interact = (IUserInteractive) element;
			return ((interact.Interaction & UserInteraction.BringToFront) == UserInteraction.BringToFront);
		}

		private void CheckDragScroll()
		{
			//Only drag scroll if moving elements
			if (mMouseHandle == null) return;
			if (Render == null) return;
			if (mMouseHandle.Type != HandleType.Move) return;

			SaveStatus();
			SetStatus(Status.DragScroll);

			Point autoScrollPoint = new Point(AutoScrollPosition.X * -1, AutoScrollPosition.Y * -1);
			int dx = 0;
			int dy = 0;

			if (mLastPoint.X > Width) dx = Component.Instance.DragScrollAmount;
			if (mLastPoint.X < 0 && autoScrollPoint.X > 0) dx = -Component.Instance.DragScrollAmount;
			if (mLastPoint.Y > Height) dy = Component.Instance.DragScrollAmount;
			if (mLastPoint.Y < 0 && autoScrollPoint.Y > 0) dy = -Component.Instance.DragScrollAmount;

			if (dx != 0 || dy != 0)
			{
				float dxZoom = dx * Render.ZoomFactor;
				float dyZoom = dy * Render.ZoomFactor;

				//Prevent control from repainting
				Suspend();

				//Get the renderer
				Render render = (Render) Render;
				
				//Unlock the renderer so that the elements can be re-rendered
				render.Unlock();

				//Offset all the action elements by the amount of the drag scroll
				OffsetElements(dxZoom,dyZoom);

				//Offset the scrollable control base autoscrollposition by the drag scroll
				autoScrollPoint.X += dx;
				autoScrollPoint.Y += dy;
				base.AutoScrollPosition = autoScrollPoint;

				//Update the internal render rectangles and re-render
				//Must not render decoration path
				//Initial scroll rectangle must equal invalidate rectangle so that not destroyed by renderer
				render.DrawDecorations = false;
				SetScrollRectangles();
				Resume();
				Invalidate();
				render.DrawDecorations = true;
				render.Lock();

				//Allow the control to paint itself and repaint
				Invalidate();
			}

			RestoreStatus();
		}

		private void DoCommandImplementation(string command, PointF location)
		{
			//Make comand lower case
			command = command.ToLower();
			Cursor currentCursor = Cursor;

			//Set cursor
			Cursor = Cursors.WaitCursor;

			switch (command)
			{
				case "undo":

					UndoList.Suspend();
					Suspend();
					UndoList.Undo(this);
					SelectElements(false,this);
					Resume();
					Refresh();
					UndoList.Resume();

					break;

				case "redo":

					UndoList.Suspend();
					Suspend();
					UndoList.Redo(this);
					SelectElements(false,this);
					Resume();
					Refresh();
					UndoList.Resume();

					break;

				case "cut":
					DoCutCopyDelete(true, true);
					mClipboard.IsCopy = false;
					break;
				
				case "copy":
					DoCutCopyDelete(true, false);
					mClipboard.IsCopy = true;
					break;
				
				case "delete":
					DoCutCopyDelete(false, true);
					Refresh();
					break;
				
				case "paste":
					Suspend();
					IContainer container = GetContainer();
					SelectElements(false);
					ReadFromClipboard();
					ResolveClipboardItems();
					DoPaste(container, location);
					mClipboard.IsCopy = true;
					Resume();
					Refresh();
					break;

				case "bold":
					Suspend();
					DoCommandText(true,false,false,false);
					Resume();
					Invalidate();
					break;
				case "italic":
					Suspend();
					DoCommandText(false,true,false,false);
					Resume();
					Invalidate();
					break;
				case "strikeout":
					Suspend();
					DoCommandText(false,false,true,false);
					Resume();
					Invalidate();
					break;
				case "underline":
					Suspend();
					DoCommandText(false,false,false,true);
					Resume();
					Invalidate();
					break;
				case "align left":
					Suspend();
					DoCommandAlign(true,false,false);
					Resume();
					Invalidate();
					break;
				case "align center":
					Suspend();
					DoCommandAlign(false,true,false);
					Resume();
					Invalidate();
					break;
				case "align right":
					Suspend();
					DoCommandAlign(false,false,true);
					Resume();
					Invalidate();
					break;
				case "align top":
					Suspend();
					DoCommandVerticalAlign(true,false,false);
					Resume();
					Invalidate();
					break;
				case "align middle":
					Suspend();
					DoCommandVerticalAlign(false,true,false);
					Resume();
					Invalidate();
					break;
				case "align bottom":
					Suspend();
					DoCommandVerticalAlign(false,false,true);
					Resume();
					Invalidate();
					break;
			}

			//Reset cursor
			Cursor = currentCursor;
		}

		private void DoCommandImplementation(string command, string value)
		{
			//Make comand lower case
			command = command.ToLower();

			switch (command)
			{
				case "font":
					Suspend();
					DoCommandFont(value);
					Resume();
					Invalidate();
					break;
				case "font size":
					Suspend();
					DoCommandFontSize(value);
					Resume();
					Invalidate();
					break;
			}
		}

		private bool GetCommandImplementation(string command)
		{
			//Make comand lower case
			command = command.ToLower();

			switch (command)
			{
				case "undo":
					return (UndoList.UndoPointer > 0);
					break;
				case "redo":
					return (UndoList.UndoPointer < UndoList.Count - 1);
					break;
				case "cut":
					return (SelectedElements().Count  > 0);
					break;
				case "copy":
					return (SelectedElements().Count > 0);
					break;
				case "delete":
					return (SelectedElements().Count > 0);
					break;
				case "paste": 
					return (mClipboard.Elements != null);
					break;
				case "bold":
					return GetCommandTextAvailable();
					break;
				case "italic":
					return GetCommandTextAvailable();
					break;
				case "strikeout":
					return GetCommandTextAvailable();
					break;
				case "underline":
					return GetCommandTextAvailable();
					break;
				case "bold status":
					return GetCommandTextStatus(true,false,false,false);
					break;
				case "italic status":
					return GetCommandTextStatus(false,true,false,false);
					break;
				case "strikeout status":
					return GetCommandTextStatus(false,false,true,false);
					break;
				case "underline status":
					return GetCommandTextStatus(false,false,false,true);
					break;
				case "align left":
					return GetCommandTextAvailable();
					break;
				case "align center":
					return GetCommandTextAvailable();
					break;
				case "align right":
					return GetCommandTextAvailable();
					break;
				case "align left status":
					return GetAlignStatus(true,false,false);
					break;
				case "align center status":
					return GetAlignStatus(false,true,false);
					break;
				case "align right status":
					return GetAlignStatus(false,false,true);
					break;
				case "align top":
					return GetCommandTextAvailable();
					break;
				case "align middle":
					return GetCommandTextAvailable();
					break;
				case "align bottom":
					return GetCommandTextAvailable();
					break;
				case "align top status":
					return GetVerticalAlignStatus(true,false,false);
					break;
				case "align middle status":
					return GetVerticalAlignStatus(false,true,false);
					break;
				case "align bottom status":
					return GetVerticalAlignStatus(false,false,true);
					break;
				case "read clipboard":
					ReadFromClipboard();
					return true;
				case "write clipboard":
					WriteToClipBoard();
					return true;
			}

			return false;
		}

		private string GetCommandStringImplementation(string command)
		{
			//Make comand lower case
			command = command.ToLower();

			switch (command)
			{
				case "font status":
					return GetFontNameStatus();
					break;
				case "font size status":
					return GetFontSizeStatus();
					break;
			}

			return "";
		}

		private void SetTooltip(Element element)
		{
			CancelTooltip();

			if (element.Tooltip != null && element.Tooltip != string.Empty)
			{
				mTooltip = new ToolTip();
				mTooltip.SetToolTip(this, element.Tooltip);
				mTooltip.Active = true;
			}
		}

		//Can be called via reflection to remove a tooltip
		private void CancelTooltip()
		{
			if (mTooltip != null)
			{
				mTooltip.Active = false;
				mTooltip.Dispose();
				mTooltip = null;
			}
		}

		//Loop through and add if cut or copy  selected shapes, lines with either shape selected, selected lines 
		//Remove if cut or delete 
		//Do not rewrite keys in copy buffer
		private void DoCutCopyDelete(bool add, bool remove)
		{
			ArrayList shapes = new ArrayList();
			ArrayList lines = new ArrayList();

			mClipboard.Elements = new Elements();

			Suspend();

			//Clear the selection cache lists
			mSelectedElements = null;
			mSelectedShapes = null;
			mSelectedLines = null;

			//Add any cut/copied shapes to a remove list
			DoCutCopyDeleteSelectedShapes(add, remove, Shapes, shapes);
			DoCutCopyDeleteSelectedLines(add,remove, Lines, lines);
			
			//Check for elements in a container
			if (shapes.Count == 0 && lines.Count ==0)
			{
				//Check groups
				foreach (Shape shape in Shapes.Values)
				{
					if (shape is Group)
					{
						Group group = (Group) shape;
						
						DoCutCopyDeleteSelectedShapes(add, remove, group.Shapes, shapes);
						if (shapes.Count > 0) break;

						DoCutCopyDeleteSelectedLines(add, remove, group.Lines, lines);
						if (lines.Count > 0) break;
					}
				}
			}

			//Write the items to the clipboard before they are removed and change the origins etc
			WriteToClipBoard();

			//Remove any shapes from the collection
			if (remove)
			{
				foreach (Shape shape in shapes)
				{
					shape.Container.Shapes.Remove(shape.Key);
				}

				foreach (Line line in lines)
				{
					line.Container.Lines.Remove(line.Key);
				}
			}

			Resume();
			Refresh();
		}

		//Loop through each item in the copycut buffer and add it back to the model
		//Paste into centre of container
		private void DoPaste(IContainer container, PointF location)
		{
			if (mClipboard.Elements == null) return;

			//Create a renderlist and add the elements
			RenderList list = new RenderList();
			foreach (Element element in mClipboard.Elements.Values)
			{
				list.Add(element);
			}

			//Get the bounds
			RectangleF bounds = list.GetBounds();

			//Calculate
			float dx = -bounds.X + ((container.ContainerSize.Width - bounds.Width) / 2);
			float dy = -bounds.Y + ((container.ContainerSize.Height - bounds.Height) / 2);

			dx = Convert.ToSingle(Math.Round(dx,0));
			dy = Convert.ToSingle(Math.Round(dy,0));

			//Loop through each element and add
			if (mClipboard.Elements != null)
			{
				//Add Shapes
				foreach (Element element in mClipboard.Elements.Values)
				{
					if (element is Shape && (! (element is IContainer && container is Group)))
					{
						Shape shape = (Shape) element;
						Shape newShape = (Shape) shape.Clone();
						string key = null;

						//Set key
						key = (container.Shapes.Contains(shape.Key)) ? container.Shapes.CreateKey(): shape.Key;

						//Set temporary action element
						shape.ActionElement = newShape;

						//Change any settings required
						newShape.SetLayer(null);
						newShape.Selected = true;

						//Offset by container offset
						newShape.Move(dx,dy);

						//Round values if appropriate
						if (Runtime.RoundPixels)
						{
							newShape.Location = Point.Round(newShape.Location);
							newShape.Size = System.Drawing.Size.Round(newShape.Size);
						}

						//Add if allowed by the runtime
						if (Runtime.CanAdd(newShape)) container.Shapes.Add(key,newShape);
					}
				}

				//Add Lines
				foreach (Element element in mClipboard.Elements.Values)
				{
					if (element is Line)
					{
						Line line = (Line) element;
						Line newLine = (Line) line.Clone();
						string key = null;

						//Define the key
						key = (container.Lines.Contains(line.Key)) ? container.Lines.CreateKey(): key = line.Key;

						//Change any settings required
						newLine.SetLayer(null);
						newLine.Selected = true;

						if (element is ComplexLine)
						{
							ComplexLine complexLine = (ComplexLine) newLine;
							Segment segment = null;

							for (int i=0; i<complexLine.Segments.Count; i++)
							{
								segment = complexLine.Segments[i];
								segment.Start.Move(dx,dy);								
							}
						}
						else if (element is Curve)
						{
							Curve curve = (Curve) newLine;
							
							curve.Start.Move(dx,dy);
							curve.End.Move(dx,dy);
							
							PointF[] newPoints = new PointF[curve.ControlPoints.GetUpperBound(0)+1];
							for(int i=0; i<=curve.ControlPoints.GetUpperBound(0); i++)
							{
								newPoints[i] = new PointF(curve.ControlPoints[i].X + dx,curve.ControlPoints[i].Y + dy);
							}
							curve.ControlPoints = newPoints;
						}
						else if (element is Connector) //Connectors cannot be moved in this way
						{
							Connector conn = (Connector) newLine;
							ArrayList points = new ArrayList();
                            
							foreach (PointF point in conn.Points)
							{
								points.Add(new PointF(point.X + dx, point.Y + dy));
							}
							conn.SetPoints(points);
							conn.DrawPath();
						}
						else
						{
							newLine.Start.Move(dx,dy);
							newLine.End.Move(dx,dy);
							newLine.DrawPath();
						}

						//Reconnect start origins
						if (line.Start.DockedElement != null)
						{
							if (line.Start.Shape != null && line.Start.Shape.ActionElement != null)
							{
								newLine.Start.Shape = (Shape) line.Start.Shape.ActionElement; 
							}
							else if (line.Start.Port != null && line.Start.Port.Parent is Shape)
							{
								Shape shape = (Shape) line.Start.Port.Parent;
								Shape newShape = (Shape) shape.ActionElement;
								
								newLine.Start.Port = (Port) newShape.Ports[line.Start.Port.Key];
							}
						}

						//Reconnect end origins
						if (line.End.DockedElement != null)
						{
							if (line.End.Shape != null && line.End.Shape.ActionElement != null)
							{
								newLine.End.Shape = (Shape) line.End.Shape.ActionElement; 
							}
							else if (line.End.Port != null && line.End.Port.Parent is Shape)
							{
								Shape shape = (Shape) line.End.Port.Parent;
								Shape newShape = (Shape) shape.ActionElement;
								
								newLine.End.Port = (Port) newShape.Ports[line.End.Port.Key];
							}
						}

						//Create and add to lines collection
						if (Runtime.CanAdd(newLine)) container.Lines.Add(key,newLine);
					}
				}

				//Remove any temporary action elements
				foreach (Element element in mClipboard.Elements.Values)
				{
					element.ActionElement = null;
				}
			}
		}

		private void DoCutCopyDeleteSelectedShapes(bool add, bool remove, Elements collection, ArrayList shapes)
		{
			//Add any cut/copied shapes to a remove list
			foreach (Shape shape in collection.Values)
			{
				if (shape.Selected)
				{
					if (add) mClipboard.Elements.Add(shape.Key, shape);
					if (remove && Runtime.CanDelete(shape)) shapes.Add(shape);
				}
			}
		}

		private void DoCutCopyDeleteSelectedLines(bool add, bool remove, Elements collection, ArrayList lines)
		{
			//Add any cut/copied lines 
			foreach (Line line in collection.Values)
			{
				//Set line origin to location to avoid cut/copy reference problems
				//if (line.Start.DockedElement != null && ! shapes.Contains(line.Start.DockedElement)) line.Start.Location = line.FirstPoint;
				//if (line.End.DockedElement != null && ! shapes.Contains(line.End.DockedElement)) line.End.Location = line.LastPoint;
				
				if (line.Selected)
				{
					if (remove && Runtime.CanDelete(line)) lines.Add(line);
					if (add) mClipboard.Elements.Add(line.Key, line);
				}
			}

		}

		private void DoCommandText(bool bold, bool italic, bool strikeout, bool underline)
		{
			Elements elements = SelectedShapes();
			
			bool boldflag =  false;
			bool italicflag = false;
			bool strikeflag = false;
			bool underlineflag = false;

			//Get settings becuase may be mixed mode
			if (bold) boldflag = !GetCommand("bold status");
			if (italic) italicflag = !GetCommand("italic status");
			if (strikeout) strikeflag = !GetCommand("strike status");
			if (underline) underlineflag = !GetCommand("underline status");

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					Label label = shape.Label;
					
					if (bold) label.Bold = boldflag;
					if (italic) label.Italic = italicflag;
					if (strikeout) label.Strikeout = strikeflag;
					if (underline) label.Underline = underlineflag;
				}
			}
		}

		private bool GetCommandTextStatus(bool bold, bool italic, bool strikeout, bool underline)
		{
			Elements elements = SelectedShapes();

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					Label label = shape.Label;
					
					if (bold && !label.Bold) return false;
					if (italic && !label.Italic) return false;
					if (strikeout && !label.Strikeout) return false;
					if (underline && !label.Underline) return false;
				}
			}

			return true;
		}

		private void DoCommandAlign(bool left, bool center, bool right)
		{
			Elements elements = SelectedShapes();
			
			bool leftflag =  false;
			bool centerflag = false;
			bool rightflag = false;

			//Get settings becuase may be mixed mode
			if (left) leftflag = !GetCommand("align left status");
			if (center) centerflag = !GetCommand("align center status");
			if (right) rightflag = !GetCommand("align right status");

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					TextLabel label = shape.Label;
					
					if (left && leftflag) label.Alignment = StringAlignment.Near;
					if (center && centerflag) label.Alignment = StringAlignment.Center;
					if (right && rightflag) label.Alignment = StringAlignment.Far;
				}
			}
		}

		private bool GetAlignStatus(bool left, bool center, bool right)
		{
			Elements elements = SelectedShapes();

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					TextLabel label = shape.Label;
					
					if (left && label.Alignment != StringAlignment.Near) return false;
					if (center && label.Alignment != StringAlignment.Center) return false;
					if (right && label.Alignment != StringAlignment.Far) return false;
				}
			}

			return true;
		}

		private void DoCommandVerticalAlign(bool top, bool middle, bool bottom)
		{
			Elements elements = SelectedShapes();
			
			bool topflag =  false;
			bool middleflag = false;
			bool bottomflag = false;

			//Get settings becuase may be mixed mode
			if (top) topflag = !GetCommand("align top status");
			if (middle) middleflag = !GetCommand("align middle status");
			if (bottom) bottomflag = !GetCommand("align bottom status");

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					TextLabel label = shape.Label;
					
					if (top && topflag) label.LineAlignment = StringAlignment.Near;
					if (middle && middleflag) label.LineAlignment = StringAlignment.Center;
					if (bottom && bottomflag) label.LineAlignment = StringAlignment.Far;
				}
			}
		}

		private bool GetVerticalAlignStatus(bool top, bool middle, bool bottom)
		{
			Elements elements = SelectedShapes();

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					TextLabel label = shape.Label;
					
					if (top && label.LineAlignment != StringAlignment.Near) return false;
					if (middle && label.LineAlignment != StringAlignment.Center) return false;
					if (bottom && label.LineAlignment != StringAlignment.Far) return false;
				}
			}

			return true;
		}

		//Determines if all selected fonts are the same
		private string GetFontNameStatus()
		{
			Elements elements = SelectedShapes();
			string fontname = "";

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					Label label = shape.Label;
					
					if (fontname != "") if (label.FontName != fontname) return "";
					fontname = label.FontName;
				}
			}

			return fontname;
		}

		private string GetFontSizeStatus()
		{
			Elements elements = SelectedShapes();
			float fontsize = 0;

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					Label label = shape.Label;
					
					if (fontsize != 0) if (label.FontSize != fontsize) return "";
					fontsize = label.FontSize;
				}
			}

			if (fontsize == 0) return "";
			return fontsize.ToString();
		}

		private bool DoCommandFont(string fontname)
		{
			Elements elements = SelectedShapes();

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					Label label = shape.Label;
					label.FontName = fontname;
				}
			}

			return true;
		}

		private void DoCommandFontSize(string fontsize)
		{
			float size = 0;

			//Try convert string size to a single
			try
			{
				size = Convert.ToSingle(fontsize);
			}
			catch
			{

			}

			if (size == 0) return;

			Elements elements = SelectedShapes();

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null)
				{
					Label label = shape.Label;
					label.FontSize = size;
				}
			}
		}

		//Determine if there are any selected elements with text
		private bool GetCommandTextAvailable()
		{
			Elements elements = SelectedShapes();

			foreach (Shape shape in elements.Values)
			{
				if (shape.Label != null) return true;
			}
			return false;
		}

		private void WriteToClipBoard()
		{
			//Register the data format
			DataFormats.Format format = DataFormats.GetFormat(typeof(Elements).FullName);

			IDataObject ido = new DataObject();

			ido.SetData(format.Name, mClipboard.Elements);

			//True also causes immediate serialization, false delayed until it is needed
			Clipboard.SetDataObject(ido, true);
		}

		private void ReadFromClipboard()
		{
			//Get the data format
			DataFormats.Format format = DataFormats.GetFormat(typeof(Elements).FullName);

			IDataObject ido = Clipboard.GetDataObject();

			if (ido.GetDataPresent(format.Name)) mClipboard.Elements = ((Elements)(ido.GetData(format.Name)));
		}

		//When loading from a clipboard, some items may need to be modified due to the way
		//.net added them to the clipboard
		private void ResolveClipboardItems()
		{
			if (mClipboard.Elements != null)
			{
				foreach(Element element in mClipboard.Elements.Values)
				{
					element.SetContainer(Runtime.ActiveContainer);

					//Recreate line points for elements
					if (element is Line)
					{
						Line line = (Line) element;
						line.DrawPath();
					}
				}
			}
		}

		private PointF OffsetDrop(PointF point,RectangleF rectangle)
		{
			PointF offset = PointToDiagram(Convert.ToInt32(point.X),Convert.ToInt32(point.Y));
			return new PointF(offset.X - (rectangle.Width / 2), offset.Y -  (rectangle.Height / 2));
		}

		private IContainer GetContainer()
		{
			//Get the relevant container
			Elements selected = SelectedShapes();

			if (selected.Count == 1)
			{
				foreach (Element element in selected.Values)
				{
					if (element is IContainer) return (IContainer) element;
				}
			}
			return this;
		}

		private void AlignElementLocations(RenderList actions)
		{
			foreach (Element element in actions)
			{
				if (element is Shape)
				{
					Shape shape = (Shape) element;
					PointF location = shape.Location;
					SizeF gridSize = GridSize;

					Point newLocation = Point.Round(new PointF(location.X / gridSize.Width, location.Y / gridSize.Height));
					shape.Location = new PointF(newLocation.X * gridSize.Width,newLocation.Y * gridSize.Height) ;
				}
			}
		}

		private void SetScaleStartingPoint()
		{
			if (CurrentMouseElements.MouseStartElement == null || ! (CurrentMouseElements.MouseStartElement is Shape)) return;
			
			Element element = CurrentMouseElements.MouseStartElement;
			
			//Convert the rectangle to a screen rectangle
			Point topleft = DiagramToPoint(element.Rectangle.Location);
			Point bottomright = DiagramToPoint(element.Rectangle.Right,element.Rectangle.Bottom);
			
			//X Coordinates
			if (mMouseHandle.Type == HandleType.TopLeft  || mMouseHandle.Type == HandleType.Left || mMouseHandle.Type == HandleType.BottomLeft)
			{
				mStartPoint = new Point(topleft.X, mStartPoint.Y);
			}
			else if (mMouseHandle.Type == HandleType.TopRight  || mMouseHandle.Type == HandleType.Right || mMouseHandle.Type == HandleType.BottomRight)
			{
				mStartPoint = new Point(bottomright.X, mStartPoint.Y);
			}

			//Y coordinates
			if (mMouseHandle.Type == HandleType.TopLeft  || mMouseHandle.Type == HandleType.Top || mMouseHandle.Type == HandleType.TopRight)
			{
				mStartPoint = new Point(mStartPoint.X, topleft.Y);
			}
			else if (mMouseHandle.Type == HandleType.BottomLeft  || mMouseHandle.Type == HandleType.Bottom || mMouseHandle.Type == HandleType.BottomRight)
			{
				mStartPoint = new Point(mStartPoint.X, bottomright.Y);
			}

			//Align to grid
			Point newLocation = Point.Round(new PointF(mStartPoint.X / GridSize.Width, mStartPoint.Y / GridSize.Height));
			mStartPoint = Point.Round(new PointF(newLocation.X * GridSize.Width,newLocation.Y * GridSize.Height));
		}

		private void RedrawConnectors()
		{
			foreach (Line line in Lines.Values)
			{
				if (line is Connector)
				{
					Connector connector = (Connector) line;
					if (connector.Jump) connector.DrawPath();
				}
			}
		}

		private void ZoomModel(bool zoomIn)
		{
			if (zoomIn)
			{
				if (Zoom < 200) Zoom = Convert.ToInt32(((Zoom + 25) / 25)) * 25;
			}
			else
			{
				if (Zoom > 25) Zoom = Convert.ToInt32(((Zoom - 25) / 25)) * 25;
			}
		}

		private bool IsModelDockable()
		{
			if (mMouseHandle == null) return false;
			if (!mMouseHandle.CanDock) return false;
			if (CurrentMouseElements.MouseStartElement == null) return false;
			if (CurrentMouseElements.MouseMoveElement == null) return false;

			//Special case for port on a group
			if (CurrentMouseElements.MouseStartElement.Container is Group && CurrentMouseElements.MouseMoveElement is Port)
			{
				Group group = (Group) CurrentMouseElements.MouseStartElement.Container;
				Port port = (Port) CurrentMouseElements.MouseMoveElement;
				if (group.Container == port.Container) return true;
			}
			
			//Check compatible containers
			if (CurrentMouseElements.MouseStartElement.Container != CurrentMouseElements.MouseMoveElement.Container) return false;

			return true;
		}

		#endregion
	}
}
