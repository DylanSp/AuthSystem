using AuthSystem.DTOs;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Controllers
{
    [ApiController]
    [Route("resources")]
    public class ResourceController : ControllerBase
    {
        [HttpGet]
        [Route("")]
        public async Task<ActionResult<List<ResourceDTO>>> GetAllResourcesAsync()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ResourceDTO>> CreateResourceAsync([FromBody] ResourceDetailsDTO resourceDetails)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Route("{resourceId:guid}")]
        public async Task<ActionResult<ResourceDTO>> GetResourceAsync([FromRoute] Guid resourceId)
        {
            throw new NotImplementedException();
        }

        [HttpPut]
        [Route("{resourceId:guid}")]
        public async Task<ActionResult<ResourceDTO>> UpdateResourceAsync([FromRoute] Guid resourceId,
            [FromBody] ResourceDetailsDTO resourceDetails)
        {
            throw new NotImplementedException();
        }

        [HttpDelete]
        [Route("{resourceId:guid}")]
        public async Task<IActionResult> DeleteResource([FromRoute] Guid resourceId)
        {
            throw new NotImplementedException();
        }
    }
}
