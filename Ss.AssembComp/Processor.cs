using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Ss.AssembComp.Model;
using Ss.AssembComp.Model.Analysis;
using Ss.AssembComp.Scanners;

namespace Ss.AssembComp
{
	public class Processor : IProcessor
	{
		[ExcludeFromCodeCoverage]
		public void Process(ExecutionOptions executionOptions)
		{
			//This is a three step process.
			//1. Gather
			var result = GetScanResult(executionOptions);
			//2. Analysis

			//3. Report

		}

		public ScanResult GetScanResult(ExecutionOptions options)
		{
			if (options.ExecutionMethod == ExecutionMethod.Folder)
			{
				return new FolderScanner(options.BaselinePath, options.ComparePath).Scan();
			}

			return ModuleScanner.Create(options.BaselineAssembly, options.CompareAssembly).Scan();
		}

		public IEnumerable<AnalysisResult> AnalyzeScan(ScanResult scanResult)
		{
			throw new NotImplementedException();
		}


		public void Output(ExecutionOptions options)
		{
			//Basic 
		}

	}


}