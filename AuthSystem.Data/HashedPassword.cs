namespace AuthSystem.Data
{
    public struct HashedPassword
    {
        public Base64Hash Base64PasswordHash { get; }
        public Base64Salt Base64Salt { get; }

        public HashedPassword(Base64Hash passwordHash, Base64Salt salt)
        {
            Base64PasswordHash = passwordHash;
            Base64Salt = salt;
        }
    }
}
