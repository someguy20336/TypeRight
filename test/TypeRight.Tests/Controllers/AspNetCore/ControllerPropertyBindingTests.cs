using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TypeRight.Tests.TestBuilders;

namespace TypeRight.Tests.Controllers.AspNetCore
{
	[TestClass]
	public class ControllerPropertyBindingTests : ControllerTestsBase
	{

		[TestInitialize]
		 public void Initialize()
		{
			base.TestInitialize();

			ControllerBuilder.AddAttribute(MvcConstants.RouteAttributeFullName_AspNetCore)
				.AddConstructorArg("\"api/things/{id}/action\"")
				.Commit();

			ControllerBuilder.AddPropertyAndBuildAttributes("TestID", "int")
				.AddAttribute(MvcConstants.FromRouteAttributeFullName_AspNetCore)
				.AddNamedArg("Name", "\"id\"")
				.Commit();

			ControllerBuilder.AddPropertyAndBuildAttributes("NotBaseRoute", "string")
				.AddAttribute(MvcConstants.FromRouteAttributeFullName_AspNetCore)
				.AddNamedArg("Name", "\"other\"")
				.Commit();

		}

		[TestMethod]
		public void BaseRouteParameterOnly_IsFound_AddedToRoute()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(id: number): void {
	fetchWrapper(""GET"", `/api/things/${id}/action`, null);
}");
		}

		[TestMethod]
		public void BaseRouteParameterWithMethodSpecific_IsFound_AddedToRoute()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddStringConstructorArg("sub/{other}/route")
						.Commit()
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(id: number, other: string): void {
	fetchWrapper(""GET"", `/api/things/${id}/action/sub/${other}/route`, null);
}");
		}

		[TestMethod]
		public void BaseRouteParameterWithMethodSpecificAndMore_IsFound_AddedToRoute()
		{
			ControllerBuilder
				.AddMethod("GetThing", "string")
					.AddScriptActionAttribute()
					.AddAttribute(MvcConstants.HttpGetAttributeFullName_AspNetCore)
						.AddStringConstructorArg("sub/{other}/route/{fromRoute}")
						.Commit()
					.AddParameter("fromRoute", "int", attribute: MvcConstants.FromRouteAttributeFullName_AspNetCore)
					.AddLineOfCode("return null", 0)
					.Commit()
					;

			AssertScriptTextForFunctionIs(@"
export function GetThing(id: number, other: string, fromRoute: number): void {
	fetchWrapper(""GET"", `/api/things/${id}/action/sub/${other}/route/${fromRoute}`, null);
}");
		}
	}
}
