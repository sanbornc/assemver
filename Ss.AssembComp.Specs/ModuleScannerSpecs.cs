using System;
using Machine.Specifications;
using Ss.AssembComp.Model;
using Ss.AssembComp.Scanners;

namespace Ss.AssembComp.Specs
{
	public class ApiScanerSpecs
	{
		protected static ModuleScanner scanner;
		protected static ModuleScanResult result;
	}

	public class when_no_baseline_or_compare : ApiScanerSpecs
	{
		static Exception exception;


		Because of = () => exception = Catch.Exception(() => scanner = ModuleScanner.Create(null, null));

		It should_not_be_null = () => exception.ShouldNotBeNull();
		It should_be_arg_null_ex = () => exception.ShouldBeOfExactType<ArgumentNullException>();
	}

	public class when_no_baseline : ApiScanerSpecs
	{
		Establish context = () =>
		{
			scanner = ModuleScanner.Create(null, @"TestAssembly1.dll");
		};

		Because of = () => result = scanner.Scan();

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_have_added_set_to_true = () => result.Added.ShouldBeTrue();
	}

	public class when_no_compare : ApiScanerSpecs
	{
		Establish context = () =>
		{
			scanner = ModuleScanner.Create(@"TestAssembly1.dll", null);
		};

		Because of = () => result = scanner.Scan();

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_have_removed_set_to_true = () => result.Removed.ShouldBeTrue();
	}

	public class when_comparing_the_same_two_files : ApiScanerSpecs
	{
		Establish context = () =>
		{
			scanner = ModuleScanner.Create(@"TestAssembly1.dll", @"TestAssembly1.dll");
		};

		Because of = () => result = scanner.Scan();

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_not_have_version_change = () => result.VersionChange.ShouldBeNull();
		It should_not_have_date_change = () => result.DateChange.ShouldBeNull();
		It should_not_have_no_add = () => result.AddedTypes.ShouldBeEmpty();
		It should_not_have_no_removed = () => result.RemovedTypes.ShouldBeEmpty();
		It should_have_type_scan_results = () => result.PublicTypeScanResults.ShouldNotBeEmpty();

	}

	public class when_comparint_two_differnt_files : ApiScanerSpecs
	{
		Establish context = () =>
		{
			scanner = ModuleScanner.Create(@"TestAssembly1.dll", @"TestAssembly2.dll");
		};

		Because of = () => result = scanner.Scan();

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_have_version_change = () => result.VersionChange.ShouldNotBeNull();
		It should_have_date_change = () => result.DateChange.ShouldNotBeNull();
		It should_not_have_no_add = () => result.AddedTypes.ShouldNotBeEmpty();
		It should_not_have_no_removed = () => result.RemovedTypes.ShouldNotBeEmpty();

	}


}