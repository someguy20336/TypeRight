using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Configuration;

namespace TypeRight.Tests.Configuration
{
	[TestClass]
	public class QueryParamsConfigReadingTests
	{

		[TestMethod]
		public void NullValue_ConvertedToEmptyNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": null
}
");

			Assert.AreEqual(0, config.QueryParams.Count);
		}

		[TestMethod]
		public void EmptyStringValue_ConvertedToEmptyNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": """"
}
");

			Assert.AreEqual(0, config.QueryParams.Count);
		}

		[TestMethod]
		public void StringValue_Single_ConvertedToNameValCollection()
		{
			var config = ConfigParser.ParseFromJson(@"
{
	""queryParams"": ""param1=value""
}
");

			Assert.AreEqual("value", config.QueryParams.Get("param1"));
		}

		[TestMethod]
		public void StringValue_Multi_ConvertedToNameValCollection()
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
		public void Object_StringValue_ConvertedToNameValueCollection()
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
		public void Object_NumericValue_ConvertedToNameValueCollection()
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
		public void Object_NullValue_ConvertedToNameValueCollection()
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
		public void Object_BooleanValue_ConvertedToNameValueCollection()
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
		public void Object_MultipleProperties_ConvertedToNameValueCollection()
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
	}
}
