//------------------------------------------------------------------------------
// <copyright file="GenerateScriptsCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TypeRightVsix.Shared;
using Microsoft.CodeAnalysis;
using TypeRight;

namespace TypeRightVsix.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class GenerateScriptsCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 256;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("E49229F2-6882-421F-872E-90E1B1CC7534");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="GenerateScriptsCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private GenerateScriptsCommand(Package package)
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
		/// Determines whether this command is visible
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
		{
			OleMenuCommand button = (OleMenuCommand)sender;
			button.Enabled = false;
			
			foreach (EnvDTE.Project proj in VsHelper.GetSelectedItemsOfType<EnvDTE.Project>())
			{
				if (!VsHelper.IsSolutionItemsFolder(proj) 
					&& VsHelper.IsPackageInstalled(proj)
					&& ConfigProcessing.IsGenEnabledForProject(proj))
				{
					button.Enabled = true;
				}
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static GenerateScriptsCommand Instance
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
			Instance = new GenerateScriptsCommand(package);
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
			Workspace currentWorkspace = VsHelper.Current.GetCurrentWorkspace();

			foreach (EnvDTE.Project proj in VsHelper.GetSelectedItemsOfType<EnvDTE.Project>())
			{
				if (ConfigProcessing.IsGenEnabledForProject(proj))
				{
					IScriptGenEngineProvider<Workspace> provider = Imports.ScriptGenAssemblyCache.GetForProj(proj).EngineProvider;
					IScriptGenEngine engine = provider.GetEngine(currentWorkspace, proj.FullName);
					IScriptGenerationResult result = engine.GenerateScripts();
					// Show a message box to prove we were here
					if (!result.Sucess)
					{
						VsShellUtilities.ShowMessageBox(
							this.ServiceProvider,
							result.ErrorMessage,
							"Script Generation Failed",
							OLEMSGICON.OLEMSGICON_CRITICAL,
							OLEMSGBUTTON.OLEMSGBUTTON_OK,
							OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
					}
				}
			}			
		}
	}
}
