using System.IO;
using Autofac;
using Mono.Cecil;
using Mono.CompilerServices.SymbolWriter;
using Ss.AssembComp.Model;
using Ss.AssembComp.Scanners;

namespace Ss.AssembComp
{
	public class ProcessorModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Processor>().As<IProcessor>();

		}
	}
}