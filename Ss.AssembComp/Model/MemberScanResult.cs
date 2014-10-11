namespace Ss.AssembComp.Model
{
	public class MemberScanResult : ScanResult
	{
		public bool Added { get; set; }

		public bool Removed { get; set; }

		public bool TypeChanged { get; set; }

		public TypeScanResult TypeScanResult { get; set; }
	}
}