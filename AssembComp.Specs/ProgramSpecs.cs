using System.IO;
using Autofac;
using Autofac.Core;
using Machine.Specifications;
using Ss.AssembComp;
using Ss.AssembComp.Model;
using Ss.AssembComp.Scanners;

namespace AssembComp.Specs
{
    public class ProgramSpecs
    {
	    protected static Program program;
		protected static string[] args;
	    protected static StringWriter output;
	    protected static IProcessor processor;

	    Establish context = () =>
	    {
		    output = new StringWriter();
		    processor = new FakeProcessor();
			program = new Program(output, processor);
	    };

		Cleanup after = () => output.Close();
    }

	public class when_configuring_container : ProgramSpecs
	{
		protected static IContainer container;
		Because of = () => container = Program.ConfigureContainer();

		It should_not_be_null = () => container.ShouldNotBeNull();
		
	}

	public class when_arguments_are_invalid: ProgramSpecs
	{


		Establish context = () =>
		{
			const string command = "eww --gross";
			args = command.Split(' ');
		};

		Because of = () => program.Process(args);

		It should_have_non_null_execution_options = () => program.ExecutionOptions.ShouldNotBeNull();
		It should_have_written_message_to_output_ = () => output.ToString().ShouldNotBeEmpty();
		It should_have_correct_header = () => output.ToString().StartsWith(program.HelpStringHeader).ShouldBeTrue();
	}

	public class when_arguments_are_correct_for_help : ProgramSpecs
	{

		Establish context = () =>
		{
			args = new[] { @"-h" };
		};

		Because of = () => program.Process(args);

		It should_have_non_null_execution_options = () => program.ExecutionOptions.ShouldNotBeNull();
		It should_have_help_option_set = () => program.ExecutionOptions.ShowHelp.ShouldBeTrue();
		It should_have_written_message_to_output_ = () => output.ToString().ShouldNotBeEmpty();
		It should_have_correct_header = () => output.ToString().StartsWith(program.HelpStringHeader).ShouldBeTrue();

	}

	public class when_required_option_has_no_value : ProgramSpecs
	{

		Establish context = () =>
		{
			args = new[] { "--o" };
		};

		Because of = () => program.Process(args);

		It should_have_null_execution_options = () => program.ExecutionOptions.ShouldBeNull();
		It should_have_written_message_to_output_ = () => output.ToString().ShouldNotBeEmpty();

	}

	public class when_arguments_are_correct_for_directory_scan : ProgramSpecs
	{
		

		Establish context = () =>
		{
			const string command = @"--bd:c:\directory1 --cd:c:\directory2 --o:c:\directory3";
			args = command.Split(' ');
		};

		Because of = () => program.Process(args);

		It should_have_non_null_execution_options = () => program.ExecutionOptions.ShouldNotBeNull();
		It should_have_correct_baseline = () => program.ExecutionOptions.BaselinePath.ShouldEqual(@"c:\directory1");
		It should_have_correct_compare = () => program.ExecutionOptions.ComparePath.ShouldEqual(@"c:\directory2");
		It should_have_correct_output = () => program.ExecutionOptions.OutputPath.ShouldEqual(@"c:\directory3");
	}

	

	public class when_arguments_have_spaces_for_directory_scan : ProgramSpecs
	{

		Establish context = () =>
		{
			args = new[] { @"--bd:c:\directory 1\sub", @"--cd:c:\directory 2\sub" };
		};

		Because of = () => program.Process(args);

		It should_have_non_null_execution_options = () => program.ExecutionOptions.ShouldNotBeNull();
		It should_have_correct_baseline = () => program.ExecutionOptions.BaselinePath.ShouldEqual(@"c:\directory 1\sub");
		It should_have_correct_compare = () => program.ExecutionOptions.ComparePath.ShouldEqual(@"c:\directory 2\sub");
	}

	public class when_arguments_are_correct_for_assembly_scan : ProgramSpecs
	{


		Establish context = () =>
		{
			const string command = @"--ba:c:\directory1\test.dll --ca:c:\directory2\test.dll --o:c:\directory3";
			args = command.Split(' ');
		};

		Because of = () => program.Process(args);

		It should_have_non_null_execution_options = () => program.ExecutionOptions.ShouldNotBeNull();
		It should_have_correct_baseline = () => program.ExecutionOptions.BaselineAssembly.ShouldEqual(@"c:\directory1\test.dll");
		It should_have_correct_compare = () => program.ExecutionOptions.CompareAssembly.ShouldEqual(@"c:\directory2\test.dll");
		It should_have_correct_output = () => program.ExecutionOptions.OutputPath.ShouldEqual(@"c:\directory3");
	}

	public class FakeProcessor : IProcessor
	{
		public void Process(ExecutionOptions executionOptions)
		{
			
		}
	}
}
