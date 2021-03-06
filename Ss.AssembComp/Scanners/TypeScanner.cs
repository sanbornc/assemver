﻿using System.Collections.Generic;
using Mono.Cecil;
using Ss.AssembComp.Comparer;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{
	public class TypeScanner : Scanner<TypeDefinition, TypeScanResult>
	{
		Dictionary<string, TypeScanResult> KnownTypeScanResults { get; set; }

		readonly MethodSignatureComparer methodSignatureComparer = new MethodSignatureComparer();
		readonly MemberSignatureComparer memberSignatureComparer = new MemberSignatureComparer();

		public TypeScanner(TypeDefinition baseline, TypeDefinition compareTo, 
			Dictionary<string, TypeScanResult> knownTypeScanResults)
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

			var result = new TypeScanResult
			{
				FullName = FullName,
				AddedMethods = GetAdded(t => t.Methods, m => m.IsPublic, methodSignatureComparer),
				RemovedMethods = GetRemoved(t => t.Methods, m => m.IsPublic, methodSignatureComparer),
				AddedMembers = GetAdded(t => t.Fields, m => m.IsPublic, memberSignatureComparer),
				RemovedMembers = GetRemoved(t => t.Fields, m => m.IsPublic, memberSignatureComparer)
			};

			KnownTypeScanResults.Add(Baseline.FullName, result);

			return result;
		}

		string FullName
		{
			get { return Baseline != null ? Baseline.FullName : CompareTo.FullName; }
		}
	}
}