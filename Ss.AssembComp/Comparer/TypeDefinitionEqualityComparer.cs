using System.Collections;
using System.Collections.Generic;
using Mono.Cecil;

namespace Ss.AssembComp.Comparer
{
	public class TypeDefinitionEqualityComparer : IEqualityComparer<TypeDefinition>
	{
		public bool Equals(TypeDefinition x, TypeDefinition y)
		{
			return x.FullName == y.FullName;
		}

		public int GetHashCode(TypeDefinition obj)
		{
			return obj.FullName.GetHashCode();
		}
	}
}