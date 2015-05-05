namespace Crainiate.Diagramming
{
	public class LabelException : System.Exception 
	{
		public LabelException(string message) : base(message)
		{}
	}

	public class AttributeRequiredException : System.Exception 
	{
		public AttributeRequiredException(string message) : base(message)
		{}
	}

	public class ComponentException : System.Exception 
	{
		public ComponentException(string message) : base(message)
		{}
	}

    public class BindingException : ComponentException
    {
        public BindingException(string message): base(message)
        { }
    }

	public class ElementsException : System.Exception 
	{
		public ElementsException(string message) : base(message)
		{}
	}

	public class CollectionNotModifiableException: System.Exception
	{
		public CollectionNotModifiableException(string message) :base(message)
		{}
	}

	public class LayerException : System.Exception 
	{
		public LayerException(string message) :base(message)
		{}
	}

	public class InvalidPointException : System.ArgumentException 
	{
		public InvalidPointException(string message) : base(message)
		{
		}
	}

	public class GroupException : System.Exception 
	{
		public GroupException(string message) : base(message)
		{
		}
	}

	public class ComplexShapeException : System.Exception 
	{
		public ComplexShapeException(string message) : base(message)
		{
		}
	}

	public class ComplexLineException : System.Exception 
	{
		public ComplexLineException(string message) : base(message)
		{
		}
	}

	public class CurveException : System.Exception 
	{
		public CurveException(string message) : base(message)
		{
		}
	}

	public class ConnectorException : System.Exception 
	{
		public ConnectorException(string message) : base(message)
		{
		}
	}

	public class MetafileException : System.Exception 
	{
		public MetafileException(string message) : base(message)
		{
		}
	}

	public class ModelException : System.Exception 
	{
		public ModelException(string message) : base(message)
		{
		}
	}

	public class RenderListException : System.Exception 
	{
		public RenderListException(string message) : base(message)
		{
		}
	}

	public class TableItemsException : System.Exception 
	{
		public TableItemsException(string message) : base(message)
		{
		}
	}
}

namespace Crainiate.Diagramming.Editing
{
	public class UndoPointException : System.Exception 
	{
		public UndoPointException(string message) : base(message)
		{}
	}

	public class TabsException : System.Exception 
	{
		public TabsException(string message) : base(message)
		{}
	}
}

namespace Crainiate.Diagramming.Printing
{
	public class PrintDocumentException : System.Exception 
	{
		public PrintDocumentException(string message) : base(message)
		{
		}
	}

	public class PrintRenderException : System.Exception 
	{
		public PrintRenderException(string message) : base(message)
		{
		}
	}
}
