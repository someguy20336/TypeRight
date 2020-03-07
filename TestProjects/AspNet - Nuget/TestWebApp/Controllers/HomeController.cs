using System.Collections.Generic;
using System.Web.Mvc;
using TestWebApp.TestClasses;
using TypeRight.Attributes;

namespace TestWebApp.Controllers
{
    public class HomeController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

        
        [ScriptAction]
        public JsonResult TestJson()
        {
            return Json(true);
        }

        [ScriptAction]
        public JsonResult Test_ObjectParam(ExampleClass example)
        {
            return Json(example.AnotherPropertyYea);
        }

        [ScriptAction]
        public JsonResult Test_ObjectReturn(Dictionary<string, string> dict)
        {
            return Json(new ExampleClass());
        }

        [ScriptAction]
        public JsonResult Test_Anonymous(Dictionary<string, string> dict)
        {
            return Json(new { intProp = 1, exampleProp = new ExampleClass(), stringProp = "Hey" });
        }
    }
}