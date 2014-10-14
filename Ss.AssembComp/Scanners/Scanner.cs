using System;
using System.Collections.Generic;
using System.Linq;
using Mono.Collections.Generic;
using Ss.AssembComp.Comparer;
using Ss.AssembComp.Model;

namespace Ss.AssembComp.Scanners
{
	public abstract class Scanner<TDef, TResult> where TResult : ScanResult where TDef : class 
	{
		
		

		public Scanner(TDef baseline, TDef compareTo)
		{
			Baseline = baseline;
			CompareTo = compareTo;
			
		}

		public TDef Baseline { get; private set; }
		public TDef CompareTo { get; private set; }

		public string Name { get; protected set; }
		
		public bool Added { get; protected set; }
		
		public bool Removed { get; protected set; }

		public abstract TResult Scan();

		protected List<T> GetAdded<T>(Func<TDef, IEnumerable<T>> func, Func<T, bool> filter, IEqualityComparer<T> comparer)
		{

			var filteredBaseline = ApplyFilter(Baseline, func, filter);
			var filteredCompared = ApplyFilter(CompareTo, func, filter);

			return filteredCompared.Except(filteredBaseline, comparer).ToList();
		}


		/// <summary>
		/// Returns all items of the given type that were removed 
		/// </summary>
		/// <typeparam name="T">The type to check</typeparam>
		/// <param name="func">The enumerable that we want to compare on the definitions we are comparing</param>
		/// <param name="filter"></param>
		/// <param name="comparer"></param>
		/// <returns></returns>
		protected List<T> GetRemoved<T>(Func<TDef, IEnumerable<T>> func, Func<T, bool> filter, IEqualityComparer<T> comparer)
		{
			var filteredBaseline = ApplyFilter(Baseline, func, filter);
			var filteredCompared = ApplyFilter(CompareTo, func, filter);

			return filteredBaseline.Except(filteredCompared, comparer).ToList();
		}

		private List<T> ApplyFilter<T>(TDef entity, Func<TDef, IEnumerable<T>> func, Func<T, bool> filter)
		{
			if (filter == null)
			{
				filter = (arg) => true;
			}

			var all = func(entity); 
			if (all == null) return new List<T>();

			return all.Where(filter).ToList();

		}

	}
}