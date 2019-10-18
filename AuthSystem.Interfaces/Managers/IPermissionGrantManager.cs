using AuthSystem.Data;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    public interface IPermissionGrantManager
    {
        Task<PermissionGrantId?> CreatePermissionGrantAsync(UserId userId, ResourceId resourceId, PermissionType permission);
        Task DeletePermissionGrantAsync(PermissionGrantId permissionId);
        Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission);
        Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId);
    }
}
