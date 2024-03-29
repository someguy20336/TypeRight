﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;

namespace TypeRight.Tests.Configuration
{
	[TestClass]
	public class ConfigReadingTests
	{

		[TestMethod]
		public void QueryParams_NullValue_ConvertedToEmptyNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": null
}
");

			Assert.AreEqual(0, config.QueryParams.Count);
		}

		[TestMethod]
		public void QueryParams_EmptyStringValue_ConvertedToEmptyNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": """"
}
");

			Assert.AreEqual(0, config.QueryParams.Count);
		}

		[TestMethod]
		public void QueryParams_StringValue_Single_ConvertedToNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": ""param1=value""
}
");

			Assert.AreEqual("value", config.QueryParams.Get("param1"));
		}

		[TestMethod]
		public void QueryParams_StringValue_Multi_ConvertedToNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": ""param1=value&param2=val2""
}
");

			Assert.AreEqual("value", config.QueryParams.Get("param1"));
			Assert.AreEqual("val2", config.QueryParams.Get("param2"));
		}

		[TestMethod]
		public void QueryParams_Object_StringValue_ConvertedToNameValueCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": {
		""param1"": ""value""
	}
}
");

			Assert.AreEqual("value", config.QueryParams.Get("param1"));
		}

		[TestMethod]
		public void QueryParams_Object_NumericValue_ConvertedToNameValueCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": {
		""param1"": 1
	}
}
");

			Assert.AreEqual("1", config.QueryParams.Get("param1"));
		}

		[TestMethod]
		public void QueryParams_Object_NullValue_ConvertedToNameValueCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": {
		""param1"": null
	}
}
");

			Assert.AreEqual("", config.QueryParams.Get("param1"));
		}

		[TestMethod]
		public void QueryParams_Object_BooleanValue_ConvertedToNameValueCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": {
		""param1"": true
	}
}
");

			Assert.AreEqual("true", config.QueryParams.Get("param1"));
		}

		[TestMethod]
		public void QueryParams_Object_MultipleProperties_ConvertedToNameValueCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": {
		""param1"": true,
		""param2"": 1,
		""param3"": false
	}
}
");

			Assert.AreEqual(3, config.QueryParams.Count);
		}

		[TestMethod]
		public void Casing_Camel_IsSerialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""nameCasingConverter"": ""camel""
}
");

			Assert.AreEqual(NamingStrategyType.Camel, config.NameCasingConverter);
		}

		[TestMethod]
		public void Casing_None_IsSerialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""nameCasingConverter"": ""none""
}
");

			Assert.AreEqual(NamingStrategyType.None, config.NameCasingConverter);
		}

		[TestMethod]
		public void Casing_Null_IsSerialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""nameCasingConverter"": null
}
");

			Assert.AreEqual(NamingStrategy.DefaultType, config.NameCasingConverter);
		}


		[TestMethod]
		public void FetchConfig_NamedActionParameters_AreDeserialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""fetchConfig"": {
		""parameters"": [
			""requestMethod"",
			""url"",
			""body""
		]
	}
}
");
			Assert.AreEqual(ParameterKind.RequestMethod, config.FetchConfig.Parameters[0].Kind);
			Assert.AreEqual(ParameterKind.Url, config.FetchConfig.Parameters[1].Kind);
			Assert.AreEqual(ParameterKind.Body, config.FetchConfig.Parameters[2].Kind);
		}

		[TestMethod]
		public void FetchConfig_CustomActionParameter_IsDeserialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""fetchConfig"": {
		""parameters"": [
			{
				""name"": ""test"",
				""type"": ""string""
			}
		]
	}
}
");
			var oneParam = config.FetchConfig.Parameters[0];
			Assert.AreEqual(ParameterKind.Custom, oneParam.Kind);
			Assert.AreEqual("test", oneParam.Name);
			Assert.AreEqual("string", oneParam.Type);
		}

		[TestMethod]
		public void ImportModuleNameType_NotSpecified_IsExtensionless()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""nameCasingConverter"": null
}
");
			Assert.AreEqual(ImportModuleNameStyle.Extensionless, config.ImportModuleNameStyle);
		}

		[TestMethod]
        public void ImportModuleNameType_NullValue_IsExtensionless()
        {
			var config = ConfigParser.ParseFromJson(@"
{
	""importModuleNameStyle"": null
}
");
			Assert.AreEqual(ImportModuleNameStyle.Extensionless, config.ImportModuleNameStyle);
		}

		[TestMethod]
		public void ImportModuleNameType_Extensionless_IsExtensionless()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""importModuleNameStyle"": ""extensionless""
}
");
			Assert.AreEqual(ImportModuleNameStyle.Extensionless, config.ImportModuleNameStyle);
		}

		[TestMethod]
		public void ImportModuleNameType_ReplaceWithJs_IsExtensionless()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""importModuleNameStyle"": ""replaceWithJs""
}
");
			Assert.AreEqual(ImportModuleNameStyle.ReplaceWithJs, config.ImportModuleNameStyle);
		}
	}
}
