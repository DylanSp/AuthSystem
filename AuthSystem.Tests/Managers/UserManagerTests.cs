using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;

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
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

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
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, "");

            // Assert
            Assert.AreEqual(CreateUserResults.UserCreated, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithNonexistentUser_ReturnsAppropriateError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.ChangePasswordAsync(userId, "oldpass", "newpass");

            // Assert
            Assert.AreEqual(ChangePasswordResults.UserNotPresent, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithIncorrectOldPassword_ReturnsAppropriateError()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(new User
            {
                Id = userId,
                Username = "someUser",
            });
            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<string>(), Arg.Any<HashedPassword>()).Returns(false);
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ChangePasswordAsync(userId, "oldpass", "newpass");

            // Assert
            Assert.AreEqual(ChangePasswordResults.PasswordIncorrect, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithExistingUsernameAndCorrectOldPassword_ReturnsSuccess()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(new User
            {
                Id = userId,
            });
            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<string>(), Arg.Any<HashedPassword>()).Returns(true);
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ChangePasswordAsync(userId, "oldpass", "newpass");

            // Assert
            Assert.AreEqual(ChangePasswordResults.PasswordChanged, result);
        }
    }
}
