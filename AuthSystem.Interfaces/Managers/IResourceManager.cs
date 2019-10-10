using AuthSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public interface IResourceManager
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync(Username username);
        Task<Resource?> GetResourceAsync(ResourceId resourceId, Username username);
        Task<ResourceId> CreateResourceAsync(ResourceValue value, Username username);
        Task UpdateResourceAsync(Resource newResource, Username username); // TODO - should this return result, whether success or not?
    }
}
