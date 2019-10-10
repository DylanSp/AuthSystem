namespace AuthSystem.Data
{
    public struct User
    {
        public UserId Id { get; }
        public Username Username { get; }
        public HashedPassword HashedPassword { get; }

        public User(UserId id, Username username, HashedPassword hashedPassword)
        {
            Id = id;
            Username = username;
            HashedPassword = hashedPassword;
        }
    }
}
