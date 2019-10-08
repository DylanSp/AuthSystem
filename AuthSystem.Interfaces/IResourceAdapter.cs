using AuthSystem.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IResourceAdapter
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<Resource?> GetResourceAsync(Guid resourceId);
        Task CreateResourceAsync(Resource newResource);
        Task<int> UpdateResourceAsync(Resource newResource);
    }
}
