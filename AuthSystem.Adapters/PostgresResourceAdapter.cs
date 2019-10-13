using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class PostgresResourceAdapter : IResourceAdapter
    {
        private NpgsqlConnection Connection { get; }

        public PostgresResourceAdapter(NpgsqlConnection connection)
        {
            Connection = connection;
        }

        public async Task CreateResourceAsync(Resource newResource)
        {
            var insertResourceQuery = @"INSERT INTO Resources (Id, Value)
                                        VALUES (@Id, @Value)";
            using (var command = new NpgsqlCommand(insertResourceQuery, Connection))
            {
                command.Parameters.AddWithValue("Id", newResource.Id.Value);
                command.Parameters.AddWithValue("Value", newResource.Value.Value);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            var getResourcesQuery = @"SELECT Id, Value
                                      FROM Resources";
            using (var command = new NpgsqlCommand(getResourcesQuery, Connection))
            {
                var allResources = new List<Resource>();

                var reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    var id = ResourceId.From((Guid)reader["Id"]);
                    var value = ResourceValue.From((string)reader["Value"]);
                    allResources.Add(new Resource(id, value));
                }

                return allResources;
            }
        }

        public async Task<Resource?> GetResourceAsync(ResourceId resourceId)
        {
            var getResourceQuery = @"SELECT Id, Value
                                      FROM Resources
                                      WHERE Id = @Id";
            using (var command = new NpgsqlCommand(getResourceQuery, Connection))
            {
                command.Parameters.AddWithValue("Id", resourceId.Value);

                var reader = await command.ExecuteReaderAsync();
                if (await reader.ReadAsync())
                {
                    var id = ResourceId.From((Guid)reader["Id"]);
                    var value = ResourceValue.From((string)reader["Value"]);
                    return new Resource(id, value);
                }
                else
                {
                    return null;
                }
            }
        }

        public async Task<int> UpdateResourceAsync(Resource newResource)
        {
            var updateQuery = @"UPDATE Resources
                                SET Value = @Value
                                WHERE Id = @Id";
            using (var command = new NpgsqlCommand(updateQuery, Connection))
            {
                command.Parameters.AddWithValue("Value", newResource.Value.Value);
                command.Parameters.AddWithValue("Id", newResource.Id.Value);

                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
