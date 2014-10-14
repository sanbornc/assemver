using Machine.Specifications;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Specs.Model
{
	public class ExecutionOptionsSpecs
	{
		protected static ExecutionOptions options;
		protected static ExecutionMethod method;

		Establish context = () => options = new ExecutionOptions();
	}

	public class when_has_folder_path : ExecutionOptionsSpecs
	{
		Because of = () => options.BaselinePath = "Value";

		It should_have_folder_execution_method = () => options.ExecutionMethod.ShouldEqual(ExecutionMethod.Folder);
	}

	public class when_has_assm_path : ExecutionOptionsSpecs
	{
		Because of = () => options.BaselineAssembly = "Value";

		It should_have_folder_execution_method = () => options.ExecutionMethod.ShouldEqual(ExecutionMethod.Assembly);
	}
}