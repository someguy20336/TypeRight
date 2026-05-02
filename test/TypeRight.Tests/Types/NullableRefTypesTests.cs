using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.Tests.TestBuilders;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Types;

[TestClass]
public class NullableRefTypesTests : TypesTestBase
{

	[TestMethod]
	[DataRow("string", "string")]
	[DataRow("int", "number")]
	[DataRow("DateOnly", "string")]
	[DataRow("DateTime", "string")]
	public void SimpleNullableBuiltInTypes(string type, string expectedType)
	{
		AddDefaultExtractedClass()
			.AddProperty("Property", type + "?")
			.Commit();

		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(TypeScriptHelper.TypeNameOrNull(expectedType));

	}

	[TestMethod]
	public void SimpleNullableRefTypeProperty()
	{
		AddExtractedClass("Extracted").Commit();

		AddDefaultExtractedClass()
			.AddProperty("Property", "Extracted?")
			.Commit();

		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(TypeScriptHelper.TypeNameOrNull($"{FakeTypePrefixer.Prefix}.Extracted"));

	}

	[TestMethod]
	public void NullableArrayType_WithNonNullRefType()
	{
		AddExtractedClass("Extracted").Commit();

		AddDefaultExtractedClass()
			.AddProperty("Property", "Extracted[]?")
			.Commit();

		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(TypeScriptHelper.TypeNameOrNull($"{FakeTypePrefixer.Prefix}.Extracted[]"));

	}

	[TestMethod]
	public void NullableArray_WithNullableElement()
	{
		AddExtractedClass("Extracted").Commit();

		AddDefaultExtractedClass()
			.AddProperty("Property", "Extracted?[]?")
			.Commit();

		string expected = TypeScriptHelper.TypeNameOrNull($"{FakeTypePrefixer.Prefix}.Extracted");
		expected = TypeScriptHelper.TypeNameOrNull($"({expected})[]");
		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(expected);

	}

	[TestMethod]
	public void NullableListType_WithNonNullRefType()
	{
		AddExtractedClass("Extracted").Commit();

		AddDefaultExtractedClass()
			.AddProperty("Property", "List<Extracted>?")
			.Commit();

		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(TypeScriptHelper.TypeNameOrNull($"{FakeTypePrefixer.Prefix}.Extracted[]"));

	}

	[TestMethod]
	public void NullableListType_WithNullRefType()
	{
		AddExtractedClass("Extracted").Commit();

		AddDefaultExtractedClass()
			.AddProperty("Property", "List<Extracted?>?")
			.Commit();

		string expected = TypeScriptHelper.TypeNameOrNull($"{FakeTypePrefixer.Prefix}.Extracted");
		expected = TypeScriptHelper.TypeNameOrNull($"({expected})[]");
		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(expected);
	}

	[TestMethod]
	public void NullableDictionary_SimpleTypes()
	{

		AddDefaultExtractedClass()
			.AddProperty("Property", "Dictionary<int, string>?")
			.Commit();

		string expected = TypeScriptHelper.FormatDictionaryType("number", "string");
		expected = TypeScriptHelper.TypeNameOrNull(expected);
		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(expected);
	}

	[TestMethod]
	public void NullableDictionary_SimpleTypes_NullableValue()
	{
		AddDefaultExtractedClass()
			.AddProperty("Property", "Dictionary<int, string?>?")
			.Commit();

		string expected = TypeScriptHelper.FormatDictionaryType("number", TypeScriptHelper.TypeNameOrNull("string"));
		expected = TypeScriptHelper.TypeNameOrNull(expected);
		AssertThatTheDefaultReferenceType()
			.TestPropertyWithName("Property")
			.TypescriptNameIs(expected);
	}
}
