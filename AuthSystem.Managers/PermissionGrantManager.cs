﻿using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using System;
using System.Collections.Generic;
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

        public async Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(Guid userId)
        {
            return await Adapter.GetAllPermissionsForUserAsync(userId);
        }
    }
}
