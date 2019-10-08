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
        Task<Guid> CreatePermissionGrantAsync(Guid userId, Guid resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist? do we need to return created permissionId?
        Task DeletePermissionGrantAsync(Guid permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermissionAsync(Guid userId, Guid resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
    }
}
