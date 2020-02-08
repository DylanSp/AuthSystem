using AuthSystem.Adapters;
using AuthSystem.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Npgsql;
using System;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Interfaces;

namespace AuthSystem.Tests.Adapters
{
    [TestClass]
    public class PostgresResourceAdapterTests
    {
        private IPostgresConnectionContext? connectionContext;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("TestConfig.json").Build();
            var connectionString = config["connectionString"];
            connectionContext = new PostgresConnectionContext(connectionString);
        }

        [TestCleanup]
        public void Teardown()
        {
            connectionContext!.Dispose();
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("DbTest")]
        public async Task Save_ThenReadById_RoundTrips()
        {
            // Arrange
            var resourceToCreate = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue(Guid.NewGuid().ToString()));
            var adapter = new PostgresResourceAdapter(connectionContext!);

            // Act
            await adapter.CreateResourceAsync(resourceToCreate);
            var readResource = await adapter.GetResourceAsync(resourceToCreate.Id);

            // Assert
            Assert.IsNotNull(readResource);
            Assert.AreEqual(resourceToCreate, readResource.Value);

        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("DbTest")]
        public async Task Update_ThenRead_ReturnsUpdatedData()
        {
            // Arrange
            var initialResource = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("initial"));
            var updatedResource = new Resource(initialResource.Id, new ResourceValue("updated"));

            var adapter = new PostgresResourceAdapter(connectionContext!);

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
        [TestCategory("DbTest")]
        public async Task Update_OfOneResource_ReturnsOne()
        {
            // Arrange
            var initialResource = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("initial"));
            var updatedResource = new Resource(initialResource.Id, new ResourceValue("updated"));

            var adapter = new PostgresResourceAdapter(connectionContext!);

            // Act
            await adapter.CreateResourceAsync(initialResource);
            var numUpdated = await adapter.UpdateResourceAsync(updatedResource);

            // Assert
            Assert.AreEqual(1, numUpdated);
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        [TestCategory("DbTest")]
        public async Task GetAllResources_WithMultipleResources_ReturnsAll()
        {
            // Arrange
            var resource1 = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("value1"));
            var resource2 = new Resource(new ResourceId(Guid.NewGuid()), new ResourceValue("value2"));

            var adapter = new PostgresResourceAdapter(connectionContext!);

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
