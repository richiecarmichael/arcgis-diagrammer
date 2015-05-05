using System;
using System.Collections;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Curve: Line, ISerializable, ICloneable
	{
		//Property variables
		private float mTension;
		private CurveType mCurveType;
		private PointF[] mControlPoints;

		#region Interface

		//Constructors
		public Curve(): base()
		{
			mCurveType = CurveType.Spline;			
			mTension = 0.5F;
			CreateControlPoints();
		}

		public Curve(PointF start,PointF end): base(start,end)
		{
			mCurveType = CurveType.Spline;
			mTension = 0.5F;
			CreateControlPoints();
		}

		public Curve(Shape start,Shape end): base(start,end)
		{
			mCurveType = CurveType.Spline;
			mTension = 0.5F;
			CreateControlPoints();
		}

		public Curve(Port start,Port end): base(start,end)
		{
			mCurveType = CurveType.Spline;
			mTension = 0.5F;
			CreateControlPoints();
		}

		public Curve(Curve prototype): base(prototype)
		{
			mCurveType = prototype.CurveType;
			mTension = prototype.Tension;
			mControlPoints = (PointF[]) prototype.ControlPoints.Clone();
			DrawPath();
		}
		
		protected Curve(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			
			CurveType = (CurveType) Enum.Parse(typeof(CurveType), info.GetString("CurveType"),true);
			Tension = info.GetSingle("Tension");
			ControlPoints = Serialize.GetPointFArray(info.GetString("ControlPoints"));

			SuspendEvents = false;
		}
	
		//Properties
		public virtual PointF[] ControlPoints
		{
			get
			{
				return mControlPoints;
			}
			set
			{
				mControlPoints = value;

				DrawPath();
				OnElementInvalid();
			}
		}

		public virtual float Tension
		{
			get
			{
				return mTension;
			}
			set
			{
				if (mTension != value)
				{
					mTension = value;

					DrawPath();
					OnElementInvalid();
				}
			}
		}

		public virtual CurveType CurveType
		{
			get
			{
				return mCurveType;
			}
			set
			{
				if (mCurveType != value)
				{
					mCurveType = value;

					DrawPath();
					OnElementInvalid();
				}
			}
		}

		protected virtual void SetControlPoints(PointF[] points)
		{
			mControlPoints = points;
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new Curve(this);
		}

		//Code to draw path for the line
		public override void DrawPath()
		{
			if (Container == null) return;
			if (ControlPoints == null) return;

			//Get the start and end location depending on start shapes etc
			PointF startLocation = GetOriginLocation(Start,End);
			PointF endLocation = GetOriginLocation(End,Start);

			//Add the points to the solution
			ArrayList points = new ArrayList();
			points.Add(startLocation);
			
			//Add the control points
			//If bezier must be 2,5,8 control points etc
			PointF[] controlPoints;

			//Set up control points
			if (CurveType == CurveType.Bezier)
			{
				//Must be 2, 5, 8 etc
				if (mControlPoints.GetUpperBound(0) < 1) throw new CurveException("Bezier must contain at least 2 control points.");
				
				int intMax = (((int) (mControlPoints.GetUpperBound(0) - 1) / 3) * 3) + 2;
				controlPoints = new PointF[intMax];

				for (int i = 0; i < intMax; i++ )
				{
					controlPoints[i] = mControlPoints[i];
				}
			}
			else
			{
				controlPoints = mControlPoints;
			}
			points.InsertRange(1,controlPoints);
			
			//Add the end points
			points.Add(endLocation);

			//Draw the path
			GraphicsPath path = new GraphicsPath();
			
			if (CurveType == CurveType.Bezier)
			{
				path.AddBeziers((PointF[]) points.ToArray(typeof(PointF)));
			}
			else
			{
				path.AddCurve((PointF[]) points.ToArray(typeof(PointF)),Tension);
			}

			SetPoints(points);

			//Calculate path rectangle
			RectangleF rect = path.GetBounds();
			SetRectangle(rect); //Sets the bounding rectangle
			SetPath(path); //setpath moves the line to 0,0
		}

		public override bool Intersects(RectangleF rectangle)
		{
			return CurveIntersects(rectangle);
		}

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

			foreach (PointF point in Points)
			{
				path = (GraphicsPath) defaultPath.Clone();
				matrix.Reset();
				matrix.Translate(point.X - Rectangle.X - halfRectangle.Width,point.Y - Rectangle.Y - halfRectangle.Height);
				path.Transform(matrix);
				Handles.Add(new Handle(path, HandleType.Origin));
			}

			Handles[0].CanDock = true;
			Handles[Handles.Count-1].CanDock = true;
		}

		#endregion

		#region Events

		//Handles invalid origin events
		private void Origin_OriginInvalid(object sender, EventArgs e)
		{
			DrawPath();
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("CurveType",Convert.ToInt32(CurveType).ToString());				
			info.AddValue("Tension",Tension);
			info.AddValue("ControlPoints",Serialize.AddPointFArray(ControlPoints));
			base.GetObjectData(info,context);
		}	
	
		private bool CurveIntersects(RectangleF rectangle)
		{
			//Translate rectangle to local co-ordinates
			rectangle.Location = new PointF(rectangle.Location.X - Rectangle.Location.X,rectangle.Location.Y - Rectangle.Location.Y);
	
			//If the rectangle contains the whole line rectangle then return true
			if (rectangle.Contains(Rectangle)) return true;

			Region region = new Region(GetPathInternal());

			return region.IsVisible(rectangle);
		}

		private void CreateControlPoints()
		{
			PointF start = GetOriginLocation(Start, End);
			PointF end = GetOriginLocation(Start, End);
			PointF mid = new PointF(start.X + ((end.X - start.X)/2), end.Y + ((end.Y - start.Y) /2));
			
			ControlPoints = new PointF[] {mid};
		}

		#endregion
	}
}
