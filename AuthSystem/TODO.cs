using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem
{
    // TODO - adapters should not take raw connection; should take IConnectionContext which has a CreateCommand() method, PostgresConnectionContext implements IDisposable, DI container makes sure context.Dispose() gets called
    // TODO - make sure all projects have nullable reference types
    // TODO - API will need to be versioned - see https://github.com/microsoft/aspnet-api-versioning/wiki/New-Services-Quick-Start#aspnet-core
    // TODO - make Users/Username column in DB have a UNIQUE constraint?
    // TODO - does ResourceManager need to worry about nonexistent user IDs? or is that handled by PermissionGrantManager?
    // TODO - in UserManager, make (check for username uniqueness +  user creation) atomic to avoid race conditions?
    // TODO - logging with Serilog
    // TODO - add a permission for managing permissions on a resource (grant to resource creator, admin)
    // TODO - need to autocreate admin user in DB in a migration (though how to handle password? default password that gets changed?)
    // TODO - how to grant permission-changing permission to admin automatically? isAdmin column in the DB, look up all users with it set, give them perms?
    // TODO - integration tests spin up Postgres container, run migrations + dotnet test --filter TestCategory=IntegrationTest ?
}
