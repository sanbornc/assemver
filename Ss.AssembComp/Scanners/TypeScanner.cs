using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Mono.Cecil;
using Ss.AssembComp.Comparer;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{
	public class TypeScanner : Scanner<TypeDefinition, TypeScanResult>
	{
		public Dictionary<string, TypeScanResult> KnownTypeScanResults { get; private set; }

		public TypeScanner(TypeDefinition baseline, TypeDefinition compareTo, Dictionary<string, TypeScanResult> knownTypeScanResults)
			: base(baseline, compareTo)
		{
			KnownTypeScanResults = knownTypeScanResults;
		}


		public override TypeScanResult Scan()
		{
			if (KnownTypeScanResults.ContainsKey(Baseline.FullName))
			{
				return KnownTypeScanResults[Baseline.FullName];
			}

			var baselineMethods = Baseline.Methods.Where(m => m.IsPublic).ToList();
			var comparedMethods = CompareTo.Methods.Where(m => m.IsPublic).ToList();

			var baselineMembers = Baseline.Fields.Where(m => m.IsPublic).ToList();
			var comparedMembers = CompareTo.Fields.Where(m => m.IsPublic).ToList();

			var result = new TypeScanResult
			{
				AddedMethods = baselineMethods.Except(comparedMethods, new MethodSignatureComparer()).ToList(),
				RemovedMethods = comparedMethods.Except(baselineMethods, new MethodSignatureComparer()).ToList(),
				AddedMembers = baselineMembers.Except(comparedMembers, new MemberSignatureComparer()).ToList(),
				RemovedMembers = comparedMembers.Except(baselineMembers, new MemberSignatureComparer()).ToList(),
			};



			KnownTypeScanResults.Add(Baseline.FullName, result);
			return result;
		}

	}
}