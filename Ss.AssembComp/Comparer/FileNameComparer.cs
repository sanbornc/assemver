using System.Collections.Generic;
using System.IO;

namespace Ss.AssembComp.Comparer
{
	public class FileNameComparer : IEqualityComparer<FileInfo>
	{
		public bool Equals(FileInfo x, FileInfo y)
		{
			return x.FullName == y.FullName;
		}

		public int GetHashCode(FileInfo obj)
		{
			return obj.FullName.GetHashCode();
		}
	}
}