using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Drawing2D;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class ComplexLine: Line, ISerializable, ICloneable, IDeserializationCallback
	{
		//Property variables
		private Segments mSegments;
		private bool mAllowExpand;

		#region Interface

		//Constructors
		public ComplexLine(): base()
		{
			mSegments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(PointF start,PointF end): base(start,end)
		{
			mSegments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(Shape start,Shape end): base(start,end)
		{
			mSegments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(Port start,Port end): base(start,end)
		{
			mSegments = new Segments();
			Segment segment = new Segment(Start,End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			AllowExpand = true;
		}

		public ComplexLine(ComplexLine prototype): base (prototype)
		{
			mSegments = new Segments();
			Segment segment = new Segment(Start, End);
			Segments.Add(segment);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);

			//Set up segments
			for (int i = 0; i < prototype.Segments.Count-1; i++)
			{
				segment = AddSegment(i+1,new Origin((PointF) prototype.Points[i+1])); 
				segment.Start.Marker = prototype.Segments[i+1].Start.Marker;
			}
			DrawPath();

			AllowExpand = prototype.AllowExpand;
		}

		protected ComplexLine(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			mSegments = (Segments) info.GetValue("Segments",typeof(Segments));
			if (Serialize.Contains(info,"AllowExpand")) mAllowExpand = info.GetBoolean("AllowExpand");
			
			SuspendEvents = false;
		}

		//Properties
		public virtual Segments Segments
		{
			get
			{
				return mSegments;
			}
		}

		public virtual bool AllowExpand
		{
			get
			{
				return mAllowExpand;
			}
			set
			{ 
				mAllowExpand = value;
			}
		}

		//Methods
		public virtual Segment AddSegment(int position, Origin origin)
		{
			//Valid the position
			if (position < 1) throw new ArgumentException("Position must be greater than zero.","position");
			if (position > Segments.Count) throw new ArgumentException("Position cannot be greater than the total number of segments.","position");
			if (origin == null) throw new ArgumentNullException("origin","Origin may not be null.");
			
			//Create new segment
			Segment segment = new Segment(origin,Segments[position-1].End);

			//Set the previous end to the new origin
			Segments[position-1].SetEnd(origin);

			Segments.Insert(position,segment);
			
			origin.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
			segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);
			origin.SetLine(this);
			
			DrawPath();
			OnElementInvalid();

			return segment;
		}

		public virtual void RemoveSegment(int position)
		{
			//Valid the position
			if (position < 1) throw new ArgumentException("Position must be greater than zero.","position");
			if (position > Segments.Count) throw new ArgumentException("Position can be greater than the total number of segments.","position");

            Origin end = Segments[position].End;
			Segments.RemoveAt(position);
			Segments[position-1].SetEnd(end);

            DrawPath();
            OnElementInvalid();
		}

		protected internal void SetSegments(Segments segments)
		{
			if (mSegments != null)
			{
				foreach (Segment segment in mSegments)
				{
					segment.SegmentInvalid -= new EventHandler(segment_SegmentInvalid);
				
					if (segment.End != End) segment.End.OriginInvalid -=new EventHandler(Origin_OriginInvalid);
				}
			}

			mSegments = segments;
			foreach (Segment segment in segments)
			{
				segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);
				
				if (segment.End != End) segment.End.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
			}
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new ComplexLine(this);
		}

		public override Origin Start
		{
			get
			{
				return base.Start;
			}
			set
			{
				base.Start = value;
				if (Segments != null) Segments[0].SetStart(value);
			}
		}

		public override Origin End
		{
			get
			{
				return base.End;
			}
			set
			{
				base.End = value;
				if (Segments != null) Segments[Segments.Count-1].SetEnd(value);
			}
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			if (Points == null) return;

			PointF location;
			PointF reference;
			Segment segment = null;

			//Save the current region
			Region current = graphics.Clip;

			//Mask out each marker
			for (int i=0;i<Points.Count-1;i++)
			{
				location = (PointF) Points[i];
				reference = (PointF) Points[i+1];

				segment = Segments[i];
				
				//Mask out the start marker
				if (segment.Start.Marker != null && !segment.Start.Marker.DrawBackground)
				{
					Region region = new Region(segment.Start.Marker.GetPathInternal());
					region.Transform(GetMarkerTransform(segment.Start.Marker,location,reference,new Matrix()));
					graphics.SetClip(region,CombineMode.Exclude);
				}
			}

			//Mask out final marker
			if (segment.End.Marker != null && !segment.End.Marker.DrawBackground)
			{
				location = (PointF) Points[Points.Count-1];
				reference = (PointF) Points[Points.Count-2];

				Region region = new Region(segment.End.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(segment.End.Marker,location,reference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Draw the path
			Pen pen = null;

			if (CustomPen == null)
			{
				pen = new Pen(BorderColor,BorderWidth);
				pen.DashStyle = BorderStyle;
	
				//Check if winforms renderer and ajdust color as required
				pen.Color = render.AdjustColor(BorderColor,BorderWidth,Opacity);
			}
			else	
			{
				pen = CustomPen;
			}
			graphics.DrawPath(pen,GetPathInternal());
			
			//Reset the clip
			graphics.Clip = current;

			//Render the segment items
			for (int i=0;i<Points.Count-1;i++)
			{
				segment = Segments[i];
				location = (PointF) Points[i];
				reference = (PointF) Points[i+1];

				if (segment.Start.Marker != null) RenderMarker(segment.Start.Marker,location,reference,graphics,render);

				//Render the segment image and annotation
				RenderSegment(segment,location,reference,graphics,render);
			}

			//Render final marker
			if (segment.End.Marker != null)
			{
				location = (PointF) Points[Points.Count-1];
				reference = (PointF) Points[Points.Count-2];
				RenderMarker(segment.End.Marker,location,reference,graphics,render);				
			}

		}

		protected internal override void RenderAction(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			if (Points == null) return;

			PointF location;
			PointF reference;
			Segment segment = null;

			//Save the current region
			Region current = graphics.Clip;

			//Mask out each marker
			for (int i=0; i<Points.Count-1; i++)
			{
				location = (PointF) Points[i];
				reference = (PointF) Points[i+1];

				segment = Segments[i];
				
				//Mask out the start marker
				if (segment.Start.Marker != null)
				{
					Region region = new Region(segment.Start.Marker.GetPathInternal());
					region.Transform(GetMarkerTransform(segment.Start.Marker,location,reference,new Matrix()));
					graphics.SetClip(region,CombineMode.Exclude);
				}
			}

			//Mask out final marker
			if (segment.End.Marker != null)
			{
				location = (PointF) Points[Points.Count-1];
				reference = (PointF) Points[Points.Count-2];

				Region region = new Region(segment.End.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(segment.End.Marker,location,reference,new Matrix()));
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Draw the path
			GraphicsPath path = GetPathInternal();
			if (path == null) return;

			if (renderDesign.ActionStyle == ActionStyle.Default)
			{
				Pen pen = new Pen(render.AdjustColor(BorderColor,BorderWidth,Opacity));
				pen.Width = BorderWidth;
				graphics.DrawPath(pen,path);
			}
			else
			{
				graphics.DrawPath(Component.Instance.ActionPen,path);
			}

			//Reset the clip
			graphics.Clip = current;

			//Render the markers
			for (int i=0;i<Points.Count-1;i++)
			{
				segment = Segments[i];
				location = (PointF) Points[i];
				reference = (PointF) Points[i+1];

				if (segment.Start.Marker != null) RenderMarkerAction(segment.Start.Marker,location,reference,graphics,render,renderDesign);
			}

			//Render final marker
			if (segment.End.Marker != null)
			{
				location = (PointF) Points[Points.Count-1];
				reference = (PointF) Points[Points.Count-2];
				RenderMarkerAction(segment.End.Marker,location,reference,graphics,render,renderDesign);				
			}
		}

		protected internal override void RenderShadow(Graphics graphics, IRender render)
		{
			if (Points == null) return;

			PointF location;
			PointF reference;
			Segment segment = null;

			Layer layer = this.Layer;
			Pen shadowPen = new Pen(layer.ShadowColor);
			GraphicsPath shadowPath = GetPathInternal();
			shadowPen.Color = render.AdjustColor(layer.ShadowColor,0,this.Opacity);

			//Save the current region
			Region current = graphics.Clip;

			//Mask out each marker
			for (int i=0;i<Points.Count-1;i++)
			{
				location = (PointF) Points[i];
				reference = (PointF) Points[i+1];

				segment = Segments[i];
				
				//Mask out the start marker
				if (segment.Start.Marker != null)
				{
					Region region = new Region(segment.Start.Marker.GetPathInternal());
					region.Transform(GetMarkerTransform(segment.Start.Marker,location,reference,new Matrix()));
					region.Translate(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
					graphics.SetClip(region,CombineMode.Exclude);
				}
			}

			//Mask out final marker
			if (segment.End.Marker != null)
			{
				location = (PointF) Points[Points.Count-1];
				reference = (PointF) Points[Points.Count-2];

				Region region = new Region(segment.End.Marker.GetPathInternal());
				region.Transform(GetMarkerTransform(segment.End.Marker,location,reference,new Matrix()));
				region.Translate(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
				graphics.SetClip(region,CombineMode.Exclude);
			}
			
			//Draw the path
			graphics.TranslateTransform(layer.ShadowOffset.X ,layer.ShadowOffset.Y);
			graphics.DrawPath(shadowPen,shadowPath);

			//Reset the clip
			graphics.Clip = current;

			//Render the markers
			for (int i=0;i<Points.Count-1;i++)
			{
				segment = Segments[i];
				location = (PointF) Points[i];
				reference = (PointF) Points[i+1];

				if (segment.Start.Marker != null) RenderMarkerShadow(segment.Start.Marker,location,reference,graphics,render);
			}

			//Render final marker
			if (segment.End.Marker != null)
			{
				location = (PointF) Points[Points.Count-1];
				reference = (PointF) Points[Points.Count-2];
				RenderMarkerShadow(segment.End.Marker,location,reference,graphics,render);				
			}

			graphics.TranslateTransform(-layer.ShadowOffset.X ,-layer.ShadowOffset.Y);
		}

		protected internal override void RenderSelection(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			CreateHandles();

			SmoothingMode smoothing = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.AntiAlias;

			Handle previousHandle = null;
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
					
					if (handle.Type == HandleType.Expand)
					{
						pen = Component.Instance.ExpandPen; //Set to normal brush
						brush = Component.Instance.ExpandBrush; //Set to normal pen
					}
					else
					{
						pen = Component.Instance.SelectionPen; //Set to normal brush
						brush = Component.Instance.SelectionBrush; //Set to normal pen
					}
				}
				previousHandle = handle;
			}
			graphics.FillPath(brushWhite,previousHandle.Path);
			graphics.FillPath(Component.Instance.SelectionEndBrush,previousHandle.Path);
			graphics.DrawPath(Component.Instance.SelectionEndPen,previousHandle.Path);

			graphics.SmoothingMode = smoothing;
		}

		//Code to draw path for the line
		public override void DrawPath()
		{
			if (Container == null) return;
			if (Segments == null) return;

			ArrayList points = new ArrayList();
			GraphicsPath path = new GraphicsPath();
			PointF startLocation;
			PointF endLocation;

			foreach(Segment segment in Segments)
			{
				//Get the start and end location depending on start shapes etc
				startLocation = GetOriginLocation(segment.Start,segment.End);
				endLocation = GetOriginLocation(segment.End,segment.Start);

				//Add the points to the solution
				if (points.Count == 0) points.Add(startLocation);
				points.Add(endLocation);

				//Draw the path
				path.AddLine(startLocation,endLocation);
			}

			SetPoints(points);

			//Calculate path rectangle
			RectangleF rect = path.GetBounds();
			SetRectangle(rect); //Sets the bounding rectangle
			SetPath(path); //setpath moves the line to 0,0
		}

		public override Origin OriginFromLocation(PointF location)
		{
			return GetOriginFromLocation(location);
		}

		public override bool Intersects(RectangleF rectangle)
		{
			return ComplexIntersects(rectangle);
		}

		protected override void CreateHandles()
		{
			if (Container == null) return;
			if (Points == null) return;
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
			int count = 0;
			PointF previous = new PointF();

			foreach (PointF point in Points)
			{
				//Add the split segment 
				if (count > 0 && AllowExpand)
				{
					path = (GraphicsPath) defaultPath.Clone();
					matrix.Reset();
					matrix.Translate(previous.X - Rectangle.X - halfRectangle.Width + ((point.X - previous.X) / 2), previous.Y - Rectangle.Y - halfRectangle.Height+ ((point.Y - previous.Y) / 2));
					path.Transform(matrix);

					//Create a new expand handle
					ExpandHandle expand = new ExpandHandle(Segments[count-1]);
					expand.Path = path;
					expand.Index = count-1;
					expand.CanDock = false;
					Handles.Add(expand);
				}

				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
				path.Transform(matrix);
				
				//Create handle
				Handle handle = new Handle(path,HandleType.Origin);
				handle.CanDock = false;
				Handles.Add(handle);

				count ++;
				previous = point;
			}
			
			//Set up the docking
			Handles[0].CanDock = true;
			Handles[Handles.Count-1].CanDock = true;
		}

        //Locate a port based on the percentage
        public override void LocatePort(Port port)
        {
            if (Points == null) return;

            //Work out total length of line
            PointF lastPoint = new Point();
            double totalLength = 0;

            //Loop through and add each length to total
            foreach (PointF point in Points)
            {
                if (!lastPoint.IsEmpty)
                {
                    RectangleF bounds = Geometry.CreateRectangle(point, lastPoint);
                    totalLength += Geometry.DistancefromOrigin(new PointF(bounds.Width, bounds.Height));
                }
                lastPoint = point;
            }

            //Find position by 
            double length = 0;
            double lengthPercent = totalLength * port.Percent / 100;

            PointF result = new PointF();
            lastPoint = new PointF();

            foreach (PointF point in Points)
            {
                if (!lastPoint.IsEmpty)
                {
                    double start = length;

                    RectangleF bounds = Geometry.CreateRectangle(point, lastPoint);

                    length += Geometry.DistancefromOrigin(new PointF(bounds.Width, bounds.Height));
                   
					//Check if we are in the right segment
                    if (length > lengthPercent)
                    {
                        //Work out the degrees between the last points
                        double rad = Geometry.GetAngle(lastPoint.X, lastPoint.Y, point.X, point.Y);

                        //Now work out the sides from the angle and H
                        double side1 = Math.Cos(rad) * (lengthPercent - start);
                        double side2 = Math.Sin(rad) * (lengthPercent - start);
                        
                        result = new PointF(Convert.ToSingle(side1) + lastPoint.X, Convert.ToSingle(side2) + lastPoint.Y) ;
                        break;
                    }
                } 
                lastPoint = point;
            }

            port.Validate = false;
            port.Location = result;
            port.Validate = true;
        }

		#endregion

		#region Events

		//Handles invalid origin events
		private void Origin_OriginInvalid(object sender, EventArgs e)
		{
			DrawPath();
			OnElementInvalid();
		}

		private void segment_SegmentInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public void OnDeserialization(object sender)
		{
			//Rehook the invalid events so that property changes are raised
			foreach (Segment segment in Segments)
			{
				segment.SegmentInvalid += new EventHandler(segment_SegmentInvalid);
				if (segment.End != End) segment.End.OriginInvalid +=new EventHandler(Origin_OriginInvalid);
			}
		}
		
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Segments",Segments);
			info.AddValue("AllowExpand",AllowExpand);

			base.GetObjectData(info,context);
		}	
	
		private Origin GetOriginFromLocation(PointF location)
		{
			if (Handles == null) CreateHandles();
			location = new PointF(location.X - Rectangle.X - Container.Offset.X,location.Y - Rectangle.Y - Container.Offset.Y);
		
			int count = 0;
			foreach (Handle handle in Handles)
			{
				if (handle.Type == HandleType.Origin) count ++;
				if (handle.Path.IsVisible(location))
				{
					if (count > Segments.Count) return Segments[count-2].End;
					return Segments[count-1].Start;
				}
			}

			return null;
		}

		private bool ComplexIntersects(RectangleF rectangle)
		{
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

		//Renders a graphics marker
		protected virtual void RenderSegment(Segment segment,PointF targetPoint,PointF referencePoint,Graphics graphics, IRender render)
		{
			if (segment.Label == null && segment.Image == null) return;
			
			//Get midpoint of segment
			PointF midPoint = new PointF(targetPoint.X + ((referencePoint.X - targetPoint.X) /2),targetPoint.Y + ((referencePoint.Y - targetPoint.Y) /2));
            
			//Save the graphics state
			Matrix gstate = graphics.Transform;
			
			//Apply the rotation transform around the centre
			graphics.Transform = GetSegmentTransform(midPoint,referencePoint,graphics.Transform);

			//Offset and draw image 
			if (segment.Image != null)
			{
				graphics.TranslateTransform(0,-segment.Image.Bitmap.Height /2);
				segment.Image.Render(graphics,render);
				graphics.TranslateTransform(0,segment.Image.Bitmap.Height /2);
			}

			//Draw annotation
			if (segment.Label != null)
			{
				segment.Label.Render(graphics,render);
			}

			//Restore the graphics state
			graphics.Transform = gstate;
		}

		//Returns a marker matrix from a diagram matrix
		protected Matrix GetSegmentTransform(PointF targetPoint,PointF referencePoint,Matrix initialMatrix)
		{
			//Get the angle between the start and end points of the line
			Double rotation = Geometry.DegreesFromRadians(Geometry.GetAngle(targetPoint.X,targetPoint.Y,referencePoint.X,referencePoint.Y));
			
			//Save the graphics state and translate and transform to the marker origin.
			initialMatrix.Translate(targetPoint.X-Rectangle.X,targetPoint.Y-Rectangle.Y);
			initialMatrix.Rotate(Convert.ToSingle(rotation));
			
			return initialMatrix;
		}

		#endregion

	}
}
