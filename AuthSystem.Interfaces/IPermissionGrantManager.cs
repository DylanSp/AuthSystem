using System;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces
{
    public enum PermissionType
    {
        Unknown = 0,
        Read = 1,
        Write = 2,
    }

    public interface IPermissionGrantManager
    {
        Task<Guid> CreatePermissionGrant(Guid userId, Guid resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist?
        Task DeletePermissionGrant(Guid permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermission(Guid userId, Guid resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
    }
}
