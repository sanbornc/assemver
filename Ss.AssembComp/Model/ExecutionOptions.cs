namespace Ss.AssembComp.Model
{
	public enum ExecutionMethod
	{
		Folder, Assembly
	}

	public class ExecutionOptions
	{
		public string BaselinePath { get; set; }
		public string ComparePath { get; set; }

		public string BaselineAssembly { get; set; }
		public string CompareAssembly { get; set; }

		public string OutputPath { get; set; }

		public bool ShowHelp { get; set; }

		public bool Valid
		{
			get
			{
				if (string.IsNullOrWhiteSpace(BaselinePath) && string.IsNullOrWhiteSpace(BaselineAssembly) && !ShowHelp)
				{
					return false;
				}

				return true;
			}
		}

		public ExecutionMethod ExecutionMethod
		{
			get
			{
				if (!string.IsNullOrWhiteSpace(BaselinePath))
				{
					return ExecutionMethod.Folder;
				}

				return ExecutionMethod.Assembly;
			}
		}
	}
}