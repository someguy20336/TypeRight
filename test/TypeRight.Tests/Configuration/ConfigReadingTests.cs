using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.ScriptWriting;

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
	""propNameCasingConverter"": ""camel""
}
");

			Assert.AreEqual(PropertyNamingStrategyType.Camel, config.PropNameCasingConverter);
		}

		[TestMethod]
		public void Casing_None_IsSerialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""propNameCasingConverter"": ""none""
}
");

			Assert.AreEqual(PropertyNamingStrategyType.None, config.PropNameCasingConverter);
		}

		[TestMethod]
		public void Casing_Null_IsSerialized()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""propNameCasingConverter"": null
}
");

			Assert.AreEqual(PropertyNamingStrategyType.None, config.PropNameCasingConverter);
		}
	}
}
