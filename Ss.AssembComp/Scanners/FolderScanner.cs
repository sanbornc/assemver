using System;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Ss.AssembComp.Comparer;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{
	public class FolderScanner : Scanner<DirectoryInfo, FolderScanResult>
	{
		readonly DirectoryNameComparer directoryComparer = new DirectoryNameComparer();
		readonly FileNameComparer fileComparer = new FileNameComparer();

		public FolderScanner(DirectoryInfo baseline, DirectoryInfo compareTo) : base(baseline, compareTo)
		{
			if (Baseline == null && CompareTo == null)
			{
				throw new ArgumentNullException();
			}

			if (Baseline != null && !Baseline.Exists)
			{
				throw new ArgumentException(string.Format("Directory {0} must exist.", Baseline.FullName));	
			}

			if (CompareTo != null && !CompareTo.Exists)
			{
				throw new ArgumentException(string.Format("Directory {0} must exist.", CompareTo.FullName));
			}

			if (Baseline != null)
			{
				Name = Baseline.Name;
				if (CompareTo == null)
				{
					Removed = true;
				}

				return;
			}

			if (CompareTo != null)
			{
				Name = CompareTo.Name;
				if (Baseline == null)
				{
					Added = true;
				}
			}

			
		}

		public FolderScanner(string baselinePath, string comparePath)
			: this(baselinePath == null ? null : new DirectoryInfo(baselinePath) , 
			comparePath == null ? null: new DirectoryInfo(comparePath))
		{
			
		}

		public override FolderScanResult Scan()
		{ 
			var result =  new FolderScanResult
			{
				Added = Added, 
				Removed = Removed, 
				FullName = FullName
			};

			if (Baseline == null || CompareTo == null)
			{
				return result;
			}

			result.AddedFolders = GetAdded(f => f.EnumerateDirectories(), null, directoryComparer);
			result.RemovedFolders = GetRemoved(f => f.EnumerateDirectories(), null, directoryComparer);

			result.AddedModules = GetAdded(d => d.EnumerateFiles(),
				f => f.Name.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase), fileComparer)
				.Select(f => ModuleDefinition.ReadModule(f.FullName)).ToList();

			result.RemovedModules = GetRemoved(d => d.EnumerateFiles(),
				f => f.Name.EndsWith(".dll", StringComparison.InvariantCultureIgnoreCase), fileComparer)
				.Select(f => ModuleDefinition.ReadModule(f.FullName)).ToList();

			return result;

		}

		public string FullName
		{
			get { return Baseline != null ? Baseline.FullName : CompareTo.FullName; }
		}
	}
}