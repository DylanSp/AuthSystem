using System;

namespace AuthSystem.Data
{
    public struct User
    {
        public Guid Id { get; }
        public string Username { get; }
        public HashedPassword HashedPassword { get; }

        public User(Guid id, string username, HashedPassword hashedPassword)
        {
            Id = id;
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}
