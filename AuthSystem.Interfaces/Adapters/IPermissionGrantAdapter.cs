using AuthSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface IPermissionGrantAdapter
    {
        Task CreatePermissionGrantAsync(PermissionGrant grant);    // TODO - return type? return differently if user/resource don't exist? what if grant already exists?
        Task DeletePermissionGrantAsync(PermissionGrantId permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
        Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId);
    }
}
