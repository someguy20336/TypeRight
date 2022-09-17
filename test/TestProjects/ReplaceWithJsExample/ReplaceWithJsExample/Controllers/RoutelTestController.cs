using Microsoft.AspNetCore.Mvc;
using TypeRight.Attributes;

namespace ReplaceWithJsExample.Controllers
{
    // I tested these routes with Postman - legit
    [ApiController]
    [Route("base")]
    public class RouteTestController : ControllerBase
    {
        // GET /api/test/{id}
        [HttpGet("/rooted/test/{id}")]
        [ScriptAction]
        public string RootedPath(int id)
        {
            return $"Got {id}";
        }

        // GET /base/api/test/{id}
        [HttpGet("not-rooted/test/{id}")]
        [ScriptAction]
        public string NotRootedPath(int id)
        {
            return $"Got routeless {id}";
        }
    }
}
