using Microsoft.AspNetCore.Mvc;

namespace ReplaceWithJsExample.Controllers
{
    // I tested these routes with Postman - legit
    [ApiController]
    [Route("base")]
    public class RouteTestController : ControllerBase
    {
        // GET /api/test/{id}
        [HttpGet("/api/test/{id}")]
        public string GetThing(int id)
        {
            return $"Got {id}";
        }

        // GET /base/api/test/{id}
        [HttpGet("api/test/{id}")]
        public string GetOtherThing(int id)
        {
            return $"Got routeless {id}";
        }
    }
}
