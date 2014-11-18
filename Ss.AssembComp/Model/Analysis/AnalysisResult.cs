namespace Ss.AssembComp.Model.Analysis
{
	public enum ResultType
	{
		Pass, Warn, Fail
	}

	public enum MemberType
	{
		Member, Method, Type, Module, Folder
	}

	public enum ChangeType
	{
		Add, Remove, Date, Version
	}

	public class AnalysisResult
	{
		public string Name { get; set; }

		public ResultType Result { get; set; }

		public string Message { get; set; }

		public MemberType MemberType { get; set; }

		public ChangeType ChangeType { get; set; }
	}
}