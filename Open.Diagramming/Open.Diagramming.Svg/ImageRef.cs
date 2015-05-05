using System;
using System.Collections;
using System.Data;
using System.Drawing;
using System.ComponentModel;
using System.Text;

namespace Crainiate.Diagramming.Svg
{
	public class ImageRef
	{
		private Image mImage;
		private SolidElement mSolid;

		#region  Interface 

		public ImageRef()
		{

		}

		public ImageRef(SolidElement solid, Image image)
		{
			mImage = image;
			mSolid = solid;
		}

		//Extracts SVG image information for a given image
		public virtual string ExtractImage()
		{
			return ExtractImageImplementation();
		}

		//Sets or gets the ERM Image associated with this class
		public virtual Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				mImage = value;
			}
		}

		//Sets or gets the shape containing this image
		public virtual SolidElement SolidElement
		{
			get
			{
				return mSolid;
			}
			set
			{
				mSolid = value;
			}
		}

		#endregion

		#region  Implementation 

		private string ExtractImageImplementation()
		{
			if (mImage == null || mSolid == null) return null;

			StringBuilder builder = new StringBuilder();
			string newPath = mImage.Path;

			PointF location = new PointF();

			location = mImage.Location;

			builder.Append("<image id=\"");
			if (mSolid != null) builder.Append(mSolid.Key);
			builder.Append("\"");

			//Add class if clipping required
			if (mSolid.Clip) builder.Append(" class=\"\"");
		
			builder.Append(" x = \"");
			builder.Append(location.X.ToString());
			builder.Append("\" y=\"");
			builder.Append(location.Y.ToString());
			builder.Append("\" width=\"");
			builder.Append(mImage.Bitmap.Width.ToString());
			builder.Append("\" height=\"");
			builder.Append(mImage.Bitmap.Height.ToString());
			builder.Append("\" href=\"");
			builder.Append(newPath);
			builder.Append("\"/>");

			return builder.ToString();
		}

	#endregion

	}

}