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
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ResourceId { get; set; }
        public PermissionType PermissionType { get; set; }

        public PermissionGrant(Guid id, Guid userId, Guid resourceId, PermissionType permissionType)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            PermissionType = permissionType;
        }
    }
}
