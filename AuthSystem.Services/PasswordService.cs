﻿using AuthSystem.Data;
using AuthSystem.Interfaces;
using Sodium;

namespace AuthSystem.Services
{
    public class PasswordService : IPasswordService
    {
        private PasswordHash.StrengthArgon Strength { get; }

        public PasswordService(PasswordHash.StrengthArgon strength)
        {
            Strength = strength;
        }

        public SaltedHashedPassword GeneratePasswordHashAndSalt(PlaintextPassword password)
        {
            var hash = PasswordHash.ArgonHashString(password.Value, Strength).TrimEnd('\0');
            return new SaltedHashedPassword(hash);
        }

        public bool CheckIfPasswordMatchesHash(PlaintextPassword password, SaltedHashedPassword hash)
        {
            return PasswordHash.ArgonHashStringVerify(hash.Value, password.Value);
        }
    }
}
