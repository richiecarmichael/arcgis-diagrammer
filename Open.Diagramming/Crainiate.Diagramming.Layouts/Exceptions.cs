using System;

namespace Crainiate.Diagramming.Layouts
{
	public class GraphException : System.Exception 
	{
		public GraphException(string message) : base(message)
		{}
	}
	public class LayoutException : System.Exception 
	{
		public LayoutException(string message) : base(message)
		{}
	}
}
