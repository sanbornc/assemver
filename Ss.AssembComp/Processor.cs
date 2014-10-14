using System.Diagnostics.CodeAnalysis;
using Ss.AssembComp.Model;
using Ss.AssembComp.Scanners;

namespace Ss.AssembComp
{
	public class Processor : IProcessor
	{
		[ExcludeFromCodeCoverage]
		public void Process(ExecutionOptions executionOptions)
		{
			
		}

		public ScanResult GetScanResult(ExecutionOptions options)
		{
			if (options.ExecutionMethod == ExecutionMethod.Folder)
			{
				return new FolderScanner(options.BaselinePath, options.ComparePath).Scan();
			}

			return ModuleScanner.Create(options.BaselineAssembly, options.CompareAssembly).Scan();
		}


	}


}