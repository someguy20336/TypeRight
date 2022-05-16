using EnvDTE;
using Microsoft.VisualStudio.PlatformUI;

namespace TypeRightVsix.Dialogs
{
	public class ProjectInformationDialog : DialogWindow
	{
		public ProjectInformationDialog(Project proj)
		{
			Microsoft.VisualStudio.Shell.ThreadHelper.ThrowIfNotOnUIThread();
			Title = $"{ proj.Name } Information";
			Content = new ProjectInformation(proj);
			Width = 700;
			Height = 400;
		}
	}
}
