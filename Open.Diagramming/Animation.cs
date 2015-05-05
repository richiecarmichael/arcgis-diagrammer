using System;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	public delegate void AnimateElementEventHandler(object sender, Element e);
	
	[Serializable]
	public class Animation: ISerializable, ICloneable
	{
		//Private variables
		private int mTotalFrames; //The frames in this animation
		private int mCurrentFrame; //The current frame the animation is on
		private float mFramesPerSecond; //The number of frames per second in the animation
		private long mNextTick; //The next reference tick from the animator
		private bool mEnabled; //Whether the animation is currently enabled
		private Animator mAnimator; //The animator animating this animation

		#region Interface
		
		//Events
		public event AnimateElementEventHandler AnimateElement;
		
		//Constructors
		public Animation()
		{
			TotalFrames = 1;
			CurrentFrame = 1;
			FramesPerSecond = 1;
			Enabled = true;
		}

		//Creates a new element by copying an existing element
		public Animation(Animation prototype)
		{
			mTotalFrames = prototype.TotalFrames;
			mFramesPerSecond = prototype.FramesPerSecond;
		}

		//Creates a new element from the supplied XML.
		protected internal Animation(SerializationInfo info, StreamingContext context)
		{
			mTotalFrames = info.GetInt32("TotalFrames");
			mCurrentFrame = info.GetInt32("CurrentFrame");
			mFramesPerSecond = info.GetSingle("FramesPerSecond");
			mNextTick = info.GetInt64("NextTick");
			mEnabled = info.GetBoolean("Enabled");
		}

		//Properties
		public virtual bool Enabled
		{
			get
			{
				return mEnabled;
			}
			set
			{
				mEnabled = true;
			}
		}

		public virtual int TotalFrames
		{
			get
			{
				return mTotalFrames;
			}
			set
			{
				if (value == 0) throw new ArgumentException("TotalFrames cannot be zero.");
				mTotalFrames = value;
			}
		}

		public virtual int CurrentFrame
		{
			get
			{
				return mCurrentFrame;
			}
			set
			{
				if (value == 0) throw new ArgumentException("CurrentFrame cannot be zero.");
				if (value > TotalFrames) throw new ArgumentException("CurrentFrame cannot be greater than Total Frames.");
				mCurrentFrame = value;
			}
		}

		public virtual float FramesPerSecond
		{
			get
			{
				return mFramesPerSecond;
			}
			set
			{
				if (value <= 0) throw new ArgumentException("Frames Per Second cannot be zero or less than zero.");
				mFramesPerSecond = value;
			}
		}

		public virtual long NextTick
		{
			get
			{
				return mNextTick;
			}
		}

		public virtual Animator Animator
		{
			get
			{
				return mAnimator;
			}
		}

		//Methods
		public virtual void IncrementFrame()
		{
			CurrentFrame = (CurrentFrame == TotalFrames) ? 1 : CurrentFrame + 1;
		}

		public virtual void Reset()
		{
			mCurrentFrame = 1;
			mNextTick = 0;
		}

		protected internal virtual void OnAnimateElement(Element e)
		{
			if (AnimateElement != null) AnimateElement(this, e);
		}

		protected internal virtual void SetNextTick(long tick)
		{
			mNextTick = tick;
		}

		protected internal virtual void SetAnimator(Animator animator)
		{
			mAnimator = animator;
		}

		#endregion

		#region Implementation

		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("TotalFrames", TotalFrames);
			info.AddValue("CurrentFrame", CurrentFrame);
			info.AddValue("FramesPerSecond", FramesPerSecond);
			info.AddValue("NextTick", NextTick);
			info.AddValue("Enabled", Enabled);
		}

		//Clones an Animate class instance
		public virtual object Clone()
		{
			return new Animation(this);
		}

		#endregion


	}
}
