using System;
using System.Collections.Generic;
using System.Text;

namespace AuthSystem.Data
{
    public struct HashedPassword
    {
        public string Base64PasswordHash { get; }
        public string Base64Salt { get; }

        public HashedPassword(string passwordHash, string salt)
        {
            Base64PasswordHash = passwordHash;
            Base64Salt = salt;
        }
    }
}
