using System;

namespace AuthSystem.Data
{
    public struct User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Base64PasswordHash { get; set; }
        public string Base64Salt { get; set; }

        public User(string username, string passwordHash, string salt)
        {
            Id = Guid.NewGuid();    // TODO - make this ctor parameter, enforce NewGuid on creation in manager
            Username = username;
            Base64PasswordHash = passwordHash;
            Base64Salt = salt;
        }
    }
}
