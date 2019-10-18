using AuthSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    [ApiController]
    public class PermissionsController : ControllerBase
    {
        [HttpGet]
        [Route("resources/{resourceId:guid}/permissions")]
        public async Task<ActionResult<List<PermissionGrantDTO>>> GetPermissionsForResourceAsync(
            [FromRoute] Guid resourceId)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("permissions")]
        public async Task<ActionResult<PermissionGrantDTO>> CreatePermissionGrantAsync([FromBody] PermissionGrantDetailsDTO permissionGrantDetails)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("permissions/{permissionId:guid}")]
        public async Task<IActionResult> DeletePermissionGrantAsync([FromRoute] Guid permissionId)
        {
            throw new NotImplementedException();
        }
    }
}
