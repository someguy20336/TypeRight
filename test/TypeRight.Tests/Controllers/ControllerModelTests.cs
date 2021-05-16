using Microsoft.VisualStudio.TestTools.UnitTesting;
using TypeRight.Configuration;
using TypeRight.TypeProcessing;

namespace TypeRight.Tests.Controllers
{
	[TestClass]
	public class ControllerModelTests : ControllerTestsBase
	{

		protected override bool IsAspNetCore => true;


		[TestMethod]
		public void IdentifiesQueryParameter()
		{
			AddControllerAction("FromQuerySingleParamAction", "string")
					.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit();

			AssertThatThisControllerAction("FromQuerySingleParamAction")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query);
		}

		[TestMethod]
		public void IdentifiesQueryAndBodyParams()
		{
			AddControllerAction("HasFromQueryAndBody", "string")
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("body", "int", attribute: MvcConstants.FromBodyAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerAction("HasFromQueryAndBody")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("body", ActionParameterSourceType.Body);
		}

		[TestMethod]
		public void FromServicesIsIgnored()
		{
			AddControllerAction("HasFromQueryAndServices", "string")
				.AddParameter("id", "string", attribute: MvcConstants.FromQueryAttributeFullName_AspNetCore)
				.AddParameter("service", "int", attribute: MvcConstants.FromServicesAttributeFullName_AspNetCore)
				.AddLineOfCode("return null", 0)
				.Commit();

			AssertThatThisControllerAction("HasFromQueryAndServices")
				.ParameterSourceTypeIs("id", ActionParameterSourceType.Query)
				.ParameterSourceTypeIs("service", ActionParameterSourceType.Ignored);
		}

		[TestMethod]
		public void ImportsFetchFile()
		{
			ConfigOptions.FetchConfig.FilePath = @".\FolderM\FolderN\FetchStuff.ts";

			// Turns out actions are required
			AddControllerAction("PostMethod", "string")
				.AddAttribute(MvcConstants.HttpPostAttributeFullName_AspNetCore).Commit()
				.AddLineOfCode("return null", 0)
				.Commit();


			AssertThatThisController()
				.HasImportForFile(@"../../FolderM/FolderN/FetchStuff");
		}
	}
}
