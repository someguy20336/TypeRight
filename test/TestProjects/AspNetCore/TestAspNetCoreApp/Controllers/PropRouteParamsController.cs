using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TypeRight.Attributes;

namespace TestAspNetCoreApp.Controllers
{
    [Route("api/[controller]/{id}/some-thing")]
    [ApiController]
    [ScriptOutput("../Scripts/PropRouteParamsController.ts")]
    public class PropRouteParamsController : ControllerBase
    {

        [FromRoute(Name = "id")]
        public int Id { get; set; }

        [FromRoute(Name = "other")]
        public string OtherThing { get; set; }

        [HttpGet("get/{other}/thing"), ScriptAction]
        public string GetWithAdditionalPropParam()
        {
            return "";
        }

        [HttpGet("thing"), ScriptAction]
        public string GetWithoutAdditionalPropParam()
        {
            return "";
        }

        [HttpGet("thing/{fromMethod}/yea"), ScriptAction]
        public string GetWithMethodParam([FromRoute] int fromMethod)
        {
            return "";
        }

        [HttpGet("thing/{other}/{fromMethod}/yea"), ScriptAction]
        public string GetWithEverything([FromRoute] int fromMethod)
        {
            return "";
        }
    }
}
