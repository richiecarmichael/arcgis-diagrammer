using System;
using System.IO;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crainiate.Diagramming.Editing
{
	public sealed class UndoPoint
	{
		private string mKey;
		private byte[] mBytes;
		private UndoAction mAction;
		private string mDescription;
		private Type mObjectType;
		private UndoType mUndoType;

		private IContainer mContainer;
		private Layer mLayer;

		public UndoPoint(Element element, UndoAction action)
		{
			if (! (element is Shape || element is Line)) throw new UndoPointException("Element must be of type Shape or type Line.");
			
			mKey = element.Key;
			mBytes = SerializeElement(element);
			mAction = action;
			mObjectType = element.GetType();
			if (element is Shape) mUndoType = UndoType.Shape;
			if (element is Line) mUndoType = UndoType.Line;

			mContainer = element.Container;
			mLayer = element.Layer;
		}

		public UndoPoint(Element element, UndoAction action, string description)
		{
			if (! (element is Shape || element is Line)) throw new UndoPointException("Element must be of type Shape or type Line.");

			mKey = element.Key;
			mBytes = SerializeElement(element);
			mAction = Action;
			mDescription = description;
			mObjectType = element.GetType();
			if (element is Shape) mUndoType = UndoType.Shape;
			if (element is Line) mUndoType = UndoType.Line;

			mContainer = element.Container;
			mLayer = element.Layer;
		}

		//Sets or returns a reference to the key.
		public string Key
		{
			get
			{
				return mKey;
			}
			set
			{
				mKey = value;
			}
		}

		//Returns the bytes for the undo point
		public Byte[] Bytes
		{
			get
			{
				return mBytes;
			}
			set
			{
				mBytes = value;
			}
		}

		//Sets or returns the undo action type
		public UndoAction Action
		{
			get
			{
				return mAction;
			}
			set
			{
				mAction = value;
			}
		}

		//Sets or returns the undo description.
		public string Description
		{
			get
			{
				return mDescription;
			}
			set
			{
				mDescription = value;
			}
		}

		//Returns the runtime object type held by this undo point.
		public Type ObjectType
		{
			get
			{
				return mObjectType;
			}
		}

		public UndoType UndoType
		{
			get
			{
				return mUndoType;
			}
		}

		public IContainer Container
		{
			get
			{
				return mContainer;
			}
		}

		public Layer Layer
		{
			get
			{
				return mLayer;
			}
		}

		private byte[] SerializeElement(Element element)
		{
			//if (!(element is ISerializable)) throw new UndoPointException("Element passed to undopoint doesnt implement ISerializable");
			
			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			
			formatter.Serialize(ms,element);
			
			return ms.GetBuffer();
		}
	}

}
