using AuthSystem.Data;
using OneOf;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Managers
{
    // CreatePermissionGrant return types
    public class UnableToGrantPermission { }
    public class PermissionGrantCreated { }

    public interface IPermissionGrantManager
    {
        Task<OneOf<UnableToGrantPermission, PermissionGrantCreated>> CreatePermissionGrantAsync(UserId userId, ResourceId resourceId, PermissionType permission);
        Task DeletePermissionGrantAsync(PermissionGrantId permissionId);
        Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission);
        Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId);
    }
}
