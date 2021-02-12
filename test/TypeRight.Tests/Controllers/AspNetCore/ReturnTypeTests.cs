using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class ReturnTypeTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => true;


		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();
		}

		[TestMethod]
		public void WebApi_ReturnObjectIsFound()
		{
			AddControllerAction("GetStringList", "List<string>")
				.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore).AddConstructorArg("\"getList\"").Commit()
				.AddLineOfCode("return new List<string>();", 0)
				.Commit();

			AssertThatThisControllerAction("GetStringList")
				.ReturnTypeTypescriptNameIs("string[]");
		}

		[TestMethod]
		public void WebApi_ActionResultUsesTypeParam()
		{
			AddControllerAction("GetStringActionResult", MvcConstants.ActionResult_AspNetCore + "<string>")
				.AddLineOfCode("return null;", 0)
				.Commit();


			AssertThatThisControllerAction("GetStringActionResult")
				.ReturnTypeTypescriptNameIs("string");
		}

		[TestMethod]
		public void WebApi_ActionResultObject_GetsType()
		{
			AddControllerAction("GetActionResultForTestClass", MvcConstants.ActionResult_AspNetCore + "<object>")
				.AddLineOfCode($"return new {MvcConstants.ActionResult_AspNetCore}{"<object>"}(new TestClass());", 0)
				.Commit();

			AssertThatThisControllerAction("GetActionResultForTestClass")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		[TestMethod]
		public void WebApi_ActionResultAnonymousObject_GetsType()
		{
			AddControllerAction("GetActionResultForAnonymous", MvcConstants.ActionResult_AspNetCore + "<object>")
				.AddLineOfCode($"return new {MvcConstants.ActionResult_AspNetCore}{"<object>"}(new {{ prop = 1, thing = 4 }});", 0)
				.Commit();

			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "prop", TypeScriptHelper.NumericTypeName },
				{ "thing", TypeScriptHelper.NumericTypeName }
			};

			AssertThatThisControllerAction("GetActionResultForAnonymous")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));
		}

		[TestMethod]
		public void WebApi_Task_GetsType()
		{
			AddControllerAction("GetTaskResult", "System.Threading.Tasks.Task<string>")
				.AddLineOfCode("return null;", 0)
				.Commit();

			AssertThatThisControllerAction("GetTaskResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.StringTypeName);
		}

		[TestMethod]
		public void WebApi_Task_GetsClassType()
		{
			AddControllerAction("GetTaskResultClass", "System.Threading.Tasks.Task<TestClass>")
				.AddLineOfCode("return null;", 0)
				.Commit();

			AssertThatThisControllerAction("GetTaskResultClass")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}



		[TestMethod]
		public void AnonymousTypes_Simple()
		{
			AddControllerAction("SimpleAnonymousType", MvcConstants.JsonResult_AspNetCore)
				.AddLineOfCode("return Json(new { stringProp = \"Hi\", intProp = 1 });", 0)
				.Commit();

			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "stringProp", TypeScriptHelper.StringTypeName },
				{ "intProp", TypeScriptHelper.NumericTypeName }
			};

			AssertThatThisControllerAction("SimpleAnonymousType")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));

		}

		[TestMethod]
		public void AnonymousTypes_WithExtractedObject()
		{
			AddControllerAction("AnonymousTypeWithExtracted", "object")
				.AddLineOfCode("TestClass test = new TestClass();", 0)
				.AddLineOfCode("return new { testClassProp = test, intProp = 1 };", 0)
				.Commit();

			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "testClassProp", $"{FakeTypePrefixer.Prefix}.TestClass" },
				{ "intProp", TypeScriptHelper.NumericTypeName }
			};

			AssertThatThisControllerAction("AnonymousTypeWithExtracted")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));

		}

		[TestMethod]
		public void AnonymousTypes_WithNonExtractedObject()
		{
			AddClass("NotExtracted")
				.AddProperty("DontCare", "int")
				.Commit();

			AddControllerAction("AnonymousTypeWithNonExtracted", MvcConstants.JsonResult_AspNetCore)
				.AddLineOfCode("NotExtracted test = new NotExtracted();", 0)
				.AddLineOfCode("return Json(new { testClassProp = test, intProp = 1 });", 0)
				.Commit();

			Dictionary<string, string> expected = new Dictionary<string, string>
			{
				{ "testClassProp", "any" },
				{ "intProp", TypeScriptHelper.NumericTypeName }
			};

			AssertThatThisControllerAction("AnonymousTypeWithNonExtracted")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BuildAnonymousType(expected));

		}
	}
}
