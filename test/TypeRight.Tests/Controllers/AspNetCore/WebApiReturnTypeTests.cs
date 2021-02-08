using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class WebApiReturnTypeTests : ControllerTestsBase
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
	}
}
