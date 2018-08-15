using Microsoft.Build.Framework;
using System;
using System.Collections;
using Microsoft.Build.Evaluation;

namespace TypeRight.Build
{
	/// <summary>
	/// A dummy build engine used.  None of this really matters, it is just needed for faking MSBuild with resolving references
	/// </summary>
	class DummyBuildEngine : IBuildEngine
	{
		public int ColumnNumberOfTaskNode { get; } = 0;

		public bool ContinueOnError { get; } = false;

		public int LineNumberOfTaskNode { get; } = 0;

		public string ProjectFileOfTaskNode { get; }

		public bool BuildProjectFile(string projectFileName, string[] targetNames, IDictionary globalProperties, IDictionary targetOutputs)
		{
			throw new NotImplementedException();
		}

		public void LogCustomEvent(CustomBuildEventArgs e)
		{
#if DEBUG
			//System.Diagnostics.Debug.WriteLine(e.Message); 
#endif
		}

		public void LogErrorEvent(BuildErrorEventArgs e)
		{
#if DEBUG
			//System.Diagnostics.Debug.WriteLine(e.Message);
#endif
		}

		public void LogMessageEvent(BuildMessageEventArgs e)
		{
#if DEBUG
			//System.Diagnostics.Debug.WriteLine(e.Message);
#endif
		}

		public void LogWarningEvent(BuildWarningEventArgs e)
		{
#if DEBUG
			//System.Diagnostics.Debug.WriteLine(e.Message);
#endif
		}

		public DummyBuildEngine(Project proj)
		{
			ProjectFileOfTaskNode = proj.FullPath;
		}
	}
}
