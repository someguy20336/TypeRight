using EnvDTE;
using EnvDTE80;
using Microsoft;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using NuGet.VisualStudio;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TypeRightVsix.Shared
{
	/// <summary>
	/// Useful functions for working with visual studio
	/// </summary>
	class VsHelper
	{
		/// <summary>
		/// Gets the current VS helper
		/// </summary>
		public static VsHelper Current { get; private set; }

		/// <summary>
		/// The service provider for this instance
		/// </summary>
		private IServiceProvider _servProvider;

		/// <summary>
		/// Gets the current DTE
		/// </summary>
		public DTE Dte { get; private set; }

		/// <summary>
		/// Gets the current DTE as DTE2
		/// </summary>
		public DTE2 Dte2
		{
			get { return (DTE2)Dte; }
		}

		/// <summary>
		/// Creates a new VS Helper
		/// </summary>
		/// <param name="serviceProvider">The service provider to use</param>
		private VsHelper(IServiceProvider serviceProvider)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			_servProvider = serviceProvider;
			Dte = _servProvider.GetService(typeof(DTE)) as DTE;
			Assumes.Present(Dte);
		}

		/// <summary>
		/// Initializes the VS Helper
		/// </summary>
		/// <param name="serviceProvider">The service provider to use</param>
		public static void Initialize(IServiceProvider serviceProvider)
		{			
			if (Current == null)
			{
				Current = new VsHelper(serviceProvider);
			}			
		}

		/// <summary>
		/// Gets the current workspace
		/// </summary>
		/// <returns>The current workspace</returns>
		public Workspace GetCurrentWorkspace()
		{
			// https://joshvarty.wordpress.com/2014/09/12/learn-roslyn-now-part-6-working-with-workspaces/
			IComponentModel componentModel = (IComponentModel)_servProvider.GetService(typeof(SComponentModel));
			Assumes.Present(componentModel);
			Workspace workspace = componentModel.GetService<VisualStudioWorkspace>();
			return workspace;
		}

		/// <summary>
		/// Checks if the given solution item exists
		/// </summary>
		/// <param name="filePath">The file path to check</param>
		/// <returns>True if it exists</returns>
		public static bool SolutionItemExists(string filePath)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			Solution2 solution = ((Solution2)Current.Dte.Solution);
			ProjectItem item = solution.FindProjectItem(filePath);
			return item != null;
		}

		public static IEnumerable<EnvDTE.Project> GetSelectedCsharpProjects()
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			return GetSelectedItemsOfType<EnvDTE.Project>()
				.Where(p => p.FullName.EndsWith(".csproj", StringComparison.OrdinalIgnoreCase));
		}

		/// <summary>
		/// Gets selected items of the given type from the solution explorer
		/// </summary>
		/// <typeparam name="TItemType">The item type to retrieve</typeparam>
		/// <returns>An enumerable list of types</returns>
		public static IEnumerable<TItemType> GetSelectedItemsOfType<TItemType>() where TItemType : class
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			Array items = (Array)(Current.Dte2).ToolWindows.SolutionExplorer.SelectedItems;

			foreach (UIHierarchyItem selItem in items)
			{
				if (selItem.Object is TItemType itemAsType)
				{
					yield return itemAsType;
				}
			}
		}

		/// <summary>
		/// Gets whether the client scrip generator package is installed for the given project
		/// </summary>
		/// <param name="proj">The project to check</param>
		/// <returns>True if installed</returns>
		public static bool IsPackageInstalled(EnvDTE.Project proj)
		{
#if DEBUG && !NUGET
			return true;

#else
			IComponentModel componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
			IVsPackageInstallerServices installerServices = componentModel.GetService<IVsPackageInstallerServices>();

			try
			{
				return installerServices.IsPackageInstalled(proj, TypeRightPackage.NugetID);
			}
			catch (Exception)
			{
				return false;
			}
#endif
		}

		/// <summary>
		/// Sets the status bar text
		/// </summary>
		/// <param name="text"></param>
		public static void SetStatusBar(string text)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			IVsStatusbar statusBar = (IVsStatusbar)Current._servProvider.GetService(typeof(SVsStatusbar));
			if (statusBar == null)
			{
				return;
			}

			// Make sure the status bar is not frozen
			statusBar.IsFrozen(out int frozen);

			if (frozen != 0)
			{
				statusBar.FreezeOutput(0);
			}

			// Set the status bar text and make its display static.
			statusBar.SetText(text);
			//// Freeze the status bar.
			statusBar.FreezeOutput(1);

		}
		
	}
}
