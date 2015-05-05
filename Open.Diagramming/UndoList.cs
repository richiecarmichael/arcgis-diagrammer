using System;
using System.IO;
using System.Collections;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace Crainiate.Diagramming.Editing
{
	public class UndoList: CollectionBase
	{
		//Property variables
		private int mUndoPointer = -1;

		//Working variables
		private int mSuspendCount = 0;

		#region  Interface 

		public delegate void UndoneEventHandler(UndoPoint e);
		public event UndoneEventHandler Undone;
		public delegate void RedoneEventHandler(UndoPoint e);
		public event RedoneEventHandler Redone;

		//Constructors
		public UndoList()
		{
			Suspend();
		}

		//Gets or sets the position of the undo in the list
		public int UndoPointer
		{
			get
			{
				return mUndoPointer;
			}
			set
			{
				mUndoPointer = value;
			}
		}

		//Methods
		//Adds a new element to the list
		public new UndoPoint Add(Element element, UndoAction action)
		{
			return AddPoint(element, action, "", false);
		}

		public new UndoPoint Add(Element element, UndoAction action, bool overwrite)
		{
			return AddPoint(element, action, "", overwrite);
		}

		public new UndoPoint Add(Element element, UndoAction action, string description)
		{
			return AddPoint(element, action, description, false);
		}

		public new UndoPoint Add(Element element, UndoAction action, string description, bool overwrite)
		{
			return AddPoint(element, action, description, overwrite);
		}

		public virtual void Clear()
		{
			mUndoPointer = -1;
			List.Clear();
		}

		public virtual void Suspend()
		{
			mSuspendCount += 1;
		}

		public virtual void Resume()
		{
			mSuspendCount -=1;
		}

		public virtual bool Suspended
		{
			get
			{
				return mSuspendCount > 0;
			}
		}

		public virtual void Resume(bool force)
		{
			if (force)
			{
				mSuspendCount = 0;
			}
			else
			{
				mSuspendCount -=1;
			}
		}

		public virtual void Undo(Diagram diagram)
		{
			UndoRedoImplementation(diagram, true);
		}

		public virtual void Redo(Diagram diagram)
		{
			UndoRedoImplementation(diagram, false);
		}

		protected virtual void OnUndone(UndoPoint e)
		{
			if (Undone != null) Undone(e);
		}

		protected virtual void OnRedone(UndoPoint e)
		{
			if (Redone != null)	Redone(e);
		}

		#endregion

		#region  Implementation 

		private UndoPoint AddPoint(Element element, UndoAction action, string description, bool overwrite)
		{
			if (Suspended) return null;

			UndoPoint undo = null;
			string key = null;
			string createDesc = null;

			//Create undopoint
			if (element is Shape || element is Line)
			{
				undo = new UndoPoint(element, action);
				key = element.Key;
				createDesc = element.GetType().Name;
			}
			else
			{
				return null;
			}

			//Remove any previous undos ahead of this one
			if (mUndoPointer < List.Count)
			{
				for (int i = mUndoPointer+1;  i < List.Count; i++)
				{
					List.RemoveAt(i);
				}
			}

			//Set description
			if (description == null || description == "")
			{
				if (action == UndoAction.Add)
				{
					undo.Description = "Add " + createDesc;
				}
				else if (action == UndoAction.Edit)
				{
					undo.Description = "Edit " + createDesc;
				}
				else if (action == UndoAction.Remove)
				{
					undo.Description = "Remove " + createDesc;
				}
			}
			else
			{
				undo.Description = description;
			}

			//Get current undopoint
			UndoPoint currentUndo = null;

			if (mUndoPointer > -1) currentUndo = (UndoPoint) List[mUndoPointer];

			//Check if must add or update the undopoint
			if (overwrite && mUndoPointer > -1 && action == UndoAction.Edit && currentUndo.Key == key)
			{
				List[mUndoPointer] = undo; 
			}
			else
			{
				//Add object to internal arraylist
				List.Add(undo);
				mUndoPointer = List.Count - 1;
			}

			return undo;
		}

		private void UndoRedoImplementation(Diagram diagram, bool isUndo)
		{
			Elements shapes = diagram.Shapes;
			Elements lines = diagram.Lines;

			//Set the redo pointer correctly
			if (isUndo)
			{
				if (mUndoPointer < 0) return;
			}
			else
			{
				if (mUndoPointer >= List.Count - 1)
				{
					return;
				}
				else
				{
					mUndoPointer += 1;
				}
			}

			Suspend();

			UndoPoint undo = (UndoPoint) List[mUndoPointer];

			//Do a remove undo
			if ((isUndo & undo.Action == UndoAction.Add) | (! isUndo & undo.Action == UndoAction.Remove))
			{
				if (undo.UndoType == UndoType.Shape)
				{
					shapes.Remove(undo.Key);
				}
				else
				{
					lines.Remove(undo.Key);
				}
			}

			//Do an add undo
			if ((isUndo & undo.Action == UndoAction.Remove) | (! isUndo & undo.Action == UndoAction.Add))
			{
				MemoryStream ms = new MemoryStream(undo.Bytes);
				BinaryFormatter formatter = new BinaryFormatter();
				Element element = (Element) formatter.Deserialize(ms);
				element.SetContainer(undo.Container);
				element.SetLayer(undo.Layer);
                
				if (undo.UndoType == UndoType.Shape)
				{
					Shape shape = (Shape) element;
					shapes.Add(undo.Key, shape);
				}
				else
				{
					Line line = (Line) element;
					lines.Add(undo.Key, line);
				}
			}

			//Do an edit undo
			if (undo.Action == UndoAction.Edit)
			{
				//Recreate element from binary stream
				MemoryStream ms = new MemoryStream(undo.Bytes);
				BinaryFormatter formatter = new BinaryFormatter();
				Element element = (Element) formatter.Deserialize(ms);

				if (element is Shape)
				{
					diagram.Shapes.Remove(element.Key);
					diagram.Shapes.Add(element.Key, element);
				}
				else if (element is Line)
				{
					diagram.Lines.Remove(element.Key);
					diagram.Lines.Add(element.Key, element);
				}
			}

			//Set the undo pointer correctly
			if (isUndo)
			{
				if (mUndoPointer > -1) mUndoPointer -= 1;
			}

			//Raise the event
			if (isUndo)
			{
				OnUndone(undo);
			}
			else
			{
				OnRedone(undo);
			}

			Resume();
		}

		#endregion
	}
}


