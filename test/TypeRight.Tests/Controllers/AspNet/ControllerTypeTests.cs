using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers.AspNet
{
	[TestClass]
	public class ControllerTypeTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => false;

		[TestInitialize]
		public override void TestInitialize()
		{
			base.TestInitialize();
		}


		[TestMethod]
		public void StringResult_IsTypescriptString()
		{
			AddControllerAction("StringResult", "string")
				.AddLineOfCode("return \"Hi\";", 0)
				.Commit();

			AssertThatThisControllerAction("StringResult")
				.ReturnTypeTypescriptNameIs("string");

		}


		[TestMethod]
		public void NewObjectResult_IsExtractedType()
		{
			AddControllerAction("NewObjectResult", "TestClass")
				.AddLineOfCode("return new TestClass();", 0)
				.Commit();

			AssertThatThisControllerAction("NewObjectResult")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		[TestMethod]
		public void JsonResult_NewObjectResult_IsExtractedType()
		{
			AddControllerAction("NewObjectResult_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("return Json(new TestClass());", 0)
				.Commit();

			AssertThatThisControllerAction("NewObjectResult_Json")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");

		}

		[TestMethod]
		public void VariableResult_UsesVariableType()
		{
			AddControllerAction("VariableResult", "bool")
				.AddLineOfCode("bool testVar = true;", 0)
				.AddLineOfCode("return testVar;", 0)
				.Commit();

			AssertThatThisControllerAction("VariableResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BooleanTypeName);
		}

		[TestMethod]
		public void JsonResult_VariableResult_UsesVariableType()
		{
			AddControllerAction("VariableResult_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("bool testVar = true;", 0)
				.AddLineOfCode("return Json(testVar);", 0)
				.Commit();

			//VariableResult
			AssertThatThisControllerAction("VariableResult_Json")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.BooleanTypeName);
		}

		[TestMethod]
		public void String_ConcatResult_IsStringType()
		{
			AddControllerAction("StringConcatResult", "string")
				.AddLineOfCode("return \"Hello\" + \"World\";", 0)
				.Commit();

			AssertThatThisControllerAction("StringConcatResult")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.StringTypeName);
		}

		[TestMethod]
		public void Method_IsMethodType()
		{
			ControllerBuilder
				.AddMethod("ARandomMethod", "TestClass")  
				.AddLineOfCode("return null;", 0)
				.Commit();

			AddControllerAction("MethodResult", "TestClass")
				.AddLineOfCode("return ARandomMethod();", 0)
				.Commit();

			AssertThatThisControllerAction("MethodResult")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}


		[TestMethod]
		public void JsonResult_WithMethod_FindsReturnType()
		{
			AddControllerAction("TestAction", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("return Json(AMethod());", 0)
				.Commit();

			ControllerBuilder.AddMethod("AMethod", "string")
				.AddLineOfCode("return null;", 0)
				.Commit();

			AssertThatThisControllerAction("TestAction")
				.ReturnTypeTypescriptNameIs($"string");
		}

		[TestMethod]
		public void SimpleParameter_IsParameterType()
		{
			AddControllerAction("SimpleParameter_Json", MvcConstants.JsonResult_AspNet)
				.AddParameter("testParam", "TestClass")
				.AddLineOfCode("return Json(testParam);", 0)
				.Commit();

			AssertThatThisControllerAction("SimpleParameter_Json")
				.ParameterTypeIs("testParam", $"{FakeTypePrefixer.Prefix}.TestClass");

		}

		[TestMethod]
		public void JsonResult_SimpleParameter_IsParameterType()
		{
			AddControllerAction("SimpleParameter_Json", MvcConstants.JsonResult_AspNet)
				.AddParameter("testParam", "TestClass")
				.AddLineOfCode("return Json(testParam);", 0)
				.Commit();

			AssertThatThisControllerAction("SimpleParameter_Json")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

		[TestMethod]
		public void NotExtracted_IsAnyType()
		{
			AddControllerAction("NotExtracted_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("return Json(new NotExtracted());", 0)
				.Commit();

			AssertThatThisControllerAction("NotExtracted_Json")
				.ReturnTypeTypescriptNameIs(TypeScriptHelper.AnyTypeName);
		}

		[TestMethod]
		public void Parameter_Dictionary_IsDictionaryType()
		{
			AddControllerAction("ComplexMethod", MvcConstants.JsonResult_AspNet)
				.AddParameter("dateStringDict", "Dictionary<DateTime, string>")
				.AddParameter("intStringDict", "Dictionary<int, string>")
				.AddParameter("stringStringDict", "Dictionary<string, string>")
				.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
				.AddLineOfCode("return Json(gen);", 0)
				.Commit();

			//DictionaryParams
			AssertThatThisControllerAction("ComplexMethod")
				.ParameterTypeIs("dateStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.StringTypeName, TypeScriptHelper.StringTypeName))
				.ParameterTypeIs("intStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.NumericTypeName, TypeScriptHelper.StringTypeName))
				.ParameterTypeIs("stringStringDict", TypeScriptHelper.FormatDictionaryType(TypeScriptHelper.StringTypeName, TypeScriptHelper.StringTypeName))
				;
		}

		[TestMethod]
		public void ClassWithGeneric_ReturnTypeIsGenertic()
		{
			AddControllerAction("ComplexMethod", MvcConstants.JsonResult_AspNet)
				.AddParameter("dateStringDict", "Dictionary<DateTime, string>")
				.AddParameter("intStringDict", "Dictionary<int, string>")
				.AddParameter("stringStringDict", "Dictionary<string, string>")
				.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
				.AddLineOfCode("return Json(gen);", 0)
				.Commit();


			AssertThatThisControllerAction("ComplexMethod")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestGenericClass<{FakeTypePrefixer.Prefix}.TestClass>");
		}

		[TestMethod]
		public void GenericValue_ReturnTypeIsGenericType()
		{
			AddControllerAction("GenericPropReturn_Json", MvcConstants.JsonResult_AspNet)
				.AddLineOfCode("TestGenericClass<TestClass> gen = new TestGenericClass<TestClass>();", 0)
				.AddLineOfCode("return Json(gen.GenericProp);", 0)
				.Commit();

			AssertThatThisControllerAction("GenericPropReturn_Json")
				.ReturnTypeTypescriptNameIs($"{FakeTypePrefixer.Prefix}.TestClass");
		}

	}
}
