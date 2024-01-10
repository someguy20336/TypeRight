using Microsoft.Build.Locator;
using System.Diagnostics;

namespace TypeRight.BuildTests;

internal class MsBuildTestRunner
{
	private static string s_msBuild = null!;
	private static string _testFolderRoot = null!;

	private readonly List<string> _output = [];
	private readonly Dictionary<string, string> _expectedContents = new();

	private readonly string _projRelPath;
	private readonly IEnumerable<string> _testFiles;

	static MsBuildTestRunner()
	{
		SetTestRootFolder();
		SetMsBuildLocation();
	}

	public MsBuildTestRunner(string projRelPath, IEnumerable<string> testFiles)
	{
		_projRelPath = projRelPath;
		_testFiles = testFiles;

		CacheExpected();
	}

	public void BuildProject(string addlArgs = "")
	{

		string aspNetCorePath = Path.Combine(_testFolderRoot, _projRelPath);

		Process buildProcess = new();
		buildProcess.StartInfo.FileName = s_msBuild;
		buildProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		buildProcess.StartInfo.CreateNoWindow = true;
		buildProcess.StartInfo.RedirectStandardOutput = true;
		buildProcess.StartInfo.Arguments = $"build \"{aspNetCorePath}\" --disable-build-servers --force -p:TestingFromPackageRef=false {addlArgs}";
		buildProcess.Start();
		while (!buildProcess.StandardOutput.EndOfStream)
		{
			_output.Add(buildProcess.StandardOutput.ReadLine() ?? string.Empty);
		}
		buildProcess.WaitForExit();
		Assert.IsTrue(buildProcess.ExitCode == 0);
		buildProcess.Close();
	}

	public void AssertContentsMatchExpected()
	{
		Assert.IsTrue(_output.Count > 0);
		foreach (var item in _testFiles)
		{
			string fullPath = Path.Combine(_testFolderRoot, item);
			string contents = File.ReadAllText(fullPath);
			Assert.AreEqual(_expectedContents[fullPath], contents);
		}
	}

	public void AssertFilesDontExist()
	{
		Assert.IsTrue(_output.Count > 0);
		foreach (var fullPath in _expectedContents.Keys)
		{
			Assert.IsFalse(File.Exists(fullPath));
		}
	}

	private static void SetMsBuildLocation()
	{
		if (!string.IsNullOrEmpty(s_msBuild))
		{
			return;
		}

		// This appears to go several directories in with SDKs and the actual
		// dotnet.exe appears to be at the root of the install.  Hopefully it works..
		// Default install may be "C:\\Program Files\\dotnet\\dotnet.exe", but not in a github action
		// https://github.com/actions/setup-dotnet/blob/main/externals/install-dotnet.ps1#L763
		VisualStudioInstance firstSdkInstance = MSBuildLocator.QueryVisualStudioInstances().First();

		DirectoryInfo dir = new(firstSdkInstance.MSBuildPath);
		while (dir.Parent != null)
		{
			string testPath = Path.Combine(dir.FullName, "dotnet.exe");
			if (File.Exists(testPath))
			{
				s_msBuild = testPath;
				break;
			}
			dir = dir.Parent;
		}

		Assert.IsNotNull(s_msBuild);
	}


	private static void SetTestRootFolder()
	{
		string assembDir = Path.GetDirectoryName(typeof(AspNetCoreProjectTests).Assembly.Location)!;
		DirectoryInfo dir = new(assembDir);
		while (dir.Parent != null)
		{
			if (dir.Name == "test")
			{
				_testFolderRoot = dir.FullName;
				break;
			}
			dir = dir.Parent;
		}
		Assert.IsNotNull(_testFolderRoot);
	}


	private void CacheExpected()
	{
		foreach (var item in _testFiles)
		{
			string fullPath = Path.Combine(_testFolderRoot, item);
			string contents = File.ReadAllText(fullPath);
			_expectedContents.Add(fullPath, contents);
		}
	}


	public void DeleteOriginalFiles()
	{
		foreach (var fullPath in _expectedContents.Keys)
		{
			File.Delete(fullPath);
		}
	}

	public void RestoreOriginalFiles()
	{
		foreach (var fullPath in _expectedContents.Keys)
		{
			File.WriteAllText(fullPath, _expectedContents[fullPath]);
		}
	}
}
