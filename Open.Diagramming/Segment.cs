using System;
using System.Runtime.Serialization;
using Crainiate.Diagramming.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Segment: ISerializable
	{
		//Property variables
		private Origin mStart;
		private Origin mEnd;
		private TextLabel mLabel;
		private Image mImage;

		private bool mSuspendEvents;
		
		#region Interface

		public event EventHandler SegmentInvalid;

		//Constructors
		public Segment()
		{

		}

		public Segment(Origin start, Origin end)
		{
			SetStart(start);
			SetEnd(end);
		}

		//Deserializes info into a new Line
		protected Segment(SerializationInfo info, StreamingContext context)
		{
			SuspendEvents = true;
	
			SetStart((Origin) info.GetValue("Start",typeof(Origin)));
			SetEnd((Origin) info.GetValue("End",typeof(Origin)));
			if (Serialize.Contains(info,"Label",typeof(TextLabel))) Label = (TextLabel) info.GetValue("Label",typeof(TextLabel)); 
			if (Serialize.Contains(info,"Image",typeof(Image))) Image = (Image) info.GetValue("Image",typeof(Image));

			SuspendEvents = false;	
		}

		//Properties
		//Returns the starting origin for this segment
		public virtual Origin Start
		{
			get
			{
				return mStart;
			}
		}

		//Returns the ending origin for this segment
		public virtual Origin End
		{
			get
			{
				return mEnd;
			}
		}

		//Returns the annotation for this segment
		public virtual TextLabel Label
		{
			get
			{
				return mLabel;
			}
			set
			{
				if (mLabel != null)
				{
					mLabel.LabelInvalid -= new EventHandler(Label_LabelInvalid);
				}

				mLabel = value;
				if (mLabel != null)
				{
					mLabel.LabelInvalid += new EventHandler(Label_LabelInvalid);
				}
				OnSegmentInvalid();
			}
		}

		//Returns the Image object which which displays an image for this shape.
		public Image Image
		{
			get
			{
				return mImage;
			}
			set
			{
				if (mImage != null) 
				{
					mImage.ImageInvalid -= new EventHandler(Image_ImageInvalid);
				}

				mImage = value;
				if (mImage != null) 
				{
					mImage.ImageInvalid += new EventHandler(Image_ImageInvalid);
				}
				OnSegmentInvalid();
			}
		}

		//Determines whether events are prevented from being raised by this class.
		protected internal virtual bool SuspendEvents
		{
			get
			{
				return mSuspendEvents;
			}
			set
			{
				mSuspendEvents = value;
			}
		}

		//Methods
		protected internal void SetStart(Origin value)
		{
			mStart = value;
		}

		protected internal void SetEnd(Origin value)
		{
			mEnd = value;
		}

		//Raises the OriginInvalid event
		protected void OnSegmentInvalid()
		{
			if (!SuspendEvents && SegmentInvalid != null) SegmentInvalid(this,new EventArgs());
		}

		#endregion

		#region Events

		//Handles annotation invalid events
		private void Label_LabelInvalid(object sender, EventArgs e)
		{
			OnSegmentInvalid();
		}

		//Handles image invalid events
		private void Image_ImageInvalid(object sender, EventArgs e)
		{
			OnSegmentInvalid();
		}

		#endregion

		#region Implementation

		public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Start",Start);
			info.AddValue("End",End);
			if (Label !=null) info.AddValue("Label",Label);
			if (Image != null)info.AddValue("Image",Image);
		}

		#endregion
	}
}
