using AuthSystem.Interfaces;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Managers
{
    public class PermissionGrantManager : IPermissionGrantManager
    {
        private IPermissionGrantAdapter Adapter { get; }

        public PermissionGrantManager(IPermissionGrantAdapter adapter)
        {
            Adapter = adapter;
        }

        public async Task<bool> CheckIfUserHasPermissionAsync(Guid userId, Guid resourceId, PermissionType permission)
        {
            return await Adapter.CheckIfUserHasPermissionAsync(userId, resourceId, permission);
        }

        public async Task<Guid> CreatePermissionGrantAsync(Guid userId, Guid resourceId, PermissionType permission)
        {
            return await Adapter.CreatePermissionGrantAsync(userId, resourceId, permission);
        }

        public async Task DeletePermissionGrantAsync(Guid permissionId)
        {
            await Adapter.DeletePermissionGrantAsync(permissionId);
        }
    }
}
