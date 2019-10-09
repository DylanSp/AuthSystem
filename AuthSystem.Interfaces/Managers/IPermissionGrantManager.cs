using AuthSystem.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public interface IPermissionGrantManager
    {
        // TODO - have these look up by userId or username?
        Task<Guid> CreatePermissionGrantAsync(Guid userId, Guid resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist? do we need to return created permissionId?
        Task DeletePermissionGrantAsync(Guid permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermissionAsync(Guid userId, Guid resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
        Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(Guid userId);
    }
}
