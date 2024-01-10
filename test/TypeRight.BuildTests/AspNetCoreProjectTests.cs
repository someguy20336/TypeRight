
namespace TypeRight.BuildTests;

[TestClass]
public class AspNetCoreProjectTests
{

	private MsBuildTestRunner _runner = null!;

	[TestInitialize]
	public void BuildProject()
	{
		_runner = new MsBuildTestRunner("TestProjects\\AspNetCore\\TestAspNetCore.sln", [
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\ServerObjects.ts",
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\CustomGroup.ts",
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\Models.ts",
			@"TestProjects\AspNetCore\TestAspNetCoreApp\Scripts\Home\HomeActions.ts"
		]);
		_runner.DeleteOriginalFiles();
	}

	[TestCleanup]
	public void Cleanup()
	{
		_runner.RestoreOriginalFiles();
	}

	[TestMethod]
	public void SkipTypeRightFlagSet_BuildIsSkipped()
	{
		_runner.BuildProject("-p:SkipTypeRight=true");
		_runner.AssertFilesDontExist();
	}

	[TestMethod]
	public void ContentsMatchExpected()
	{
		_runner.BuildProject();
		_runner.AssertContentsMatchExpected();
	}

}