using System;
using System.Collections;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.ComponentModel;
using System.Text;
using System.Xml;

namespace Crainiate.Diagramming.Svg
{
	public class Definition
	{
		//Property variables
		private GraphicsPath mPath; //Cloned path to use

		private bool mIsRectangle;
		private bool mIsEllipse;
		private bool mIsPolygon;
		private bool mIncludeId;

		#region Interface 

		//Constructors
		//Create a new graphicspath holder
		public Definition(GraphicsPath graphicsPath)
		{
			Path = graphicsPath;
			mIncludeId = true;
		}

		public void Rotate(float degrees)
		{
			Matrix translateMatrix = new Matrix();
			RectangleF rectF = mPath.GetBounds();
			
			translateMatrix.RotateAt(degrees, new PointF(rectF.Width / 2, rectF.Height /2));
			mPath.Transform(translateMatrix);
			translateMatrix.Dispose();
		}

		//Gets or sets the internal graphicspath object.
		public GraphicsPath Path
		{
			get
			{
				return mPath;
			}
			set
			{
				if (value != null)
				{
					Matrix translateMatrix = new Matrix();
					RectangleF rectF = new RectangleF();

					mPath = (GraphicsPath) value.Clone();
					rectF = mPath.GetBounds();

					translateMatrix.Translate(-rectF.X, -rectF.Y);
					mPath.Transform(translateMatrix);
					translateMatrix.Dispose();
				}

				mIsEllipse = false;
				mIsPolygon = false;
				mIsRectangle = false;
			}
		}

		public bool IncludeId
		{
			get
			{
				return mIncludeId;
			}
			set
			{
				mIncludeId = value;
			}
		}

		//Determine if the path is a rectangle")]
		public virtual bool IsPathRectangle()
		{
			return IsPathRectangleImplementation(mPath);
		}

		//Determines if path is an ellipse")]
		public virtual bool IsPathEllipse()
		{
			return IsPathEllipseImplementation(mPath);
		}

		//Determines if path is a polygon")]
		public virtual bool IsPathPolygon()
		{
			return IsPathPolygonImplementation(mPath);
		}

		//Extracts a polygon from a valid path.")]
		public virtual string ExtractPolygon()
		{
			return ExtractPolygonImplementation(mPath.PathData);
		}

		//Extracts a path from a valid gdi path")]
		public virtual string ExtractPath()
		{
			return ExtractPathImplementation(mPath.PathData);
		}

		//Extracts a rectangle from a valid path.")]
		public virtual string ExtractRectangle()
		{
			return ExtractRectangleImplementation(mPath);
		}

		//Extracts an ellipse from a valid path.")]
		public virtual string ExtractEllipse()
		{
			return ExtractEllipseImplementation(mPath.PathData);
		}

		public virtual string ExtractDefinition()
		{
			if (IsPathRectangle()) //Check for rectangle
			{
				return ExtractRectangle();
			}
			else if (IsPathEllipse())
			{
				return ExtractEllipse();
			}
			else if (IsPathPolygon())
			{
				return ExtractPolygon();
			}

			return ExtractPath();
		}

		#endregion

		#region  Implementation 

		private bool IsPathRectangleImplementation(GraphicsPath path)
		{
			mIsRectangle = (path.PointCount == 4 && path.PathData.Points[0].X == path.PathData.Points[3].X && path.PathData.Points[0].Y == path.PathData.Points[1].Y && path.PathData.Points[1].X == path.PathData.Points[2].X);
			return mIsRectangle;
		}

		private bool IsPathEllipseImplementation(GraphicsPath path)
		{
			int i = 0;

			if (path.PointCount != 13) return false;

			for (i = 1; i <= 11; i++)
			{
				if (path.PathData.Types[i] != 3) return false;
			}
			mIsEllipse = true;
			return true;
		}

		//Determines if path is a polygon
		private bool IsPathPolygonImplementation(GraphicsPath path)
		{
			int i = 0;

			if (path.PointCount < 3) return false;

			for (i = 1; i <= path.PointCount - 2; i++)
			{
				if (path.PathData.Types[i] != 1) return false;
			}
			mIsPolygon = true;
			return true;
		}

		private string ExtractRectangleImplementation(GraphicsPath path)
		{
			//Check if path is valid
			if (! mIsRectangle)
			{
				if (! IsPathRectangleImplementation(mPath)) throw new DefinitionException("The path cannot be converted to an SVG rectangle");
			}

			StringBuilder def = new StringBuilder();
			RectangleF rect = path.GetBounds();

			def.Append("<rect ");
			if (IncludeId) def.Append("id=\"\" ");
			def.Append("width=\"");
			def.Append(XmlConvert.ToString(rect.Width));
			def.Append("\" height=\"");
			def.Append(XmlConvert.ToString(rect.Height));
			def.Append("\"/>");

			return def.ToString();
		}

		//Extracts an ellipse from a valid path."
		private string ExtractEllipseImplementation(PathData pathData)
		{
			//Check if path is valid
			if (! mIsEllipse)
			{
				if (! IsPathEllipseImplementation(mPath)) throw new Exception("The path cannot be converted to an SVG ellipse");
			}

			StringBuilder def = new StringBuilder();

			def.Append("<ellipse ");
			if (IncludeId) def.Append("id=\"\" ");
			def.Append("cx=\"");
			def.Append(XmlConvert.ToString(pathData.Points[3].X));
			def.Append("\" cy=\"");
			def.Append(XmlConvert.ToString(pathData.Points[0].Y));
			def.Append("\" rx=\"");
			def.Append(XmlConvert.ToString(pathData.Points[3].X));
			def.Append("\" ry=\"");
			def.Append(XmlConvert.ToString(pathData.Points[0].Y));
			def.Append("\"/>");

			return def.ToString();
		}

		private string ExtractPolygonImplementation(PathData pathData)
		{

			//Check if path is valid
			if (! mIsPolygon)
			{
				if (! IsPathPolygonImplementation(mPath)) throw new Exception("The path cannot be converted to an SVG polygon");
			}

			StringBuilder def = new StringBuilder();
			int i = 0;

			def.Append("<polygon ");
			if (IncludeId) def.Append("id=\"\" ");
			def.Append("points=\"");

			for (i = 0; i <= pathData.Points.GetUpperBound(0); i++)
			{
				def.Append(XmlConvert.ToString(Math.Round(pathData.Points[i].X, 2)));
				def.Append(",");
				def.Append(XmlConvert.ToString(Math.Round(pathData.Points[i].Y, 2)));
				def.Append(" ");
			}

			def.Append("\"/>");

			return def.ToString();
		}

		//Extracts a path from a valid gdi path")]
		private string ExtractPathImplementation(PathData pathData)
		{
			StringBuilder def = new StringBuilder();
			int i = 0;

			byte bytType;
			int cCount = 0;

			string strX = null;
			string strY = null;

			def.Append("<path ");
			if (IncludeId) def.Append("id=\"\" ");
			def.Append("fill-rule=\"evenodd\" d=\"");

			for (i = 0; i <= pathData.Points.GetUpperBound(0); i++)
			{
				bytType = pathData.Types[i];
				strX = XmlConvert.ToString(Math.Round(pathData.Points[i].X, 2));
				strY = XmlConvert.ToString(Math.Round(pathData.Points[i].Y, 2));

				if (bytType == 0) //Moveto
				{
					def.Append("M ");
					def.Append(strX);
					def.Append(",");
					def.Append(strY);
					def.Append(" ");
					cCount = 0;
				}
				else
				{
					if (bytType == 1) //Lineto
					{
						def.Append("L ");
						def.Append(strX);
						def.Append(",");
						def.Append(strY);
						def.Append(" ");
						cCount = 0;
					}
					else
					{
						if (bytType > 128) //lineto or curve and closepath
						{
							if (cCount == 0)
							{
							def.Append("L ");
						}
							def.Append(strX);
							def.Append(",");
							def.Append(strY);
							def.Append(" Z ");
							cCount = 0;
						}
						else
						{
							if (cCount == 0) //Curve, in groups of 3
							{
							def.Append("C ");
						}
							def.Append(strX);
							def.Append(",");
							def.Append(strY);
							def.Append(" ");
							cCount += 1;
							if (cCount == 3)
							{
							cCount = 0;
						}
						}
					}

				}
			}

			def.Append("\"/>");

			return def.ToString();
		}

	#endregion

	}

}