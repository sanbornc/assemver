using System;
using System.Diagnostics.CodeAnalysis;


namespace TestAssembly
{
	[ExcludeFromCodeCoverage]
	public class Class1
	{
		public DateTime Member1;
		public string Member2;
		string Member3 = "";

		public int Property1 { get; set; }

		public Class2 Method1(string arg1, Class2 arg2) 
		{
			return arg2;
		}

		string Method2(Class3 arg1, bool arg2, string arg3) 
		{
			return Member2;
		}
	}

	public class Class2
	{
		public bool Member1; 

		public Class3 Property2 { get; set; } 
	}


	public class Class3  //no longer public
	{
		public DateTime Property3 { get; set; }
	}

	class Class4 //now public
	{
		public string Property4 { get; set; }
	}
}

namespace TestAssembly2.Other
{
	[ExcludeFromCodeCoverage]
	public class Class1
	{
		public string Member1;
		string Member2 = "";

		public int Property1 { get; set; }

		public Class2 Method1(Class3 arg1, Class2 arg2, bool arg3)
		{
			return arg2;
		}

		string Method2(Class3 arg1, Class2 arg2, bool arg3)
		{
			return Member2;
		}
	}

	public class Class2
	{
		public string Member1;

		public Class3 Property2 { get; set; }
	}

	public class Class3
	{
		public DateTime Property3 { get; set; }
	}

	class Class4
	{
		public string Property4 { get; set; }
	}
}