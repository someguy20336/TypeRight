﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Microsoft.CodeAnalysis;
using EnvDTE;
using TypeRightVsix.Commands;
using TypeRightVsix.Shared;
using System.Collections.Generic;
using TypeRight;
using TypeRightVsix.Imports;
using System.Threading;
using System.Threading.Tasks;
using TypeRight.VsixContract;

namespace TypeRightVsix
{
	/// <summary>
	/// This is the class that implements the package exposed by this assembly.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The minimum requirement for a class to be considered a valid package for Visual Studio
	/// is to implement the IVsPackage interface and register itself with the shell.
	/// This package uses the helper classes defined inside the Managed Package Framework (MPF)
	/// to do it: it derives from the Package class that provides the implementation of the
	/// IVsPackage interface and uses the registration attributes defined in the framework to
	/// register itself and its components with the shell. These attributes tell the pkgdef creation
	/// utility what data to put into .pkgdef file.
	/// </para>
	/// <para>
	/// To get loaded into VS, the package must be referred by &lt;Asset Type="Microsoft.VisualStudio.VsPackage" ...&gt; in .vsixmanifest file.
	/// </para>
	/// </remarks>
	[PackageRegistration(UseManagedResourcesOnly = true, AllowsBackgroundLoading = true)]
	[InstalledProductRegistration("#110", "#112", "1.0", IconResourceID = 400)] // Info on this package for Help/About
	[Guid(TypeRightPackage.PackageGuidString)]
	[ProvideAutoLoad(UIContextGuids80.SolutionExists, PackageAutoLoadFlags.BackgroundLoad)]
	[SuppressMessage("StyleCop.CSharp.DocumentationRules", "SA1650:ElementDocumentationMustBeSpelledCorrectly", Justification = "pkgdef, VS and vsixmanifest are valid VS terms")]
	[ProvideMenuResource("Menus.ctmenu", 1)]
	public sealed class TypeRightPackage : AsyncPackage
	{

#if DEBUG && NUGET && LOCAL
		/// <summary>
		/// The name used by Nuget
		/// </summary>
		public const string NugetID = "TypeRightDebug";
#else
		/// <summary>
		/// The name used by Nuget
		/// </summary>
		public const string NugetID = "TypeRight";
#endif
		/// <summary>
		/// <see cref="TypeRightPackage"/> GUID string.
		/// </summary>
		public const string PackageGuidString = "B70245DF-B372-4992-B566-FB8CF2D8B557";

		/// <summary>
		/// Reference to the build events object.  This needs to be a private variable
		/// so it doesn't get garbage collected.
		/// </summary>
		private BuildEvents _buildEvents;

		/// <summary>
		/// Initializes a new instance of the <see cref="TypeRightPackage"/> class.
		/// </summary>
		public TypeRightPackage()
		{
			// Inside this method you can place any initialization code that does not require
			// any Visual Studio service because at this point the package object is created but
			// not sited yet inside Visual Studio environment. The place to do all the other
			// initialization is the Initialize method.
		}


#region Package Members
		
		protected async override System.Threading.Tasks.Task InitializeAsync(CancellationToken cancellationToken, IProgress<ServiceProgressData> progress)
		{
			ScriptGenAssemblyCache.TryClearCache();

			await JoinableTaskFactory.SwitchToMainThreadAsync(cancellationToken);

			VsHelper.Initialize(this);
			_buildEvents = VsHelper.Current.Dte.Events.BuildEvents;
			_buildEvents.OnBuildBegin += BuildEvents_OnBuildBegin;
			_buildEvents.OnBuildDone += BuildEvents_OnBuildDone;
			AddConfigCommand.Initialize(this);
			GenerateScriptsCommand.Initialize(this);
			InstallNugetPackageCommand.Initialize(this);
			ClearCacheCommand.Initialize(this);
			await base.InitializeAsync(cancellationToken, progress);
		}


		/// <summary>
		/// Event handler for the build event, which will silently generate the typescript files
		/// </summary>
		/// <param name="Scope">the build scope</param>
		/// <param name="Action">The build action</param>
		private void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			Workspace workspace = VsHelper.Current.GetCurrentWorkspace();
			List<EnvDTE.Project> enabledProj = ConfigProcessing.GetEnabledProjectsForSolution();
			if (enabledProj.Count > 0)
			{
				foreach (EnvDTE.Project proj in enabledProj)
				{
					BuildHelper.StartBuild(proj.FullName);
					try
					{
						IScriptGenerationAdapter adapter = ScriptGenAssemblyCache.GetForProj(proj).ScriptGenerator;
						adapter.GenerateScripts(workspace, proj.FullName);
					}
					catch (Exception e)
					{
						VsHelper.SetStatusBar("There was an error generating scripts.  Scripts will be generated by MSBuild: " + e.Message);
						BuildHelper.EndBuild(proj.FullName);
					}
				}
			}

		}

		/// <summary>
		/// Occurs when the build is finished - removes the global property
		/// </summary>
		/// <param name="Scope"></param>
		/// <param name="Action"></param>
		private void BuildEvents_OnBuildDone(vsBuildScope Scope, vsBuildAction Action)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			List<EnvDTE.Project> enabledProj = ConfigProcessing.GetEnabledProjectsForSolution();
			foreach (EnvDTE.Project project in enabledProj)
			{
				BuildHelper.EndBuild(project.FullName);
			}
		}
		
#endregion
	}
}
