using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
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

        public async Task<Guid> CreateResourceAsync(string value, string username)
        {
            var possibleUserId = await UserManager.GetIdForUsername(username);
            return await possibleUserId.Match(
                usernameDoesNotExist => throw new Exception(),   // TODO - put in return type
                async userId =>
                {
                    // TODO - is there a way to move this code out? local function?
                    var resourceId = Guid.NewGuid();    // TODO - bother with checking for uniqueness with adapter, i.e. user manager?
                    var resource = new Resource(resourceId, value);
                    await Adapter.CreateResourceAsync(resource);

                    await PermissionGrantManager.CreatePermissionGrantAsync(userId.Value, resourceId, PermissionType.Read);
                    await PermissionGrantManager.CreatePermissionGrantAsync(userId.Value, resourceId, PermissionType.Write);

                    return resourceId;
                }
            );
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync(string username)
        {
            throw new NotImplementedException();
            // TODO - to avoid a bunch of DB queries, implement IPermissionGrantManager method to return all grants for a given user?
        }

        public async Task<Resource?> GetResourceAsync(Guid resourceId, string username)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateResourceAsync(Resource newResource, string username)
        {
            throw new NotImplementedException();
        }
    }
}
