﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
		public CustomGroupObject1 WithFromQuery([FromQuery] string id, [FromBody] CustomGroupObj3 body, [FromServices] string service)
		{
			return new CustomGroupObject1();
		}

		[HttpGet, ScriptAction]
		public string TestGetMethod([FromQuery] string id)
		{
			return "";
		}
	}
}