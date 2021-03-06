﻿using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Controllers
{
	[Route("api/[controller]")]
    [ApiController]
	[ScriptOutput("../Scripts/Home/CustomNamedActions.ts")]
    public class TestWebApiController : ControllerBase
    {
		[ScriptAction]
		public List<string> GetStringList()
		{
			return new List<string>();
		}

		[ScriptAction]
		public CustomGroupObject1 GetRandoGroupObject([FromBody] string id)
		{
			return new CustomGroupObject1();
		}

		[HttpPost, ScriptAction]
		public CustomGroupObject1 WithFromQuery([FromQuery] string id, [FromBody] CustomGroupObj3 body, [FromServices] AService service)
		{
			return new CustomGroupObject1()
			{
				AnotherStringList = new List<string>() { id, body.StringProp }
			};
		}

		[HttpPost("complex"), ScriptAction]
		public object ComplexFromQuery([FromQuery] string id, [FromQuery] ASimpleModel fromQueryModel)
		{
			return new 
			{
				id = id,
				PropOne = fromQueryModel.PropOne,
				PropTwo = fromQueryModel.PropTwo
			};
		}

		[HttpPost("complex-list"), ScriptAction]
		public object ComplexWithListFromQuery([FromQuery] ModelWithArray fromQueryModel)
		{
			return new
			{
				simple = fromQueryModel.SimpleType,
				array = fromQueryModel.ArrayType,
			};
		}

		[HttpGet, ScriptAction]
		public string TestGetMethod([FromQuery] string id)
		{
			return "";
		}

		[HttpGet("things/{id}/action"), ScriptAction]
		public string GetSomething(string id)
		{
			return "";
		}

		[HttpPut("things/{id}/action"), ScriptAction]
		public string PutSomething(string id, [FromBody] bool body)
		{
			return "";
		}

		[ScriptAction]
		public ActionResult<string> TestActionResult()
		{
			return "";
		}

		[ScriptAction]
		public ActionResult<CustomGroupObject1> TestClassActionResult()
		{
			return null;
		}

        [HttpGet, ScriptAction]
        public string TestOverrideSingleParamTypeMethod(
            [FromQuery, ScriptParamTypes(typeof(int))] string id
            )
        {
            return "";
        }

		[HttpGet, ScriptAction]
		public string TestOverrideMultParamTypesMethod(
		   [FromQuery, ScriptParamTypes(typeof(int), typeof(bool), typeof(string))] string id
		   )
		{
			return "";
		}

		[HttpGet("{id}"), ScriptAction]
		public string FromRoute_TestOverrideMultParamTypesMethod(
		   [ScriptParamTypes(typeof(int), typeof(bool), typeof(string))] string id
		   )
		{
			return "";
		}
	}
}