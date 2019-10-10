using System;
using ValueOf;

namespace AuthSystem.Data
{
    public class UserId : ValueOf<Guid, UserId>
    {
    }

    public class ResourceId : ValueOf<Guid, ResourceId>
    {
    }

    public class PermissionGrantId : ValueOf<Guid, PermissionGrantId>
    {
    }

    public class ResourceValue : ValueOf<string, ResourceValue>
    {
    }

    public class Username : ValueOf<string, Username>
    {
    }

    public class PlaintextPassword : ValueOf<string, PlaintextPassword>
    {
    }

    public class Base64Hash : ValueOf<string, Base64Hash>
    {
    }

    public class Base64Salt : ValueOf<string, Base64Salt>
    {
    }
}

