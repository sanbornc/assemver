using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using Mono.Cecil;
using Ss.AssembComp.Model;
using Ss.AssembComp.Scanners;

namespace Ss.SchemaCheck.Specs
{
	public class TypeScannerSpecs
	{
		protected static TypeScanner scanner;
		protected static TypeScanResult result;

		protected static TypeDefinition ref1;
		protected static TypeDefinition ref2;


		public static TypeDefinition LoadTypeDef(string assemblyPath, string className)
		{
			var moduleRef = ModuleDefinition.ReadModule(assemblyPath);
			return moduleRef.Types.SingleOrDefault(t => t.FullName == className);
		}

	}

	public class when_already_scanned : TypeScannerSpecs
	{
		static TypeScanner newScanner;
		static TypeScanResult returnedResult;
		static Dictionary<string, TypeScanResult> cache = new Dictionary<string, TypeScanResult>();
		Establish context = () =>
		{
			ref1 = LoadTypeDef(@"TestAssembly1.dll", "TestAssembly.Class1");
			ref2 = LoadTypeDef(@"TestAssembly1.dll", "TestAssembly.Class1");

			scanner = new TypeScanner(ref1, ref2, cache);
			result = scanner.Scan();
			newScanner = new TypeScanner(ref1, ref2, cache);
		};

		Because of = () => returnedResult = newScanner.Scan();

		It should_be_same_ref = () => returnedResult.ShouldEqual(result);
	}

	public class when_the_references_are_same : TypeScannerSpecs
	{
		Establish context = () =>
		{
			ref1 = LoadTypeDef(@"TestAssembly1.dll", "TestAssembly.Class1");
			ref2 = LoadTypeDef(@"TestAssembly1.dll", "TestAssembly.Class1");

			scanner = new TypeScanner(ref1, ref2, new Dictionary<string, TypeScanResult>());
		};

		Because of = () => result = scanner.Scan();

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_not_have_added_methods = () => result.AddedMethods.ShouldBeEmpty();
		It should_not_have_removed_methods = () => result.RemovedMethods.ShouldBeEmpty();
		It should_not_have_added_members = () => result.AddedMembers.ShouldBeEmpty();
		It should_not_have_removed_members = () => result.RemovedMembers.ShouldBeEmpty();
	}

	public class when_the_references_are_different : TypeScannerSpecs
	{
		Establish context = () =>
		{
			ref1 = LoadTypeDef(@"TestAssembly1.dll", "TestAssembly.Class1");
			ref2 = LoadTypeDef(@"TestAssembly2.dll", "TestAssembly.Class1");

			scanner = new TypeScanner(ref1, ref2, new Dictionary<string, TypeScanResult>());
			
		};

		Because of = () => result = scanner.Scan();

		It should_not_be_null = () => result.ShouldNotBeNull();
		It should_have_added_methods = () => result.AddedMethods.ShouldNotBeEmpty();
		It should_have_removed_methods = () => result.RemovedMethods.ShouldNotBeEmpty();
		It should_have_added_members = () => result.AddedMembers.ShouldNotBeEmpty();
		It should_have_removed_members = () => result.RemovedMembers.ShouldNotBeEmpty();
	}
}