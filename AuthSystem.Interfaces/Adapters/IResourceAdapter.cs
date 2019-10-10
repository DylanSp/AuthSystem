using AuthSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface IResourceAdapter
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync();
        Task<Resource?> GetResourceAsync(ResourceId resourceId);
        Task CreateResourceAsync(Resource newResource);
        Task<int> UpdateResourceAsync(Resource newResource);
    }
}
