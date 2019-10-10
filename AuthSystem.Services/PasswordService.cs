using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;

namespace AuthSystem.Services
{
    public class PasswordService : IPasswordService
    {
        public HashedPassword GeneratePasswordHashAndSalt(PlaintextPassword password)
        {
            throw new NotImplementedException();
        }

        public bool CheckIfPasswordMatchesHash(PlaintextPassword password, HashedPassword hash)
        {
            throw new NotImplementedException();
        }
    }
}
