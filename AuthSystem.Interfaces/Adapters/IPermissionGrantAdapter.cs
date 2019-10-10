using AuthSystem.Data;
using AuthSystem.Interfaces.Managers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface IPermissionGrantAdapter
    {
        Task<PermissionGrantId> CreatePermissionGrantAsync(UserId userId, ResourceId resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist?
        Task DeletePermissionGrantAsync(PermissionGrantId permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
        Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId);
    }
}
