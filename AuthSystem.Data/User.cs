using AuthSystem.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace AuthSystem.Data
{
    public class User : IEntity
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Base64PasswordHash { get; set; }
        public string Base64Salt { get; set; }

        public User(string username, string passwordHash, string salt)
        {
            Id = Guid.NewGuid();
            Username = username;
            Base64PasswordHash = passwordHash;
            Base64Salt = salt;
        }
    }
}
