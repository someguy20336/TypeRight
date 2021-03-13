using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using TypeRightVsix.Shared;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.ComponentModelHost;
using NuGet.VisualStudio;
using System.Windows.Forms;

namespace TypeRightVsix.Commands
{
	/// <summary>
	/// Command handler
	/// </summary>
	internal sealed class InstallNugetPackageCommand
	{
		/// <summary>
		/// Command ID.
		/// </summary>
		public const int CommandId = 4130;

		/// <summary>
		/// Command menu group (command set GUID).
		/// </summary>
		public static readonly Guid CommandSet = new Guid("E49229F2-6882-421F-872E-90E1B1CC7534");

		/// <summary>
		/// VS Package that provides this command, not null.
		/// </summary>
		private readonly Package _package;

		/// <summary>
		/// Initializes a new instance of the <see cref="InstallNugetPackageCommand"/> class.
		/// Adds our command handlers for menu (commands must exist in the command table file)
		/// </summary>
		/// <param name="package">Owner package, not null.</param>
		private InstallNugetPackageCommand(Package package)
		{
			this._package = package ?? throw new ArgumentNullException(nameof(package));

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
			ThreadHelper.ThrowIfNotOnUIThread();
			OleMenuCommand button = (OleMenuCommand)sender;
			button.Enabled = false;
			button.Visible = false;

			if (!VsHelper.GetSelectedItemsOfType<Project>().Any())
			{
				return;
			}

			button.Visible = true;
						
			foreach (Project proj in VsHelper.GetSelectedItemsOfType<Project>())
			{
				if (!VsHelper.IsSolutionItemsFolder(proj) && !VsHelper.IsPackageInstalled(proj))
				{
					button.Enabled = true;
				}
			}			
		}

		/// <summary>
		/// Gets the instance of the command.
		/// </summary>
		public static InstallNugetPackageCommand Instance
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
			Instance = new InstallNugetPackageCommand(package);
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
			var question = MessageBox.Show("Selecting this command will install a NuGet package that will enable this project for script generation. " + 
				"\r\rDo you want to continue?", "Install NuGet Package?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

			if (question == DialogResult.No)
				return;

			foreach (Project proj in VsHelper.GetSelectedItemsOfType<Project>())
			{
				if (!VsHelper.IsSolutionItemsFolder(proj) && !VsHelper.IsPackageInstalled(proj))
				{
					try
					{
						IComponentModel componentModel = (IComponentModel)Package.GetGlobalService(typeof(SComponentModel));
						IVsPackageInstaller installer = componentModel.GetService<IVsPackageInstaller>();
						installer.InstallPackage(null, proj, TypeRightPackage.NugetID, (string)null, false);
					}
					catch (Exception exception)
					{
						// Show a message box to prove we were here
						VsShellUtilities.ShowMessageBox(
							this.ServiceProvider,
							"There was an error installing the package: \n\n" + exception.Message,	
							"Could not install package",
							OLEMSGICON.OLEMSGICON_CRITICAL,
							OLEMSGBUTTON.OLEMSGBUTTON_OK,
							OLEMSGDEFBUTTON.OLEMSGDEFBUTTON_FIRST);
					}
					
				}
			}
			
		}
	}
}
