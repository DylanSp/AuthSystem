using System;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public interface IPermissionGrantAdapter
    {
        Task<Guid> CreatePermissionGrant(Guid userId, Guid resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist?
        Task DeletePermissionGrant(Guid permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermission(Guid userId, Guid resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
    }
}
