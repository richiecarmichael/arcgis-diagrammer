using System;
using System.Drawing;	
using System.Windows.Forms;
using System.ComponentModel;

//Required so that the resource can be found outside this namespace
namespace Crainiate.Diagramming
{
	internal class NavigationResources
	{
	}
}

namespace Crainiate.Diagramming.Navigation
{
	[ToolboxBitmap(typeof(NavigationResources), "Resource.overview.bmp")]
	public class Overview: Diagram
	{
		//Property variables
		private Diagram mDiagram;

		#region Interface
		
		//Constructors
		public Overview()
		{
			SetRender(new OverviewRender());
			Zoom = 10;
		}

		//Properties
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public virtual Diagram Diagram
		{
			get
			{
				return mDiagram;
			}
			set
			{
				if (mDiagram != null)
				{
					mDiagram.ElementInserted -=new ElementsEventHandler(mDiagram_ElementsChanged);
					mDiagram.ElementRemoved -=new ElementsEventHandler(mDiagram_ElementsChanged);

					if (mDiagram is Model)
					{
						Model model = (Model) mDiagram;
						model.UpdateActions -= new UserActionEventHandler(model_UpdateActions);
					}
				}

				if (value == null)
				{
					mDiagram = null;
					Shapes = new Elements(typeof(Shape),"Shape");
					Lines = new Elements(typeof(Line),"Line");
					Layers = new Layers();
				}
				else
				{
					mDiagram = value;
					SetShapes(mDiagram.Shapes);
					SetLines(mDiagram.Lines);
					SetLayers(mDiagram.Layers);
					
					//Set events
					mDiagram.ElementInserted +=new ElementsEventHandler(mDiagram_ElementsChanged);
					mDiagram.ElementRemoved +=new ElementsEventHandler(mDiagram_ElementsChanged);
					
					if (mDiagram is Model)
					{
						Model model = (Model) mDiagram;
						model.UpdateActions += new UserActionEventHandler(model_UpdateActions);
					}

					GetRenderList(ControlRectangle);
				}
			}
		}

		//Methods
		public virtual void Update()
		{
			if (! (Diagram == null) && Diagram.Status == Status.Default) Invalidate();
		}

		public virtual void PositionDiagram(MouseEventArgs e)
		{
			PositionDiagramImplementation(e.X,e.Y);
		}

		public virtual void PositionDiagram(int x, int y)
		{
			PositionDiagramImplementation(x,y);
		}

		#endregion

		#region Events

		private void model_UpdateActions(object sender, UserActionEventArgs e)
		{
			Invalidate();
		}

		#endregion

		#region Implementation

		private void PositionDiagramImplementation(int x, int y)
		{
			if (mDiagram == null) return;

			Diagram diagram = (Diagram) mDiagram;

			PointF diagramPoint = PointToDiagram(x,y);
			int intX = 0;
			int intY = 0;
			double zoom = diagram.Zoom / 100;

			intX = Convert.ToInt32(diagramPoint.X - ((diagram.Width / 2) * zoom));
			intY = Convert.ToInt32(diagramPoint.Y - ((diagram.Height / 2) * zoom));

			if (intX < 0) intX = 0;
			if (intY < 0) intY = 0;

			diagram.AutoScrollPosition = new Point(intX, intY);
		}

		#endregion

		//Handles elements being added or removed from the target diagram
		private void mDiagram_ElementsChanged(object sender, ElementsEventArgs e)
		{
			GetRenderList(ControlRectangle);
			Invalidate();
		}
	}
}
