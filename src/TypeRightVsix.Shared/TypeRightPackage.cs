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

		private const string SkipTypeRightPropertyName = "SkipTypeRight";
		private const string SkipTypeRightReasonPropName = "SkipTypeRightReason";

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
			DebugInfoCommand.Initialize(this);
			await base.InitializeAsync(cancellationToken, progress);
			await UpgradeConfigCommand.InitializeAsync(this);
		}


		/// <summary>
		/// Event handler for the build event, which will silently generate the typescript files
		/// </summary>
		/// <param name="Scope">the build scope</param>
		/// <param name="Action">The build action</param>
		private void BuildEvents_OnBuildBegin(vsBuildScope Scope, vsBuildAction Action)
		{

			ThreadHelper.ThrowIfNotOnUIThread();
			var sol = GetService(typeof(IVsSolution)) as IVsSolution;
			Workspace workspace = VsHelper.Current.GetCurrentWorkspace();
			List<EnvDTE.Project> enabledProj = ConfigProcessing.GetEnabledProjectsForSolution();
			if (enabledProj.Count > 0)
			{
				foreach (EnvDTE.Project proj in enabledProj)
				{
					try
					{
						sol.GetProjectOfUniqueName(proj.UniqueName, out IVsHierarchy hier);
						var storage = hier as IVsBuildPropertyStorage;
						storage.SetPropertyValue(SkipTypeRightPropertyName, "", (uint)_PersistStorageType.PST_USER_FILE, "true");
						storage.SetPropertyValue(SkipTypeRightReasonPropName, "", (uint)_PersistStorageType.PST_USER_FILE, "Handled by Visual Studio Extension");
						ScriptGenAssemblyCache.GetForProj(proj).GenerateScripts(workspace, proj.FullName, false);
					}
					catch (Exception e)
					{
						VsHelper.SetStatusBar("There was an error generating scripts.  Scripts may not be generated: " + e.Message);
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
			var sol = GetService(typeof(IVsSolution)) as IVsSolution;
			List<EnvDTE.Project> enabledProj = ConfigProcessing.GetEnabledProjectsForSolution();
			foreach (EnvDTE.Project project in enabledProj)
			{
				sol.GetProjectOfUniqueName(project.UniqueName, out IVsHierarchy hier);
				var storage = hier as IVsBuildPropertyStorage;
				storage.RemoveProperty(SkipTypeRightPropertyName, "", (uint)_PersistStorageType.PST_USER_FILE);
				storage.RemoveProperty(SkipTypeRightReasonPropName, "", (uint)_PersistStorageType.PST_USER_FILE);				
			}
		}

		#endregion
	}
}
