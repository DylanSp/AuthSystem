using AuthSystem.Data;
using AuthSystem.Interfaces;
using System;

namespace AuthSystem.Services
{
    public class PasswordService : IPasswordService
    {
        public HashedPassword GeneratePasswordHashAndSalt(PlaintextPassword password)
        {
            // TODO - implement
            throw new NotImplementedException();
        }

        public bool CheckIfPasswordMatchesHash(PlaintextPassword password, HashedPassword hash)
        {
            // TODO - implement
            throw new NotImplementedException();
        }
    }
}
