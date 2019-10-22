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
            var userToCreate = new User(new UserId(Guid.NewGuid()), new Username(Guid.NewGuid().ToString()), new SaltedHashedPassword("someSaltedHash"));
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
            var userToCreate = new User(new UserId(Guid.NewGuid()), new Username(Guid.NewGuid().ToString()), new SaltedHashedPassword("someSaltedHash"));
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
            var initialUser = new User(new UserId(Guid.NewGuid()), new Username(Guid.NewGuid().ToString()), new SaltedHashedPassword("someSaltedHash"));
            var updatedUser = new User(initialUser.Id, initialUser.Username, new SaltedHashedPassword("someOtherSaltedHash"));

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
            var initialUser = new User(new UserId(Guid.NewGuid()), new Username(Guid.NewGuid().ToString()), new SaltedHashedPassword("someSaltedHash"));
            var updatedUser = new User(initialUser.Id, initialUser.Username, new SaltedHashedPassword("someOtherSaltedHash"));

            var adapter = new PostgresUserAdapter(connection!);

            // Act
            await adapter.CreateUserAsync(initialUser);
            var numUpdated = await adapter.UpdateUserAsync(updatedUser);

            // Assert
            Assert.AreEqual(1, numUpdated);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task CreateUser_WithNewUsername_ReturnsOne()
        {
            // Arrange
            var user = new User(new UserId(Guid.NewGuid()), new Username(Guid.NewGuid().ToString()), new SaltedHashedPassword("someSaltedHash"));
            var adapter = new PostgresUserAdapter(connection!);

            // Act
            var numInserted = await adapter.CreateUserAsync(user);

            // Assert
            Assert.AreEqual(1, numInserted);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task CreateUser_WithExistingUsername_ReturnsZero()
        {
            // Arrange
            var user = new User(new UserId(Guid.NewGuid()), new Username(Guid.NewGuid().ToString()), new SaltedHashedPassword("someSaltedHash"));
            var adapter = new PostgresUserAdapter(connection!);
            await adapter.CreateUserAsync(user);    // make sure user already exists...

            // Act
            // ...then try creating it again
            var newUser = new User(new UserId(Guid.NewGuid()), user.Username, user.SaltedHashedPassword);
            var numInserted = await adapter.CreateUserAsync(newUser);

            // Assert
            Assert.AreEqual(0, numInserted);
        }
    }
}
