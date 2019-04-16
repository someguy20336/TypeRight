﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetStandardLib;
using TestAspNetCoreApp.Models;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Controllers
{
	public class HomeController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}

		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Privacy()
		{
			return View();
		}

		[ScriptAction]
		public JsonResult TestJson()
		{
			return Json(new NetStandardClass());
		}

		[ScriptAction]
		public JsonResult AnonTypeWithDictionaryProperty(CustomGroupObj3 model)
		{
			return Json(new
			{
				listObj = new List<CustomGroupObject1>()
			});
		}


		[ScriptAction]
		public JsonResult FunctionWithModel([FromBody] ASimpleModel model)
		{
			return Json(model);
		}

		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
		public IActionResult Error()
		{
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}

		[ScriptAction]
		public JsonResult OtherFunctionWithModel([FromBody] CustomGroupObject1 model)
		{
			return Json(model);
		}
	}
}
