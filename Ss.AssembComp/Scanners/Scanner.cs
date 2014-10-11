using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{
	public abstract class Scanner<T, TResult> where TResult : ScanResult
	{
		public Scanner(T baseline, T compareTo)
		{
			Baseline = baseline;
			CompareTo = compareTo;
			
		}

		public T Baseline { get; private set; }
		public T CompareTo { get; private set; }

		public string Name { get; protected set; }
		
		public bool Added { get; protected set; }
		
		public bool Removed { get; protected set; }

		public abstract TResult Scan();
	}
}