using System;

namespace AuthSystem.Data
{
    public enum PermissionType
    {
        Unknown = 0,
        Read = 1,
        Write = 2,
    }

    public struct PermissionGrant
    {
        Guid Id { get; set; }
        Guid UserId { get; set; }
        Guid ResourceId { get; set; }
        PermissionType PermissionType { get; set; }

        public PermissionGrant(Guid id, Guid userId, Guid resourceId, PermissionType permissionType)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            PermissionType = permissionType;
        }
    }
}
