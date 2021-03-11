//------------------------------------------------------------------------------
// <copyright file="AddConfigCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using EnvDTE;
using System.Linq;
using TypeRightVsix.Shared;
using System.IO;
using TypeRightVsix.Imports;
using Microsoft.VisualStudio.Shell.Interop;

namespace TypeRightVsix.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class AddConfigCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 4129;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("E49229F2-6882-421F-872E-90E1B1CC7534");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="AddConfigCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private AddConfigCommand(Package package)
		{
			this._package = package ?? throw new ArgumentNullException("package");

			if (this.ServiceProvider.GetService(typeof(IMenuCommandService)) is OleMenuCommandService commandService)
			{
				var menuCommandID = new CommandID(CommandSet, CommandId);
				var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);
				menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
			}
		}

		/// <summary>
		/// Before query for Adding a config file to the solution
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			OleMenuCommand button = (OleMenuCommand)sender;

			button.Visible = false;
			button.Enabled = false;

			foreach (Project proj in VsHelper.GetSelectedItemsOfType<Project>())
			{
				button.Visible = true;  // At least one project is selected...
				if (!ConfigProcessing.ConfigExistsForProject(proj) && VsHelper.IsPackageInstalled(proj))
				{
					button.Enabled = true;
				}
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static AddConfigCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private IServiceProvider ServiceProvider
		{
			get
			{
				return this._package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new AddConfigCommand(package);
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void MenuItemCallback(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			foreach (Project proj in VsHelper.GetSelectedItemsOfType<Project>())
			{
				if (!VsHelper.IsSolutionItemsFolder(proj) 
					&& !ConfigProcessing.ConfigExistsForProject(proj))
				{

					string configPath = ScriptGenAssemblyCache.GetForProj(proj)?.GetConfigFilepath(proj.FullName);
					if (string.IsNullOrEmpty(configPath))
					{
						VsShellUtilities.ShowMessageBox(
							ServiceProvider,
							"Failed to find target configuration file path.  It is possible you need to update the Nuget Package.",
							"Add Config Failed",
							OLEMSGICON.OLEMSGICON_CRITICAL,
							OLEMSGBUTTON.OLEMSGBUTTON_OK,
							OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
						return;
					}


					if (!File.Exists(configPath))
					{
						ScriptGenAssemblyCache.GetForProj(proj).CreateNewConfigFile(configPath);
					}
					proj.ProjectItems.AddFromFile(configPath);

				}				
			}
			
		}


	}
}
