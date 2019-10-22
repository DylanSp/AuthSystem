using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuthSystem.Interfaces;

namespace AuthSystem.Adapters
{
    public class PostgresPermissionGrantAdapter : IPermissionGrantAdapter
    {
        private IPostgresConnectionContext ConnectionContext { get; }

        public PostgresPermissionGrantAdapter(IPostgresConnectionContext connectionContext)
        {
            ConnectionContext = connectionContext;
        }

        public async Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission)
        {
            using (var command = ConnectionContext.CreateCommand())
            {
                command.CommandText = @"SELECT COUNT(*)
                                        FROM PermissionGrants
                                        WHERE UserId = @UserId
                                        AND ResourceId = @ResourceId
                                        AND PermissionType = @PermissionType";
                command.Parameters.AddWithValue("UserId", userId.Value);
                command.Parameters.AddWithValue("ResourceId", resourceId.Value);
                command.Parameters.AddWithValue("PermissionType", (int) permission);

                var numGrants = (long) await command.ExecuteScalarAsync();
                return numGrants > 0;
            }
        }

        public async Task CreatePermissionGrantAsync(PermissionGrant grant)
        {
            using (var command = ConnectionContext.CreateCommand())
            {
                command.CommandText = @"INSERT INTO PermissionGrants (Id, UserId, ResourceId, PermissionType)
                                        VALUES (@Id, @UserId, @ResourceId, @PermissionType)";
                command.Parameters.AddWithValue("Id", grant.Id.Value);
                command.Parameters.AddWithValue("UserId", grant.UserId.Value);
                command.Parameters.AddWithValue("ResourceId", grant.ResourceId.Value);
                command.Parameters.AddWithValue("PermissionType", (int) grant.PermissionType);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeletePermissionGrantAsync(PermissionGrantId permissionId)
        {
            using (var command = ConnectionContext.CreateCommand())
            {
                command.CommandText = @"DELETE FROM PermissionGrants
                                        WHERE Id = @Id";
                command.Parameters.AddWithValue("Id", permissionId.Value);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId)
        {
            using (var command = ConnectionContext.CreateCommand())
            {
                command.CommandText = @"SELECT Id, UserId, ResourceId, PermissionType
                                        FROM PermissionGrants
                                        WHERE UserId = @UserId";
                command.Parameters.AddWithValue("UserId", userId.Value);

                var reader = await command.ExecuteReaderAsync();

                var permissionGrants = new List<PermissionGrant>();

                while (await reader.ReadAsync())
                {
                    permissionGrants.Add(ReadGrant(reader));
                }

                return permissionGrants;
            }
        }

        public async Task<IEnumerable<PermissionGrant>> GetAllPermissionsForResourceAsync(ResourceId resourceId)
        {
            using (var command = ConnectionContext.CreateCommand())
            {
                command.CommandText = @"SELECT Id, UserId, ResourceId, PermissionType
                                        FROM PermissionGrants
                                        WHERE ResourceId = @ResourceId";
                command.Parameters.AddWithValue("ResourceId", resourceId.Value);

                var reader = await command.ExecuteReaderAsync();

                var permissionGrants = new List<PermissionGrant>();

                while (await reader.ReadAsync())
                {
                    permissionGrants.Add(ReadGrant(reader));
                }

                return permissionGrants;
            }
        }


        private PermissionGrant ReadGrant(NpgsqlDataReader reader)
        {
            var id = new PermissionGrantId((Guid)reader["Id"]);
            var readUserId = new UserId((Guid)reader["UserId"]);
            var resourceId = new ResourceId((Guid)reader["ResourceId"]);
            var permissionType = (int)reader["PermissionType"] switch
            {
                1 => PermissionType.Read,
                2 => PermissionType.Write,
                _ => PermissionType.Unknown
            };

            return new PermissionGrant(id, readUserId, resourceId, permissionType);
        }
    }
}
