using TypeRight.ScriptWriting;
using TypeRight.TypeProcessing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace TypeRight.Tests.Testers
{
	public class MvcActionTester
	{
		private readonly ControllerContext _context;
		private MvcActionInfo _method;
		private MvcControllerInfo ControllerInfo => _context.Controller;

		private readonly TypeFormatter _typeFormatter;

		public MvcActionTester(ControllerContext context, MvcActionInfo method, TypeFormatter typeFormatter)
		{
			_context = context;
			_method = method;
			_typeFormatter = typeFormatter;
		}

		public MvcActionTester ParameterTypeIs(string paramName, string typescriptName)
		{
			MvcActionParameter parameter = _method.Parameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(typescriptName, parameter.Types.First().FormatType(_typeFormatter));
			return this;
		}

		public MvcActionTester ReturnTypeTypescriptNameIs(string typescriptName)
		{
			Assert.AreEqual(typescriptName, _method.ReturnType.FormatType(_typeFormatter));
			return this;
		}

		
		public MvcActionTester UrlTemplateIs(string expected)
		{
			string baseUrl = MvcRouteGenerator.CreateGenerator(_context.Controller, _context.BaseUrl).GenerateRouteTemplate(_method);
			Assert.AreEqual(expected, baseUrl);
			return this;
		}

		public MvcActionTester ParameterSourceTypeIs(string paramName, ActionParameterSourceType expectedType)
		{
			var parameter = _method.Parameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(expectedType, parameter.BindingType);
			return this;
		}

		public MvcActionTester FetchFunctionIs(string expectedFuncName)
		{
			FetchFunctionDescriptor desc = _context.FetchFunctionResolver.Resolve(_method.RequestMethod.Name);
			Assert.AreEqual(expectedFuncName, desc.FunctionName);
			return this;
		}
	}

}
