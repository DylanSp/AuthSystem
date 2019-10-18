using AuthSystem.Data;
using AuthSystem.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            var password = new PlaintextPassword("somePass");

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

            var password = new PlaintextPassword("somePass");
            var otherPass = new PlaintextPassword("otherPass");

            // Act
            var hash = service.GeneratePasswordHashAndSalt(password);
            var checkResult = service.CheckIfPasswordMatchesHash(otherPass, hash);

            // Assert
            Assert.IsFalse(checkResult);
        }
    }
}
