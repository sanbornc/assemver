﻿using System.Collections.Generic;
using Mono.Cecil;

namespace Ss.AssembComp.Comparer
{
	public class MethodSignatureComparer : IEqualityComparer<MethodDefinition>
	{
		public bool Equals(MethodDefinition x, MethodDefinition y)
		{
			return x.FullName == y.FullName;
		}

		public int GetHashCode(MethodDefinition obj)
		{
			return obj.FullName.GetHashCode();
		}
	}
}