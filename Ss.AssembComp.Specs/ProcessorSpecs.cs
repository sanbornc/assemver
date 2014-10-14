using System.IO;
using Formo;
using Machine.Specifications;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Specs
{
	public class ProcessorSpecs
	{
		protected static Processor processor;
		protected static ExecutionOptions options;

		protected static string BaselinePath;
		protected static string ComparePath;

		Establish context = () =>
		{
			processor = new Processor();
			dynamic config = new Configuration();

			var basePath = config.TestDirectory;

			BaselinePath = basePath + @"\Folder1";
			ComparePath = basePath + @"\Folder2";

		};
	}

	public class when_resolving_scanner_for_folder : ProcessorSpecs
	{
		protected static ScanResult result;

		Establish context = () => options = new ExecutionOptions {BaselinePath = BaselinePath, ComparePath = ComparePath};

		Because of = () => result = processor.GetScanResult(options);

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_be_a_FolderScanResult = () => result.ShouldBeOfExactType<FolderScanResult>();
	}

	public class when_resolving_scanner_for_assembly : ProcessorSpecs
	{
		protected static ScanResult result;

		Establish context = () => options = new ExecutionOptions { BaselineAssembly = @"TestAssembly1.dll", CompareAssembly = @"TestAssembly1.dll" };

		Because of = () => result = processor.GetScanResult(options);

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_be_a_ModuleScanResult = () => result.ShouldBeOfExactType<ModuleScanResult>();
	}


}