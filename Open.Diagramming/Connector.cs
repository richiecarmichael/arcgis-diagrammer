using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;
using Crainiate.Diagramming.Layouts;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Connector : Line, ISerializable, ICloneable
	{
		//Property variables
		private bool mAvoid;
		private SizeF mPadding;
		private bool mJump;
		private bool mRounded;

		private TextLabel mLabel;
		private Image mImage;
		private ArrayList mPointTypes;

		#region Interface

		//Constructors
		//Points are calculated by a call to CalculateRoute when added to Diagram or Group
		public Connector()
		{
			SuspendEvents = true;

			Avoid = true;
			Padding = new SizeF(20,20);
			SmoothingMode = SmoothingMode.None;

			SuspendEvents = false;
		}
		public Connector(Shape start,Shape end) : base(start,end)
		{
			SuspendEvents = true;

			Avoid = true;
			Padding = new SizeF(20,20);
			SmoothingMode = SmoothingMode.None;

			SuspendEvents = false;		
		}
		public Connector(PointF start,PointF end) : base(start,end)
		{
			SuspendEvents = true;

			Avoid = true;
			Padding = new SizeF(20,20);
			SmoothingMode = SmoothingMode.None;

			SuspendEvents = false;
		}
		public Connector(Port start,Port end) : base(start,end)
		{
			SuspendEvents = true;

			Avoid = true;
			Padding = new SizeF(20,20);
			SmoothingMode = SmoothingMode.None;

			SuspendEvents = false;
		}

		public Connector(Connector prototype): base(prototype)
		{
			mAvoid = prototype.Avoid;
			mRounded = prototype.Rounded;
			mJump = prototype.Jump;
			mPadding = prototype.Padding;
		}
		
		protected Connector(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
	
			Avoid = info.GetBoolean("Avoid");
			Rounded = info.GetBoolean("Rounded");
			Padding = (SizeF) Serialize.GetSizeF(info.GetString("Padding"));
			SetPoints(Serialize.GetPointFArrayList(info.GetString("Points")));

			if (Serialize.Contains(info,"Label",typeof(TextLabel))) Label = (TextLabel) info.GetValue("Label",typeof(TextLabel)); 
			if (Serialize.Contains(info,"Image",typeof(Image))) Image = (Image) info.GetValue("Image",typeof(Image));
			if (Serialize.Contains(info,"Jump")) Jump = info.GetBoolean("Jump");
			
			SuspendEvents = false;	
		}

		//Properties
		//Determines whether the line will try avoid other shapes in the diagram
		public virtual bool Avoid
		{
			get
			{
				return mAvoid;
			}
			set
			{
				if (mAvoid != value)
				{
					mAvoid = value;
				}
			}
		}

		//Determines the padding used when drawing around shapes
		public virtual SizeF Padding
		{
			get
			{
				return mPadding;
			}
			set
			{
				if (!mPadding.Equals(value))
				{
					mPadding = value;
					DrawPath();
				}
			}
		}

		//Determines whether the line is drawn with jumps
		public virtual bool Rounded
		{
			get
			{
				return mRounded;
			}
			set
			{
				if (mRounded != value)
				{
					mRounded = value;
					DrawPath();
					OnElementInvalid();
				}
			}
		}

		//Determines whether the line is drawn with jumps
		public virtual bool Jump
		{
			get
			{
				return mJump;
			}
			set
			{
				if (mJump != value)
				{
					mJump = value;
					DrawPath();
					OnElementInvalid();
				}
			}
		}

		//Returns the label for this connector
		public virtual TextLabel Label
		{
			get
			{
				return mLabel;
			}
			set
			{
				if (mLabel != null)
				{
					mLabel.LabelInvalid -= new EventHandler(Label_LabelInvalid);
				}

				mLabel = value;
				if (mLabel != null)
				{
					mLabel.LabelInvalid += new EventHandler(Label_LabelInvalid);
					mLabel.SetParent(this);
				}
				OnElementInvalid();
			}
		}

		//Returns the Image object which which displays an image for this connector
		public Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				if (mImage != null)
				{
					mImage.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}

				mImage = value;
				if (mImage != null) 
				{
					mImage.ImageInvalid += new EventHandler(Image_ImageInvalid);
				}
				OnElementInvalid();
			}
		}

		//Methods
		public virtual void Route()
		{
			//Forces to recalculate the route because we are calling the route explicity
			CalculateRoute(); 
			DrawPath();
			OnElementInvalid();
		}

		//Refines the line points
		public virtual void Refine()
		{
			Route route = Container.Route;
			RefinePoints();
		}

		#endregion

		#region Events

		//Handles annotation invalid events
		private void Label_LabelInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		//Handles image invalid events
		private void Image_ImageInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Connector(this);
		}

		protected internal override void SetPoints(ArrayList points)
		{
			base.SetPoints(points);
			
			//Refine the points by removing ones in a straight line
			if (Container != null) RefinePoints();
		}

		public override bool Intersects(RectangleF rectangle)
		{
			return ConnectorIntersects(rectangle);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			base.Render (graphics, render);
			
			//Render Image
			if (Image != null || Label != null)
			{
				//Determine central point
				//If even number of points, then position half way on segment
				PointF center;
				if (Points.Count % 2 == 0)
				{
					int index = (Points.Count / 2)-1;
					PointF start = (PointF) Points[index];
					PointF end = (PointF) Points[index+1];
					center = new PointF(start.X + ((end.X - start.X) /2),start.Y + ((end.Y - start.Y) /2));
				}
				else //Else calculate from middle segment direction
				{
					int index = Convert.ToInt32(Math.Floor((Double)(Points.Count / 2)));
					center = (PointF) Points[index];
				}

				//Offset to line co-ordinates
				center = new PointF(center.X - Rectangle.X,center.Y - Rectangle.Y);

				graphics.TranslateTransform(center.X,center.Y);
				if (Image != null) Image.Render(graphics,render);
				if (Label != null) Label.Render(graphics,render);
				graphics.TranslateTransform(-center.X,-center.Y);						
			}

			//Draw test squares for points
//			if (Selected)
//			{
//				graphics.TranslateTransform(-Rectangle.X,-Rectangle.Y);
//				Pen newPen = new Pen(Color.Red,1);
//
//				Font font = new Font("Arial",8);
//				SolidBrush brush = new SolidBrush(Color.Blue);
//				int offset = 0;
//
//				foreach (PointF point in Points)
//				{
//					offset+=20;
//					graphics.DrawRectangle(newPen,point.X-2,point.Y-2,4,4);
//					graphics.DrawString(point.ToString(),font,brush,10,offset);
//					
//				}	
//				graphics.TranslateTransform(Rectangle.X,Rectangle.Y);
//			}

		}

		//Returns the type of cursor from this point
		public override Handle Handle(PointF location)
		{
			return GetConnectorHandle(location);
		}

		//Create a list of handles 
		protected override void CreateHandles()
		{
			if (Container == null) return;
			SetHandles(new Handles());
			
			IRender render = RenderFromContainer();
			GraphicsPath defaultPath;
			GraphicsPath path;
			Matrix matrix = new Matrix();
			RectangleF pathRectangle;
			RectangleF halfRectangle;
			PointF point;
			
			//Add any other handles
			if (Points.Count > 3)
			{
				//Get the default graphics path and scale it
				defaultPath = (GraphicsPath) Component.Instance.DefaultLargeHandlePath.Clone();
				matrix.Scale(render.ZoomFactor,render.ZoomFactor);
				defaultPath.Transform(matrix);
				pathRectangle = defaultPath.GetBounds();
				halfRectangle = new RectangleF(0,0,pathRectangle.Width / 2, pathRectangle.Height / 2);

				//Loop through each point and add an orthogonal handle
				for (int i = 0; i< Points.Count; i++)
				{
					if (i > 1 && i < Points.Count -1)
					{
						//get the middle point for this segment
						PointF current = (PointF) Points[i];
						PointF previous = (PointF) Points[i-1];
						point = Geometry.GetMiddlePoint(current,previous);
						
						//Determine which kind of handle 
						HandleType handleType = (current.X == previous.X) ? HandleType.LeftRight : HandleType.UpDown;

						path = (GraphicsPath) defaultPath.Clone();
						matrix.Reset();

						//Offset the path to the points location
						//matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
						matrix.Translate(point.X - Rectangle.X - halfRectangle.Width, point.Y - Rectangle.Y - halfRectangle.Height);

						//Rotate the handle 90 degrees if left right handle
						if (handleType == HandleType.LeftRight) matrix.RotateAt(90,new PointF(pathRectangle.Width / 2, pathRectangle.Height / 2));

						path.Transform(matrix);

						Handles.Add(new ConnectorHandle(path,handleType,i));
					}
				}
			}

			defaultPath = (GraphicsPath) Component.Instance.DefaultHandlePath.Clone();
			matrix = new Matrix();
			matrix.Scale(render.ZoomFactor,render.ZoomFactor);
			defaultPath.Transform(matrix);
			pathRectangle = defaultPath.GetBounds();
			halfRectangle = new RectangleF(0,0,pathRectangle.Width /2, pathRectangle.Height /2);

			//Add first and last points
			point = (PointF) Points[0];
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
			path.Transform(matrix);
			Handles.Insert(0,new Handle(path,HandleType.Origin, true));

			point = (PointF) Points[Points.Count-1];
			path = (GraphicsPath) defaultPath.Clone();
			matrix.Reset();
			matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
			path.Transform(matrix);
			Handles.Add(new Handle(path,HandleType.Origin, true));

		}

		protected override void OnOriginInvalid(object sender)
		{
			Origin origin = (Origin) sender;
			
			//Mark for terrain reform if a shape has moved
			if (origin.DockedElement != null && Container != null && Container.Route != null) Container.Route.Reform();
				
			CalculateRoute();			

			base.OnOriginInvalid (sender);
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Avoid",Avoid);
			info.AddValue("Rounded",Rounded);
			info.AddValue("Padding",Serialize.AddSizeF(Padding));
			info.AddValue("Points",Serialize.AddPointFArrayList(Points));
			info.AddValue("Jump",Jump);

			if (Label !=null) info.AddValue("Label",Label);
			if (Image != null)info.AddValue("Image",Image);

			base.GetObjectData(info,context);
		}

		public override void DrawPath()
		{
			if (Container == null) return;
			if (Points == null) return;

			GraphicsPath path = new GraphicsPath();

			//Loop through the solution points and types and draw the path
			PointF previousPoint = new PointF();
			PointF nextPoint = new PointF();
			float radius = Component.Instance.RoundingRadius;
			float diameter = Component.Instance.RoundingRadius * 2;
			
			int i = 0;
			int total = Points.Count;

			foreach (PointF point in Points)
			{
				//Draw a line to the next point
				if (i > 0)
				{
					//Check for rounded corners and that there are at least 2 points to go
					if (Rounded && (i + 1) < total)
					{
						PointF next = (PointF) Points[i+1]; //Get next point
						
						PointF start = point; //Starting point of arc
						PointF end = point; //Point to become new previous point
						RectangleF bound = new RectangleF(point,new SizeF(diameter,diameter)); //Arc rectangle
						int arc = 0; //stores the arc starting point
						int sweep = 0; //stores the angle sweep
						bool valid = false; //Determines if either side of corner is long enough

						//Offset the x and y coordinates of the arc rectangle
						if (point.X == previousPoint.X)
						{
							if (point.Y > previousPoint.Y && next.X > point.X)
							{
								start.Y += -diameter;
								bound.Offset(0,- diameter);
								end.X += radius;
								arc = 180;
								sweep = -90;

								valid = ((point.Y - previousPoint.Y) > diameter && (next.X - point.X) > diameter);
							}
							else if (point.Y < previousPoint.Y && next.X > point.X)
							{
								start.Y += diameter;
								end.X += radius;
								arc = 180;
								sweep = 90;

								valid = ((previousPoint.Y - point.Y) > diameter && (next.X - point.X) > diameter);
							}
							else if (point.Y < previousPoint.Y && next.X < point.X)
							{
								start.Y += diameter;
								bound.Offset(-diameter,0);
								end.X += -radius;
								arc = 0;
								sweep = -90;

								valid = ((previousPoint.Y - point.Y) > diameter && (point.X - next.X) > diameter);
							}
							else if (point.Y > previousPoint.Y && next.X < point.X)
							{
								start.Y += -diameter;
								bound.Offset(-diameter,-diameter);
								end.X += -radius;
								arc = 0;
								sweep = 90;

								valid = ((point.Y - previousPoint.Y) > diameter && (point.X - next.X) > diameter);
							}
						}
						else
						{
							if (point.X > previousPoint.X && next.Y > point.Y)
							{
								start.X += -diameter;
								bound.Offset(-diameter,0);
								end.Y += radius;
								arc = 270;
								sweep = 90;

								valid = ((point.X - previousPoint.X) > diameter && (next.Y - point.Y) > diameter);
							}
							else if (point.X > previousPoint.X && next.Y < point.Y)
							{
								start.X += -diameter;
								bound.Offset(-diameter,-diameter);
								end.Y += -radius;
								arc = 90;
								sweep = -90;

								valid = ((point.X - previousPoint.X) > diameter && (point.Y - next.Y) > diameter);
							}
							else if (point.X < previousPoint.X && next.Y > point.Y)
							{
								start.X += diameter;
								end.Y += radius;
								arc = 270;
								sweep = -90;

								valid = ((previousPoint.X - point.X) > diameter && (next.Y - point.Y) > diameter);
							}
							else if (point.X < previousPoint.X && next.Y < point.Y)
							{
								start.X += diameter;
								bound.Offset(0,-diameter);
								end.Y += -radius;
								arc = 90;
								sweep = 90;

								valid = ((previousPoint.X - point.X) > diameter && (point.Y - next.Y) > diameter);
							}
						}

						//Check that remaining line is long enough and add jumps if required
						if (valid)
						{
							//Add jumps if enabled, as a path
							if (Jump)
							{
								path.AddPath(CalculateJumpPath(previousPoint,start), true);
							}
							else
							{
								path.AddLine(previousPoint,start);
							}

							path.AddArc(bound,arc,sweep);
						
							//Set the new previous point
							previousPoint = end;
						}
						else
						{
							path.AddLine(previousPoint,point);
							previousPoint = point;
						}
					}
					else //Else add ordinary line
					{
						if (Jump)
						{
							path.AddPath(CalculateJumpPath(previousPoint,point), true);
						}
						else
						{
							path.AddLine(previousPoint,point);
						}
						previousPoint = point;
					}
				}
				
				if (i == 0) previousPoint = point; //Set the previous point if first iteration
				i+=1; //Add one to count
			}

			RectangleF rect = path.GetBounds();
			SetRectangle(rect); //Sets the bounding rectangle
			SetPath(path); //setpath moves the line to 0,0
		}

		protected internal virtual void CalculateRoute()
		{
			if (Container == null || Container.Route == null) return;

			ArrayList result = CalculatePoints();
			SetPoints(result);

			//Connect to shape or port
			ConnectOrigin(Start);
			ConnectOrigin(End);

			//Apply route smoothing
			Route route = Container.Route;			
			route.SmoothRoute(Points);
		}

		protected internal virtual ArrayList CalculatePoints()
		{
			if (Container == null || Container.Route == null) return null;

			//Get the start and end location depending on start shapes etc
			PointF startLocation = GetOriginOffset(Start,End);
			PointF endLocation = GetOriginOffset(End,Start);

			//Offset the location if the containers are different
			if (Container is Group && Start.Port != null && Start.Port.Container is Diagram) startLocation = Geometry.OffsetPoint(startLocation,Container.Offset);
			if (Container is Group && End.Port != null && End.Port.Container is Diagram) endLocation = Geometry.OffsetPoint(endLocation,Container.Offset);

			//Route the line using A* algorithm
			Route route = Container.Route;
			
			//Set the padding
			route.Reset();
			route.Padding = Size.Round(Padding);
			route.Layer = Layer;

			route.Boundary = new Rectangle(new Point(0,0), Size.Round(Container.ContainerSize));
			
			//The route class will guarantee a solution
			route.Avoid = Avoid;
			route.Start = Start.DockedElement as Shape; 
			route.End = End.DockedElement as Shape;

			ArrayList result = route.GetRoute(startLocation, endLocation);

			return result;
		}

		//Returns the end point of a line depending on the starting shape and location
		protected PointF GetOriginLocation(Origin target,Origin source)
		{
			if (target.Shape == null && target.Port == null) return target.Location;

			//Set up location from source location, shape or marker
			SolidElement targetElement;
			PointF sourceLocation;
			PortOrientation orientation;

			//Set up the target element (shape or port)
			targetElement = (target.Shape != null) ? (SolidElement) target.Shape : (SolidElement) target.Port;
			sourceLocation = GetOriginOffset(target,source);
			
			//Intercept may return value that isnt orthogonal
			PointF intercept = targetElement.Intercept(sourceLocation);
			return CheckOrthogonalIntercept(sourceLocation, intercept);
		}

		//Returns the end point of a line offset by the padding and orientation
		private PointF GetOriginOffset(Origin target,Origin source)
		{
			//If no port or shape for the origin then just return the location
			if (target.Shape == null && target.Port == null) 
			{
				return target.Location;
			}
						
			PointF location = new PointF();
			PortOrientation orientation;
			RectangleF rect = target.DockedElement.Rectangle;

			//If the target is a port, returns a point based only on the target 
			if (target.Port != null)
			{
				location = target.Port.Location;
				orientation = target.Port.Orientation;

				//Move to edge of rectangle of shape (eg for parallelgram port problem)
				switch (orientation)
				{
					case PortOrientation.Top:
						location = new PointF(location.X,rect.Y);
						break;
					case PortOrientation.Bottom:
						location = new PointF(location.X,rect.Bottom);
						break;
					case PortOrientation.Left :
						location = new PointF(rect.X,location.Y);
						break;
					case PortOrientation.Right :
						location = new PointF(rect.Right,location.Y);
						break;
				}
			}
			//Work out starting point if a shape
			else
			{
				PointF center = target.Shape.Center;
				
				//Calculate the orientation based on orientation between the source and the target
				//The source center and target center is used
				orientation = Geometry.GetOrientation(GetSourceLocation(source),target.Shape.Center,target.Shape.Rectangle);

				switch (orientation)
				{
					case PortOrientation.Top:
						location = new PointF(center.X,rect.Y);
						break;
					case PortOrientation.Bottom:
						location = new PointF(center.X,rect.Bottom);
						break;
					case PortOrientation.Left :
						location = new PointF(rect.X,center.Y);
						break;
					case PortOrientation.Right :
						location = new PointF(rect.Right,center.Y);
						break;
				}
			}

			//Adjust the location to be orthogonal eg left,right,top,bottom depending on the orientation
			float width = Padding.Width +1 ;
			float height = Padding.Height + 1;
			
			switch (orientation)
			{
				case PortOrientation.Top:
					location.Y -= height;
					break;
				case PortOrientation.Bottom:
					location.Y += height;
					break;
				case PortOrientation.Left :
					location.X -= width;
					break;
				case PortOrientation.Right :
					location.X += width;
					break;
			}

			return location;
		}

		//Reconnects an origin by adding an additional line point to a connector or extending an existing one
		private void ConnectOrigin(Origin origin)
		{
			if (Points == null) return;
			if (Points.Count < 2) return;
			if (origin.Shape == null && origin.Port == null) return;

			PointF source;
			PointF previous;
			PointF intercept;

			//Get the source
			if (origin == Start)
			{
				source =  (PointF) Points[0];
				previous = (PointF) Points[1];
			}
			else
			{
				source = (PointF) Points[Points.Count-1] ;
				previous = (PointF) Points[Points.Count-2] ;
			}

			//Get the shape intercept point
			if (origin.Shape != null) 
			{
				intercept = origin.Shape.Intercept(source);
			}
			//Get port intercept
			else
			{
				intercept = origin.Port.Intercept(source);
			}

			//Make sure that the intercept is orthogonal
			intercept = CheckOrthogonalIntercept(source,intercept);

			//If the intercept is in line with the previous then update
			if (intercept.X == previous.X || intercept.Y == previous.Y)
			{
				if (origin == Start)
				{
					Points[0] = intercept;
				}
				else
				{
					Points[Points.Count-1] = intercept;
				}
			}
			//Else add a new point
			else
			{
				if (origin == Start)
				{
					Points.Insert(0,intercept);
				}
				else
				{
					Points.Add(intercept);
				}
			}
		}

		private bool ConnectorIntersects(RectangleF rectangle)
		{
			if (Points.Count < 2) return false;

			//If the rectangle contains the whole line rectangle then return true
			if (rectangle.Contains(Rectangle)) return true;

			PointF startLocation = new PointF();
			PointF endLocation = (PointF) Points[1];

			foreach (PointF point in Points)
			{
				if (!startLocation.IsEmpty)
				{
					if (! Geometry.RectangleIntersection(startLocation,endLocation,rectangle).IsEmpty) return true;
				}
				startLocation = endLocation;
				endLocation = point;
			}
			
			return !Geometry.RectangleIntersection(startLocation,endLocation,rectangle).IsEmpty;

			return true;
		}

		//Gets the cursor from the diagram point
		private Handle GetConnectorHandle(PointF location)
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
		public override void LocatePort(Port port)
		{
			if (Points == null) return;

			//Work out total length of line
			PointF lastPoint = new Point();
			float totalLength = 0;

			//Loop through and add each lenght to total
			foreach (PointF point in Points)
			{
				if (!lastPoint.IsEmpty)
				{
					if (point.X == lastPoint.X)
					{
						totalLength += Math.Abs(point.Y - lastPoint.Y);
					}
					else
					{
						totalLength += Math.Abs(point.X - lastPoint.X);
					}
				}
				lastPoint = point;
			}

			//Find position by 
			float length = 0;
			float lengthPercent = totalLength * port.Percent / 100;

			PointF result = new PointF();
			lastPoint = new PointF();

			foreach (PointF point in Points)
			{
				if (!lastPoint.IsEmpty)
				{
					if (point.X == lastPoint.X)
					{
						length += Math.Abs(point.Y - lastPoint.Y);

						//check if we are in the right segment
						if (length > lengthPercent)
						{
							if (point.Y > lastPoint.Y) 
							{
								result = new PointF(point.X, point.Y - (length - lengthPercent));
							}
							else
							{
								result = new PointF(point.X, point.Y + (length - lengthPercent));
							}
							break;
						}
					}
					else
					{
						length += Math.Abs(point.X - lastPoint.X);

						//check if we are in the right segment
						if (length > lengthPercent)
						{
							if (point.X > lastPoint.X) 
							{
								result = new PointF(point.X - (length - lengthPercent),point.Y);
							}
							else
							{
								result = new PointF(point.X + (length - lengthPercent),point.Y);
							}
							break;
						}
					}
				}
				lastPoint = point;
			}

			port.Validate = false;
			port.Location = result;
			port.Validate = true;
		}

		//Leave the percentages as they were
		override internal void GetPortPercentages()
		{
			return;
		}

		//Removes points that are line with each other
		protected internal void RefinePoints()
		{
			if (Points.Count < 3) return;

			PointF lastPoint = new PointF();
			int index = 0;
			int loop = 0;

			//Loop through and see if there is an index to remove
			//Use the loop variable to make sure havent entered endless loop
			while (index < Points.Count && loop < Points.Count)
			{
				index = 0;
				loop += 1;
				foreach (PointF point in Points)
				{
					if (index > 0)
					{
						//Check if x co-ordinates are within the grain but not equal
						if (Math.Abs(point.X - lastPoint.X) < 1 && point.X != lastPoint.X)
						{
							if (lastPoint.Equals(Points[0]))
							{
								Points[index] = new PointF(lastPoint.X,point.Y);
							}
							else
							{
								Points[index-1] = new PointF(point.X,LastPoint.Y);
							}
							break;
						}
						//Check y coordinates are not equal but within the grain
						else if (Math.Abs(point.Y - lastPoint.Y) < 1 && point.Y != lastPoint.Y)
						{
							if (lastPoint.Equals(Points[0]))
							{
								Points[index] = new PointF(point.X,lastPoint.Y);
							}
							else
							{
								Points[index-1] = new PointF(lastPoint.X,point.Y);
							}
							break;
						}
					}
					lastPoint = (PointF) Points[index];
					index+=1;
				}
			}
			
			PointF secondLast = new PointF();
			PointF refPoint = new PointF();
			int remove = 0;

			while (remove != -1)
			{
				remove = -1;
				
				//Loop through and see if there is an index to remove
				foreach (PointF point in Points)
				{
					if (!lastPoint.IsEmpty)
					{
						if (!secondLast.IsEmpty)
						{
							//Check if x coordinates are in line
							if (secondLast.X == lastPoint.X && lastPoint.X == point.X)
							{	
								remove = Points.IndexOf(lastPoint);
								break;
							}
							//Check if y coordinates are in line
							if (secondLast.Y == lastPoint.Y && lastPoint.Y == point.Y)
							{	
								remove = Points.IndexOf(lastPoint);
								break;
							}
						}
						secondLast = lastPoint;
					}
					lastPoint = point;
				}

				//Remove point if required
				if (remove != -1) Points.RemoveAt(remove);
			}
		}

		//Makes sure that intercepts are orthogonal
		private PointF CheckOrthogonalIntercept(PointF source, PointF intercept)
		{
			if (Math.Abs(source.X - intercept.X) < Math.Abs(source.Y - intercept.Y))
			{
				return new PointF(source.X, intercept.Y);
			}
			else
			{
				return new PointF(intercept.X, source.Y);
			}
		}

		private GraphicsPath CalculateJumpPath(PointF a, PointF b)
		{
			GraphicsPath path = new GraphicsPath();
			ArrayList jumps = new ArrayList();

			float diameter = 10F;
			if (Rounded) diameter = 15F;

			//Loop through all connectors in the renderlist 
			foreach (Element element in Container.RenderList)
			{
				//Check if connector lower down then this connector and overlaps this connector
				if (element is Connector && element.ZOrder > ZOrder && element.Rectangle.IntersectsWith(Rectangle))
				{
					Connector connector = (Connector) element;
					PointF prev = PointF.Empty;

					//Get the intersection of the line between the points provided and this connector
					foreach (PointF point in connector.Points)
					{
						if (!prev.IsEmpty)
						{	
							PointF intersection = PointF.Empty;

							//Return the line intersection (if any) and add to jumps arraylist
							if (Geometry.LineIntersection(a, b, prev, point,ref intersection))
							{
								//Check the intersection doesnt overlap with the point
								if (ValidateJump(intersection, b, diameter)) jumps.Add(intersection);
							}
						}
						prev = point;
					}
				}
			}

			//Sort the arraylist from smallest to biggest
			if (jumps.Count > 1)
			{
				PointFComparer comparer = new PointFComparer();
				
				if (a.X == b.X)
				{
					comparer.Horizontal = false;
					comparer.Ascending = (a.Y < b.Y);
				}
				else
				{
					comparer.Horizontal = true;
					comparer.Ascending = (a.X < b.X);
				}
				
				jumps.Sort(comparer);
			}

			float angle;
			float sweep = 180;

			//Set up the correct starting angle
			if (a.X == b.X)
			{
				if (a.Y < b.Y) 
				{
					angle = -90;
				}
				else
				{
					angle = 90;
				}
			}
			else
			{
				if (a.X < b.X)
				{
					angle = 180;
				}
				else
				{
					angle = 0;
				}
			}

			//Loop through adding arcs for jump points
			if (jumps.Count == 0)
			{
				path.AddLine(a,b);
			}
			else
			{
				PointF last = a;
				PointF start = PointF.Empty;
				PointF end = PointF.Empty;
				RectangleF bound = RectangleF.Empty;
				
				foreach (PointF point in jumps)
				{
					//Determine start and end points for the arc
					if (a.X == b.X)
					{
						if (a.Y < b.Y)
						{
							start = new PointF(point.X, point.Y - 5F);
							end = new PointF(point.X, point.Y + 5F);
						}
						else
						{
							start = new PointF(point.X, point.Y + 5F);
							end = new PointF(point.X, point.Y - 5F);
						}
					}
					else
					{
						if (a.X < b.X)
						{
							start = new PointF(point.X - 5F, point.Y);
							end = new PointF(point.X + 5F, point.Y);
						}
						else
						{
							start = new PointF(point.X + 5F, point.Y);
							end = new PointF(point.X - 5F, point.Y);
						}
					}

					//Calculate the bounds of the arc
					bound = new RectangleF(point,new SizeF(10F,10F));
					bound.Offset(-5F,-5F);

					path.AddLine(last, start);
					path.AddArc(bound, angle, sweep);
					path.StartFigure();					

					last = end;
				}
				path.AddLine(last, b);
			}

			return path;
		}

		private bool ValidateJump(PointF point, PointF next, float diameter)
		{
			//Offset the x and y coordinates of the arc rectangle
			if (point.X == next.X)
			{
				return (Math.Abs(point.Y - next.Y) > diameter);
			}
			else
			{
				return (Math.Abs(point.X - next.X) > diameter);
			}
		}

		#endregion
	}
}
