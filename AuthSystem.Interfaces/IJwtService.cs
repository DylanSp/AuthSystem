using AuthSystem.Data;
using System;

namespace AuthSystem.Interfaces
{
    public interface IJwtService
    {
        JsonWebToken CreateToken(UserId userId, DateTimeOffset expirationTime);
        UserId? DecodeToken(JsonWebToken token);
    }
}
