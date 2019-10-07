using AuthSystem.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IResourceManager
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<Resource?> GetResourceAsync(Guid resourceId);
        Task<Guid> CreateResourceAsync(string value);
        Task UpdateResourceAsync(Resource newResource); // TODO - should this return result, whether success or not?
    }
}
