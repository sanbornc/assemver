using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Mono.Cecil;
using Ss.AssembComp.Comparer;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{


	public class ModuleScanner : Scanner<ModuleDefinition, ModuleScanResult>
	{
		readonly Dictionary<string, TypeScanResult> knownTypeScanResults = new Dictionary<string, TypeScanResult>();
		readonly TypeDefinitionEqualityComparer typeDefComparer = new TypeDefinitionEqualityComparer();

		public ModuleScanner(ModuleDefinition baseline, ModuleDefinition compareTo) : base(baseline, compareTo)
		{
			if (Baseline == null && CompareTo == null)
			{
				throw new ArgumentNullException();
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

		public static ModuleScanner Create(string baselinePath, string compareToPath)
		{
			ModuleDefinition baseline = null;
			ModuleDefinition compareTo = null;

			if (baselinePath != null)
			{
				baseline = ModuleDefinition.ReadModule(baselinePath);
			}

			if (compareToPath != null)
			{
				compareTo = ModuleDefinition.ReadModule(compareToPath);
			}

			return new ModuleScanner(baseline, compareTo);
		}

		public override ModuleScanResult Scan()
		{
			var result = new ModuleScanResult
			{
				Added = Added, 
				Removed = Removed,
				FullName = FullName
			};

			if (Baseline == null || CompareTo == null)
			{
				return result;
			}

			if (!BaselineVersion.Equals(CompareToVersion))
			{
				result.VersionChange = new Change<Version>(BaselineVersion, CompareToVersion);
			}

			var baselineDate = GetModifiedDate(Baseline);
			var compareToDate = GetModifiedDate(CompareTo);

			if (!baselineDate.Equals(compareToDate))
			{
				result.DateChange = new Change<DateTime>(baselineDate, compareToDate);
			}

			result.AddedTypes = GetAdded(m => m.Types, m => m.IsPublic, typeDefComparer);
			result.RemovedTypes = GetRemoved(m => m.Types, m => m.IsPublic, typeDefComparer);

			result.PublicTypeScanResults = ScanPublicTypes().ToList();

			return result;
		}

		IEnumerable<TypeScanResult> ScanPublicTypes()
		{
			var baselineTypes = Baseline.Types.Where(t => t.IsPublic).ToList();
			var compareTypes = CompareTo.Types.Where(t => t.IsPublic).ToList();

			var types = baselineTypes.Intersect(compareTypes, typeDefComparer);

			foreach (var typeDefinition in types)
			{
				var compared = compareTypes.SingleOrDefault(ct => ct.FullName == typeDefinition.FullName);
				var result = new TypeScanner(typeDefinition, compared, knownTypeScanResults).Scan();
				if (result != null)
				{
					yield return result;
				}
			}
		}

		public Version BaselineVersion
		{
			get { return GetVersion(Baseline); }
		}

		public Version CompareToVersion
		{
			get { return GetVersion(CompareTo); }
		}

		Version GetVersion(ModuleDefinition module)
		{
			return module.Assembly.Name.Version;
		}

		public string FullName
		{
			get { return Baseline != null ? Baseline.FullyQualifiedName : CompareTo.FullyQualifiedName; }
		}

		

		static DateTime GetModifiedDate(ModuleDefinition module)
		{
			var fileInfo = new FileInfo(module.FullyQualifiedName);
			return fileInfo.LastWriteTime;
		}

	}
}