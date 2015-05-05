using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

using Crainiate.Diagramming.Drawing2D;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Line : Element, ISerializable, ICloneable, ISelectable, IUserInteractive, IPortContainer
	{
		//Property variables
		private LineJoin mLineJoin;
		private bool mAllowMove;

		private Origin mStartOrigin;
		private Origin mEndOrigin;
		
		private bool mDrawSelected;
		private bool mSelected;
		private UserInteraction mInteraction;

		//Working variables
		private bool mSuspendEvents;
		private ArrayList mPoints;

		private Elements mPorts;
		private Animation mAnimation;

		#region Interface

		//Events
		public event EventHandler SelectedChanged;
		public event EventHandler OriginInvalid;

		//Constructors
		public Line()
		{
			SuspendEvents = true;

			AllowMove = true;
			DrawSelected = true;
			Start = new Origin();
			End = new Origin();
			Interaction = UserInteraction.BringToFront;
			SmoothingMode = SmoothingMode.HighQuality;
			Ports = new Elements(typeof(Port),"Port");

			SuspendEvents = false;
		}
		
		public Line(PointF start,PointF end)
		{
			SuspendEvents = true;

			AllowMove = true;
			DrawSelected = true;
			Start = new Origin(start);
			End = new Origin(end);
			Interaction = UserInteraction.BringToFront;
			Ports = new Elements(typeof(Port),"Port");

			SuspendEvents = false;
		}

		public Line(Shape start,Shape end)
		{
			SuspendEvents = true;

			AllowMove = true;
			DrawSelected = true;
			Start = new Origin(start);
			End = new Origin(end);
			Interaction = UserInteraction.BringToFront;
			Ports = new Elements(typeof(Port),"Port");

			SuspendEvents = false;
		}

		public Line(Port start,Port end)
		{
			SuspendEvents = true;

			AllowMove = true;
			DrawSelected = true;
			Start = new Origin(start);
			End = new Origin(end);
			Interaction = UserInteraction.BringToFront;
			Ports = new Elements(typeof(Port),"Port");

			SuspendEvents = false;
		}

		public Line(Line prototype): base(prototype)
		{
			mAllowMove = prototype.AllowMove;
			mLineJoin = prototype.LineJoin;
			mDrawSelected = prototype.DrawSelected;
			mInteraction = prototype.Interaction;
			
			//Set up new origins
			Start = new Origin(prototype.FirstPoint);
			End = new Origin(prototype.LastPoint);

			Start.Marker = prototype.Start.Marker;
			End.Marker = prototype.End.Marker;

			mPoints = (ArrayList) prototype.Points.Clone();

			//Copy ports
			Ports = new Elements(typeof(Port),"Port");
			foreach (Port port in prototype.Ports.Values)
			{
				Port clone = (Port) port.Clone();
				Ports.Add(port.Key,clone);
				
				clone.SuspendValidation();
				clone.Location = port.Location;
				clone.ResumeValidation();
			}

			if (prototype.Animation != null) mAnimation = (Animation) prototype.Animation.Clone();

			DrawPath();
		}

		//Deserializes info into a new Line
		protected Line(SerializationInfo info, StreamingContext context): base(info,context)
		{
			Ports = new Elements(typeof(Port),"Port");
			SuspendEvents = true;

			AllowMove = info.GetBoolean("AllowMove");
			DrawSelected = info.GetBoolean("DrawSelected");
			Selected = info.GetBoolean("Selected");
			LineJoin = (LineJoin) Enum.Parse(typeof(LineJoin), info.GetString("LineJoin"),true);
			Interaction = (UserInteraction) Enum.Parse(typeof(UserInteraction), info.GetString("Interaction"),true);

			Start = (Origin) info.GetValue("Start",typeof(Origin));
			End = (Origin) info.GetValue("End",typeof(Origin));

			if (Serialize.Contains(info,"Ports",typeof(Elements))) Ports = (Elements) info.GetValue("Ports",typeof(Elements));
			if (Serialize.Contains(info,"Animation",typeof(Animation))) Animation = (Animation) info.GetValue("Animation",typeof(Animation));

			SuspendEvents = false;	
		}

		//Properties
		public virtual Animation Animation
		{
			get
			{
				return mAnimation;
			}
			set
			{
				mAnimation = value;
			}
		}

		public virtual UserInteraction Interaction
		{
			get
			{
				return mInteraction;
			}
			set
			{
				mInteraction = value;
			}
		}

		//Sets the start location
		public virtual Origin Start
		{
			get
			{
				return mStartOrigin;				
			}
			set
			{
				//Remove any existing handlers
				if (mStartOrigin != null) mStartOrigin.OriginInvalid -=new EventHandler(Origin_OriginInvalid);
 
				mStartOrigin = value;
				if (mStartOrigin != null) mStartOrigin.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
				mStartOrigin.SetLine(this);

				//Draw the path and relocate the ports
				DrawPath();
				LocatePorts();

				OnElementInvalid();
			}
		}

		//Sets the End location
		public virtual Origin End
		{
			get
			{
				return mEndOrigin;				
			}
			set
			{
				//Remove any existing handlers
				if (mEndOrigin != null) mEndOrigin.OriginInvalid -=new EventHandler(Origin_OriginInvalid);

				mEndOrigin = value;
				if (mEndOrigin != null) mEndOrigin.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
				mEndOrigin.SetLine(this);

				//Redraw the internal path;
				DrawPath();
				LocatePorts();

				OnElementInvalid();
			}
		}
		
		//Indicates whether the line can be moved as an element.
		public virtual bool AllowMove
		{
			get
			{
				return mAllowMove;
			}

			set
			{
				mAllowMove = value;
			}
		}

		//Gets or sets the join style for the ends of two consecutive line segements.
		public virtual LineJoin LineJoin
		{
			get
			{
				return mLineJoin;
			}
			set
			{
				if (mLineJoin != value)
				{
					mLineJoin = value;
					DrawPath();
					OnElementInvalid();
				}
			}
		}

		//Indicates whether the selection path decoration is shown when the element is selected.
		public virtual bool DrawSelected
		{
			get
			{
				return mDrawSelected;
			}
			set
			{
				if (mDrawSelected != value)
				{
					mDrawSelected = value;
					OnElementInvalid();
				}
			}
		}

		//Indicates whether or the shape is currently selected.
		public virtual bool Selected
		{
			get
			{
				return mSelected;
			}
			set
			{
				if (mSelected != value)
				{
					mSelected = value;
					OnSelectedChanged();
					OnElementInvalid();
				}
			}
		}

		//Returns the points that make up this line
		public virtual ArrayList Points
		{
			get
			{
				return mPoints;
			}
		}

		//Returns the first point of this line
		public virtual PointF FirstPoint
		{
			get
			{
				return (PointF) mPoints[0];
			}
		}

		//Returns the last point of this line
		public virtual PointF LastPoint
		{
			get
			{
				return (PointF) mPoints[mPoints.Count-1];
			}
		}

		public virtual Elements Ports
		{
			get
			{
				return mPorts;
			}
			set
			{
				if (value == null) 
				{
					mPorts = new Elements(typeof(Port),"Port");
				}
				else
				{					
					if (mPorts != null)
					{
						mPorts.InsertElement -= new ElementsEventHandler(Ports_InsertElement);

						//Set the back references for the ports
						foreach (Port port in mPorts.Values)
						{
							port.ElementInvalid -=new EventHandler(Port_ElementInvalid);
						}
					}

					mPorts = value;
					mPorts.InsertElement +=new ElementsEventHandler(Ports_InsertElement);
					
					//Set the back references for the ports
					foreach (Port port in mPorts.Values)
					{
						port.SetParent(this);
						port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
					}
				}
				OnElementInvalid();
			}
		}

		//Methods
		public virtual Origin OriginFromLocation(PointF location)
		{
			return GetOriginFromLocation(location);
		}

		protected internal virtual void SetPoints(ArrayList points)
		{
			mPoints = points;
		}

		//Raises the element SelectedChanged event.
		protected virtual void OnSelectedChanged()
		{
			if (!(SuspendEvents) && SelectedChanged!=null) SelectedChanged(this,new EventArgs());
		}

		//Raises the OriginInvalid event
		protected virtual void OnOriginInvalid(object sender)
		{
			if (!SuspendEvents && OriginInvalid != null) OriginInvalid(sender,new EventArgs());
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Line(this);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			if (mPoints == null || mPoints.Count < 2) return;
			
			PointF startLocation = (PointF) mPoints[0];
			PointF startReference = (PointF) mPoints[1];
			PointF endLocation = (PointF) mPoints[mPoints.Count-1];
			PointF endReference = (PointF) mPoints[mPoints.Count-2];

			//Save the current region
			Region current = graphics.Clip;

			//Mask out the start marker
			if (Start.Marker != null)
			{
				Region region = new Region(Start.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(Start.Marker,startLocation,startReference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
		
			//Mask out the end marker
			if (End.Marker != null)
			{
				Region region = new Region(End.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(End.Marker,endLocation,endReference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			base.Render (graphics, render);

			if (Start.Marker != null || End.Marker != null)
			{
				graphics.Clip = current;

				if (Start.Marker != null) RenderMarker(Start.Marker,startLocation,startReference,graphics,render);
				if (End.Marker != null) RenderMarker(End.Marker,endLocation,endReference,graphics,render);
			}

			//Render any ports
			if (Ports != null)
			{
				foreach (Port port in Ports.Values)
				{
					if (port.Visible)
					{
						graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
						port.SuspendValidation();
						port.Render(graphics, render);
						port.ResumeValidation();
						graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
					}
				}
			}
		}

		//Implement a base rendering of an element
		protected internal override void RenderShadow(Graphics graphics,IRender render)
		{
			if (mPoints == null) return;
			if (mPoints.Count < 2) return;

			PointF startLocation = (PointF) mPoints[0];
			PointF startReference = (PointF) mPoints[1];
			PointF endLocation = (PointF) mPoints[mPoints.Count-1];
			PointF endReference = (PointF) mPoints[mPoints.Count-2];

			Layer layer = this.Layer;
			Pen shadowPen = new Pen(layer.ShadowColor);
			GraphicsPath shadowPath = GetPathInternal();
			shadowPen.Color = render.AdjustColor(layer.ShadowColor,0,this.Opacity);

			//Save the current region
			Region current = graphics.Clip;

			//Mask out the start marker
			if (Start.Marker != null)
			{
				Region region = new Region(Start.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(Start.Marker,startLocation,startReference,new Matrix()));
				region.Translate(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Mask out the end marker
			if (End.Marker != null)
			{
				Region region = new Region(End.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(End.Marker,endLocation,endReference,new Matrix()));
				region.Translate(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
				graphics.SetClip(region,CombineMode.Exclude);
			}

			graphics.TranslateTransform(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
			
			//Draw line
			if (Layer.SoftShadows)
			{
				shadowPen.Color = Color.FromArgb(20,shadowPen.Color);
				graphics.CompositingQuality = CompositingQuality.HighQuality;
				graphics.SmoothingMode = SmoothingMode.HighQuality;
			}

			graphics.DrawPath(shadowPen, shadowPath);

			if (layer.SoftShadows)
			{
				graphics.CompositingQuality = render.CompositingQuality;
				graphics.SmoothingMode = SmoothingMode;
			}

			//Restore graphics
			if (Start.Marker != null || End.Marker != null)
			{
				graphics.Clip = current;
				if (Start.Marker != null) RenderMarkerShadow(Start.Marker,startLocation,startReference,graphics,render);
				if (End.Marker != null) RenderMarkerShadow(End.Marker,endLocation,endReference,graphics,render);
			}

			graphics.TranslateTransform(-layer.ShadowOffset.X ,-layer.ShadowOffset.Y);
		}

		protected internal override void RenderAction(Graphics graphics, IRender render,IRenderDesign renderDesign)
		{
			if (mPoints == null || mPoints.Count < 2) return;

			PointF startLocation = (PointF) mPoints[0];
			PointF startReference = (PointF) mPoints[1];
			PointF endLocation = (PointF) mPoints[mPoints.Count-1];
			PointF endReference = (PointF) mPoints[mPoints.Count-2];

			//Save the current region
			Region current = graphics.Clip;

			//Mask out the start marker
			if (Start.Marker != null)
			{
				Region region = new Region(Start.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(Start.Marker,startLocation,startReference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Mask out the end marker
			if (End.Marker != null)
			{
				Region region = new Region(End.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(End.Marker,endLocation,endReference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Render element action
			base.RenderAction (graphics,render,renderDesign);

			//Render markers
			if (Start.Marker != null || End.Marker != null)
			{
				graphics.Clip = current;

				if (Start.Marker != null) RenderMarkerAction(Start.Marker,startLocation,startReference,graphics,render,renderDesign);
				if (End.Marker != null) RenderMarkerAction(End.Marker,endLocation,endReference,graphics,render,renderDesign);
			}

			//Render any ports
			if (Ports != null)
			{
				foreach (Port port in Ports.Values)
				{
					if (port.Visible)
					{
						graphics.TranslateTransform(-Rectangle.X + port.Rectangle.X,-Rectangle.Y + port.Rectangle.Y);
						port.SuspendValidation();
						port.Render(graphics, render);
						port.ResumeValidation();
						graphics.TranslateTransform(Rectangle.X - port.Rectangle.X,Rectangle.Y - port.Rectangle.Y);						
					}
				}
			}
		}

		//Implement a base rendering of an element selection
		protected internal override void RenderSelection(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			CreateHandles();

			SmoothingMode smoothing = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Handle previousHandle=null;
			SolidBrush brushWhite = new SolidBrush(Color.White);
			Pen pen = Component.Instance.SelectionStartPen;
			SolidBrush brush = Component.Instance.SelectionStartBrush;

			foreach (Handle handle in Handles)
			{
				if (previousHandle != null)	
				{
					graphics.FillPath(brushWhite,previousHandle.Path);
					graphics.FillPath(brush,previousHandle.Path);
					graphics.DrawPath(pen,previousHandle.Path);
					pen = Component.Instance.SelectionPen; //Set to normal brush
					brush = Component.Instance.SelectionBrush; //Set to normal pen
				}
				previousHandle = handle;
			}
			graphics.FillPath(brushWhite,previousHandle.Path);
			graphics.FillPath(Component.Instance.SelectionEndBrush,previousHandle.Path);
			graphics.DrawPath(Component.Instance.SelectionEndPen,previousHandle.Path);

			graphics.SmoothingMode = smoothing;
		}

		public override bool Contains(PointF location)
		{
			return LineContains(location);
		}

		public override bool Intersects(RectangleF rectangle)
		{
			return LineIntersects(rectangle);
		}

		//Returns the type of cursor from this point
		public override Handle Handle(PointF location)
		{
			return GetLineHandle(location);
		}

		//Create a list of handles 
		protected override void CreateHandles()
		{
			if (Container == null) return;
			SetHandles(new Handles());

			//Get the default graphics path and scale it
			IRender render = RenderFromContainer();
			GraphicsPath defaultPath = (GraphicsPath) Component.Instance.DefaultHandlePath.Clone();
			Matrix matrix = new Matrix();
			matrix.Scale(render.ZoomFactor,render.ZoomFactor);
			defaultPath.Transform(matrix);
			RectangleF pathRectangle = defaultPath.GetBounds();
			RectangleF halfRectangle = new RectangleF(0,0,pathRectangle.Width /2, pathRectangle.Height /2);

			//Loop through each point and add an offset handle
			GraphicsPath path;

			PointF point = (PointF) Points[0];
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.Origin, true));

			point = (PointF) Points[Points.Count-1];
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.Origin, true));
		}

		#endregion

		#region Events
		
		//Handles invalid origin events
		private void Origin_OriginInvalid(object sender, EventArgs e)
		{
			OnOriginInvalid(sender);
			
			CheckSameOrigin(sender);
			DrawPath();
			LocatePorts();

			OnElementInvalid();
		}

		private void Ports_InsertElement(object sender, ElementsEventArgs e)
		{
			//Sets the shape of the port
			Port port = (Port) e.Value;
			port.SetParent(this);
			port.SetLayer(Layer);
			port.ElementInvalid +=new EventHandler(Port_ElementInvalid);
			port.SetContainer(Container);
			port.SetOrder(mPorts.Count -1);

			//Locate if not deserializing
			if (port.Location.IsEmpty) LocatePort(port);
		}

		//Occurs when a port becomes invalid
		private void Port_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("AllowMove",AllowMove);
			info.AddValue("DrawSelected",DrawSelected);
			info.AddValue("Selected",Selected);
			info.AddValue("LineJoin",Convert.ToInt32(LineJoin).ToString());				
			info.AddValue("Interaction",Convert.ToInt32(Interaction).ToString());
			
			info.AddValue("Start",Start);
			info.AddValue("End",End);

			if (Ports.Count > 0) info.AddValue("Ports",Ports);
			if (Animation != null) info.AddValue("Animation",Animation);

			base.GetObjectData(info,context);
		}

		//Code to draw path for the line
		public virtual void DrawPath()
		{
			if (Container == null) return; //Reserializing
			if (Start == null || End == null) return; //Cloning line

			//Get the start and end location depending on start shapes etc
			PointF startLocation = GetOriginLocation(Start,End);
			PointF endLocation = GetOriginLocation(End,Start);

			//Add the points to the solution
			ArrayList points = new ArrayList();
			points.Add(startLocation);
			points.Add(endLocation);
			SetPoints(points);

			//Draw the path
			GraphicsPath path = new GraphicsPath();
			path.AddLine(startLocation,endLocation);
			
			//Calculate path rectangle
			RectangleF rect = path.GetBounds();
			SetRectangle(rect); //Sets the location rectangle
			SetPath(path); //setpath moves the line to 0,0
		}

		//Returns the end point of a line depending on the starting shape and location
		protected internal PointF GetOriginLocation(Origin target,Origin source)
		{
			if (target.Shape == null && target.Port == null) return target.Location;

			//Set up location from source location, shape or marker
			SolidElement targetElement;
			PointF sourceLocation;

			//Set up the target element (shape or port)
			targetElement = (target.Shape != null) ? (SolidElement) target.Shape : (SolidElement) target.Port;
			sourceLocation = GetSourceLocation(source);

			//Adjust the source location for different containers
			if (source.DockedElement != null && source.DockedElement.Container != targetElement.Container)
			{
				if (source.DockedElement is Group)
				{
					IContainer container = (IContainer) source.DockedElement;
					sourceLocation = new PointF(sourceLocation.X - container.Offset.X,sourceLocation.Y - container.Offset.Y);
				}
				if (target.DockedElement is Group)
				{
					IContainer container = (IContainer) target.DockedElement;
					sourceLocation = new PointF(sourceLocation.X + container.Offset.X,sourceLocation.Y + container.Offset.Y);
				}
			}

			//Get the intercept
			PointF result = targetElement.Intercept(sourceLocation);

			//Readjust the source location for different containers
			if (source.DockedElement != null && source.DockedElement.Container != targetElement.Container)
			{
				if (target.DockedElement is Group)
				{
					IContainer container = (IContainer) target.DockedElement;
					result = new PointF(result.X - container.Offset.X,result.Y - container.Offset.Y);
				}
			}

			return result;
		}

		//Renders a graphics marker
		protected virtual void RenderMarker(MarkerBase marker,PointF markerPoint,PointF referencePoint,Graphics graphics, IRender render)
		{
			if (marker == null) return;
			
			//Save the graphics state
			Matrix gstate = graphics.Transform;
			
			//Apply the marker transform and render the marker
			graphics.Transform = GetMarkerTransform(marker,markerPoint,referencePoint,graphics.Transform);
			marker.Render(graphics,render);

			//Restore the graphics state
			graphics.Transform = gstate;
		}

		//Renders a graphics marker
		protected virtual void RenderMarkerShadow(MarkerBase marker,PointF markerPoint,PointF referencePoint,Graphics graphics, IRender render)
		{
			if (marker == null) return;
			
			//Save the graphics state
			Matrix gstate = graphics.Transform;

			//Apply the marker transform and render the marker
			graphics.Transform = GetMarkerTransform(marker,markerPoint,referencePoint,graphics.Transform);
			marker.RenderShadow(graphics,render);

			//Restore the graphics state
			graphics.Transform = gstate;
		}

		//Renders a graphics marker
		protected virtual void RenderMarkerAction(MarkerBase marker,PointF markerPoint,PointF referencePoint,Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			if (marker == null) return;
			
			//Save the graphics state
			Matrix gstate = graphics.Transform;

			//Apply the marker transform and render the marker
			graphics.Transform = GetMarkerTransform(marker,markerPoint,referencePoint,graphics.Transform);
			marker.RenderAction(graphics,render,renderDesign);

			//Restore the graphics state
			graphics.Transform = gstate;
		}

		//Returns a marker matrix from a diagram matrix
		protected virtual Matrix GetMarkerTransform(MarkerBase marker,PointF markerPoint,PointF referencePoint,Matrix initialMatrix)
		{
			//Get the angle between the start and end points of the line
			Double rotation = Geometry.DegreesFromRadians(Geometry.GetAngle(markerPoint.X,markerPoint.Y,referencePoint.X,referencePoint.Y));
			
			//Save the graphics state and translate and transform to the marker origin.
			initialMatrix.Translate(markerPoint.X-Rectangle.X,markerPoint.Y-Rectangle.Y);
			initialMatrix.Rotate(Convert.ToSingle(rotation-90));
			if (marker.Centered) 
			{
				initialMatrix.Translate(marker.Rectangle.Width / 2 * -1,marker.Rectangle.Height / 2 * -1);
			}
			else
			{
				initialMatrix.Translate(marker.Rectangle.Width / 2 * -1,1);
			}
			
			return initialMatrix;
		}

		//Determines whether a line and markers contains the supplied point
		private bool LineContains(PointF location)
		{
			if (base.Contains(location)) return true;

			//Check markers
			if (Rectangle.Contains(location))
			{
				//Translate the location to line co-ordinates
				PointF startLocation = (PointF) mPoints[0];
				PointF startReference = (PointF) mPoints[1];
				PointF endLocation = (PointF) mPoints[mPoints.Count-1];
				PointF endReference = (PointF) mPoints[mPoints.Count-2];

				if (MarkerContains(Start.Marker,startLocation,startReference,location)) return true;
				if (MarkerContains(End.Marker,endLocation,endReference,location)) return true;
			}

			return false;
		}

		//Returns the location if not docked, or the center if docked
		protected PointF GetSourceLocation(Origin source)
		{
			PointF sourceLocation;

			//Set up the source location (point,shape center, or port center)
			if (source.Shape == null && source.Port == null)
			{
				sourceLocation = source.Location;
			}
			else
			{
				sourceLocation =  (source.Shape != null) ? source.Shape.Center : source.Port.Center;
			}

			return sourceLocation;
		}

		private Origin GetOriginFromLocation(PointF location)
		{
			location = new PointF(location.X - Rectangle.X - Container.Offset.X,location.Y - Rectangle.Y - Container.Offset.Y);
			
			if (Handles == null) CreateHandles();
			if (Handles[0].Path.IsVisible(location)) return Start;
			if (Handles[Handles.Count-1].Path.IsVisible(location)) return End;
			
			return null;
		}

		//Determines if a marker contains the supplied point
		private bool MarkerContains(MarkerBase marker, PointF markerPoint,PointF referencePoint,PointF location)
		{
			if (marker == null) return false;

			//Create a new matrix at the line location
			Matrix matrix = new Matrix();
			matrix.Translate(Rectangle.X,Rectangle.Y);

			GraphicsPath path = marker.GetPath();
			path.Transform(GetMarkerTransform(marker,markerPoint,referencePoint,matrix));
			
			return path.IsVisible(location);
		}

		private bool LineIntersects(RectangleF rectangle)
		{
			//If the rectangle contains the whole line rectangle then return true
			if (rectangle.Contains(Rectangle)) return true;

			PointF startLocation = (PointF) mPoints[0];
			PointF endLocation = (PointF) mPoints[mPoints.Count-1];

			//Return the intersection of the line with the selection rectangle
			return !Geometry.RectangleIntersection(startLocation,endLocation,rectangle).IsEmpty;
		}

		//Gets the cursor from the diagram point
		private Handle GetLineHandle(PointF location)
		{
			if (!Selected || Handles == null) return Component.Instance.DefaultHandle;

			//Offset location to local co-ordinates
			location = new PointF(location.X - Rectangle.X - Container.Offset.X, location.Y - Rectangle.Y - Container.Offset.Y);

			//Check each handle
			foreach (Handle handle in Handles)
			{
				if (handle.Path.IsVisible(location)) return handle;
			}

			return Component.Instance.DefaultHandle;
		}

		//Locate a port based on the percentage
		public virtual void LocatePort(Port port)
		{
			if (mPoints == null) return;

			PointF startPoint = (PointF) mPoints[0];
			PointF endPoint = (PointF) mPoints[mPoints.Count-1];
			PointF result = new PointF();

			float ratio = 100 / port.Percent;

			float dx = (endPoint.X - startPoint.X) / ratio;
			float dy = (endPoint.Y - startPoint.Y) / ratio;

			result.X = startPoint.X + dx;
			result.Y = startPoint.Y + dy;

			port.Validate = false;
			port.Location = result;
			port.Validate = true;
		}

		//Always face up for now
		public virtual PortOrientation GetPortOrientation(Port port,PointF location)
		{
			return PortOrientation.Line;
		}

		//Takes the port and validates its location against the shape's path
		public bool ValidatePortLocation(Port port,PointF location)
		{
			//Offset location to local co-ordinates and check outline
			location.X -= Rectangle.X;
			location.Y -= Rectangle.Y;

			return GetPathInternal().IsOutlineVisible(location,new Pen(Color.Black,1));
		}

		public virtual float GetPortPercentage(Port port,PointF location)
		{
			GetPortPercentages();
			return port.Percent;
		}	

		//Loop through and calculate the port percentage
		internal virtual void GetPortPercentages()
		{
			if (mPoints == null) return;
			if (mPorts == null) return;

			PointF startPoint = (PointF) mPoints[0];
			PointF endPoint = (PointF) mPoints[mPoints.Count-1];
			float dx = (endPoint.X - startPoint.X);
			float dy = (endPoint.Y - startPoint.Y);
			
			float percent;

			foreach (Port port in Ports.Values)
			{
				percent = 0;

				//can use either x or y, unless x are equal
				if (startPoint.X != endPoint.X)
				{
					percent = 100 / (dx / (port.X - startPoint.X));
				}
				else if (startPoint.Y != endPoint.Y)
				{
					percent = 100 / (dy / (port.Y - startPoint.Y));
				}

				port.SetPercent(percent);
			}
		}

		//reposition the port based on the port percentage
		internal void LocatePorts()
		{
			if (Ports == null) return;

			foreach (Port port in Ports.Values)
			{
				LocatePort(port);
			}
		}

		private void CheckSameOrigin(object sender)
		{
			Origin origin = (Origin) sender;

			//Check for same shapes
			if (origin.Shape != null && End.Shape != null && End.Shape == Start.Shape)
			{
				//If the recent update was the start then undock the end, else the start
				if (origin == Start)
				{
					End.Location = GetOriginLocation(End,Start);
				}
				else
				{
					Start.Location = GetOriginLocation(Start,End);						
				}

			}
		}

		#endregion

	}

}