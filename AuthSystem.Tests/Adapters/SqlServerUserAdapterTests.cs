using AuthSystem.Adapters;
using AuthSystem.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Adapters
{
    [TestClass]
    public class SqlServerUserAdapterTests
    {
        private string connectionString;

        [TestInitialize]
        public void Setup()
        {
            var config = new ConfigurationBuilder().AddJsonFile("TestConfig.json").Build();
            connectionString = config["connectionString"];
        }

        [TestMethod]
        [TestCategory("IntegrationTest")]
        public async Task SaveThenRead_RoundTrips()
        {
            // Arrange
            using (var connection = new SqlConnection(connectionString))
            {
                var adapter = new SqlServerUserAdapter(connection);

                var savedUser = new User(Guid.NewGuid(), "someUsername", new HashedPassword("someHash", "someSalt"));

                // Act
                await adapter.SaveAsync(savedUser);
                var readUser = await adapter.ReadAsync(savedUser.Id);

                // Assert
                Assert.IsTrue(readUser.HasValue);
                Assert.AreEqual(savedUser, readUser);
            }
        }
    }
}
