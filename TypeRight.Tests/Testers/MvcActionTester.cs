using TypeRight.CodeModel;
using TypeRight.TypeLocation;
using TypeRight.ScriptWriting;
using TypeRight.ScriptWriting.TypeScript;
using TypeRight.TypeProcessing;
using TypeRight.Tests.HelperClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeRight.Tests.Testers
{
	class MvcActionTester
	{
		private MvcActionInfo _method;
		private MvcControllerInfo _controllerInfo;

		private readonly TypeFormatter _typeFormatter;

		public MvcActionTester(MvcControllerInfo controller, MvcActionInfo method, TypeFormatter typeFormatter)
		{
			_controllerInfo = controller;
			_method = method;
			_typeFormatter = typeFormatter;
		}

		public MvcActionTester ParameterTypeIs(string paramName, string typescriptName)
		{
			MvcActionParameter parameter = _method.Parameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(typescriptName, parameter.Type.FormatType(_typeFormatter));
			return this;
		}

		public MvcActionTester ReturnTypeTypescriptNameIs(string typescriptName)
		{
			Assert.AreEqual(typescriptName, _method.ReturnType.FormatType(_typeFormatter));
			return this;
		}

		
		public MvcActionTester UrlTemplateIs(string expected)
		{
			string baseUrl = _controllerInfo.GetActionUrlTemplate(_method);
			Assert.AreEqual(expected, baseUrl);
			return this;
		}
	}



	class MvcActionModelTester
	{
		private ControllerActionModel _actionModel;


		public MvcActionModelTester(ControllerActionModel actionModel)
		{
			_actionModel = actionModel;
		}

		public MvcActionModelTester FetchFunctionIs(string func)
		{
			Assert.AreEqual(func, _actionModel.FetchFunctionName);
			return this;
		}

		public MvcActionModelTester ParameterTypeIs(string paramName, string typescriptName)
		{
			ActionParameterModel parameter = _actionModel.Parameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(typescriptName, parameter.ParameterType);
			return this;
		}

		public MvcActionModelTester ReturnTypeTypescriptNameIs(string typescriptName)
		{
			Assert.AreEqual(typescriptName, _actionModel.ReturnType);
			return this;
		}

		public MvcActionModelTester ParameterSourceTypeIs(string paramName, ActionParameterSourceType expectedType)
		{
			ActionParameterModel parameter = _actionModel.Parameters.Where(p => p.Name == paramName).First();
			Assert.AreEqual(expectedType, parameter.ActionParameterSourceType);
			return this;
		}

		public MvcActionModelTester RouteTemplateIs(string expected)
		{
			Assert.AreEqual(expected, _actionModel.RouteTemplate);
			return this;
		}
	}
}
