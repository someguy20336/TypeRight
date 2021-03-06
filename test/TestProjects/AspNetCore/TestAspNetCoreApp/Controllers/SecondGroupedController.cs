﻿using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TypeRight.Attributes;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TestAspNetCoreApp.Controllers
{
    [Route("api/[controller]"), ScriptOutput("../Scripts/GroupedControllers.ts")]
	public class SecondGroupedController : Controller
	{
		// GET: api/<controller>
		[HttpGet, ScriptAction]
		public IEnumerable<string> GetSecond()
		{
			return new string[] { "value1", "value2" };
		}

		// GET api/<controller>/5
		[HttpGet("{id}"), ScriptAction]
		public string GetSecondById(int id)
		{
			return "value";
		}

		// POST api/<controller>
		[HttpPost, ScriptAction]
		public void PostSecond([FromBody]string value)
		{
		}

		// PUT api/<controller>/5
		[HttpPut("{id}"), ScriptAction]
		public void PutSecond(int id, [FromBody]string value)
		{
		}

		// DELETE api/<controller>/5
		[HttpDelete("{id}"), ScriptAction]
		public void DeleteSecond(int id)
		{
		}
	}
}
