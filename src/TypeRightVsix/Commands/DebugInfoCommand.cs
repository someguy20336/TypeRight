using System;
using System.ComponentModel.Design;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using TypeRightVsix.Dialogs;
using TypeRightVsix.Shared;

namespace TypeRightVsix.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class DebugInfoCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 4131;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("E49229F2-6882-421F-872E-90E1B1CC7534");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package package;

		/// <summary>
		/// Initializes a new instance of the <see cref="DebugInfoCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private DebugInfoCommand(Package package)
		{
			if (package == null)
			{
				throw new ArgumentNullException("package");
			}

			this.package = package;

			OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
			if (commandService != null)
			{
				var menuCommandID = new CommandID(CommandSet, CommandId);
				var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandID);
				commandService.AddCommand(menuItem);
				menuItem.BeforeQueryStatus += MenuItem_BeforeQueryStatus;
			}
		}

		private void MenuItem_BeforeQueryStatus(object sender, EventArgs e)
		{
			ThreadHelper.ThrowIfNotOnUIThread();
			OleMenuCommand button = (OleMenuCommand)sender;
			button.Visible = false;

			foreach (Project proj in VsHelper.GetSelectedCsharpProjects())
			{
				if (!VsHelper.IsSolutionItemsFolder(proj) && VsHelper.IsPackageInstalled(proj))
				{
					button.Visible = true;
				}
			}
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static DebugInfoCommand Instance
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
				return this.package;
			}
		}

		/// <summary>
		/// Initializes the singleton instance of the command.
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		public static void Initialize(Package package)
		{
			Instance = new DebugInfoCommand(package);
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
			ProjectInformationDialog dialog = new ProjectInformationDialog(VsHelper.GetSelectedCsharpProjects().First());
			dialog.ShowModal();
		}
	}
}
