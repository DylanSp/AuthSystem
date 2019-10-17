using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Threading.Tasks;

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
            var username = Username.From("Alice");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.ValidatePasswordAsync(username, PlaintextPassword.From("somePass"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ValidatePassword_WithWrongPassword_ReturnsFalse()
        {
            // Arrange
            var username = Username.From("Alice");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(new User(UserId.From(Guid.NewGuid()), username, SaltedHashedPassword.From("someSaltedHash")));

            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(false);
            
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ValidatePasswordAsync(username, PlaintextPassword.From("somePass"));

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ValidatePassword_WithCorrectPassword_ReturnsTrue()
        {
            // Arrange
            var username = Username.From("Alice");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByUsernameAsync(username).Returns(new User(UserId.From(Guid.NewGuid()), username, SaltedHashedPassword.From("someSaltedHash")));

            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(true);

            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ValidatePasswordAsync(username, PlaintextPassword.From("somePass"));

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithAlreadyExistingName_ReturnsAppropriateFailure()
        {
            // Arrange
            var username = Username.From("Bob");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.IsUsernameUniqueAsync(username).Returns(false);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, PlaintextPassword.From(""));

            // Assert
            result.Switch(
                usernameAlreadyExists => { },
                userCreated => throw new Exception("test failed")
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithUniqueUsername_ReturnsSuccess()
        {
            // Arrange
            var username = Username.From("Bob");
            var adapter = Substitute.For<IUserAdapter>();
            adapter.IsUsernameUniqueAsync(username).Returns(true);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.CreateUserAsync(username, PlaintextPassword.From(""));

            // Assert
            result.Switch(
                usernameAlreadyExists => throw new Exception("test failed"),
                userCreated => { }
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithNonexistentUser_ReturnsAppropriateError()
        {
            // Arrange
            var userId = UserId.From(Guid.NewGuid());
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(null as User?);
            var manager = new UserManager(adapter, Substitute.For<IPasswordService>());

            // Act
            var result = await manager.ChangePasswordAsync(userId, PlaintextPassword.From("oldpass"), PlaintextPassword.From("newpass"));

            // Assert
            Assert.AreEqual(ChangePasswordResult.UserNotPresent, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithIncorrectOldPassword_ReturnsAppropriateError()
        {
            // Arrange
            var userId = UserId.From(Guid.NewGuid());
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(new User(userId, Username.From("someUser"), SaltedHashedPassword.From("someSaltedHash")));
            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(false);
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ChangePasswordAsync(userId, PlaintextPassword.From("oldpass"), PlaintextPassword.From("newpass"));

            // Assert
            Assert.AreEqual(ChangePasswordResult.PasswordIncorrect, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task ChangePassword_WithExistingUsernameAndCorrectOldPassword_ReturnsSuccess()
        {
            // Arrange
            var userId = UserId.From(Guid.NewGuid());
            var adapter = Substitute.For<IUserAdapter>();
            adapter.GetUserByIdAsync(userId).Returns(new User(userId, Username.From(""), SaltedHashedPassword.From("someSaltedHash")));
            var passwordService = Substitute.For<IPasswordService>();
            passwordService.CheckIfPasswordMatchesHash(Arg.Any<PlaintextPassword>(), Arg.Any<SaltedHashedPassword>()).Returns(true);
            var manager = new UserManager(adapter, passwordService);

            // Act
            var result = await manager.ChangePasswordAsync(userId, PlaintextPassword.From("oldpass"), PlaintextPassword.From("newpass"));

            // Assert
            Assert.AreEqual(ChangePasswordResult.PasswordChanged, result);
        }
    }
}
