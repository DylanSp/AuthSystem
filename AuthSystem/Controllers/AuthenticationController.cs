using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}")]
    public class AuthenticationController : ControllerBase
    {
        private IUserManager UserManager { get; }
        private ISessionCookieManager SessionCookieManager { get; }

        public AuthenticationController(IUserManager userManager, ISessionCookieManager sessionCookieManager)
        {
            UserManager = userManager;
            SessionCookieManager = sessionCookieManager;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LoginAsync([FromBody] UserAuthenticationDTO userAuthentication)
        {
            var username = new Username(userAuthentication.Username);
            var isCorrectPassword =
                await UserManager.ValidatePasswordAsync(username, new PlaintextPassword(userAuthentication.Password));

            if (isCorrectPassword)
            {
                var cookieId = await SessionCookieManager.CreateSessionCookieAsync(username);
                Response.Cookies.Append(Constants.SESSION_COOKIE_NAME, cookieId.Value.ToString(), new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true,
                    SameSite = SameSiteMode.Strict,
                });
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
