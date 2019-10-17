using System;
using System.Collections.Generic;
using System.Text;
using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Services;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using Sodium;

namespace AuthSystem.Tests
{
    [TestClass]
    public class PasswordServiceTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void GenerateHashAndSalt_ThenCheckingSamePassword_ReturnsTrue()
        {
            // Arrange
            var service = new PasswordService(PasswordHash.StrengthArgon.Interactive);
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
            var service = new PasswordService(PasswordHash.StrengthArgon.Interactive);

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
