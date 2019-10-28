using System;
using System.Collections.Generic;
using System.Text;

namespace AuthSystem.Data
{
    public struct RefreshToken
    {
        public RefreshTokenId Id { get; }
        public UserId UserId { get; }
        public DateTime ExpirationTime { get; }
    }
}
