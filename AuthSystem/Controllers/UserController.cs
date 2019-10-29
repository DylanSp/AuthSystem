using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Interfaces.Managers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    [ApiVersion("1")]
    [ApiController]
    [Route("v{version:apiVersion}/users")]
    public class UserController : ControllerBase
    {
        private IUserManager UserManager { get; }

        public UserController(IUserManager userManager)
        {
            UserManager = userManager;
        }

        [HttpPost]
        [Authorize]
        [Route("")]
        public async Task<ActionResult<UserAuthenticationDTO>> CreateUserAsync([FromRoute] ApiVersion apiVersion, [FromBody] UserAuthenticationDTO userAuthentication)
        {
            var userId = await UserManager.CreateUserAsync(new Username(userAuthentication.Username), new PlaintextPassword(userAuthentication.Password));
            if (userId.HasValue)
            {
                return Created($"/v{apiVersion.MajorVersion}/users/{userId.Value.Value}", userAuthentication);
            }
            else
            {
                return BadRequest("Duplicate username");
            }
        }

        [HttpPut]
        [Route("{username}")]
        public async Task<ActionResult<UserAuthenticationDTO>> UpdatePasswordAsync([FromRoute] string username, [FromBody]UserPasswordDTO userPassword)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{username}")]
        public async Task<IActionResult> DeleteUserAsync([FromRoute] string username)
        {
            throw new NotImplementedException();
        }
    }
}
