using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using TypeRight.Configuration;
using TypeRight.Tests.Testers;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	/// <summary>
	/// This class is just for verifying the comments and imports because I want to cut that out of
	/// every other test because of how bulky and annoying it is
	/// </summary>
	[TestClass]
	public class ControllerImportAndCommentsTests : ControllerTestsBase
	{
		protected override bool IsAspNetCore => true;


		[TestMethod]
		public void ImportFromServerObjects_AreWritten()
		{
			ControllerBuilder
				.AddMethod("QueryParameterWithBody", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("fromBody", "TestClass", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.Commit()
				;

			AssertImportsAre(@$"
import * as DefaultResult from ""../../DefaultResult"";
import {{ fetchWrapper }} from ""../../FolderM/FolderN/FetchFile"";
");
		}

		[TestMethod]
		public void FetchConfig_ImportIsWritten()
		{
			GivenFetchConfig(new FetchConfig()
			{
				FilePath = TypeCollectionTester.FetchFilePath,
				Imports = new List<ImportDefinition>(),
				Name = "fetchWrapper",
				ReturnType = "void",
				Parameters = new List<ActionParameter>()
				{
					ActionParameter.RequestMethod,
					ActionParameter.Url,
					ActionParameter.Body
				}
			});

			ControllerBuilder
				.AddMethod("QueryParameterWithBody", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.Commit()
				;

			AssertImportsAre(@$"
import {{ fetchWrapper }} from ""../../FolderM/FolderN/FetchFile"";
");
		}

		[TestMethod]
		public void ActionComments_AreWritten()
		{
			ControllerBuilder
				.AddMethod("QueryParameterWithBody", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("fromBody", "TestClass", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
					.Commit()
				;

			AssertCommentsAre(@$"
/**
 * 
 * @param fromQuery 
 * @param fromBody 
 */
");
		}


		[TestMethod]
		public void ActionComments_FromServicesParameter_ParameterIsNotWritten()
		{
			ControllerBuilder
				.AddMethod("QueryParameterWithBody", MvcConstants.JsonResult_AspNetCore)
					.AddScriptActionAttribute()
					.AddParameter("fromQuery", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddParameter("fromServices", "TestClass", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
					.Commit()
				;

			AssertCommentsAre(@$"
/**
 * 
 * @param fromQuery 
 */
");
		}


		private void AssertImportsAre(string expected)
		{
			string scriptText = GetScriptText();

			var scriptLines = scriptText.Split(Environment.NewLine);
			scriptLines = scriptLines.Skip(1)
				.Where(ln => ln.StartsWith("import"))
				.ToArray();

			scriptText = string.Join(Environment.NewLine, scriptLines).Trim();
			expected = expected.Trim();
			Assert.AreEqual(expected, scriptText);
		}

		private void AssertCommentsAre(string expected)
		{
			string scriptText = GetScriptText();

			var scriptLines = scriptText.Split(Environment.NewLine);
			scriptLines = scriptLines.Skip(1)
				.Where(ln => ln.Trim().StartsWith("/*")
					|| ln.Trim().StartsWith("*")
					|| ln.Trim().StartsWith("*/"))
				.ToArray();

			scriptText = string.Join(Environment.NewLine, scriptLines).Trim();
			expected = expected.Trim();
			Assert.AreEqual(expected, scriptText);
		}
	}
}
