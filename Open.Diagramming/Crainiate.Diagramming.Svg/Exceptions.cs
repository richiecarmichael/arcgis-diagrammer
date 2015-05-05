using System;

namespace Crainiate.Diagramming.Svg
{
	public class DefinitionException: Exception 
	{
		public DefinitionException(string message) : base(message)
		{}
	}
	public class SvgDocumentException: Exception 
	{
		public SvgDocumentException(string message) : base(message)
		{}
	}
	public class PolylineException: Exception 
	{
		public PolylineException(string message) : base(message)
		{}
	}
	public class TextException: Exception 
	{
		public TextException(string message) : base(message)
		{}
	}
}
