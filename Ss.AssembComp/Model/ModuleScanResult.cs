using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Ss.AssembComp.Model
{
	public class ModuleScanResult : ScanResult
	{

		public bool Added { get; set; }

		public bool Removed { get; set; }

		public Change<Version> VersionChange { get; set; }

		public Change<DateTime> DateChange { get; set; }

		public List<TypeDefinition> AddedTypes { get; set; }

		public List<TypeDefinition> RemovedTypes { get; set; } 

		public List<TypeScanResult> PublicTypeScanResults { get; set; }
	}
}