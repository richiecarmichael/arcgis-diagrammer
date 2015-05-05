using System;
using System.Drawing;
using System.Drawing.Drawing2D;	

namespace Crainiate.Diagramming.Drawing2D
{
	internal class Geometry
	{
		//Returns the first point of intersection with a rectangle
		public static PointF RectangleIntersection(PointF p1, PointF p2, RectangleF rect)
		{
			PointF intersection = new PointF();

			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Y), ref intersection)) return intersection;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Y), new PointF(rect.Right, rect.Bottom), ref intersection)) return intersection;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Bottom), new PointF(rect.X, rect.Bottom), ref intersection)) return intersection;
			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Bottom), new PointF(rect.X, rect.Y), ref intersection)) return intersection;

			return new PointF();
		}

		//Returns the first point of intersection with a rectangle
		public static bool RectangleIntersects(PointF p1, PointF p2, RectangleF rect)
		{
			PointF intersection = new PointF();

			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Y), new PointF(rect.Right, rect.Y), ref intersection)) return true;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Y), new PointF(rect.Right, rect.Bottom), ref intersection)) return true;
			if (LineIntersection(p1, p2, new PointF(rect.Right, rect.Bottom), new PointF(rect.X, rect.Bottom), ref intersection)) return true;
			if (LineIntersection(p1, p2, new PointF(rect.X, rect.Bottom), new PointF(rect.X, rect.Y), ref intersection)) return true;

			return false;
		}

		// Based on the 2d line intersection method from "comp.graphics.algorithms Frequently Asked Questions"
		//Returns a boolean if they intersect and a reference parameter with the location
		public static bool LineIntersection(PointF line1Point1, PointF line1Point2, PointF line2Point1, PointF line2Point2)
		{
			//	   (Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
			//   r = -----------------------------  (eqn 1)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			double q = (line1Point1.Y - line2Point1.Y) * (line2Point2.X - line2Point1.X) - (line1Point1.X - line2Point1.X) * (line2Point2.Y - line2Point1.Y);
			double d = (line1Point2.X - line1Point1.X) * (line2Point2.Y - line2Point1.Y) - (line1Point2.Y - line1Point1.Y) * (line2Point2.X - line2Point1.X);

			//parallel lines so no intersection anywhere in Space(in curved space, maybe, but not here in Euclidian space.)
			if (d == 0)
			{
				return false;
			}

			double r = q / d;

			//	   (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
			//   s = -----------------------------  (eqn 2)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			q = (line1Point1.Y - line2Point1.Y) * (line1Point2.X - line1Point1.X) - (line1Point1.X - line2Point1.X) * (line1Point2.Y - line1Point1.Y);

			double s = q / d;

			//If r>1, P is located on extension of AB
			//If r<0, P is located on extension of BA
			//If s>1, P is located on extension of CD
			//If s<0, P is located on extension of DC

			//The above basically checks if the intersection is located at an extrapolated()
			//point outside of the line segments. To ensure the intersection is only(within)
			//the line segments then the above must all be false, ie r between 0 and 1 and s between 0 and 1.

			if (r < 0 || r > 1 || s < 0 || s > 1)
			{
				return false;
			}

			//Px = Ax + r(Bx - Ax)
			//Py = Ay + r(By - Ay)
			return true;
		}

		// Based on the 2d line intersection method from "comp.graphics.algorithms Frequently Asked Questions"
		//Returns a boolean if they intersect and a reference parameter with the location
		public static bool LineIntersection(PointF line1Point1, PointF line1Point2, PointF line2Point1, PointF line2Point2, ref PointF intersection)
		{
			//	   (Ay-Cy)(Dx-Cx)-(Ax-Cx)(Dy-Cy)
			//   r = -----------------------------  (eqn 1)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			double q = (line1Point1.Y - line2Point1.Y) * (line2Point2.X - line2Point1.X) - (line1Point1.X - line2Point1.X) * (line2Point2.Y - line2Point1.Y);
			double d = (line1Point2.X - line1Point1.X) * (line2Point2.Y - line2Point1.Y) - (line1Point2.Y - line1Point1.Y) * (line2Point2.X - line2Point1.X);

			//parallel lines so no intersection anywhere in Space(in curved space, maybe, but not here in Euclidian space.)
			if (d == 0)
			{
				return false;
			}

			double r = q / d;

			//	   (Ay-Cy)(Bx-Ax)-(Ax-Cx)(By-Ay)
			//   s = -----------------------------  (eqn 2)
			//	   (Bx-Ax)(Dy-Cy)-(By-Ay)(Dx-Cx)

			q = (line1Point1.Y - line2Point1.Y) * (line1Point2.X - line1Point1.X) - (line1Point1.X - line2Point1.X) * (line1Point2.Y - line1Point1.Y);

			double s = q / d;

			//If r>1, P is located on extension of AB
			//If r<0, P is located on extension of BA
			//If s>1, P is located on extension of CD
			//If s<0, P is located on extension of DC

			//The above basically checks if the intersection is located at an extrapolated()
			//point outside of the line segments. To ensure the intersection is only(within)
			//the line segments then the above must all be false, ie r between 0 and 1 and s between 0 and 1.

			if (r < 0 || r > 1 || s < 0 || s > 1)
			{
				return false;
			}

			//Px = Ax + r(Bx - Ax)
			//Py = Ay + r(By - Ay)
			intersection.X = Convert.ToSingle(line1Point1.X + r * (line1Point2.X - line1Point1.X));
			intersection.Y = Convert.ToSingle(line1Point1.Y + r * (line1Point2.Y - line1Point1.Y));
			return true;
		}

		
		public static double GetAngle(float x1, float y1, float x2, float y2)
		{
			return Math.Atan2(y2 - y1,x2 - x1);
		}

		public static double DegreesFromRadians(double Radians)
		{
			return 180 / Math.PI * Radians;
		}

		public static double RadiansFromDegrees(double Degrees)
		{
			return Degrees * Math.PI / 180;
		}

		public static PointF OffsetPoint(PointF location, float dx, float dy)
		{
			return new PointF(location.X - dx, location.Y - dy);
		}

		public static PointF OffsetPoint(PointF location, PointF offset)
		{
			return new PointF(location.X - offset.X, location.Y - offset.Y);
		}

		public static PointF GetMiddlePoint(PointF p1, PointF p2)
		{
			PointF mid = new PointF(Math.Abs(p2.X - p1.X) / 2, Math.Abs(p2.Y - p1.Y) / 2);
			
			mid.X += (p1.X < p2.X) ? p1.X : p2.X;
			mid.Y += (p1.Y < p2.Y) ? p1.Y : p2.Y;

			return mid;
		}

		public static double DistancefromOrigin(PointF point)
		{
			return Math.Sqrt(Math.Pow(point.X,2) + Math.Pow(point.Y,2));
		}

		//Creates a rectangle from 2 points
		public static RectangleF CreateRectangle(PointF A,PointF B)
		{
			RectangleF rect = new RectangleF();

			if (A.X < B.X)
			{
				rect.X = A.X;
				rect.Width = B.X - A.X;
			}
			else
			{
				rect.X = B.X;
				rect.Width = A.X - B.X;
			}
			if (A.Y < B.Y)
			{
				rect.Y = A.Y;
				rect.Height = B.Y - A.Y;
			}
			else
			{
				rect.Y = B.Y;
				rect.Height = A.Y - B.Y;
			}			
			return rect;
		}

		public static Rectangle CreateRectangle(Point A, Point B)
		{
			Rectangle rect = new Rectangle();

			if (A.X < B.X)
			{
				rect.X = A.X;
				rect.Width = B.X - A.X;
			}
			else
			{
				rect.X = B.X;
				rect.Width = A.X - B.X;
			}
			if (A.Y < B.Y)
			{
				rect.Y = A.Y;
				rect.Height = B.Y - A.Y;
			}
			else
			{
				rect.Y = B.Y;
				rect.Height = A.Y - B.Y;
			}			
			return rect;
		}

		public static RectangleF ComplementRectangle(RectangleF a,RectangleF b)
		{
			RectangleF c = new RectangleF();
			c.X = (a.Left < b.Left) ? a.Left : b.Left;
			c.Y = (a.Top < b.Top) ? a.Top : b.Top;
			c.Width = (a.Right > b.Right) ? a.Right - c.X : b.Right - c.X;
			c.Height = (a.Bottom > b.Bottom) ? a.Bottom - c.Y : b.Bottom - c.Y;
			return c;
		}

		public static bool AreAdjacent(RectangleF a, RectangleF b)
		{
			//X are adjacent
			if ((b.X > a.X && a.Right == b.X) || (a.X > b.X && b.Right == a.X))
			{
				//Check y overlap
				if ((a.Y > b.Y && a.Y < b.Bottom) || (b.Y > a.Y && b.Y < a.Bottom)) return true;
			}
			
			//Y are adjacent
			if ((b.Y > a.Y && a.Bottom == b.Y) || (a.Y > b.Y && b.Bottom == a.Y))
			{
				//Check x overlap
				if ((a.X > b.X && a.X < b.Right) || (b.X > a.X && b.X < a.Right)) return true;
			}
			
			return false;
		}

		public static RectangleF ScaleRectangle(RectangleF rect,float sx, float sy)
		{
			return new RectangleF(rect.X * sx,rect.Y * sy, rect.Width * sx,rect.Height * sy);
		}

		public static RectangleF RoundRectangleF(RectangleF rect,DiagramUnit unit)
		{
			int decimals = Units.DecimalsFromUnit(unit);
			
			return new RectangleF(Convert.ToSingle(Math.Round(rect.X,decimals)),Convert.ToSingle(Math.Round(rect.Y,decimals)),Convert.ToSingle(Math.Round(rect.Width,decimals)),Convert.ToSingle(Math.Round(rect.Height,decimals)));
		}

		public static PointF RectangleFarPoint(RectangleF rectangle)
		{
			return new PointF(rectangle.Right,rectangle.Bottom);
		}

		public static RectangleF GetInternalRectangle(GraphicsPath path)
		{
			RectangleF boundingRect = path.GetBounds();
			RectangleF internalRectangle = new RectangleF();
			
			//Set origin to centre of shape
			PointF origin = new PointF(boundingRect.X + (boundingRect.Width / 2),boundingRect.Y + (boundingRect.Height / 2));

			//Set up pen used to test borders
			Pen pen = Component.Instance.DefaultPen;

			//Define the four intercept points
			PointF topLeft,topRight,bottomLeft,bottomRight;
			PointF top,right,left,bottom;

			//Get the four diagonal intercepts from the origin
			topLeft = GetPathIntercept(boundingRect.Location,origin,path,pen,new RectangleF());
			topRight = GetPathIntercept(new PointF(boundingRect.Right,boundingRect.Y),origin,path,pen,new RectangleF());
			bottomLeft = GetPathIntercept(new PointF(boundingRect.X,boundingRect.Bottom),origin,path,pen,new RectangleF());
			bottomRight = GetPathIntercept(new PointF(boundingRect.Right,boundingRect.Bottom),origin,path,pen,new RectangleF());

			//Get the common rectangle from the four points
			internalRectangle.X = ((topLeft.X > bottomLeft.X) ? topLeft.X : bottomLeft.X);
			internalRectangle.Y = ((topLeft.Y > topRight.Y) ? topLeft.Y : topRight.Y);
			internalRectangle.Width = ((topRight.X < bottomRight.X) ? topRight.X - internalRectangle.X: bottomRight.X- internalRectangle.X);
			internalRectangle.Height = ((bottomLeft.Y < bottomRight.Y) ? bottomLeft.Y - internalRectangle.Y : bottomRight.Y- internalRectangle.Y);					

			//Get the four orthogonal intercepts
			top = GetPathIntercept(new PointF(origin.X,boundingRect.Y),origin,path,pen,new RectangleF());
			right = GetPathIntercept(new PointF(boundingRect.Right,origin.Y),origin,path,pen,new RectangleF());
			left = GetPathIntercept(new PointF(boundingRect.X,origin.Y),origin,path,pen,new RectangleF());
			bottom = GetPathIntercept(new PointF(origin.X,boundingRect.Bottom),origin,path,pen,new RectangleF());

			//Apply to internal rectangle
			if (top.Y  > internalRectangle.Y)
			{
				internalRectangle.Y = top.Y;
				internalRectangle.Height -= internalRectangle.Y - top.Y;
			}
			if (right.X < internalRectangle.Right) internalRectangle.Width -= internalRectangle.Right - right.X;
			if (left.X > internalRectangle.X)
			{
				internalRectangle.X = left.X;
				internalRectangle.Width -= internalRectangle.X - left.X;
			}
			if (bottom.Y < internalRectangle.Bottom) internalRectangle.Height -= internalRectangle.Height - bottom.Y;
			
			return internalRectangle;
		}
		
		//Gets the path intercept of a line drawn from inside a graphicspath outwards
		//Path should be flattened before passing to this procedure
		public static PointF GetPathIntercept(PointF startPoint,PointF endPoint,GraphicsPath path,Pen outlinePen,RectangleF internalRectangle)
		{
			float x = 0;
			float y = 0;
			float xStep;
			float yStep;

			bool useInternal;
			//useInternal = ! internalRectangle.IsEmpty;
			useInternal = false;

			//work out interval in steps
			xStep = ((startPoint.X <= endPoint.X) ? 1.0F : -1.0F);
			yStep = ((startPoint.Y <= endPoint.Y) ? 1.0F : -1.0F);

			float gradient = (endPoint.Y - startPoint.Y) / (endPoint.X - startPoint.X);
			float reverseGradient = 1 / gradient;

			//Loop making smaller and smaller step adjustments, longer processing time but more accuracy
			while (xStep != 0)
			{
				//Check for vertical line
				if (startPoint.X != endPoint.X)
				{
					//Step through each value of x, determining y and checking if outline visible
					for (x = startPoint.X; ((startPoint.X < endPoint.X) ? x <= endPoint.X : x >= endPoint.X); x += xStep)
					{
						//calculate Y
						//y = Convert.ToSingle((gradient * (x - startPoint.X)) + startPoint.Y);
						y = (gradient * (x - startPoint.X)) + startPoint.Y;

						//check if we have hit the outline 
						if (path.IsOutlineVisible(x, y, outlinePen)) return new PointF(x, y);
						if (useInternal && internalRectangle.Contains(x,y)) break;
					}
				}
				//Try stepping through each value of y, this is for a line with a high gradient
				//where a small change in x produces a large change in y
				//therefore try small changes in y and work out x

				//Step through each value of y, determining x and checking if outline visible
				if (startPoint.Y != endPoint.Y)
				{
					for (y = startPoint.Y; ((startPoint.Y < endPoint.Y) ? y <= endPoint.Y : y >= endPoint.Y); y += yStep)
					{
						//calculate X
						//x = Convert.ToSingle((reverseGradient * (y - startPoint.Y) + startPoint.X));
						x = (reverseGradient * (y - startPoint.Y) + startPoint.X);

						//check if we have hit the outline 
						if (path.IsOutlineVisible(x, y, outlinePen)) return new PointF(x, y);
						if (useInternal && internalRectangle.Contains(x,y)) break;
					}
				}

				//Make smaller steps if havent found intercept
				xStep += ((startPoint.X <= endPoint.X)? -0.25F : 0.25F);
				yStep += ((startPoint.Y <= endPoint.Y)? -0.25F : 0.25F);
			}

			return startPoint;
		}

		//Creates a copy of a graphics path scaled by x and y
		public static GraphicsPath ScalePath(GraphicsPath path,float sx, float sy)
		{
			if (sx == 1 && sy == 1) return path;

			GraphicsPath newPath = (GraphicsPath) path.Clone();
			Matrix translateMatrix = new Matrix();

			translateMatrix.Scale(sx, sy);
			newPath.Transform(translateMatrix);
			translateMatrix.Dispose();

			return newPath;
		}

		public static void MovePathToOrigin(GraphicsPath path)
		{
			PointF location = path.GetBounds().Location;

			if (location.X != 0 || location.Y != 0)
			{
				Matrix matrix = new Matrix();
				matrix.Translate(-location.X,-location.Y);
				path.Transform(matrix);
				matrix.Dispose();
			}
		}

		public static GraphicsPath RotatePath(GraphicsPath path, float degrees)
		{
			GraphicsPath result = (GraphicsPath) path.Clone(); 
			RectangleF bounds = result.GetBounds();
			PointF center = new PointF(bounds.Width / 2, bounds.Height / 2);

			Matrix matrix = new Matrix();
			matrix.RotateAt(degrees, center);
			result.Transform(matrix);
			return result;
		}

		public static GraphicsPath RotatePath(GraphicsPath path, PointF location, float degrees)
		{
			GraphicsPath result = (GraphicsPath) path.Clone(); 
			RectangleF bounds = result.GetBounds();
			PointF center = new PointF(bounds.Width / 2, bounds.Height / 2);

			Matrix matrix = new Matrix();
			matrix.Translate(location.X, location.Y);
			matrix.RotateAt(degrees, center);
			result.Transform(matrix);
			return result;
		}

		//Return the orientation of a point compared to a center point
		public static PortOrientation GetOrientation(PointF location,PointF center,RectangleF bounds)
		{
			//Calculate the angle between the port and center of the shape
			double angle = DegreesFromRadians(GetAngle(center.X,center.Y,location.X,location.Y));
			double absolute = Math.Abs(angle);

			//Get the limits from the bounds
			double topLeft = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.X,bounds.Y)));
			double topRight = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.Right,bounds.Y)));
			double bottomLeft = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.X,bounds.Bottom)));
			double bottomRight = Math.Abs(DegreesFromRadians(GetAngle(center.X,center.Y,bounds.Right,bounds.Bottom)));
			
			if (absolute >=0 && absolute <=topRight) return PortOrientation.Right;
			if (absolute >= bottomLeft) return PortOrientation.Left;
			if ((absolute >= bottomRight && absolute <= bottomLeft) && angle > 0) return PortOrientation.Bottom;
			return PortOrientation.Top;
		}

		public static PortOrientation GetOrientationOrthogonal(PointF source, PointF previous)
		{
			//Work out old orientation
			if (source.X == previous.X)
			{
				if (source.Y > previous.Y) 
				{
					return PortOrientation.Top;
				}
				else
				{
					return PortOrientation.Bottom;
				}
			}
			else
			{

				if (source.X > previous.X)
				{
					return PortOrientation.Left;
				}
				else
				{
					return PortOrientation.Right;
				}
			}
		}
	}
}

