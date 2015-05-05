using System;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class Animator: ISerializable
	{
		private System.Windows.Forms.Timer mTimer;
		private int mFrameRate;

		//Working variables
		private Diagram mDiagram;
		private long mTick;
		private long mFinalTick;

		#region Interface

		//Events
		[Category("Behavior"),Description("Occurs when a frame is animated.")]
		public event EventHandler Animate;

		[Category("Behavior"),Description("Occurs when the animator is started.")]
		public event EventHandler AnimatorStart;

		[Category("Behavior"),Description("Occurs when the animator finishes animation.")]
		public event EventHandler AnimatorEnd;

		[Category("Behavior"),Description("Occurs when the animator is paused.")]
		public event EventHandler AnimatorPause;

		[Category("Behavior"),Description("Occurs when animator is restarted.")]
		public event EventHandler AnimatorRestart;

		//Constructor
		public Animator(Diagram diagram)
		{
			if (diagram == null) throw new ArgumentNullException("Diagram may not be a null reference.");
			SetDiagram(diagram);
			mFrameRate = 12;
		}

		//Creates a new element from the supplied XML.
		protected internal Animator(SerializationInfo info, StreamingContext context)
		{
			mFrameRate = info.GetInt32("FrameRate");
			mTick = info.GetInt32("CurrentTick");
			mFinalTick = info.GetInt32("FinalTick");
		}

		//Properties
		public virtual int FrameRate
		{
			get
			{
				return mFrameRate;
			}
			set
			{
				if (value == 0) throw new ArgumentException("Frame Rate cannot be zero.");
				mFrameRate = value;
			}
		}

		public virtual long CurrentTick
		{
			get
			{
				return mTick;
			}
		}

		public virtual long FinalTick
		{
			get
			{
				return mFinalTick;
			}
			set
			{
				mFinalTick = value;
			}
		}

		public virtual Diagram Diagram
		{
			get
			{
				return mDiagram;
			}
		}

		//Methods
		//Starts or resumes from a pause
		public virtual void Start()
		{
			if (mTimer == null)
			{
				mTimer = new System.Windows.Forms.Timer();
				mTimer.Interval = 1000 / FrameRate;
				mTimer.Tick += new EventHandler(Timer_Tick);
			}
			
			OnAnimatorStart();
			mTimer.Start();
		}

		//Ends animation
		public virtual void Stop()
		{
			mTimer.Stop();
			mTimer.Dispose();
			
			OnAnimatorEnd();
			ResetElements();

			mTick = 0;
		}

		public virtual void Pause()
		{
			mTimer.Stop();
			OnAnimatorPause();
		}

		public virtual void Restart()
		{
			mTick = 0;
			OnAnimatorRestart();
			ResetElements();
			Start();
		}

		protected internal void SetDiagram(Diagram diagram)
		{
			mDiagram = diagram;
		}

		protected virtual void OnAnimate()
		{
			if (Animate != null) Animate(this, EventArgs.Empty);
		}

		protected virtual void OnAnimatorStart()
		{
			if (AnimatorStart != null) AnimatorStart(this, EventArgs.Empty);
		}
		protected virtual void OnAnimatorEnd()
		{
			if (AnimatorEnd != null) AnimatorEnd(this, EventArgs.Empty);
		}
		protected virtual void OnAnimatorPause()
		{
			if (AnimatorPause != null) AnimatorPause(this, EventArgs.Empty);
		}
		protected virtual void OnAnimatorRestart()
		{
			if (AnimatorRestart != null) AnimatorRestart(this, EventArgs.Empty);
		}

		#endregion

		#region Events

		private void Timer_Tick(object sender, EventArgs e)
		{
			if (Diagram.Status == Status.Default)
			{
				//Check if we have reached the final tick
				if (FinalTick > 0 && mTick == FinalTick) 
				{
					Stop();
					return;
				}

				Diagram.SetStatus(Status.Animate);
				OnAnimate();

				try
				{
					AnimateElements();
			
					//Check if we go over maxiumum stops
					if (mTick == long.MaxValue)
					{
						Restart();
					}
					else
					{
						mTick += 1;
					}
				}
				catch (Exception ex)
				{
					Diagram.SetStatus(Status.Default);
					throw ex;
				}

				Diagram.SetStatus(Status.Default);
			}
		}

		#endregion

		#region Implementation

		//Implement ISerializable
		public virtual void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
		{
			info.AddValue("FrameRate", FrameRate);
			info.AddValue("CurrentTick", CurrentTick);
			info.AddValue("FinalTick", FinalTick);
		}
		
		protected virtual void AnimateElements()
		{
			Diagram.Suspend();

			foreach (Element element in Diagram.RenderList)
			{
				if (element is IAnimatable)
				{
					IAnimatable animatable = (IAnimatable) element;
					
					if (animatable.Animation != null)
					{
						Animation animate = animatable.Animation;

						//Set the animator reference
						if (animate.Animator == null) animate.SetAnimator(this);

						//Animate shape if stop is zero or stop is set to this stop
						if (animate.NextTick <= mTick)
						{
							animate.OnAnimateElement(element);
							animate.IncrementFrame();
							animate.SetNextTick(mTick + Convert.ToInt64((FrameRate / animate.FramesPerSecond)));
						}
					}
				}
			}

			Diagram.Resume();
			Diagram.Invalidate();
		}

		private void ResetElements()
		{
			foreach (Element element in Diagram.RenderList)
			{
				if (element is IAnimatable)
				{
					IAnimatable animatable = (IAnimatable) element;
					if (animatable.Animation != null) animatable.Animation.Reset();;
				}
			}
		}

		
		#endregion
	}
}
