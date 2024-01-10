namespace TypeRight.BuildTests;

[TestClass]
public class ReplaceWithJsTest
{

	private MsBuildTestRunner _runner = null!;

	[TestInitialize]
	public void BuildProject()
	{
		_runner = new MsBuildTestRunner("TestProjects\\ReplaceWithJsExample\\ReplaceWithJsExample.sln", [
			@"TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\ServerObjects.ts",
			@"TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\CustomGroup.ts",
			@"TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\Home\Models.ts",
			@"TestProjects\ReplaceWithJsExample\ReplaceWithJsExample\Scripts\Home\HomeActions.ts"
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
