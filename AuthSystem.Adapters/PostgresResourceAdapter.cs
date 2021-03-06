﻿using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Interfaces.Adapters;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class PostgresResourceAdapter : IResourceAdapter
    {
        private IPostgresConnectionContext ConnectionContext { get; }

        public PostgresResourceAdapter(IPostgresConnectionContext connectionContext)
        {
            ConnectionContext = connectionContext;
        }

        public async Task CreateResourceAsync(Resource newResource)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"INSERT INTO Resources (Id, Value)
                                        VALUES (@Id, @Value)";
                command.Parameters.AddWithValue("Id", newResource.Id.Value);
                command.Parameters.AddWithValue("Value", newResource.Value.Value);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Resource>> GetAllResourcesAsync()
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"SELECT Id, Value
                                        FROM Resources";

                var allResources = new List<Resource>();

                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        var id = new ResourceId((Guid) reader["Id"]);
                        var value = new ResourceValue((string) reader["Value"]);
                        allResources.Add(new Resource(id, value));
                    }

                    return allResources;
                }
            }
        }

        public async Task<Resource?> GetResourceAsync(ResourceId resourceId)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"SELECT Id, Value
                                        FROM Resources
                                        WHERE Id = @Id";
                command.Parameters.AddWithValue("Id", resourceId.Value);

                using (var reader = await command.ExecuteReaderAsync())
                {
                    if (await reader.ReadAsync())
                    {
                        var id = new ResourceId((Guid) reader["Id"]);
                        var value = new ResourceValue((string) reader["Value"]);
                        return new Resource(id, value);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public async Task<int> UpdateResourceAsync(Resource newResource)
        {
            using (var command = await ConnectionContext.CreateCommandAsync())
            {
                command.CommandText = @"UPDATE Resources
                                        SET Value = @Value
                                        WHERE Id = @Id";
                command.Parameters.AddWithValue("Value", newResource.Value.Value);
                command.Parameters.AddWithValue("Id", newResource.Id.Value);

                return await command.ExecuteNonQueryAsync();
            }
        }
    }
}
