using EnvDTE;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using TypeRightVsix.Imports;

namespace TypeRightVsix.Dialogs
{
	/// <summary>
	/// Interaction logic for ProjInfoXaml.xaml
	/// </summary>
	public partial class ProjectInformation : UserControl
	{
		private DispatcherTimer _messageTimer;
		public ProjectInformation(Project proj)
		{
			InitializeComponent();
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			txtProjectName.Text = proj.Name;

			var imported = ScriptGenAssemblyCache.GetForProj(proj);

			lblVersion.Content = imported.AssemblyVersion;

			AddLink(lnkFromDirectory, imported.AssemblyDirectory);
			AddLink(lnkCachedPath, imported.CachePath);
			AddLink(lnkCachedBasePath, ScriptGenAssemblyCache.CacheBasePath);

			_messageTimer = new DispatcherTimer();
			_messageTimer.Interval = TimeSpan.FromSeconds(5);
			_messageTimer.Tick += _messageTimer_Tick; ;

			Dispatcher.ShutdownStarted += Dispatcher_ShutdownStarted;
		}

		private void _messageTimer_Tick(object sender, EventArgs e)
		{
			_messageTimer.Stop();
			txtClearResult.Text = "";
		}

		private void Dispatcher_ShutdownStarted(object sender, EventArgs e)
		{
			_messageTimer.Stop();
			_messageTimer = null;
		}


		private void AddLink(Hyperlink hyperlink, string path)
		{
			if (!string.IsNullOrEmpty(path))
			{
				hyperlink.Inlines.Add(path);
				hyperlink.NavigateUri = new Uri(path);
			}
		}

		private void OnPathClicked(object sender, RoutedEventArgs e)
		{
			Label label = sender as Label;

			string path = label.Content.ToString();
			if (!string.IsNullOrEmpty(path))
			{
				
				e.Handled = true;
			}
		}

		private void Link_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			System.Diagnostics.Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}

		private void ClearCache_Click(object sender, RoutedEventArgs e)
		{
			string message;
			if (!ScriptGenAssemblyCache.ClearCache())
			{
				// Show a message box to prove we were here
				message = "The cache is in use and cannot be cleared.  It will attempt again next time Visual Studio is restarted.";
				ScriptGenAssemblyCache.MarkForClear();
			}
			else
			{
				message = "Cache succesfully cleared.";
			}
			txtClearResult.Text = message;

			_messageTimer.Stop();
			_messageTimer.Start();
		}

	}
}
