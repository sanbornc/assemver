using System.Collections.Generic;
using System.IO;

namespace Ss.AssembComp.Comparer
{
	public class DirectoryNameComparer: IEqualityComparer<DirectoryInfo>
	{
		public bool Equals(DirectoryInfo x, DirectoryInfo y)
		{
			return x.FullName == y.FullName;
		}

		public int GetHashCode(DirectoryInfo obj)
		{
			return obj.FullName.GetHashCode();
		}
	}

	
}