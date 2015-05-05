using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.ComponentModel;

namespace Crainiate.Diagramming.Printing
{
	[ToolboxItem(false)]
	public class PrintDocument : System.Drawing.Printing.PrintDocument
	{
		//Property variables
		private Diagram mDiagram;
		private PrintRender mRender;
		private PageNumberStyle mPageNumberStyle;
		private BorderStyle mBorderStyle;
		private float mScale;
		private bool mScaleToFit;
		private bool mClip;

		//Working variables
		private static int mHorizonalPages;
		private int mVerticalPages;
		private int mLastPage;
		private int mTotalPages;

		#region Interface 

		//Constructors
		public PrintDocument(Diagram Diagram)
		{
			this.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(PrintPageHandlerRender);
			
			mDiagram = Diagram;
			this.Render = new PrintRender();
			PageNumberStyle = PageNumberStyle.ColumnRow;
			BorderStyle = BorderStyle.Dot;
			Scale = 100;
			mClip = true;

			SetRenderLists();
		}

		//Properties
		//Gets or sets the style for showing printed page numbers
		[Description("Sets or gets the style of the numbers printed on each page.")]
		public virtual PageNumberStyle PageNumberStyle
		{
			get
			{
				return mPageNumberStyle;
			}
			set
			{
				mPageNumberStyle = value;
			}
		}

		//Gets or sets the style for drawing page borders
		[Description("Sets or gets the style of the border around each page.")]
		public virtual BorderStyle BorderStyle
		{
			get
			{
				return mBorderStyle;
			}
			set
			{
				mBorderStyle = value;
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets or gets the Diagram used for printing.")]
		public virtual Diagram Diagram
		{
			get
			{
				return mDiagram;
			}
			set
			{
				mDiagram = value;
				SetRenderLists();
			}
		}

		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Description("Sets or gets the render used to render the print document.")]
		public virtual PrintRender Render
		{
			get
			{
				return mRender;
			}
			set
			{
				mRender = value;
				SetRenderLists();
			}
		}

		[Description("Sets or gets the scale of the print out.")]
		public virtual float Scale
		{
			get
			{
				return mScale;
			}
			set
			{
				mScale = value;
			}
		}

		[Description("Determines if blank columns and rows are removed from the print out.")]
		public virtual bool Clip
		{
			get
			{
				return mClip;
			}
			set
			{
				mClip = value;
			}
		}

		[Description("Determines whether the document should be made to fit on a page.")]
		public virtual bool ScaleToFit
		{
			get
			{
				return mScaleToFit;
			}
			set
			{
				mScaleToFit = value;
			}
		}

		[Description("Determines whether the document is printed with filled shapes.")]
		public virtual bool DrawBackground 
		{
			get
			{
				return mRender.DrawBackground;
			}
			set
			{
				mRender.DrawBackground = value;
			}
		}

		[Description("Determines whether the document is printed with shadows.")]
		public virtual bool DrawShadows
		{
			get
			{
				return mRender.DrawShadows;
			}
			set
			{
				mRender.DrawShadows = value;
			}
		}

		[Description("Determines whether the document is printed without color.")]
		public virtual bool Greyscale
		{
			get
			{
				return mRender.Greyscale;
			}
			set
			{
				mRender.Greyscale = value;
			}
		}

		//Methods
		[Description("Shows the document in print preview mode .")]
		public virtual void PrintPreview()
		{
			if (mDiagram == null) throw new PrintDocumentException("A Diagram reference was not set for this PrintDocument.");

			System.Windows.Forms.PrintPreviewDialog objDialog = new System.Windows.Forms.PrintPreviewDialog();
			objDialog.Document = this;

			//Set the zoom
			mRender.Zoom = mDiagram.Zoom;

			objDialog.ShowDialog();
			objDialog.Dispose();
		}

		[Description("Shows the document in print preview mode at the specified location and size.")]
		public virtual void PrintPreview(Point Location, Size Size)
		{
			if (mDiagram == null) throw new PrintDocumentException("A Diagram reference was not set for this PrintDocument.");

			System.Windows.Forms.PrintPreviewDialog objDialog = new System.Windows.Forms.PrintPreviewDialog();
			objDialog.Document = this;
			objDialog.Location = Location;
			objDialog.Size = Size;

			//Set the zoom
			mRender.Zoom = mDiagram.Zoom;

			objDialog.ShowDialog();
			objDialog.Dispose();
		}

		#endregion

		#region Implementation

		protected virtual void PrintPageHandlerRender(object sender, System.Drawing.Printing.PrintPageEventArgs e)
		{
			if (mDiagram == null) return;

			Rectangle renderRectangle = new Rectangle();

			int column = 0;
			int row = 0;
			float scale = Scale / 100;

			//Calculate defaults if this is the first page we are printing
			if (mLastPage == 0)
			{
				//Set the scale if ScaleToFit is set to true
				if (ScaleToFit)
				{
					float rx = (Convert.ToSingle(e.MarginBounds.Width) / Convert.ToSingle(Diagram.DiagramSize.Width));
					float ry = (Convert.ToSingle(e.MarginBounds.Height) / Convert.ToSingle(Diagram.DiagramSize.Height));
					
					scale = (rx < ry) ? rx : ry;
					Scale = scale;
				}
								
				SizeF scaledSize = new SizeF(mDiagram.DiagramSize.Width * scale,mDiagram.DiagramSize.Height * scale);

				//Consider the visible boundary of the elements in the diagram if truncating
				if (mClip)
				{
					RectangleF bounds = Render.Elements.GetBounds();

                    //Only consider if there are elements
                    if (!bounds.IsEmpty)
                    {
                        RectangleF visible = new RectangleF(0, 0, bounds.Right, bounds.Bottom);

                        scaledSize = new SizeF(visible.Width * scale, visible.Height * scale);
                    }
				}
								
				//Get the horizontal and vertical scaled pages
				mHorizonalPages = Convert.ToInt32(Math.Floor(scaledSize.Width / e.MarginBounds.Width));
				if (scaledSize.Width % e.MarginBounds.Width > 0) mHorizonalPages += 1;

				mVerticalPages = Convert.ToInt32(Math.Floor(scaledSize.Height / e.MarginBounds.Height));
				if (scaledSize.Height % e.MarginBounds.Height > 0) mVerticalPages += 1;

				if (PrinterSettings.PrintRange == PrintRange.SomePages)
				{
					if (PrinterSettings.FromPage > 1)
					{
						mLastPage = this.PrinterSettings.FromPage - 1;
					}
				}

				mTotalPages = mHorizonalPages * mVerticalPages;
			}

			column = mLastPage % mHorizonalPages;
			row = (mLastPage / mHorizonalPages);

			//Calculate the render rectangle for this page
			renderRectangle.Location = new Point(column * e.MarginBounds.Width, row * e.MarginBounds.Height);
			renderRectangle.Width = e.MarginBounds.Width;
			renderRectangle.Height = e.MarginBounds.Height;

			e.Graphics.Clip = new Region(e.MarginBounds);
			e.Graphics.TranslateTransform(-renderRectangle.X + e.MarginBounds.X, -renderRectangle.Y + e.MarginBounds.Y);
			
			//Perform any scaling if required
			e.Graphics.ScaleTransform(scale,scale);

			//mRender.MarginBounds = e.MarginBounds;
			mRender.RenderRectangle = renderRectangle;
			mRender.RenderDiagramElements(e.Graphics);

			//Draw row and column information
			StringFormat stringFormat = new StringFormat();
			RectangleF pageNumberRect = new RectangleF(e.MarginBounds.Left, e.MarginBounds.Bottom, e.MarginBounds.Width, e.PageBounds.Height - e.MarginBounds.Top - e.MarginBounds.Height);

			stringFormat.Alignment = StringAlignment.Center;
			stringFormat.LineAlignment = StringAlignment.Center;

			//Add 1 to row and column for printing
			row += 1;
			column += 1;

			e.Graphics.Clip = new Region();
			e.Graphics.ResetTransform();

			string pageNumber = GetPageNumber(row, column, mLastPage + 1);
			if (pageNumber != "")
			{
				e.Graphics.DrawString(pageNumber, mDiagram.Font, new SolidBrush(Color.Black), pageNumberRect, stringFormat);
			}

			//Draw a border
			if (mBorderStyle != BorderStyle.None)
			{
				Pen pen = GetBorderPen();
				e.Graphics.DrawRectangle(pen, e.MarginBounds);
				pen.Dispose();
			}

			//Calculate if another page needs printing
			if (mLastPage < mTotalPages - 1)
			{
				e.HasMorePages = true;
				mLastPage += 1;

				if (this.PrinterSettings.PrintRange == PrintRange.SomePages)
				{
					e.HasMorePages = ! (mLastPage >= this.PrinterSettings.ToPage);
				}
			}
			else
			{
				mLastPage = 0;
				mTotalPages = 0;
			}
		}

		protected virtual string GetPageNumber(int intRow, int intColumn, int intSequence)
		{
			switch (mPageNumberStyle)
			{
				case PageNumberStyle.ColumnRow:
					return intColumn.ToString() + "," + intRow.ToString();
					break;
				case PageNumberStyle.RowColumn:
					return intRow.ToString() + "," + intColumn.ToString();
					break;
				case PageNumberStyle.Sequence:
					return intSequence.ToString();
					break;
			}
			return "";
		}

		//Add all shapes and lines to renderlist
		private void SetRenderLists()
		{
			Render.Elements = new RenderList();
			Render.Layers = Diagram.Layers;
			
			foreach (Shape shape in Diagram.Shapes.Values)
			{
				Render.Elements.Add(shape);
			}

			foreach (Line line in Diagram.Lines.Values)
			{
				Render.Elements.Add(line);
			}
			Render.Elements.Sort();
		}

		private Pen GetBorderPen()
		{
			Pen pen = new Pen(Color.Black);

			switch (mBorderStyle)
			{
				case BorderStyle.Dash:
					pen.DashStyle = DashStyle.Dash;
					break;
				case BorderStyle.Dot:
					pen.DashStyle = DashStyle.Dot;
					break;
				case BorderStyle.Solid:
					pen.DashStyle = DashStyle.Solid;
					break;
			}

			return pen;
		}

		#endregion
	}
}