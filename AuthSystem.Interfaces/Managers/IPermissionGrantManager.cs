using AuthSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public interface IPermissionGrantManager
    {
        // TODO - have these look up by userId or username?
        Task<PermissionGrantId> CreatePermissionGrantAsync(UserId userId, ResourceId resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist? do we need to return created permissionId?
        Task DeletePermissionGrantAsync(PermissionGrantId permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
        Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId);
    }
}
