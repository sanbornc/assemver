using System.Collections.Generic;
using System.IO;
using Mono.Cecil;

namespace Ss.AssembComp.Model
{
	public class FolderScanResult : ScanResult
	{
		public bool Added { get; set; }

		public bool Removed { get; set; }

		public List<DirectoryInfo> AddedFolders { get; set; }
		public List<DirectoryInfo> RemovedFolders { get; set; }

		public List<ModuleDefinition> AddedModules { get; set; }
		public List<ModuleDefinition> RemovedModules { get; set; }

		
	}
}