using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuthSystem.Adapters
{
    public class PostgresPermissionGrantAdapter : IPermissionGrantAdapter
    {
        private NpgsqlConnection Connection { get; }

        public PostgresPermissionGrantAdapter(NpgsqlConnection connection)
        {
            Connection = connection;
        }

        public async Task<bool> CheckIfUserHasPermissionAsync(UserId userId, ResourceId resourceId, PermissionType permission)
        {
            var checkQuery = @"SELECT COUNT(*)
                               FROM PermissionGrants
                               WHERE UserId = @UserId
                               AND ResourceId = @ResourceId
                               AND PermissionType = @PermissionType";
            using (var command = new NpgsqlCommand(checkQuery, Connection))
            {
                command.Parameters.AddWithValue("UserId", userId.Value);
                command.Parameters.AddWithValue("ResourceId", resourceId.Value);
                command.Parameters.AddWithValue("PermissionType", (int) permission);

                var numGrants = (long) await command.ExecuteScalarAsync();
                return numGrants > 0;
            }
        }

        public async Task CreatePermissionGrantAsync(PermissionGrant grant)
        {
            var insertQuery = @"INSERT INTO PermissionGrants (Id, UserId, ResourceId, PermissionType)
                                VALUES (@Id, @UserId, @ResourceId, @PermissionType)";
            using (var command = new NpgsqlCommand(insertQuery, Connection))
            {
                command.Parameters.AddWithValue("Id", grant.Id.Value);
                command.Parameters.AddWithValue("UserId", grant.UserId.Value);
                command.Parameters.AddWithValue("ResourceId", grant.ResourceId.Value);
                command.Parameters.AddWithValue("PermissionType", (int) grant.PermissionType);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeletePermissionGrantAsync(PermissionGrantId permissionId)
        {
            var deleteQuery = @"DELETE FROM PermissionGrants
                                WHERE Id = @Id";
            using (var command = new NpgsqlCommand(deleteQuery, Connection))
            {
                command.Parameters.AddWithValue("Id", permissionId.Value);

                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<PermissionGrant>> GetAllPermissionsForUserAsync(UserId userId)
        {
            var getAllQuery = @"SELECT Id, UserId, ResourceId, PermissionType
                                FROM PermissionGrants
                                WHERE UserId = @UserId";
            using (var command = new NpgsqlCommand(getAllQuery, Connection))
            {
                command.Parameters.AddWithValue("UserId", userId.Value);

                var reader = await command.ExecuteReaderAsync();

                var permissionGrants = new List<PermissionGrant>();

                while (await reader.ReadAsync())
                {
                    var id = PermissionGrantId.From((Guid) reader["Id"]);
                    var readUserId = UserId.From((Guid) reader["UserId"]);
                    var resourceId = ResourceId.From((Guid) reader["ResourceId"]);
                    var permissionType = (int)reader["PermissionType"] switch
                    {
                        1 => PermissionType.Read,
                        2 => PermissionType.Write,
                        _ => PermissionType.Unknown
                    };

                    permissionGrants.Add(new PermissionGrant(id, readUserId, resourceId, permissionType));
                }

                return permissionGrants;
            }
        }
    }
}
