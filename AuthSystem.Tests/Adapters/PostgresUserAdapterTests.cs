using AuthSystem.Adapters;
using AuthSystem.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Adapters
{
    [TestClass]
    public class PostgresUserAdapterTests
    {
        private NpgsqlConnection? connection;

        [TestInitialize]
        public async Task Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("TestConfig.json").Build();
            var connectionString = config["connectionString"];
            connection = new NpgsqlConnection(connectionString);
            await connection.OpenAsync();
        }

        [TestCleanup]
        public async Task Teardown()
        {
            await connection!.DisposeAsync();
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Save_ThenReadById_RoundTrips()
        {
            // Arrange
            var userToCreate = new User(UserId.From(Guid.NewGuid()), Username.From("Alfred"), SaltedHashedPassword.From("someSaltedHash"));
            var adapter = new PostgresUserAdapter(connection!);

            // Act
            await adapter.CreateUserAsync(userToCreate);
            var readUser = await adapter.GetUserByIdAsync(userToCreate.Id);

            // Assert
            Assert.IsNotNull(readUser);
            Assert.AreEqual(userToCreate, readUser.Value);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Save_ThenReadByName_RoundTrips()
        {
            // Arrange
            var userToCreate = new User(UserId.From(Guid.NewGuid()), Username.From(Guid.NewGuid().ToString()), SaltedHashedPassword.From("someSaltedHash"));
            var adapter = new PostgresUserAdapter(connection!);

            // Act
            await adapter.CreateUserAsync(userToCreate);
            var readUser = await adapter.GetUserByUsernameAsync(userToCreate.Username);

            // Assert
            Assert.IsNotNull(readUser);
            Assert.AreEqual(userToCreate, readUser.Value);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Update_ThenRead_ReturnsUpdatedData()
        {
            // Arrange
            var initialUser = new User(UserId.From(Guid.NewGuid()), Username.From(Guid.NewGuid().ToString()), SaltedHashedPassword.From("someSaltedHash"));
            var updatedUser = new User(initialUser.Id, initialUser.Username, SaltedHashedPassword.From("someOtherSaltedHash"));

            var adapter = new PostgresUserAdapter(connection!);

            // Act
            await adapter.CreateUserAsync(initialUser);
            await adapter.UpdateUserAsync(updatedUser);
            var readUser = await adapter.GetUserByIdAsync(updatedUser.Id);

            // Assert
            Assert.AreNotEqual(initialUser, readUser.Value);
            Assert.AreEqual(updatedUser, readUser.Value);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Update_OfOneUser_ReturnsOne()
        {
            // Arrange
            var initialUser = new User(UserId.From(Guid.NewGuid()), Username.From(Guid.NewGuid().ToString()), SaltedHashedPassword.From("someSaltedHash"));
            var updatedUser = new User(initialUser.Id, initialUser.Username, SaltedHashedPassword.From("someOtherSaltedHash"));

            var adapter = new PostgresUserAdapter(connection!);

            // Act
            await adapter.CreateUserAsync(initialUser);
            var numUpdated = await adapter.UpdateUserAsync(updatedUser);

            // Assert
            Assert.AreEqual(1, numUpdated);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task IsUsernameUnique_WithUniqueNameInDatabase_ReturnsTrue()
        {
            // Arrange
            var user = new User(UserId.From(Guid.NewGuid()), Username.From(Guid.NewGuid().ToString()), SaltedHashedPassword.From("someSaltedHash"));
            var adapter = new PostgresUserAdapter(connection!);

            // Act
            await adapter.CreateUserAsync(user);
            var isUnique = await adapter.IsUsernameUniqueAsync(user.Username);

            // Assert
            Assert.IsTrue(isUnique);
        }
    }
}
