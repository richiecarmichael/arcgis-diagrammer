using System;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Crainiate.Diagramming
{
	public class Metafile
	{
		//Property variables
		private System.Drawing.Imaging.Metafile mMetafile;

		#region Interface

		//Constructors
		public Metafile()
		{
			CreateInternalMetafile(EmfType.EmfPlusOnly);	
		}

		public Metafile(EmfType type)
		{
			CreateInternalMetafile(type);	
		}

		//Methods
		public virtual void AddDiagram(Diagram diagram)
		{
			AddDiagramToFile(diagram);
		}

		public virtual void Save(string filename)
		{
			mMetafile.Save(filename);
		}

		public virtual void Save(FileStream stream)
		{
			mMetafile.Save(stream,ImageFormat.Emf);
		}

		protected virtual System.Drawing.Imaging.Metafile GetInternalMetafile()
		{
			return mMetafile;
		}

		#endregion

		#region Implementation

		private void CreateInternalMetafile(EmfType type)
		{
			//Create a temporary bitmap to get an HDC
			Graphics graphics = Component.Instance.CreateGraphics();
			IntPtr hDC = graphics.GetHdc();

			//Create metafile based on type and get graphics handle
			mMetafile = new System.Drawing.Imaging.Metafile(hDC, type);
			graphics.ReleaseHdc(hDC);

			graphics.Dispose();
		}

		private void AddDiagramToFile(Diagram diagram)
		{
			Graphics graphics = Graphics.FromImage(mMetafile);

			//Set the renderlists to the whole diagram
			Rectangle renderRect = new Rectangle(new Point(0,0),diagram.DiagramSize);
			diagram.GetRenderList(renderRect);

			//Set the render rectangle
			diagram.Render.RenderRectangle = renderRect;
			diagram.Render.RenderDiagramElements(graphics);

			graphics.Dispose();
		}
		#endregion

	}

}
