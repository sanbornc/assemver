using System;
using System.IO;
using Autofac;

namespace AssembComp
{
	public class ProgramModule : Module
	{
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<Program>().AsSelf();
			builder.RegisterInstance(Console.Out).As<TextWriter>();
		}
	}
}