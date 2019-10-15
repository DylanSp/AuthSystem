using System;
using System.Collections.Generic;
using System.Text;
using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace AuthSystem.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void GenerateHashAndSalt_ReturnsSalt_WithNumberOfBytesEqualToSaltLengthParameter()
        {
            // Arrange
            var iterationCount = 10_000;
            var saltLength = 16;
            var keyLength = 64;
            var parameters = new KeyDerivationParameters(KeyDerivationPrf.HMACSHA512,
                IterationCount.From(iterationCount), SaltLength.From(saltLength), KeyLength.From(keyLength));

            var rng = Substitute.For<ICryptoRng>();
            rng.GetRandomBytes(Arg.Any<int>()).Returns(args => new byte[args.Arg<int>()]);

            var service = new PasswordService(parameters, rng);

            // Act
            var hash = service.GeneratePasswordHashAndSalt(PlaintextPassword.From("somePassword"));

            // Assert
            Assert.AreEqual(saltLength, Convert.FromBase64String(hash.Base64Salt.Value).Length);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GenerateHashAndSalt_ReturnsHash_WithNumberOfBytesEqualToKeyLengthParameter()
        {
            // Arrange
            var iterationCount = 10_000;
            var saltLength = 16;
            var keyLength = 64;
            var parameters = new KeyDerivationParameters(KeyDerivationPrf.HMACSHA512,
                IterationCount.From(iterationCount), SaltLength.From(saltLength), KeyLength.From(keyLength));

            var rng = Substitute.For<ICryptoRng>();
            rng.GetRandomBytes(Arg.Any<int>()).Returns(args => new byte[args.Arg<int>()]);

            var service = new PasswordService(parameters, rng);

            // Act
            var hash = service.GeneratePasswordHashAndSalt(PlaintextPassword.From("somePassword"));

            // Assert
            Assert.AreEqual(keyLength, Convert.FromBase64String(hash.Base64PasswordHash.Value).Length);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GenerateHashAndSalt_ThenCheckingSamePassword_ReturnsTrue()
        {
            // Arrange
            var iterationCount = 10_000;
            var saltLength = 16;
            var keyLength = 64;
            var parameters = new KeyDerivationParameters(KeyDerivationPrf.HMACSHA512,
                IterationCount.From(iterationCount), SaltLength.From(saltLength), KeyLength.From(keyLength));

            var rng = Substitute.For<ICryptoRng>();
            rng.GetRandomBytes(Arg.Any<int>()).Returns(args => new byte[args.Arg<int>()]);

            var service = new PasswordService(parameters, rng);

            var password = PlaintextPassword.From("somePass");

            // Act
            var hash = service.GeneratePasswordHashAndSalt(password);
            var checkResult = service.CheckIfPasswordMatchesHash(password, hash);

            // Assert
            Assert.IsTrue(checkResult);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void GenerateHashAndSalt_ThenCheckingOtherPassword_ReturnsFalse()
        {
            // Arrange
            var iterationCount = 10_000;
            var saltLength = 16;
            var keyLength = 64;
            var parameters = new KeyDerivationParameters(KeyDerivationPrf.HMACSHA512,
                IterationCount.From(iterationCount), SaltLength.From(saltLength), KeyLength.From(keyLength));

            var rng = Substitute.For<ICryptoRng>();
            rng.GetRandomBytes(Arg.Any<int>()).Returns(args => new byte[args.Arg<int>()]);

            var service = new PasswordService(parameters, rng);

            var password = PlaintextPassword.From("somePass");
            var otherPass = PlaintextPassword.From("otherPass");

            // Act
            var hash = service.GeneratePasswordHashAndSalt(password);
            var checkResult = service.CheckIfPasswordMatchesHash(otherPass, hash);

            // Assert
            Assert.IsFalse(checkResult);
        }
    }
}
