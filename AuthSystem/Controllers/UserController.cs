using AuthSystem.DTOs;
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
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<UserAuthenticationDTO>> CreateUserAsync([FromBody] UserAuthenticationDTO userAuthentication)
        {
            throw new NotImplementedException();
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
