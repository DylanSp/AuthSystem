using System;
using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
    public class PermissionGrantManager : IPermissionGrantManager
    {
        private IPermissionGrantAdapter Adapter { get; }

        public PermissionGrantManager(IPermissionGrantAdapter adapter)
        {
            Adapter = adapter;
        }

        public async Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission)
        {
            return await Adapter.CheckIfUserHasPermissionAsync(userId, resourceId, permission);
        }

        public async Task<OneOf<UnableToGrantPermission, PermissionGrantCreated>> CreatePermissionGrantAsync(UserId userId, ResourceId resourceId, PermissionType permission)
        {
            // TODO - catch exceptions, return UnableToGrantPermission in that case? or handle that at Adapter level?
            var id = new PermissionGrantId(Guid.NewGuid());
            var grant = new PermissionGrant(id, userId, resourceId, permission);
            await Adapter.CreatePermissionGrantAsync(grant);
            return new PermissionGrantCreated();
        }

        public async Task DeletePermissionGrantAsync(PermissionGrantId permissionId)
        {
            await Adapter.DeletePermissionGrantAsync(permissionId);
        }

        public async Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId)
        {
            return await Adapter.GetAllPermissionsForUserAsync(userId);
        }
    }
}
