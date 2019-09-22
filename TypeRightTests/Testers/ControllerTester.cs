using TypeRight.TypeLocation;
using TypeRight.ScriptWriting;
using TypeRight.TypeProcessing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TypeRightTests.Testers
{
	class ControllerTester
	{
		private MvcControllerInfo _controllerInfo;

		private TypeFormatter _typeFormatter;

		public ControllerTester(MvcControllerInfo controller, TypeFormatter typeFormatter)
		{
			_controllerInfo = controller;
			_typeFormatter = typeFormatter;
		}

		public ControllerTester BaseUrlIs(string expected)
		{
			string baseUrl = _controllerInfo.GetBaseUrl();
			Assert.AreEqual(expected, baseUrl);
			return this;
		}

		public MvcActionTester TestActionWithName(string name)
		{
			return new MvcActionTester(_controllerInfo.Actions.Where(m => m.Name == name).First(), _typeFormatter);
		}
	}
}
