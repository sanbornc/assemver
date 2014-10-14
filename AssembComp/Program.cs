using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Reflection;
using System.Text;
using Autofac;
using Mono.Options;
using Ss.AssembComp;
using Ss.AssembComp.Model;

namespace AssembComp
{
	
	public class Program
	{
		static IContainer container;

		[ExcludeFromCodeCoverage]
		public static void Main(string[] args)
		{
			container = ConfigureContainer();

			var program = container.Resolve<Program>();
			program.Process(args);
		}

		
		public ExecutionOptions ExecutionOptions { get; set; }
		readonly string assemblyName;
		readonly TextWriter output;
		OptionSet optionSet;
		IProcessor processor;


		public Program(TextWriter writer, IProcessor processor)
		{
			assemblyName = Assembly.GetExecutingAssembly().GetName().Name;
			output = writer;
			this.processor = processor;
		}

		public void Process(IEnumerable<string> args)
		{
			try
			{
				ExecutionOptions = ParseExecutionOptions(args);
			}
			catch (OptionException e)
			{
				output.Write("{0}: ", assemblyName);
				output.WriteLine(e.Message);
				output.WriteLine("Try `{0} --help' for more information.", assemblyName);
				return;
			}

			if (!ExecutionOptions.Valid || ExecutionOptions.ShowHelp)
			{
				ShowHelp(optionSet);
				return;
			}


			processor.Process(ExecutionOptions);
			
		}

		public static IContainer ConfigureContainer()
		{
			var builder = new ContainerBuilder();

			builder.RegisterModule<ProgramModule>();
			builder.RegisterModule<ProcessorModule>();

			return builder.Build();

		}

		public ExecutionOptions ParseExecutionOptions(IEnumerable<string> args)
		{
			
			var executionOptions = new ExecutionOptions();

			optionSet = new OptionSet
			{
				{"o=", "The path of the output", value => executionOptions.OutputPath = value},
				{"?|h|help", "Show this message and exit.", value => executionOptions.ShowHelp = value != null},
				{"bd:", "The baseline directory used for comparison", value => executionOptions.BaselinePath = value},
				{"cd:", "The directory that is compared to the baseline", value => executionOptions.ComparePath = value},
				{"ba:", "The baseline assembly used for comparison", value => executionOptions.BaselineAssembly = value},
				{"ca:", "The assembly that is compared to the baseline", value => executionOptions.CompareAssembly = value},
			};

			optionSet.Parse(args);

			return executionOptions;
		}

		void ShowHelp(OptionSet p)
		{
			output.Write(HelpStringHeader);
			p.WriteOptionDescriptions(output);
		}

		public string HelpStringHeader
		{
			get
			{
				return new StringBuilder().AppendLine()
					.Append("Usage: ")
					.Append(assemblyName).AppendLine(" [OPTIONS]")
					.AppendLine("Use to compare assemblies to determine their compatibility level.  ")
					.AppendLine()
					.AppendLine("Compare two folders and all the assemblies within the folders using: ") 
					.AppendLine("--bd (baseline directory) --cd (compare directory) options")
					.AppendLine()
					.AppendLine("Or compare two specific aseemblies using the ")
					.AppendLine("--ba (baseline assembly) --ca(compare assembly) arguments.")
					.AppendLine()
					.AppendLine("Options:")
					.ToString();
			}
		}
	}
}
