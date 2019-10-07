using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace AuthSystem.Tests.Managers
{
    [TestClass]
    public class UserManagerTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithAlreadyExistingName_ReturnsAppropriateFailure()
        {
            // Arrange
            var username = "Bob";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.IsUserIdUniqueAsync(Arg.Any<Guid>()).Returns(true);
            adapter.IsUsernameUniqueAsync(username).Returns(false);
            var manager = new UserManager(adapter);

            // Act
            var result = await manager.CreateUserAsync(username, "");

            // Assert
            Assert.AreEqual(CreateUserResults.UsernameAlreadyExists, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithUniqueUsername_ReturnsSuccess()
        {
            // Arrange
            var username = "Bob";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.IsUserIdUniqueAsync(Arg.Any<Guid>()).Returns(true);
            adapter.IsUsernameUniqueAsync(username).Returns(true);
            var manager = new UserManager(adapter);

            // Act
            var result = await manager.CreateUserAsync(username, "");

            // Assert
            Assert.AreEqual(CreateUserResults.UserCreated, result);
        }
    }
}
