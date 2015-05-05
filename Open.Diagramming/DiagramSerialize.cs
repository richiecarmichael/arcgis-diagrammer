using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Layouts;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class DiagramSerialize: ISerializationSurrogate
	{
		private Elements mShapes;
		private Elements mLines;
		private Layers mLayers;

		//Constructors
		public DiagramSerialize()
		{

		}

		//Properties
		public virtual Elements Shapes
		{
			get
			{
				return mShapes;
			}
		}

		public virtual Elements Lines
		{
			get
			{
				return mLines;
			}
		}

		public virtual Layers Layers
		{
			get
			{
				return mLayers;
			}
		}

		//Implement ISerializeSurrogate
		public virtual void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			Diagram diagram = (Diagram) obj;
			
			info.AddValue("Shapes",diagram.Shapes,typeof(Elements));
			info.AddValue("Lines",diagram.Lines,typeof(Elements));
			info.AddValue("Layers",diagram.Layers,typeof(Layers));
			info.AddValue("DiagramSize",Serialize.AddSize(diagram.DiagramSize));
			info.AddValue("Zoom",diagram.Zoom);
			info.AddValue("ShowTooltips",diagram.ShowTooltips);
			info.AddValue("Paged",diagram.Paged);
			info.AddValue("CheckBounds",diagram.CheckBounds);
			info.AddValue("Margin",diagram.Margin);
			info.AddValue("WorkspaceColor",diagram.WorkspaceColor.ToArgb().ToString());
			info.AddValue("Animator",diagram.Animator);
		}

		public virtual object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			Diagram diagram = (Diagram) obj;

			diagram.SuspendEvents = true;
			diagram.Suspend();
			
			mShapes = (Elements) info.GetValue("Shapes",typeof(Elements));
			mLines = (Elements) info.GetValue("Lines",typeof(Elements));
			mLayers = (Layers) info.GetValue("Layers",typeof(Layers));
			
			//Diagram is created without a constructor, so need to do some initialization
			diagram.SetRender(new Render());

			diagram.DiagramSize = Serialize.GetSize(info.GetString("DiagramSize"));
			diagram.Zoom = info.GetSingle("Zoom");
			diagram.ShowTooltips = info.GetBoolean("ShowTooltips");
			diagram.Paged = info.GetBoolean("Paged");
			diagram.CheckBounds = info.GetBoolean("CheckBounds");
			diagram.Margin = (Margin) info.GetValue("Margin",typeof(Margin));
			diagram.WorkspaceColor = Color.FromArgb(Convert.ToInt32(info.GetString("WorkspaceColor")));
			if (Serialize.Contains(info,"Animator", typeof(Animator))) diagram.Animator = (Animator) info.GetValue("Animator", typeof(Animator));

			diagram.Resume();
			diagram.SuspendEvents = false;
			return diagram;
		}

		public virtual void UpdateObjectReferences()
		{
			//Relink layers
			foreach (Layer layer in mLayers)
			{	
				foreach (Element element in layer.Elements.Values)
				{
					element.SetLayer(layer);
				}
			}
		}
	}
}
