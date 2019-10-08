using AuthSystem.Interfaces.Managers;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Interfaces.Adapters
{
    public interface IPermissionGrantAdapter
    {
        Task<Guid> CreatePermissionGrantAsync(Guid userId, Guid resourceId, PermissionType permission);    // TODO - return type? return differently if user/resource don't exist?
        Task DeletePermissionGrantAsync(Guid permissionId);  // TODO - return type? return differently if permission doesn't exist?
        Task<bool> CheckIfUserHasPermissionAsync(Guid userId, Guid resourceId, PermissionType permission);   // TODO - return type? return differently if user/resource don't exist?
    }
}
