using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Layouts;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Group: Shape, IExpandable, IContainer, ICloneable, ISerializable, IDeserializationCallback
	{
		//Property variables
		private Elements mShapes;
		private Elements mLines;
		private Margin mMargin;
		private bool mCheckBounds;
		private bool mExpanded;
		private bool mDrawExpand;
		private SizeF mExpandedSize;
		private SizeF mContractedSize;

		//Working variables
		private RenderList mRenderList;
		private GraphicsPath mExpandPath;
		private Route mRoute;
		
		#region Interface
		
		//Events
		public event ExpandedChangedEventHandler ExpandedChanged;
		
		public event ElementsEventHandler ElementInserted;
		public event ElementsEventHandler ElementRemoved;
		
		//Constructors
		public Group()
		{
			SuspendEvents = true;

			DrawBackground = false;
			DrawShadow = false;
			BorderStyle = DashStyle.Dash;
			ExpandedSize = MaximumSize;
			ContractedSize = Size;
			DrawExpand = true;
			Expanded = true;
			CheckBounds = true;
            
			//Set up objects and event handlers
			Shapes = new Elements(typeof(Shape),"Shape");
			Lines = new Elements(typeof(Line),"Line");
			mMargin = new Margin();
			mRenderList = new RenderList();

			SuspendEvents = false;
		}

		public Group(Group prototype): base(prototype)
		{
			Shapes = new Elements(typeof(Shape),"Shape");
			Lines = new Elements(typeof(Line),"Line");
			mRenderList = new RenderList();
			mMargin = prototype.Margin;

			mCheckBounds = prototype.CheckBounds;
			mDrawExpand = prototype.DrawExpand;
			mExpanded = prototype.Expanded;
			mContractedSize = prototype.ContractedSize;
			mExpandedSize = prototype.ExpandedSize;
		}

		//Deserializes info into a new solid element
		protected Group(SerializationInfo info, StreamingContext context): base(info,context)
		{
			mRenderList = new RenderList();

			SuspendEvents = true;
			
			Shapes = (Elements) info.GetValue("Shapes",typeof(Elements));
			Lines = (Elements) info.GetValue("Lines",typeof(Elements));
			Margin = (Margin) info.GetValue("Margin",typeof(Margin));
			
			CheckBounds = info.GetBoolean("CheckBounds");
			DrawExpand = info.GetBoolean("DrawExpand");
			ExpandedSize = Serialize.GetSizeF(info.GetString("ExpandedSize"));
			ContractedSize = Serialize.GetSizeF(info.GetString("ContractedSize"));
			Expanded = info.GetBoolean("Expanded");
			
			SuspendEvents = false;
		}

		//Properties
		//Provides access to the group's shapes collection
		public virtual Elements Shapes
		{
			get
			{
				return mShapes;
			}
			set
			{
				//Remove any existing handlers
				if (mShapes != null)
				{
					mShapes.InsertElement -= new ElementsEventHandler(Element_Insert);
					mShapes.RemoveElement -= new ElementsEventHandler(Element_Remove);
				}

				if (value == null)
				{
					mShapes = new Elements(typeof(Shape),"Shape");
				}
				else
				{
					mShapes = value;
				}

				mShapes.InsertElement += new ElementsEventHandler(Element_Insert);
				mShapes.RemoveElement += new ElementsEventHandler(Element_Remove);

				CreateRenderList();
				OnElementInvalid();
			}
		}

		//Provides access to the group's lines collection
		public virtual Elements Lines
		{
			get
			{
				return mLines;
			}
			set
			{
				//Remove any existing handlers
				if (mLines != null)
				{
					mLines.InsertElement -= new ElementsEventHandler(Element_Insert);
					mLines.RemoveElement -= new ElementsEventHandler(Element_Remove);
				}

				if (value == null)
				{
					mLines = new Elements(typeof(Line),"Line");
				}
				else
				{
					mLines = value;
				}

				mLines.InsertElement += new ElementsEventHandler(Element_Insert);
				mLines.RemoveElement += new ElementsEventHandler(Element_Remove);

				CreateRenderList();
				OnElementInvalid();
			}
		}

		//Returns the internal renderlist
		public virtual RenderList RenderList
		{
			get
			{
				return mRenderList;
			}
		}

		//Sets up the routing (AStar) class instance
		public Route Route
		{
			get
			{
				return mRoute;
			}
			set
			{
				mRoute = value;
				mRoute.Container = Container;
			}
		}

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

		//Returns the container offset for this group equal to the location
		public PointF Offset
		{
			get
			{
				return Location;
			}
		}

		public SizeF ContainerSize
		{
			get
			{
				return Rectangle.Size;
			}
		}

		public virtual bool CheckBounds
		{
			get
			{
				return mCheckBounds;
			}
			set
			{
				mCheckBounds = value;
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

		//Methods
		//Additional contains to the standard override
		public virtual bool Contains(PointF location,bool transparent)
		{
			return GroupContains(location,transparent);
		}

		public virtual bool Contains(PointF location,Margin margin)
		{
			//Offset by group location
			location.X -= Rectangle.X;
			location.Y -= Rectangle.Y;

			return new RectangleF(margin.Left, margin.Top, Size.Width - margin.Right - margin.Left,Size.Height - margin.Bottom - margin.Top).Contains(location);
		}

		protected virtual void OnExpandedChanged()
		{
			if (!SuspendEvents && ExpandedChanged != null) ExpandedChanged(this,Expanded);
		}

		protected virtual void OnElementInserted(Element element)
		{
			if (! SuspendEvents && ElementInserted != null) ElementInserted(this,new ElementsEventArgs(element.Key,element));
		}

		protected virtual void OnElementRemoved(Element element)
		{
			if (! SuspendEvents && ElementRemoved != null) ElementRemoved(this,new ElementsEventArgs(element.Key,element));
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Group(this);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			base.Render (graphics, render);

			if (Expanded)
			{
				Region region = new Region(GetPathInternal());
				Region current = graphics.Clip;
				graphics.SetClip(region,CombineMode.Intersect);

				//Render shadows
				foreach (Element element in mRenderList)
				{
                    GraphicsState graphicsState = graphics.Save();

					if (element.DrawShadow)
					{
						graphics.TranslateTransform(element.Rectangle.X+Layer.ShadowOffset.X ,element.Rectangle.Y+Layer.ShadowOffset.Y);
						element.RenderShadow(graphics,render);
						graphics.TranslateTransform(-element.Rectangle.X-Layer.ShadowOffset.X,-element.Rectangle.Y-Layer.ShadowOffset.Y);
					}

                    graphics.Restore(graphicsState);
				}

				//Draw each element 
				foreach (Element element in mRenderList)
				{
					//Draw shapes
					graphics.TranslateTransform(element.Rectangle.X,element.Rectangle.Y);
					element.Render(graphics,render);
					graphics.TranslateTransform(-element.Rectangle.X,-element.Rectangle.Y);
				}

				//Draw selections
				if (render is IRenderDesign)
				{
					foreach (Element element in mRenderList)
					{
						if (element is ISelectable)
						{
							ISelectable select = (ISelectable) element;
							if (select.Selected && select.DrawSelected)
							{
								graphics.TranslateTransform(element.Rectangle.X,element.Rectangle.Y);
								element.RenderSelection(graphics,render,(IRenderDesign) render);
								graphics.TranslateTransform(-element.Rectangle.X,-element.Rectangle.Y);
							}
						}
					}
				}
				graphics.Clip = current;
			}

			//Draw the expander
			RenderExpand(graphics,render);
		}

		protected internal override void RenderAction(Graphics graphics, IRender render, IRenderDesign renderDesign)
		{
			base.RenderAction (graphics, render, renderDesign);
		}

		public override bool Contains(PointF location)
		{
			return GroupContains(location,true);
		}

		#endregion

		#region Events
		
		//Occurs when an element is added to the elements collection
		private void Element_Insert(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;

			if (element is Group) throw new GroupException("A group cannot be added to another group.");
						
			//Set the layer if not already set
			//Deserialized elements have layer information set
			if (element.Layer == null && Layer != null)
			{
				Layer layer = Layer;
				element.SetLayer(layer);

				//Add to the default level
				string key = layer.Elements.CreateKey();
				layer.Elements.SetModifiable(true);
				layer.Elements.Add(key, element); 
				layer.Elements.SetModifiable(false);

				//Set the layer key 
				element.SetLayerKey(key);
				
				//Set the element z order
				element.mZOrder = layer.Elements.Count;
				
				if (element is IUserInteractive)
				{
					IUserInteractive interactive = (IUserInteractive) element;
					if ((interactive.Interaction & UserInteraction.BringToFront) == UserInteraction.BringToFront) layer.Elements.BringToFront(element);
				}
			}

			//Set the container
			element.SetContainer(this);

			//Set handlers
			element.ElementInvalid +=new EventHandler(Element_ElementInvalid);

			//Draw the path if a line
			if (element is Line) 
			{
				Line line = (Line) element;
				
				//If a connector and is not auto routed then calculate points
				if (element is Connector)
				{
					Connector connector = (Connector) element;
					if (connector.Points == null) connector.CalculateRoute();
				}

				line.DrawPath();
			}

			//Set any containers for child elements
			if (element is IPortContainer)
			{
				IPortContainer container = (IPortContainer) element;
				
				foreach (Port port in container.Ports.Values)
				{
					port.SetContainer(this);
				}
			}

			CreateRenderList();
			OnElementInvalid();

			//Raise the ElementInserted event
			OnElementInserted(element);
		}

		//Occurs when an element is removed from the elements collection
		private void Element_Remove(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;

			//Remove handlers
			element.ElementInvalid -=new EventHandler(Element_ElementInvalid);

			//Remove the element from default level
			element.Layer.Elements.SetModifiable(true);
			element.Layer.Elements.Remove(element.LayerKey);
			element.Layer.Elements.SetModifiable(false);

			if (element is Shape) ResetLines((Shape) element);

			//Remove origin handlers
			if (element is Line)
			{
				Line line = (Line) element;
				line.Start.RemoveHandlers();
				line.End.RemoveHandlers();
			}

			//Re-render and redraw
			CreateRenderList();
			Invalidate();

			//Raise the ElementRemovedEvent
			OnElementRemoved(element);
		}

		//Occurs when an element raises an invalid event
		private void Element_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Shapes",Shapes,typeof(Elements));
			info.AddValue("Lines",Lines,typeof(Elements));
			info.AddValue("Margin",Margin);
			info.AddValue("CheckBounds",CheckBounds);
			info.AddValue("DrawExpand",DrawExpand);
			info.AddValue("Expanded",Expanded);
			info.AddValue("ExpandedSize",Serialize.AddSizeF(ExpandedSize));
			info.AddValue("ContractedSize",Serialize.AddSizeF(ContractedSize));

			base.GetObjectData(info,context);
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
		
		//Perform post deserialization tasks
		public void OnDeserialization(object sender)
		{
			//Recreate origin references
			foreach (Line line in Lines.Values)
			{
				line.Start.Resolve(Shapes);
				line.End.Resolve(Shapes);
			}

			//##what if line origin is parent?
		}

		private void CreateRenderList()
		{
			mRenderList = new RenderList();

			//Check for intersections with the zoomed rectangle;
			foreach (Shape shape in mShapes.Values)
			{
				if (shape.Visible) mRenderList.Add(shape);
			}

			if (mLines != null) //Can be null when shapes first set
			{
				foreach (Line line in mLines.Values)
				{
					if (line.Visible) mRenderList.Add(line);
				}
			}
			mRenderList.Sort();
		}

		private bool GroupContains(PointF location,bool transparent)
		{
			//Inflate rectangle to include selection handles
			RectangleF bounds = Rectangle;
			bounds.Inflate(6,6);

			//If inside inflate boundary
			if (bounds.Contains(location))
			{
				//Return true if clicked in selection rectangle but not path rectangle
				if (Selected && !Rectangle.Contains(location)) return true;

				//Check the outline offset to the path (0,0)
				location.X -= Rectangle.X;
				location.Y -= Rectangle.Y;

				//If background is drawn or transparency checking is not enabled
				if (DrawBackground || !transparent)
				{
					if (GetPathInternal().IsVisible(location)) return true;
				}
				else
				{
					if (DrawExpand && Expander.IsVisible(location)) return true;
					
					//Check if contains an element when draw transparently
					if (Expanded)
					{
						//Check local renderlist
						foreach (Element element in mRenderList)
						{
							if (element.Contains(location)) return true;
						}
					}

					//Check outline of path
					if (GetPathInternal().IsOutlineVisible(location,new Pen(Color.Black, BorderWidth + 2))) return true;
				}
			}
			
			return false;
		}

		private void ResetLines(Shape shape)
		{
			//Loop through each line and create a remove list
			foreach (Line line in Lines.Values)
			{
				if (line.Start.DockedElement == shape) line.Start.Location = line.FirstPoint;
				if (line.End.DockedElement == shape) line.End.Location = line.LastPoint;
			}
		}

		#endregion


	}
}
