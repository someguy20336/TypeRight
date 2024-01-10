using System.Diagnostics;

namespace TypeRight.BuildTests;

[TestClass]
public class MsBuildTests
{
	public const string MsBuild = "C:\\Program Files\\dotnet\\dotnet.exe";
	private static string s_testFolderRoot = null!;

	private static Process s_buildProcess = new();
	private static List<string> s_output = [];

	private static List<string> s_testFiles = 
		[
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\ServerObjects.ts",
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\CustomGroup.ts",
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\Models.ts",
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\HomeActions.ts"
		];

	private static Dictionary<string, string> s_expectedContents = new();

	[ClassInitialize]
	public static void BuildProject(TestContext _)
	{
		SetTestRootFolder();
		CacheExpected();
		string aspNetCorePath = Path.Combine(s_testFolderRoot, "TestProjects\\AspNetCore\\TestAspNetCore.sln");
		s_buildProcess.StartInfo.FileName = MsBuild;
		s_buildProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
		s_buildProcess.StartInfo.CreateNoWindow = true;
		s_buildProcess.StartInfo.RedirectStandardOutput = true;
		s_buildProcess.StartInfo.Arguments = $"build \"{aspNetCorePath}\"";
		s_buildProcess.Start();
		while (!s_buildProcess.StandardOutput.EndOfStream)
		{
			s_output.Add(s_buildProcess.StandardOutput.ReadLine() ?? string.Empty);
		}
		s_buildProcess.WaitForExit();
	}

	[ClassCleanup]
	public static void Cleanup()
	{
		s_buildProcess.Close();
		// TODO: re-create files
	}

	[TestMethod]
	public void ContentsMatchExpected()
	{
		foreach (var item in s_testFiles)
		{
			string fullPath = Path.Combine(s_testFolderRoot, item);
			string contents = File.ReadAllText(fullPath);
			Assert.AreEqual(s_expectedContents[fullPath], contents);
		}
	}

	private static void SetTestRootFolder()
	{
		string assembDir = Path.GetDirectoryName(typeof(MsBuildTests).Assembly.Location)!;
		DirectoryInfo dir = new(assembDir);
		while (dir.Parent != null)
		{		
			if (dir.Name == "test")
			{
				s_testFolderRoot = dir.FullName;
				break;
			}
			dir = dir.Parent;
		}
		Assert.IsNotNull(s_testFolderRoot);
	}


	private static void CacheExpected()
	{
		foreach (var item in s_testFiles)
		{
			string fullPath = Path.Combine(s_testFolderRoot, item);
			string contents = File.ReadAllText(fullPath);
			s_expectedContents.Add(fullPath, contents);
		}
	}
}