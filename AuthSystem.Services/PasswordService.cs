using AuthSystem.Data;
using System;

namespace AuthSystem.Services
{
    public class PasswordService
    {
        public HashedPassword GeneratePasswordHashAndSalt(string password)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfPasswordMatchesHash(string password, HashedPassword hash)
        {
            throw new NotImplementedException();
        }
    }
}
