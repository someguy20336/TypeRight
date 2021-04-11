using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Configuration;

namespace TypeRight.Tests.Configuration
{
	[TestClass]
	public class ActionParameterJsonWritingTests
	{


		[TestMethod]
		public void NonCustomType_Url_IsSerializedToEnum()
		{
			string result = JsonConvert.SerializeObject(ActionParameter.Url).Trim('\"');

			Assert.AreEqual("url", result);
		}

		[TestMethod]
		public void NonCustomType_RequestMethod_IsSerializedToEnum()
		{
			string result = JsonConvert.SerializeObject(ActionParameter.RequestMethod).Trim('\"');

			Assert.AreEqual("requestMethod", result);
		}

		[TestMethod]
		public void NonCustomType_Body_IsSerializedToEnum()
		{
			string result = JsonConvert.SerializeObject(ActionParameter.Body).Trim('\"');

			Assert.AreEqual("body", result);
		}

		[TestMethod]
		public void CustomType_IsSerializedWithoutKind()
		{
			ActionParameter parameter = new ActionParameter("test", "string", false);
			string result = JsonConvert.SerializeObject(parameter).Trim('\"');

			Assert.AreEqual("{\"name\":\"test\",\"type\":\"string\",\"optional\":false}", result);
		}
	}
}
