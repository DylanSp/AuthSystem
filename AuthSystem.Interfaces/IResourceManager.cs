using AuthSystem.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IResourceManager
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync(string username);
        Task<Resource?> GetResourceAsync(Guid resourceId, string username);
        Task<Guid> CreateResourceAsync(string value, string username);
        Task UpdateResourceAsync(Resource newResource, string username); // TODO - should this return result, whether success or not?
    }
}
