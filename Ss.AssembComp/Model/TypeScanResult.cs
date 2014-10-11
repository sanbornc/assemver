using System;
using System.Collections.Generic;
using Mono.Cecil;

namespace Ss.AssembComp.Model
{
	public class TypeScanResult : ScanResult
	{
		
		public List<MethodDefinition> AddedMethods { get; set; }
		public List<MethodDefinition> RemovedMethods { get; set; }

		public List<FieldDefinition> AddedMembers { get; set; }
		public List<FieldDefinition> RemovedMembers { get; set; }
 

		public List<MemberScanResult> MemberScanResults { get; set; }

		public List<MemberScanResult> MethodScanResults { get; set; }  
	}
}