using System;
using System.Runtime.Serialization;

namespace Crainiate.Diagramming.Serialization
{
	public class DiagramBinder: SerializationBinder
	{
		//Return the type neccessary from the current executing assembly
		public override Type BindToType(string assemblyName, string typeName)
		{
            Type type = System.Type.GetType(typeName);
            if (type == null) throw new BindingException(string.Format("A type could not be found for {0}, {1}", assemblyName, typeName));
			return type;
		}
	}
}
