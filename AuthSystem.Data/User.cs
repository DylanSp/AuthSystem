namespace AuthSystem.Data
{
    public struct User
    {
        public UserId Id { get; }
        public Username Username { get; }
        public SaltedHashedPassword SaltedHashedPassword { get; }

        public User(UserId id, Username username, SaltedHashedPassword saltedHashedPassword)
        {
            Id = id;
            Username = username;
            SaltedHashedPassword = saltedHashedPassword;
        }
    }
}
