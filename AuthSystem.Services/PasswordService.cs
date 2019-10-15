using AuthSystem.Data;
using AuthSystem.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Linq;

namespace AuthSystem.Services
{
    public class PasswordService : IPasswordService
    {
        private KeyDerivationParameters Parameters { get; }
        private ICryptoRng Rng { get; }

        public PasswordService(KeyDerivationParameters parameters, ICryptoRng rng)
        {
            Parameters = parameters;
            Rng = rng;
        }

        public HashedPassword GeneratePasswordHashAndSalt(PlaintextPassword password)
        {
            var saltBytes = Rng.GetRandomBytes(Parameters.SaltLength.Value);
            var salt = Convert.ToBase64String(saltBytes);

            var hashBytes = KeyDerivation.Pbkdf2(password.Value, saltBytes, Parameters.DerivationFunction,
                Parameters.IterationCount.Value, Parameters.KeyLength.Value);
            var hash = Convert.ToBase64String(hashBytes);
            
            return new HashedPassword(Base64Hash.From(hash), Base64Salt.From(salt));
        }

        public bool CheckIfPasswordMatchesHash(PlaintextPassword password, HashedPassword hash)
        {
            var passwordHash = KeyDerivation.Pbkdf2(password.Value, Convert.FromBase64String(hash.Base64Salt.Value), Parameters.DerivationFunction,
                Parameters.IterationCount.Value, Parameters.KeyLength.Value);
            return passwordHash.SequenceEqual(Convert.FromBase64String(hash.Base64PasswordHash.Value));
        }
    }
}
