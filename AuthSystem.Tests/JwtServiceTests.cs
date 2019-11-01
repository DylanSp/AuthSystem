using AuthSystem.Data;
using AuthSystem.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AuthSystem.Tests
{
    [TestClass]
    public class JwtServiceTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public void Decode_GivenExpiredToken_ReturnsNull()
        {
            // Arrange
            var service = new JwtService(new JwtSecret("someSecret"));
            var token = service.CreateToken(new UserId(Guid.NewGuid()), DateTimeOffset.MinValue);

            // Act
            var result = service.DecodeToken(token);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public void Decode_GivenTokenWithInvalidSignature_ReturnsNull()
        {
            // Test by decoding with a different secret than what was used for decoding
            
            // Arrange
            var encoder = new JwtService(new JwtSecret("someSecret"));
            var token = encoder.CreateToken(new UserId(Guid.NewGuid()), DateTimeOffset.MaxValue);

            var decoder = new JwtService(new JwtSecret("someOtherSecret"));

            // Act
            var result = decoder.DecodeToken(token);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        public void Encode_ThenDecode_ReturnsSameUserId()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());
            var service = new JwtService(new JwtSecret("someSecret"));

            // Act
            var result = service.DecodeToken(service.CreateToken(userId, DateTimeOffset.MaxValue));

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(userId, result!.Value);
        }
    }
}
