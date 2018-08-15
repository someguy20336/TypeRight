using EnvDTE;
using EnvDTE80;
using Microsoft.CodeAnalysis;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.LanguageServices;
using Microsoft.VisualStudio.Shell;
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
			_servProvider = serviceProvider;
			Dte = _servProvider.GetService(typeof(DTE)) as DTE;
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
			Solution2 solution = ((Solution2)Current.Dte.Solution);
			ProjectItem item = solution.FindProjectItem(filePath);
			return item != null;
		}

		/// <summary>
		/// Gets selected items of the given type from the solution explorer
		/// </summary>
		/// <typeparam name="TItemType">The item type to retrieve</typeparam>
		/// <returns>An enumerable list of types</returns>
		public static IEnumerable<TItemType> GetSelectedItemsOfType<TItemType>() where TItemType : class
		{
			Array items = (Array)(Current.Dte2).ToolWindows.SolutionExplorer.SelectedItems;

			foreach (UIHierarchyItem selItem in items)
			{
				TItemType item = selItem.Object as TItemType;
				if (item != null)
				{
					yield return item;
				}
			}
		}

		/// <summary>
		/// Gets whether the given project is the solution items folder
		/// </summary>
		/// <param name="proj">The project</param>
		/// <returns>True if it is the solution items folder</returns>
		public static bool IsSolutionItemsFolder(EnvDTE.Project proj)
		{
			return proj.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";  // ... I think
		}

		/// <summary>
		/// Gets whether the client scrip generator package is installed for the given project
		/// </summary>
		/// <param name="proj">The project to check</param>
		/// <returns>True if installed</returns>
		public static bool IsPackageInstalled(EnvDTE.Project proj)
		{
			
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
		}

		
	}
}
