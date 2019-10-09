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
        public Guid Id { get; }
        public Guid UserId { get; }
        public Guid ResourceId { get; }
        public PermissionType PermissionType { get; }

        public PermissionGrant(Guid id, Guid userId, Guid resourceId, PermissionType permissionType)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            PermissionType = permissionType;
        }
    }
}
