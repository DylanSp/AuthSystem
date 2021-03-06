﻿using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;
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
            var username = new Username("Alice");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.ValidatePasswordAsync(username, new PlaintextPassword("somePass"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ValidatePassword_WithWrongPassword_ReturnsFalse()
        {
            // Arrange
            var username = new Username("Alice");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(new User(new UserId(Guid.NewGuid()), username, new SaltedHashedPassword("someSaltedHash")));

            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(false);
            
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ValidatePasswordAsync(username, new PlaintextPassword("somePass"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ValidatePassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var username = new Username("Alice");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(new User(new UserId(Guid.NewGuid()), username, new SaltedHashedPassword("someSaltedHash")));

            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(true);

            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ValidatePasswordAsync(username, new PlaintextPassword("somePass"));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithAlreadyExistingName_ReturnsAppropriateFailure()
        {
            // Arrange
            var username = new Username("Bob");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.CreateUserAsync(Arg.Any<User>()).Returns(0);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, new PlaintextPassword(""));

            // Assert
            Assert.IsFalse(result.HasValue);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithUniqueUsername_ReturnsSuccess()
        {
            // Arrange
            var username = new Username("Bob");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.CreateUserAsync(Arg.Any<User>()).Returns(1);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, new PlaintextPassword(""));

            // Assert
            Assert.IsTrue(result.HasValue);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithNonexistentUser_ReturnsAppropriateError()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.ChangePasswordAsync(userId, new PlaintextPassword("oldPass"), new PlaintextPassword("newPass"));

            // Assert
            Assert.AreEqual(ChangePasswordResult.UserNotPresent, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithIncorrectOldPassword_ReturnsAppropriateError()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(new User(userId, new Username("someUser"), new SaltedHashedPassword("someSaltedHash")));
            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(false);
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ChangePasswordAsync(userId, new PlaintextPassword("oldPass"), new PlaintextPassword("newPass"));

            // Assert
            Assert.AreEqual(ChangePasswordResult.PasswordIncorrect, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithExistingUsernameAndCorrectOldPassword_ReturnsSuccess()
        {
            // Arrange
            var userId = new UserId(Guid.NewGuid());
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(new User(userId, new Username(""), new SaltedHashedPassword("someSaltedHash")));
            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(true);
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ChangePasswordAsync(userId, new PlaintextPassword("oldPass"), new PlaintextPassword("newPass"));

            // Assert
            Assert.AreEqual(ChangePasswordResult.PasswordChanged, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetIdForUsername_WithNonexistentUsername_ReturnsNull()
        {
            // Arrange
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(Arg.Any<Username>()).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.GetIdForUsernameAsync(new Username("Bob"));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetIdForUsername_WithExistingUser_ReturnsUserId()
        {
            // Arrange
            var user = new User(new UserId(Guid.NewGuid()), new Username("Carl"), new SaltedHashedPassword("someHash"));
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(user.Username).Returns(user);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.GetIdForUsernameAsync(user.Username);

            // Assert
            Assert.IsTrue(result.HasValue);
            Assert.AreEqual(user.Id, result.Value);
        }
    }
}
