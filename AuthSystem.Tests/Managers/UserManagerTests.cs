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
        public async Task ValidatePassword_ForNonexistentUser_ReturnsFalse()
        {
            // Arrange
            var username = "Alice";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.ValidatePasswordAsync(username, "somePass");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ValidatePassword_WithWrongPassword_ReturnsFalse()
        {
            // Arrange
            var username = "Alice";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(new User
            {
                Username = username,
                HashedPassword = new HashedPassword(),
            });

            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<string>(), Arg.Any<HashedPassword>()).Returns(false);
            
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ValidatePasswordAsync(username, "somePass");

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ValidatePassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var username = "Alice";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(new User
            {
                Username = username,
                HashedPassword = new HashedPassword(),
            });

            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<string>(), Arg.Any<HashedPassword>()).Returns(true);

            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ValidatePasswordAsync(username, "somePass");

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithAlreadyExistingName_ReturnsAppropriateFailure()
        {
            // Arrange
            var username = "Bob";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.IsUsernameUniqueAsync(username).Returns(false);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, "");

            // Assert
            result.Match(
                usernameAlreadyExists => true,
                userCreated => throw new Exception("test failed")
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithUniqueUsername_ReturnsSuccess()
        {
            // Arrange
            var username = "Bob";
            var adapter = Substitute.For<IUserAdapter>();
            adapter.IsUsernameUniqueAsync(username).Returns(true);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, "");

            // Assert
            result.Match(
                usernameAlreadyExists => throw new Exception("test failed"),
                userCreated => true
            );
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
            Assert.AreEqual(ChangePasswordResult.UserNotPresent, result);
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
            Assert.AreEqual(ChangePasswordResult.PasswordIncorrect, result);
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
            Assert.AreEqual(ChangePasswordResult.PasswordChanged, result);
        }
    }
}
