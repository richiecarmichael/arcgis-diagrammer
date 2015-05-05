using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming
{
	[Serializable]
	public class ComplexShape: Shape, ISerializable, ICloneable
	{
		//Property variables
		private Elements mChildren;

		//Working variables
		private RenderList mRenderList;

		#region Interface

		//Constructors
		public ComplexShape()
		{
			SuspendEvents = true;
			
			Children = new Elements(typeof(SolidElement),"Solid");
			KeepAspect = true;

			SuspendEvents = false;
		}

		public ComplexShape(ComplexShape prototype): base(prototype)
		{
			Children = new Elements(typeof(SolidElement),"Solid");
			foreach (SolidElement solid in prototype.Children.Values)
			{
				mChildren.Add(solid.Key,(SolidElement) solid.Clone());
			}
		}

		protected ComplexShape(SerializationInfo info, StreamingContext context): base(info,context)
		{
			Children = new Elements(typeof(SolidElement),"Solid");
			SuspendEvents = true;
			
			Children = (Elements) info.GetValue("Children",typeof(Elements));

			SuspendEvents = false;
		}

		//Properties
		public virtual Elements Children
		{
			get
			{
				return mChildren;
			}
			set
			{
				//Remove previous handlers
				if (mChildren != null)
				{
					mChildren.InsertElement -= new ElementsEventHandler(Element_Insert);
					mChildren.RemoveElement -= new ElementsEventHandler(Element_Remove);
				}

				if (value == null) 
				{
					mChildren = new Elements(typeof(SolidElement),"Solid");
				}
				else
				{
					mChildren = value;	
				}

				mChildren.InsertElement += new ElementsEventHandler(Element_Insert);
				mChildren.RemoveElement += new ElementsEventHandler(Element_Remove);

				CreateRenderList();
				OnElementInvalid(); 
			}
		}

		//Returns the internal renderlist
		public virtual RenderList RenderList
		{
			get
			{
				return mRenderList;
			}
		}

		#endregion

		#region Overrides

		public override object Clone()
		{
			return new ComplexShape(this);
		}

		protected internal override void Render(Graphics graphics, IRender render)
		{
			//Render this shape
			base.Render (graphics, render);

			Region current = null;

			//Set up clipping if required
			if (Clip)
			{
				Region region = new Region(GetPathInternal());
				current = graphics.Clip;
				graphics.SetClip(region,CombineMode.Intersect);
			}
			
			//Render the children
			if (Children != null)
			{
				foreach (SolidElement solid in RenderList)
				{
					graphics.TranslateTransform(solid.Rectangle.X ,solid.Rectangle.Y);
					solid.Render(graphics,render);
					graphics.TranslateTransform(-solid.Rectangle.X ,-solid.Rectangle.Y);
				}
			}

			if (Clip) graphics.Clip = current;
		}

		protected internal override void RenderShadow(Graphics graphics, IRender render)
		{
			base.RenderShadow (graphics, render);

			Region current = null;

			//Set up clipping if required
			if (Clip)
			{
				Region region = new Region(GetPathInternal());
				current = graphics.Clip;
				graphics.SetClip(region,CombineMode.Intersect);
			}
			
			//Render the children
			if (Children != null)
			{
				foreach (SolidElement solid in RenderList)
				{
					graphics.TranslateTransform(solid.Rectangle.X ,solid.Rectangle.Y);
					solid.RenderShadow(graphics,render);
					graphics.TranslateTransform(-solid.Rectangle.X ,-solid.Rectangle.Y);
				}
			}

			if (Clip) graphics.Clip = current;
		}

		protected internal override void RenderAction(Graphics graphics,IRender render,IRenderDesign renderDesign)
		{
			base.RenderAction (graphics,render,renderDesign);

			Region current = null;

			//Set up clipping if required
			if (Clip)
			{
				Region region = new Region(GetPathInternal());
				current = graphics.Clip;
				graphics.SetClip(region,CombineMode.Intersect);
			}
			
			//Render the children
			if (Children != null)
			{
				foreach (SolidElement solid in RenderList)
				{
					graphics.TranslateTransform(solid.Rectangle.X ,solid.Rectangle.Y);
					solid.RenderAction(graphics,render,renderDesign);
					graphics.TranslateTransform(-solid.Rectangle.X ,-solid.Rectangle.Y);
				}
			}

			if (Clip) graphics.Clip = current;
		}

		public override bool Contains(PointF location)
		{
			//Check if parent shape contains the point
			if (base.Contains (location)) return true;

			PointF offset = new PointF(location.X - Rectangle.X - Container.Offset.X,location.Y - Rectangle.Y - Container.Offset.Y);
			
			foreach (SolidElement solid in RenderList)
			{
				if (solid.Contains(offset)) return true;
			}

			return false;
		}

		public override void Scale(float x, float y, float dx, float dy, bool maintainAspect)
		{
			SuspendEvents = true;
			
			//Store current rectangle
			RectangleF rect = Rectangle;
			
			//Scale the parent shape
			base.Scale(x, y, dx, dy, KeepAspect);
			
			//Check the values
			if (Width < (rect.Width * x)) x = Convert.ToSingle(Width / rect.Width);
			if (Height < (rect.Height * y)) y = Convert.ToSingle(Height / rect.Height);
			if (Width > (rect.Width * x)) x = Convert.ToSingle(Width / rect.Width);
			if (Height > (rect.Height * y)) y = Convert.ToSingle(Height / rect.Height);

			ScaleChildren(x, y, dx, dy, KeepAspect);

			SuspendEvents = false;
			Invalidate();
		}

		public override SizeF Size
		{
			get
			{
				return base.Size;
			}
			set
			{
				if (!value.Equals(Size))
				{
					SuspendEvents = true;
					
					SizeF newsize = ValidateSize(value.Width, value.Height); 
					
					float scalex = (newsize.Width / Size.Width);
					float scaley = (newsize.Height / Size.Height);

					ScaleChildren(scalex, scaley, 0, 0, false);

					SuspendEvents = false;
					
					base.Size = newsize;
				}
			}
		}


		#endregion

		#region Events

		//Occurs when an element is added to the elements collection
		private void Element_Insert(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;

			//Set the layer
			element.SetLayer(Layer);

			//Set handlers
			element.ElementInvalid +=new EventHandler(Element_ElementInvalid);

			//Set the container
			element.SetContainer(Container);

			CreateRenderList();
			OnElementInvalid();
		}

		//Occurs when an element is added to the elements collection
		private void Element_Remove(object sender, ElementsEventArgs e)
		{
			Element element = e.Value;
			element.ElementInvalid -=new EventHandler(Element_ElementInvalid);

			CreateRenderList();
			OnElementInvalid();
		}

		//Occurs when an element raises an invalid event
		private void Element_ElementInvalid(object sender, EventArgs e)
		{
			OnElementInvalid();
		}

		#endregion

		#region Implementation

		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			info.AddValue("Children",Children);
			
			base.GetObjectData(info,context);
		}	
	
		private void CreateRenderList()
		{
			mRenderList = new RenderList();

			//Check for intersections with the zoomed rectangle;
			foreach (SolidElement solid in Children.Values)
			{
				if (solid.Visible) mRenderList.Add(solid);
			}
			mRenderList.Sort();
		}

		protected virtual void ScaleChildren(float scaleX, float scaleY, float dx, float dy, bool maintainAspect)
		{
			foreach (SolidElement solid in Children.Values)
			{
				RectangleF rect = solid.InternalRectangle;
				rect = new RectangleF(rect.X * scaleX,rect.Y * scaleY,rect.Width * scaleX,rect.Height * scaleY);

				float mx = solid.Location.X * (scaleX - 1);
				float my = solid.Location.Y * (scaleY - 1);
				solid.ScalePath(scaleX,scaleY,mx,my,rect);
			}
		}

		#endregion
	}
}
