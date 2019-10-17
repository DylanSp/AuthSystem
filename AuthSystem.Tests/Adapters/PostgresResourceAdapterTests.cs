using AuthSystem.Adapters;
using AuthSystem.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Adapters
{
    [TestClass]
    public class PostgresResourceAdapterTests
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
            var resourceToCreate = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From(Guid.NewGuid().ToString()));
            var adapter = new PostgresResourceAdapter(connection!);

            // Act
            await adapter.CreateResourceAsync(resourceToCreate);
            var readResource = await adapter.GetResourceAsync(resourceToCreate.Id);

            // Assert
            Assert.IsNotNull(readResource);
            Assert.AreEqual(resourceToCreate, readResource.Value);

        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Update_ThenRead_ReturnsUpdatedData()
        {
            // Arrange
            var initialResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("initial"));
            var updatedResource = new Resource(initialResource.Id, ResourceValue.From("updated"));

            var adapter = new PostgresResourceAdapter(connection!);

            // Act
            await adapter.CreateResourceAsync(initialResource);
            await adapter.UpdateResourceAsync(updatedResource);
            var readResource = await adapter.GetResourceAsync(updatedResource.Id);

            // Assert
            Assert.AreNotEqual(initialResource, readResource.Value);
            Assert.AreEqual(updatedResource, readResource.Value);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task Update_OfOneResource_ReturnsOne()
        {
            // Arrange
            var initialResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("initial"));
            var updatedResource = new Resource(initialResource.Id, ResourceValue.From("updated"));

            var adapter = new PostgresResourceAdapter(connection!);

            // Act
            await adapter.CreateResourceAsync(initialResource);
            var numUpdated = await adapter.UpdateResourceAsync(updatedResource);

            // Assert
            Assert.AreEqual(1, numUpdated);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task GetAllResources_WithMultipleResources_ReturnsAll()
        {
            // Arrange
            var resource1 = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("value1"));
            var resource2 = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("value2"));

            var adapter = new PostgresResourceAdapter(connection!);

            // Act
            await adapter.CreateResourceAsync(resource1);
            await adapter.CreateResourceAsync(resource2);
            var readResources = await adapter.GetAllResourcesAsync();

            // Assert
            Assert.IsTrue(readResources.Contains(resource1));
            Assert.IsTrue(readResources.Contains(resource2));
        }
    }
}
