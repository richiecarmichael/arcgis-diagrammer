using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class TableRow: TableItem, ISerializable, ICloneable
	{
		//Property variables
		private Crainiate.Diagramming.Image mImage;
		
		//Working variables

		#region Interface
		
		//Events
				
		//Constructors
		public TableRow()
		{
			
		}

		public TableRow(TableRow prototype): base(prototype)
		{
			mImage = prototype.Image;
		}

		protected TableRow(SerializationInfo info, StreamingContext context): base(info,context)
		{
			SuspendEvents = true;
			Image = (Image) info.GetValue("Image",typeof(Image));
			SuspendEvents = false;
		}

		//Properties
		public virtual Crainiate.Diagramming.Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				mImage = value;
				OnTableItemInvalid();
			}
		}

		#endregion

		#region Overrides
		
		public override void Render(Graphics graphics, IRender render)
		{
			byte opacity = 100;
			if (Table != null) opacity = Table.Opacity;

			//Draw indent
			SolidBrush brush = new SolidBrush(render.AdjustColor(Backcolor,1,opacity));
			brush.Color = Color.FromArgb(brush.Color.A /2, brush.Color);
			graphics.FillRectangle(brush,0,Rectangle.Top,Indent,Rectangle.Height);

			//Draw image
			float imageWidth = 0;
			if (Image != null)
			{
				System.Drawing.Image bitmap = Image.Bitmap;

				//Work out position of image
				float imageTop = (Rectangle.Height - bitmap.Height) / 2;
				if (imageTop < 0) imageTop = 0;
				
				imageWidth = bitmap.Width;
				graphics.DrawImageUnscaled(bitmap,Convert.ToInt32(Indent),Convert.ToInt32(Rectangle.Top+imageTop));
			}

			//Draw text
			RectangleF textRectangle = new RectangleF(Indent+imageWidth+4,Rectangle.Top,Rectangle.Width - Indent -4,Rectangle.Height);
			brush = new SolidBrush(render.AdjustColor(Forecolor,1,opacity));
			StringFormat format = new StringFormat();
			format.LineAlignment = StringAlignment.Center;
			format.FormatFlags = StringFormatFlags.NoWrap;
			graphics.DrawString(Text,Component.Instance.GetFont(FontName,FontSize,FontStyle),brush,textRectangle,format);
		}

		public override void RenderSelection(Graphics graphics, IRender render)
		{
			SmoothingMode mode = graphics.SmoothingMode;
			graphics.SmoothingMode = SmoothingMode.None;

			Pen pen = Component.Instance.SelectionPen;
			SolidBrush brush = Component.Instance.SelectionBrush;
			RectangleF rect = new RectangleF(Rectangle.X+2,Rectangle.Y,Rectangle.Width-4,Rectangle.Height);
			graphics.FillRectangle(brush,rect);
			graphics.DrawRectangle(pen,rect.X,rect.Y,rect.Width,rect.Height);

			graphics.SmoothingMode = mode;
		}

		#endregion

		#region Events
		
		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Image",Image);	
			base.GetObjectData(info,context);
		}	
	
		public virtual object Clone()
		{
			return new TableRow(this);
		}

		#endregion

	}
}
