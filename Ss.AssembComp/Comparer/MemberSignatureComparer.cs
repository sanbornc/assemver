using System.Collections.Generic;
using Mono.Cecil;

namespace Ss.AssembComp.Comparer
{
	public class MemberSignatureComparer : IEqualityComparer<FieldDefinition>
	{
		public bool Equals(FieldDefinition x, FieldDefinition y)
		{
			return x.FullName == y.FullName;
		}

		public int GetHashCode(FieldDefinition obj)
		{
			return obj.FullName.GetHashCode();
		}
	}
}