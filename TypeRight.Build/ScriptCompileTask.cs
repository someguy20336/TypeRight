using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;
using System;
using System.IO;
using TypeRight.Build;

namespace TypeRight.Workspaces.BuildTasks
{
	/// <summary>
	/// MSBuild task to compile scripts
	/// </summary>
	[Serializable]
	public class ScriptCompileTask : Task
	{

		private ReferenceResolverParameters _refResolvParams = new ReferenceResolverParameters();

		/// <summary>
		/// Gets or sets the Project path
		/// </summary>
		[Required]
		public string ProjectPath { get; set; }

		/// <summary>
		/// Gets or sets the search paths
		/// </summary>
		public string[] SearchPaths
		{
			get { return _refResolvParams.SearchPaths; }
			set { _refResolvParams.SearchPaths = value; }
		}

		/// <summary>
		/// Gets or sets the target framework directories
		/// </summary>
		public string[] TargetFrameworkDirectories
		{
			get { return _refResolvParams.TargetFrameworkDirectories; }
			set { _refResolvParams.TargetFrameworkDirectories = value; }
		}

		/// <summary>
		/// Gets or sets the full framework folders
		/// </summary>
		public string[] FullFrameworkFolders
		{
			get { return _refResolvParams.FullFrameworkFolders; }
			set { _refResolvParams.FullFrameworkFolders = value; }
		}

		/// <summary>
		/// Gets or sets the full framework subset names
		/// </summary>
		public string[] FullTargetFrameworkSubsetNames
		{
			get { return _refResolvParams.FullTargetFrameworkSubsetNames; }
			set { _refResolvParams.FullTargetFrameworkSubsetNames = value; }
		}

		/// <summary>
		/// Gets or sets the allowed assembly extensions
		/// </summary>
		public string[] AllowedAssemblyExtensions
		{
			get { return _refResolvParams.AllowedAssemblyExtensions; }
			set { _refResolvParams.AllowedAssemblyExtensions = value; }
		}

		/// <summary>
		/// Gets or sets the allowed related file extensions
		/// </summary>
		public string[] AllowedRelatedFileExtensions
		{
			get { return _refResolvParams.AllowedRelatedFileExtensions; }
			set { _refResolvParams.AllowedRelatedFileExtensions = value; }
		}

		/// <summary>
		/// Gets or sets the target processor architechure
		/// </summary>
		public string TargetProcessorArchitecture
		{
			get { return _refResolvParams.TargetProcessorArchitecture; }
			set { _refResolvParams.TargetProcessorArchitecture = value; }
		}

		/// <summary>
		/// Gets or sets the target framework moniker
		/// </summary>
		public string TargetFrameworkMoniker
		{
			get { return _refResolvParams.TargetFrameworkMoniker; }
			set { _refResolvParams.TargetFrameworkMoniker = value; }
		}

		/// <summary>
		/// Gets or sets the target framework version in the form "vX.Y"
		/// </summary>
		public string TargetFrameworkVersion
		{
			get { return _refResolvParams.TargetFrameworkVersion; }
			set { _refResolvParams.TargetFrameworkVersion = value; }
		}

		/// <summary>
		/// Gets or sets the targeted runtime version
		/// </summary>
		public string TargetedRuntimeVersion
		{
			get { return _refResolvParams.TargetedRuntimeVersion; }
			set { _refResolvParams.TargetedRuntimeVersion = value; }
		}

		/// <summary>
		/// Gets or sets the legacy method of whether it was done by the extension
		/// </summary>
		public bool LegacyDoneByExtension { get; set; }

		/// <summary>
		/// Gets whether this task is building is visual studio
		/// </summary>
		public bool BuildingInVS { get; set; }

		/// <summary>
		/// Gets whether we are actually building the project
		/// </summary>
		public bool BuildingProject { get; set; }


		/// <summary>
		/// Executes the build task
		/// </summary>
		/// <returns>True if sucessful, false if not</returns>
		public override bool Execute()
		{
			// https://blogs.msdn.microsoft.com/msbuild/2005/11/19/msbuild-in-visual-studio-part-12-compiling-inside-visual-studio/
			if (!BuildingProject)
			{
				Log.LogMessage("BuildProject not set - skipping");
				return true;
			}

			if (LegacyDoneByExtension || !BuildHelper.NeedsScriptGeneration(ProjectPath))
			{
				Log.LogMessage("Script generation skipped: done by extension");
				return true;
			}

			if (BuildingInVS)
			{
				Log.LogMessage("Warning! You are generating scripts using the MSBuild task inside Visual Studio. Consider installing the extension for a faster build.");
			}

			/*
			 * As you can see, I am creating a new app domain here.  this is because
			 * I had so many problems keeping the Nuget packages up to date without
			 * something breaking during MS Build.  This is likely due to the fact
			 * that the roslyn related DLLs (and dependencies) are compiled against different
			 * versions than I expect here.  Well, since the task runs in the MS Build app domain base directory,
			 * it tries to resolve those dependencies there and thus everything breaks.  This was still
			 * an issue with using AppDomainIsolatedTask.
			 */

			AppDomainSetup setup = new AppDomainSetup
			{
				// Use the application base of this directory
				ApplicationBase = new FileInfo(GetType().Assembly.Location).DirectoryName
			};

			AppDomain newDomain = AppDomain.CreateDomain("scriptGenDomain", null, setup);
			bool success = false;
			try
			{
				newDomain.Load(typeof(MsBuildLikeScriptGenerator).Assembly.GetName());

				MsBuildLikeScriptGenerator task = (MsBuildLikeScriptGenerator)newDomain.CreateInstanceAndUnwrap(GetType().Assembly.FullName, typeof(MsBuildLikeScriptGenerator).FullName);
				task.ProjectPath = ProjectPath;
				task.Parameters = _refResolvParams;
				task.Log = Log;
				task.Execute();
				success = task.Success;
			}
			catch (Exception e)
			{
				Log.LogWarningFromException(e, true);
			}
			finally
			{
				AppDomain.Unload(newDomain);
			}

			
			return true;	// Don't ever fail build
		}

	}
}
