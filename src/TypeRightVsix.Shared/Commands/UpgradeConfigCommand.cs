using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TypeRightVsix.Imports;
using TypeRightVsix.Shared;
using Task = System.Threading.Tasks.Task;

namespace TypeRightVsix.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class UpgradeConfigCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 4132;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("E49229F2-6882-421F-872E-90E1B1CC7534");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly AsyncPackage package;

		/// <summary>
		/// Initializes a new instance of the <see cref="UpgradeConfigCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		/// <param name="commandService">Command service to add command to, not null.</param>
		private UpgradeConfigCommand(AsyncPackage package, OleMenuCommandService commandService)
		{
			this.package = package ?? throw new ArgumentNullException(nameof(package));
			commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

			var menuCommandID = new CommandID(CommandSet, CommandId);
			var menuItem = new OleMenuCommand(this.Execute, menuCommandID);
			commandService.AddCommand(menuItem);
			menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
		}

		private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			OleMenuCommand button = (OleMenuCommand)sender;
			button.Visible = false;

			var item = GetSelectedItem();
			if (item == null)
			{
				return;
			}

			var proj = item.ContainingProject;

			var importedTool = ScriptGenAssemblyCache.GetForProj(proj);
			if (importedTool is NullImportedTool)
			{
				return;
			}

			string configPath = importedTool.GetConfigFilepath(proj.FullName);
			if (configPath != item.FileNames[0])
			{
				return;
			}

			if (!importedTool.CanUpgradeConfig(configPath))
			{
				return;
			}

			button.Visible = true;

		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static UpgradeConfigCommand Instance
		{
			get;
			private set;
		}

		/// <summary>
		/// Gets the service provider from the owner package.
		/// </summary>
		private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
		{
			get
			{
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static async Task InitializeAsync(AsyncPackage package)
		{
			// Switch to the main thread - the call to AddCommand in UpgradeConfigCommand's constructor requires
			// the UI thread.
			await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

			OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
			Instance = new UpgradeConfigCommand(package, commandService);
		}

		private ProjectItem GetSelectedItem()
		{
			var selItems = VsHelper.GetSelectedItemsOfType<ProjectItem>().ToList();
			if (selItems.Count != 1)
			{
				return null;
			}

			return selItems.First();
		}

		/// <summary>
		/// This function is the callback used to execute the command when the menu item is clicked.
		/// See the constructor to see how the menu item is associated with this function using
		/// OleMenuCommandService service and MenuCommand class.
		/// </summary>
		/// <param name="sender">Event sender.</param>
		/// <param name="e">Event args.</param>
		private void Execute(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();

			var question = MessageBox.Show("Selecting this command will regenerate the config file.  Any unsupported options will be removed, but all others should remain unchanged." +
				"\r\rDo you want to continue?", "Upgrade Config?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (question == DialogResult.No)
			{
				return;
			}

			var item = GetSelectedItem();
			var proj = item.ContainingProject;
			var importedTool = ScriptGenAssemblyCache.GetForProj(proj);
			importedTool.UpgradeConfig(importedTool.GetConfigFilepath(proj.FullName));

		}
	}
}
