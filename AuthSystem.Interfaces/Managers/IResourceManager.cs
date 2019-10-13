using AuthSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public enum UpdateResourceResult
    {
        UserDoesNotHavePermission,  // covers case where username does not exist, to avoid user enumeration
        ResourceUpdated,
    }

    public interface IResourceManager
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync(UserId userId);
        Task<Resource?> GetResourceAsync(ResourceId resourceId, UserId userId);
        Task<ResourceId> CreateResourceAsync(ResourceValue value, UserId userId);
        Task<UpdateResourceResult> UpdateResourceAsync(Resource newResource, UserId userId);
    }
}
