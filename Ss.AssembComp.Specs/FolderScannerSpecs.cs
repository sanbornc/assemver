using System;
using System.IO;
using Formo;
using Machine.Specifications;
using Machine.Specifications.Model;
using Ss.AssembComp.Model;
using Ss.AssembComp.Scanners;

namespace Ss.AssembComp.Specs
{
	public class FolderScannerSpecs
	{
		protected static FolderScanner Scanner;
		protected static FolderScanResult Result;
		protected static string BaselinePath;
		protected static string ComparePath;
		protected static string BadPath;

		protected static DirectoryInfo BaselineDirectoryInfo;
		protected static DirectoryInfo CompareDirectoryInfo;
		protected static DirectoryInfo BadDirectoryInfo;

		Establish context = () =>
		{
			dynamic config = new Configuration();

			var basePath = config.TestDirectory;

			BaselinePath = basePath + @"\Folder1";
			ComparePath = basePath + @"\Folder2";
			BadPath = basePath + @"\Folder3";

			BaselineDirectoryInfo = new DirectoryInfo(BaselinePath);
			CompareDirectoryInfo = new DirectoryInfo(ComparePath);
			BadDirectoryInfo = new DirectoryInfo(BadPath);

		};

	}

	public class when_no_baseline__or_compare_folder : FolderScannerSpecs
	{
		static Exception exception;

		Because of = () => exception = Catch.Exception(() => Scanner = new FolderScanner((DirectoryInfo)null, (DirectoryInfo)null));

		It should_not_be_null = () => exception.ShouldNotBeNull();
		It should_be_arg_null_ex = () => exception.ShouldBeOfExactType<ArgumentNullException>();
	}


	public class when_pointing_non_existant_baseline : FolderScannerSpecs
	{
		static Exception exception;

		Because of = () => exception = Catch.Exception(() => Scanner = new FolderScanner(BadDirectoryInfo, CompareDirectoryInfo));

		It should_not_be_null = () => exception.ShouldNotBeNull();
		It should_be_arg_null_ex = () => exception.ShouldBeOfExactType<ArgumentException>();
	}

	public class when_pointing_non_existant_compare : FolderScannerSpecs
	{
		static Exception exception;

		Because of = () => exception = Catch.Exception(() => Scanner = new FolderScanner(BaselineDirectoryInfo, BadDirectoryInfo));

		It should_not_be_null = () => exception.ShouldNotBeNull();
		It should_be_arg_null_ex = () => exception.ShouldBeOfExactType<ArgumentException>();
	}

	public class when_no_baseline_folder : FolderScannerSpecs
	{
		Establish context = () =>
		{
			Scanner = new FolderScanner(null, CompareDirectoryInfo);
		};

		Because of = () => Result = Scanner.Scan();

		It should_not_be_null = () => Result.ShouldNotBeNull();
		It should_have_added_set_to_true = () => Result.Added.ShouldBeTrue();
	}

	public class when_no_compare_folder : FolderScannerSpecs
	{
		Establish context = () =>
		{
			Scanner = new FolderScanner(BaselineDirectoryInfo, null);
		};

		Because of = () => Result = Scanner.Scan();

		It should_not_be_null = () => Result.ShouldNotBeNull();
		It should_have_removed_set_to_true = () => Result.Removed.ShouldBeTrue();
	}

	public class when_contructed_from_paths_it_should_contstruct : FolderScannerSpecs
	{
		Because of = () => Scanner = new FolderScanner(BaselinePath, ComparePath);

		It should_not_be_null = () => Scanner.ShouldNotBeNull();
	}

	public class when_comparing_same_folder : FolderScannerSpecs
	{
		Establish context = () =>
		{
			Scanner = new FolderScanner(BaselineDirectoryInfo, BaselineDirectoryInfo);
		};

		Because of = () => Result = Scanner.Scan();

		It should_not_be_null = () => Result.ShouldNotBeNull();
		It should_not_be_added = () => Result.Added.ShouldBeFalse();
		It should_not_be_removed = () => Result.Removed.ShouldBeFalse();
		It should_have_no_added_folders = () => Result.AddedFolders.ShouldBeEmpty();
		It should_have_no_removed_folders = () => Result.RemovedFolders.ShouldBeEmpty();
		It should_have_no_added_modules = () => Result.AddedModules.ShouldBeEmpty();
		It should_have_no_removed_modules = () => Result.RemovedModules.ShouldBeEmpty();

	}


	public class when_comparing_different_folders : FolderScannerSpecs
	{
		Establish context = () =>
		{
			Scanner = new FolderScanner(BaselineDirectoryInfo, CompareDirectoryInfo);
		};

		Because of = () => Result = Scanner.Scan();

		It should_not_be_null = () => Result.ShouldNotBeNull();
		It should_not_be_added = () => Result.Added.ShouldBeFalse();
		It should_not_be_removed = () => Result.Removed.ShouldBeFalse();
		It should_have_added_folders = () => Result.AddedFolders.ShouldNotBeEmpty();
		It should_have_removed_folders = () => Result.RemovedFolders.ShouldNotBeEmpty();
		It should_have_added_modules = () => Result.AddedModules.ShouldNotBeEmpty();
		It should_have_removed_modules = () => Result.RemovedModules.ShouldNotBeEmpty();

	}

}