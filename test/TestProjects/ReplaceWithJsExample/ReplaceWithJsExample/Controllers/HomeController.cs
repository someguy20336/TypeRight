using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
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
		public JsonResult AnonTypeWithDictionaryProperty([FromBody] CustomGroupObj3 model, [FromQuery] ASimpleEnum thing, [FromServices] string service)
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

		[ScriptAction, HttpPost]
		public JsonResult MultipleFromStuff([FromRoute] string route, [FromServices] string service, [FromBody] ASimpleModel model)
		{
			return Json(model);
		}

		[ScriptAction]
		public JsonResult NoFromBodyParams([FromRoute] string route, [FromServices] string service, [FromHeader] ASimpleModel model)
		{
			return Json(model);
		}

		[ScriptAction]
		public JsonResult OtherFunctionWithModel([FromBody] CustomGroupObject1 model)
		{
			return Json(model);
		}
	}
}
