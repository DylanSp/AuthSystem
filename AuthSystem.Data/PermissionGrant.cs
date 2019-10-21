namespace AuthSystem.Data
{
    public enum PermissionType
    {
        Unknown = 0,
        Read = 1,
        Write = 2,
        ManagePermissions = 3,
    }

    public readonly struct PermissionGrant
    {
        public PermissionGrantId Id { get; }
        public UserId UserId { get; }
        public ResourceId ResourceId { get; }
        public PermissionType PermissionType { get; }

        public PermissionGrant(PermissionGrantId id, UserId userId, ResourceId resourceId, PermissionType permissionType)
        {
            Id = id;
            UserId = userId;
            ResourceId = resourceId;
            PermissionType = permissionType;
        }
    }
}
