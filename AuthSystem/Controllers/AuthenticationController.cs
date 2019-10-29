using AuthSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using AuthSystem.Data;
using AuthSystem.Interfaces.Managers;

namespace AuthSystem.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}")]
    public class AuthenticationController : ControllerBase
    {
        private IUserManager UserManager { get; }

        public AuthenticationController(IUserManager userManager)
        {
            UserManager = userManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserAuthenticationDTO userAuthentication)
        {
            var isCorrectPassword = await UserManager.ValidatePasswordAsync(new Username(userAuthentication.Username),
                new PlaintextPassword(userAuthentication.Password));

            if (isCorrectPassword)
            {
                // TODO - construct access token, refresh token, save refresh token, return tokens
                // TODO - access token should have user ID, not username
                return Ok();
            }
            else
            {
                return Unauthorized();
            }
        }

        [HttpPost]
        [Route("logout")]
        public async Task<IActionResult> LogoutAsync()
        {
            throw new NotImplementedException();
        }
    }
}
