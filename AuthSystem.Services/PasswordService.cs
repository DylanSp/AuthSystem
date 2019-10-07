using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;

namespace AuthSystem.Services
{
    public class PasswordService : IPasswordService
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
