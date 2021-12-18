using TypeRight.ScriptWriting;
using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TypeRight.Tests.Testers
{
	public class MvcActionTester
	{
		private readonly ControllerContext _context;
		private MvcAction _method;

		private readonly TypeFormatter _typeFormatter;

		public MvcActionTester(ControllerContext context, MvcAction method, TypeFormatter typeFormatter)
		{
			_context = context;
			_method = method;
			_typeFormatter = typeFormatter;
		}

		public MvcActionTester ParameterTypeIs(string paramName, string typescriptName)
		{
			MvcActionParameter parameter = _method.ActionParameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(typescriptName, parameter.Types.First().FormatType(_typeFormatter));
			return this;
		}

		public MvcActionTester ReturnTypeTypescriptNameIs(string typescriptName)
		{
			Assert.AreEqual(typescriptName, _method.ReturnType.FormatType(_typeFormatter));
			return this;
		}

		
		public MvcActionTester RouteTemplateIs(string expected, string baseUrl)
		{
			string url = MvcRouteGenerator.CreateGenerator(_context.Controllers.First(), baseUrl).GenerateRouteTemplate(_method);
			Assert.AreEqual(expected, url);
			return this;
		}

		public MvcActionTester ParameterSourceTypeIs(string paramName, ActionParameterSourceType expectedType)
		{
			var parameter = _method.ActionParameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(expectedType, parameter.BindingType);
			return this;
		}

	}

}
