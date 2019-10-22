using AuthSystem.Adapters;
using AuthSystem.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Adapters
{
    [TestClass]
    public class PostgresPermissionGrantAdapterTests
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
        public async Task CreateGrant_ThenCheckForThatGrant_ReturnsTrue()
        {
            // Arrange
            var user = new User(new UserId(Guid.NewGuid()), new Username("Alice"), new SaltedHashedPassword("someSaltedHash"));
            var userAdapter = new PostgresUserAdapter(connection!);
            await userAdapter.CreateUserAsync(user);

            var resource = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("someSecret"));
            var resourceAdapter = new PostgresResourceAdapter(connection!);
            await resourceAdapter.CreateResourceAsync(resource);

            var grantToCreate = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Read);
            var permissionGrantAdapter = new PostgresPermissionGrantAdapter(connection!);

            // Act
            await permissionGrantAdapter.CreatePermissionGrantAsync(grantToCreate);
            var result =
                await permissionGrantAdapter.CheckIfUserHasPermissionAsync(user.Id, resource.Id, PermissionType.Read);

            // Assert
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task DeleteGrant_ThenCheckForThatGrant_ReturnsFalse()
        {
            // Arrange

            // create grant
            var user = new User(new UserId(Guid.NewGuid()), new Username("Alice"), new SaltedHashedPassword("someSaltedHash"));
            var userAdapter = new PostgresUserAdapter(connection!);
            await userAdapter.CreateUserAsync(user);

            var resource = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("someSecret"));
            var resourceAdapter = new PostgresResourceAdapter(connection!);
            await resourceAdapter.CreateResourceAsync(resource);

            var grant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Read);
            var permissionGrantAdapter = new PostgresPermissionGrantAdapter(connection!);

            await permissionGrantAdapter.CreatePermissionGrantAsync(grant);

            // Act
            await permissionGrantAdapter.DeletePermissionGrantAsync(grant.Id);
            var result =
                await permissionGrantAdapter.CheckIfUserHasPermissionAsync(user.Id, resource.Id, PermissionType.Read);

            // Assert
            Assert.AreEqual(false, result);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GetAllPermissionsForUser_WithMultipleGrants_ReturnsAll()
        {
            // Arrange
            var user = new User(new UserId(Guid.NewGuid()), new Username("Alice"), new SaltedHashedPassword("someSaltedHash"));
            var userAdapter = new PostgresUserAdapter(connection!);
            await userAdapter.CreateUserAsync(user);

            var resource = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("someSecret"));
            var resourceAdapter = new PostgresResourceAdapter(connection!);
            await resourceAdapter.CreateResourceAsync(resource);

            var readGrant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Read);
            var writeGrant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Write);
            var permissionGrantAdapter = new PostgresPermissionGrantAdapter(connection!);

            await permissionGrantAdapter.CreatePermissionGrantAsync(readGrant);
            await permissionGrantAdapter.CreatePermissionGrantAsync(writeGrant);

            // Act
            var allGrants = await permissionGrantAdapter.GetAllPermissionsForUserAsync(user.Id);

            // Assert
            Assert.IsTrue(allGrants.Count() >= 2);
            Assert.IsTrue(allGrants.Contains(readGrant));
            Assert.IsTrue(allGrants.Contains(writeGrant));
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GetAllPermissionsForResource_WithMultipleGrants_ReturnsAll()
        {
            // Arrange
            var user = new User(new UserId(Guid.NewGuid()), new Username("Alice"), new SaltedHashedPassword("someSaltedHash"));
            var userAdapter = new PostgresUserAdapter(connection!);
            await userAdapter.CreateUserAsync(user);

            var resource = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("someSecret"));
            var resourceAdapter = new PostgresResourceAdapter(connection!);
            await resourceAdapter.CreateResourceAsync(resource);

            var readGrant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Read);
            var writeGrant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Write);
            var permissionGrantAdapter = new PostgresPermissionGrantAdapter(connection!);

            await permissionGrantAdapter.CreatePermissionGrantAsync(readGrant);
            await permissionGrantAdapter.CreatePermissionGrantAsync(writeGrant);

            // Act
            var allGrants = await permissionGrantAdapter.GetAllPermissionsForResourceAsync(resource.Id);

            // Assert
            Assert.IsTrue(allGrants.Count() >= 2);
            Assert.IsTrue(allGrants.Contains(readGrant));
            Assert.IsTrue(allGrants.Contains(writeGrant));
        }
    }
}
