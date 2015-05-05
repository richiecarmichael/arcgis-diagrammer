using System;
using System.Drawing;
using Crainiate.Diagramming.Printing.PaperSizes;

namespace Crainiate.Diagramming
{
	public class Units: IDisposable	
	{
		//Property variables
		private Graphics mGraphics;

		//Constructor
		public Units()
		{
			mGraphics = Component.Instance.CreateGraphics();
		}

		//Properties
		//Sets or gets the graphcis object used to perofrm operations in this class
		public virtual Graphics Graphics
		{
			get
			{
				return mGraphics;
			}
			set
			{
				if (value != null) mGraphics = value;
			}
		}

		//Methods
		public void Dispose()
		{
			mGraphics.Dispose();
		}

		//Returns the pixel scaling factors on both axis
		public static PointF CalculateUnitFactors(Graphics g)
		{
			switch(g.PageUnit) 
			{ 
				case GraphicsUnit.Pixel:
					return new PointF(1f,1f);
				case GraphicsUnit.Display:
					return new PointF(g.DpiX/75f,g.DpiY/75f); 
				case GraphicsUnit.Document:
					return new PointF(g.DpiX/300f,g.DpiY/300f); 
				case GraphicsUnit.Inch: 
					return new PointF(g.DpiX,g.DpiY); 
				case GraphicsUnit.Millimeter: 
					return new PointF(g.DpiX/25.4f,g.DpiY/25.4f); 
				case GraphicsUnit.Point:
					return new PointF(g.DpiX/72f,g.DpiY/72f); 
				default:
					return new PointF(1f,1f);
			}
		}

		//Converts diagram units to graphics units
		public static GraphicsUnit ConvertUnit(DiagramUnit diagramUnit)
		{
			return (GraphicsUnit) Enum.Parse(typeof(GraphicsUnit),diagramUnit.ToString());
		}

		//Convert a point in source units to target units
		public virtual PointF ConvertPoint(PointF point,DiagramUnit source, DiagramUnit target)
		{
			Graphics g = Graphics;

			g.PageUnit = ConvertUnit(source);
			PointF sourceFactors = CalculateUnitFactors(g);

			g.PageUnit = ConvertUnit(target);
			PointF targetFactors = CalculateUnitFactors(g);

			//Convert point by dividing source factors and multiplying target factors
			return new PointF(point.X * sourceFactors.X / targetFactors.X,point.Y * sourceFactors.Y / targetFactors.Y);
		}

		//Convert a point in source units to target units
		public virtual PointF ConvertPoint(PointF point, DiagramUnit target)
		{
			Graphics g = Graphics;

			g.PageUnit = ConvertUnit(target);
			PointF targetFactors = CalculateUnitFactors(g);

			//Convert point by dividing source factors and multiplying target factors
			return new PointF(point.X / targetFactors.X,point.Y / targetFactors.Y);
		}

		//Convert a point in source units to target units
		public virtual RectangleF ConvertRectangle(RectangleF rectangle,DiagramUnit source, DiagramUnit target)
		{
			Graphics g = Graphics;

			g.PageUnit = ConvertUnit(source);
			PointF sourceFactors = CalculateUnitFactors(g);

			g.PageUnit = ConvertUnit(target);
			PointF targetFactors = CalculateUnitFactors(g);

			//Convert rectangle by dividing source factors and multiplying target factors
			return new RectangleF(rectangle.X * sourceFactors.X / targetFactors.X,rectangle.Y * sourceFactors.Y / targetFactors.Y,rectangle.Width * sourceFactors.X / targetFactors.X,rectangle.Height * sourceFactors.Y / targetFactors.Y);
		}

		//Convert a point in source units to target units
		public virtual RectangleF ConvertRectangle(RectangleF rectangle, DiagramUnit target)
		{
			Graphics g = Graphics;

			g.PageUnit = ConvertUnit(target);
			PointF targetFactors = CalculateUnitFactors(g);

			//Convert rectangle by dividing source factors and multiplying target factors
			return new RectangleF(rectangle.X / targetFactors.X,rectangle.Y / targetFactors.Y,rectangle.Width / targetFactors.X,rectangle.Height / targetFactors.Y);
		}

		//Convert a point in source uits to target units
		public virtual SizeF ConvertSize(SizeF size,DiagramUnit source, DiagramUnit target)
		{
			//Return if the same
			if (source == target) return size;

			Graphics g = Graphics;

			g.PageUnit = ConvertUnit(source);
			PointF sourceFactors = CalculateUnitFactors(g);

			g.PageUnit = ConvertUnit(target);
			PointF targetFactors = CalculateUnitFactors(g);

			//Convert point by dividing source factors and multiplying target factors
			return new SizeF(size.Width * sourceFactors.X / targetFactors.X,size.Height * sourceFactors.Y / targetFactors.Y);
		}

		//Convert a point in source uits to target units
		public virtual SizeF ConvertSize(SizeF size, DiagramUnit target)
		{
			Graphics g = Graphics;

			g.PageUnit = ConvertUnit(target);
			PointF targetFactors = CalculateUnitFactors(g);

			//Convert point by dividing source factors and multiplying target factors
			return new SizeF(size.Width / targetFactors.X,size.Height / targetFactors.Y);
		}

		public static PointF RoundPoint(PointF point, int decimals)
		{
			return new PointF(Convert.ToSingle(Math.Round(point.X,decimals)),Convert.ToSingle(Math.Round(point.Y,decimals)));
		}

		public static SizeF RoundSize(SizeF size, int decimals)
		{
			return new SizeF(Convert.ToSingle(Math.Round(size.Width,decimals)),Convert.ToSingle(Math.Round(size.Height,decimals)));
		}

		public static int DecimalsFromUnit(DiagramUnit unit)
		{
			if (unit == DiagramUnit.Pixel || unit == DiagramUnit.Display || unit == DiagramUnit.Point) return 1;
			if (unit == DiagramUnit.Millimeter) return 3;			
			if (unit == DiagramUnit.Document) return 1;
			if (unit == DiagramUnit.Inch) return 4;

			return 1;
		}

		public static string Abbreviate(DiagramUnit unit)
		{
			switch (unit)
			{
				case DiagramUnit.Pixel:
					return "px";
					break;
				case DiagramUnit.Display:
					return "ds";
					break;
				case DiagramUnit.Document:
					return "dc";
					break;
				case DiagramUnit.Inch:
					return "in";
					break;
				case DiagramUnit.Millimeter:
					return "mm";
					break;
				case DiagramUnit.Point:
					return "pt";
					break;
				default:
					return "";
			}
		}
	
		public SizeF GetPaperSize(DiagramUnit unit, Iso name)
		{
			Size mm = new Size();

			if (name == Iso.FourA) mm = new Size(1682,2378);
			if (name == Iso.TwoA) mm = new Size(1189,1682);
			if (name == Iso.A0) mm = new Size(841,1189);
			if (name == Iso.A1) mm = new Size(594,841);
			if (name == Iso.A2) mm = new Size(420,594);
			if (name == Iso.A3) mm = new Size(297,420);
			if (name == Iso.A4) mm = new Size(210,297);
			if (name == Iso.A5) mm = new Size(148,210);
			if (name == Iso.A6) mm = new Size(105,148);
			if (name == Iso.A7) mm = new Size(74,105);
			if (name == Iso.A8) mm = new Size(52,74);
			if (name == Iso.A9) mm = new Size(37,52);
			if (name == Iso.A10) mm = new Size(26,37);

			if (name == Iso.FourB) mm = new Size(2000,2828);
			if (name == Iso.TwoB) mm = new Size(1414,2000);
			if (name == Iso.B0) mm = new Size(1000,1414);
			if (name == Iso.B1) mm = new Size(707,1000);
			if (name == Iso.B2) mm = new Size(500,707);
			if (name == Iso.B3) mm = new Size(353,500);
			if (name == Iso.B4) mm = new Size(250,353);
			if (name == Iso.B5) mm = new Size(176,250);
			if (name == Iso.B6) mm = new Size(125,176);
			if (name == Iso.B7) mm = new Size(88,125);
			if (name == Iso.B8) mm = new Size(62,88);
			if (name == Iso.B9) mm = new Size(44,62);
			if (name == Iso.B10) mm = new Size(31,44);

			if (name == Iso.C0) mm = new Size(917,1297);
			if (name == Iso.C1) mm = new Size(648,917);
			if (name == Iso.C2) mm = new Size(458,648);
			if (name == Iso.C3) mm = new Size(324,458);
			if (name == Iso.C4) mm = new Size(229,324);
			if (name == Iso.C5) mm = new Size(162,229);
			if (name == Iso.C6) mm = new Size(114,162);
			if (name == Iso.C7) mm = new Size(81,114);
			if (name == Iso.C8) mm = new Size(57,81);
			if (name == Iso.C9) mm = new Size(40,57);
			if (name == Iso.C10) mm = new Size(28,40);

			//Convert to diagram unit supplied
			return ConvertSize(mm,DiagramUnit.Millimeter,unit);
		}

		public SizeF GetPaperSize(DiagramUnit unit, Jis name)
		{
			Size mm = new Size();

			if (name == Jis.FourA) mm = new Size(1682,2378);
			if (name == Jis.TwoA) mm = new Size(1189,1682);
			if (name == Jis.A0) mm = new Size(841,1189);
			if (name == Jis.A1) mm = new Size(594,841);
			if (name == Jis.A2) mm = new Size(420,594);
			if (name == Jis.A3) mm = new Size(297,420);
			if (name == Jis.A4) mm = new Size(210,297);
			if (name == Jis.A5) mm = new Size(148,210);
			if (name == Jis.A6) mm = new Size(105,148);
			if (name == Jis.A7) mm = new Size(74,105);
			if (name == Jis.A8) mm = new Size(52,74);
			if (name == Jis.A9) mm = new Size(37,52);
			if (name == Jis.A10) mm = new Size(26,37);

			if (name == Jis.B0) mm = new Size(1030,1456);
			if (name == Jis.B1) mm = new Size(728,1030);
			if (name == Jis.B2) mm = new Size(515,728);
			if (name == Jis.B3) mm = new Size(364,515);
			if (name == Jis.B4) mm = new Size(257,364);
			if (name == Jis.B5) mm = new Size(182,257);
			if (name == Jis.B6) mm = new Size(128,182);
			if (name == Jis.B7) mm = new Size(91,128);
			if (name == Jis.B8) mm = new Size(64,91);
			if (name == Jis.B9) mm = new Size(45,64);
			if (name == Jis.B10) mm = new Size(32,45);

			//Convert to diagram unit supplied
			return ConvertSize(mm,DiagramUnit.Millimeter,unit);
		}
	}
}
