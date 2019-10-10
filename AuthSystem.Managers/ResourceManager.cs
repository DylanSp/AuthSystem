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
        private IUserManager UserManager { get; }
        private IPermissionGrantManager PermissionGrantManager { get; }

        public ResourceManager(IResourceAdapter adapter, IUserManager userManager, IPermissionGrantManager permissionGrantManager)
        {
            Adapter = adapter;
            UserManager = userManager;
            PermissionGrantManager = permissionGrantManager;
        }

        public async Task<ResourceId> CreateResourceAsync(ResourceValue value, Username username)
        {
            var possibleUserId = await UserManager.GetIdForUsername(username);
            return await possibleUserId.Match(
                usernameDoesNotExist => throw new Exception(),   // TODO - put in return type
                async userId =>
                {
                    // TODO - is there a way to move this code out? local function?
                    var resourceId = ResourceId.From(Guid.NewGuid());    // TODO - bother with checking for uniqueness with adapter, i.e. user manager?
                    var resource = new Resource(resourceId, value);
                    await Adapter.CreateResourceAsync(resource);

                    await PermissionGrantManager.CreatePermissionGrantAsync(userId.Value, resourceId, PermissionType.Read);
                    await PermissionGrantManager.CreatePermissionGrantAsync(userId.Value, resourceId, PermissionType.Write);

                    return resourceId;
                }
            );
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync(Username username)
        {
            var possibleUser = await UserManager.GetIdForUsername(username);
            return await possibleUser.Match<Task<IEnumerable<Resource>>>(
                async usernameDoesNotExist => await Task.FromResult(new List<Resource>()),
                async userId =>
                {
                    // TODO - is there a way to move this code out? local function?
                    var grants = await PermissionGrantManager.GetAllPermissionsForUserAsync(userId.Value);
                    var allResources = await Adapter.GetAllResourcesAsync();
                    var allowedResources = from resource in allResources
                                           join grant in grants
                                                on resource.Id equals grant.ResourceId
                                           where grant.UserId == userId.Value && grant.PermissionType == PermissionType.Read
                                           select resource;
                    return allowedResources;
                }
            );
        }

        public async Task<Resource?> GetResourceAsync(ResourceId resourceId, Username username)
        {
            var possibleUser = await UserManager.GetIdForUsername(username);
            return await possibleUser.Match(
                async usernameDoesNotExist => await Task.FromResult<Resource?>(null),
                async userId =>
                {
                    // TODO - is there a way to move this code out? local function?
                    var hasPermission = await PermissionGrantManager.CheckIfUserHasPermissionAsync(userId.Value, resourceId, PermissionType.Read);
                    if (hasPermission)
                    {
                        return await Adapter.GetResourceAsync(resourceId);
                    }
                    else
                    {
                        return null;
                    }
                }
            );
        }

        public async Task UpdateResourceAsync(Resource newResource, Username username)
        {
            var possibleUser = await UserManager.GetIdForUsername(username);
            await possibleUser.Match(
                usernameDoesNotExist => Task.CompletedTask,
                async userId =>
                {
                    // TODO - is there a way to move this code out? local function?
                    var hasPermission = await PermissionGrantManager.CheckIfUserHasPermissionAsync(userId.Value, newResource.Id, PermissionType.Write);
                    if (hasPermission)
                    {
                        await Adapter.UpdateResourceAsync(newResource);
                    }
                }
            );
        }
    }
}
