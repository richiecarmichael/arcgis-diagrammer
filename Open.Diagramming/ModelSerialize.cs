using System;
using System.Drawing;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Editing;

namespace Crainiate.Diagramming.Serialization
{
	public class ModelSerialize: DiagramSerialize
	{
		//Property variables
		private Runtime mRuntime;

		//Constructors
		public ModelSerialize(): base()
		{
		}

		//Properties
		public Runtime Runtime
		{
			get
			{
				return mRuntime;
			}
		}

		//Implement ISerializable
		public override void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
		{
			base.GetObjectData(obj,info,context);

			Model model = (Model) obj;
			
			info.AddValue("AlignGrid",model.AlignGrid);
			info.AddValue("DragScroll",model.DragScroll);
			info.AddValue("DragSelect",model.DragSelect);
			info.AddValue("DrawGrid",model.DrawGrid);
			info.AddValue("DrawPageLines",model.DrawPageLines);
			info.AddValue("DrawSelections",model.DrawSelections);
			info.AddValue("GridColor",model.GridColor.ToArgb().ToString());
			info.AddValue("GridSize",Serialize.AddSize(model.GridSize));
			info.AddValue("GridStyle",Convert.ToInt32(model.GridStyle).ToString());
			info.AddValue("PageLineSize",Serialize.AddSizeF(model.PageLineSize));

			info.AddValue("Runtime",model.Runtime);
		}

		public override object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
		{
			base.SetObjectData(obj,info,context,selector);

			Model model = (Model) obj;
			
			model.SuspendEvents = true;
			model.Suspend();

			model.AlignGrid = info.GetBoolean("AlignGrid");
			model.DragScroll = info.GetBoolean("DragScroll");
			model.DrawGrid = info.GetBoolean("DrawGrid");
			model.DrawPageLines = info.GetBoolean("DrawPageLines");
			model.DrawSelections = info.GetBoolean("DrawSelections");
			model.GridColor = Color.FromArgb(Convert.ToInt32(info.GetString("GridColor")));
			model.GridSize = Serialize.GetSize(info.GetString("GridSize"));
			model.GridStyle = (GridStyle) Enum.Parse(typeof(GridStyle), info.GetString("GridStyle"));
			model.PageLineSize = Serialize.GetSizeF(info.GetString("PageLineSize"));

			if (Serialize.Contains(info,"DragSelect")) model.DragSelect = info.GetBoolean("DragSelect");

			mRuntime = (Runtime) info.GetValue("Runtime",typeof(Runtime));

			model.SuspendEvents = false;
			model.Resume();
			
			return model;
		}
	}
}
