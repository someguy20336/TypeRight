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

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		public ActionResult Contact()
		{
			ViewBag.Message = "Your contact page.";

			return View();
		}
        
        [MvcAction]
        public JsonResult TestJson()
        {
            return Json(true);
        }

        [MvcAction]
        public JsonResult Test_ObjectParam(ExampleClass example)
        {
            return Json(example.AnotherPropertyYea);
        }

        [MvcAction]
        public JsonResult Test_ObjectReturn(Dictionary<string, string> dict)
        {
            return Json(new ExampleClass());
        }

        [MvcAction]
        public JsonResult Test_Anonymous(Dictionary<string, string> dict)
        {
            return Json(new { intProp = 1, exampleProp = new ExampleClass(), stringProp = "Hey" });
        }
    }
}