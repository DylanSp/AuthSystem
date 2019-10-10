using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Controllers
{
    [Route("api/valuetesting")]
    [ApiController]
    public class ValueTypeTestingController : ControllerBase
    {
        [HttpGet]
        [Route("resources/{value}")]
        public ActionResult<string> TestResourceValue([FromRoute] ResourceValue value)
        {
            return "ok";
        }

        [HttpGet]
        [Route("users/{userId:guid}")]
        public ActionResult<string> TestUserId([FromRoute] UserId userId)
        {
            return "ok";
        }
    }
}