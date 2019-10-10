using System;
using System.ComponentModel;
using ValueOf;

namespace AuthSystem.Data
{
    [TypeConverter(typeof(GuidValueTypeConverter<UserId>))]
    public class UserId : ValueOf<Guid, UserId>
    {
    }

    [TypeConverter(typeof(GuidValueTypeConverter<ResourceId>))]
    public class ResourceId : ValueOf<Guid, ResourceId>
    {
    }

    [TypeConverter(typeof(GuidValueTypeConverter<PermissionGrantId>))]
    public class PermissionGrantId : ValueOf<Guid, PermissionGrantId>
    {
    }

    [TypeConverter(typeof(StringValueTypeConverter<ResourceValue>))]
    public class ResourceValue : ValueOf<string, ResourceValue>
    {
    }

    [TypeConverter(typeof(StringValueTypeConverter<Username>))]
    public class Username : ValueOf<string, Username>
    {
    }

    [TypeConverter(typeof(StringValueTypeConverter<PlaintextPassword>))]
    public class PlaintextPassword : ValueOf<string, PlaintextPassword>
    {
    }

    [TypeConverter(typeof(StringValueTypeConverter<Base64Hash>))]
    public class Base64Hash : ValueOf<string, Base64Hash>
    {
    }

    [TypeConverter(typeof(StringValueTypeConverter<Base64Salt>))]
    public class Base64Salt : ValueOf<string, Base64Salt>
    {
    }
}

