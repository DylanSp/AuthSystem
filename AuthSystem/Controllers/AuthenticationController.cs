using AuthSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserAuthenticationDTO userAuthentication)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
