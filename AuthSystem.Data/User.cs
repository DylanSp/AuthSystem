using System;

namespace AuthSystem.Data
{
    public struct User
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public HashedPassword HashedPassword { get; set; }

        public User(Guid id, string username, HashedPassword hashedPassword)
        {
            Id = id;
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}
