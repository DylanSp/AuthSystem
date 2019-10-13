using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
    public class ResourceManager : IResourceManager
    {
        private IResourceAdapter Adapter { get; }
        private IPermissionGrantManager PermissionGrantManager { get; }

        public ResourceManager(IResourceAdapter adapter, IPermissionGrantManager permissionGrantManager)
        {
            Adapter = adapter;
            PermissionGrantManager = permissionGrantManager;
        }

        public async Task<ResourceId> CreateResourceAsync(ResourceValue value, UserId userId)
        {
            var resourceId = ResourceId.From(Guid.NewGuid());
            var resource = new Resource(resourceId, value);
            await Adapter.CreateResourceAsync(resource);

            await PermissionGrantManager.CreatePermissionGrantAsync(userId, resourceId, PermissionType.Read);
            await PermissionGrantManager.CreatePermissionGrantAsync(userId, resourceId, PermissionType.Write);

            return resourceId;
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync(UserId userId)
        {
            var grants = await PermissionGrantManager.GetAllPermissionsForUserAsync(userId);
            var allResources = await Adapter.GetAllResourcesAsync();
            var allowedResources = from resource in allResources
                                    join grant in grants
                                        on resource.Id equals grant.ResourceId
                                    where grant.UserId == userId && grant.PermissionType == PermissionType.Read
                                    select resource;
            return allowedResources;
        }

        public async Task<Resource?> GetResourceAsync(ResourceId resourceId, UserId userId)
        {
            var hasPermission = await PermissionGrantManager.CheckIfUserHasPermissionAsync(userId, resourceId, PermissionType.Read);
            return hasPermission ? await Adapter.GetResourceAsync(resourceId) : null;
        }

        public async Task<UpdateResourceResult> UpdateResourceAsync(Resource newResource, UserId userId)
        {
            var hasPermission = await PermissionGrantManager.CheckIfUserHasPermissionAsync(userId, newResource.Id, PermissionType.Write);
            if (hasPermission)
            {
                await Adapter.UpdateResourceAsync(newResource);
                return UpdateResourceResult.ResourceUpdated;
            }
            else
            {
                return UpdateResourceResult.UserDoesNotHavePermission;
            }
        }
    }
}
