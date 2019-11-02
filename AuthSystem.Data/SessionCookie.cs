using System;
using System.Collections.Generic;
using System.Text;

namespace AuthSystem.Data
{
    public readonly struct SessionCookie
    {
        public SessionCookieId Id { get; }
        public UserId UserId { get; }

        public SessionCookie(SessionCookieId id, UserId userId)
        {
            Id = id;
            UserId = userId;
        }
    }
}
