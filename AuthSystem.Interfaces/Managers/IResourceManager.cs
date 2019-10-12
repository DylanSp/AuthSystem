using AuthSystem.Data;
using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;
using ValueOf;

namespace AuthSystem.Interfaces.Managers
{
    // CreateResource() return types
    public class CreatingUserDoesNotExist { }
    public class ResourceCreated : ValueOf<ResourceId, ResourceCreated> { }

    public interface IResourceManager
    {
        Task<IEnumerable<Resource>> GetAllResourcesAsync(Username username);
        Task<Resource?> GetResourceAsync(ResourceId resourceId, Username username);
        Task<OneOf<CreatingUserDoesNotExist, ResourceCreated>> CreateResourceAsync(ResourceValue value, Username username);
        Task UpdateResourceAsync(Resource newResource, Username username); // TODO - should this return result, whether success or not?
    }
}
