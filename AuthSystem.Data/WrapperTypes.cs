using System;
using System.ComponentModel;

namespace AuthSystem.Data
{
    public readonly struct UserId
    {
        public Guid Value { get; }

        public UserId(Guid value)
        {
            Value = value;
        }
    }

    public readonly struct ResourceId
    {
        public Guid Value { get; }

        public ResourceId(Guid value)
        {
            Value = value;
        }
    }

    public readonly struct PermissionGrantId
    {
        public Guid Value { get; }

        public PermissionGrantId(Guid value)
        {
            Value = value;
        }
    }

    public readonly struct RefreshTokenId
    {
        public Guid Value { get; }

        public RefreshTokenId(Guid value)
        {
            Value = value;
        }
    }

    public readonly struct ResourceValue
    {
        public string Value { get; }

        public ResourceValue(string value)
        {
            Value = value;
        }
    }

    public readonly struct Username
    {
        public string Value { get; }

        public Username(string value)
        {
            Value = value;
        }
    }

    public readonly struct PlaintextPassword
    {
        public string Value { get; }

        public PlaintextPassword(string value)
        {
            Value = value;
        }
    }

    public readonly struct SaltedHashedPassword
    {
        public string Value { get; }

        public SaltedHashedPassword(string value)
        {
            Value = value;
        }
    }

    public readonly struct JsonWebToken
    {
        public string Value { get; }

        public JsonWebToken(string value)
        {
            Value = value;
        }
    }

    public readonly struct JwtSecret
    {
        public string Value { get; }

        public JwtSecret(string value)
        {
            Value = value;
        }
    }
}

