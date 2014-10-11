using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using Mono.Cecil;
using Ss.AssembComp.Comparer;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{


	public class ModuleScanner : Scanner<ModuleDefinition, ModuleScanResult>
	{
		Dictionary<string, TypeScanResult> knownTypeScanResults = new Dictionary<string, TypeScanResult>(); 

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
			var result = new ModuleScanResult() {Added = Added, Removed = Removed};
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

			var baselineTypes = Baseline.Types.Where(t => t.IsPublic).ToList();
			var compareToTypes = CompareTo.Types.Where(t => t.IsPublic).ToList();

			result.AddedTypes = compareToTypes.Except(baselineTypes, new TypeDefinitionEqualityComparer()).ToList();
			result.RemovedTypes = baselineTypes.Except(compareToTypes, new TypeDefinitionEqualityComparer()).ToList();

			result.PublicTypeScanResults = ScanPublicTypes(baselineTypes.Intersect(compareToTypes, new TypeDefinitionEqualityComparer()), compareToTypes).ToList();

			return result;
		}

		IEnumerable<TypeScanResult> ScanPublicTypes(IEnumerable<TypeDefinition> types, IList<TypeDefinition> compareTypes)
		{
			//We need to create a TypeDefinition cache (Dict) so we can avoid a 
			//if(knownTypeDefinitions.ContainsKey())
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

		

		static DateTime GetModifiedDate(ModuleDefinition module)
		{
			var fileInfo = new FileInfo(module.FullyQualifiedName);
			return fileInfo.LastWriteTime;
		}

	}
}