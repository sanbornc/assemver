namespace Ss.AssembComp.Model
{
	public class MethodScanResult
	{


		public string Name { get; set; }
		
		public bool Added { get; set; }

		public bool Removed { get; set; }

		public bool ReturnTypeChanged { get; set; }

		public TypeScanResult ReturnTypeScanResult { get; set; }

		
	}
}