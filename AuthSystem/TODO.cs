﻿namespace AuthSystem
{
    // TODO - API will need to be versioned - see https://github.com/microsoft/aspnet-api-versioning/wiki/New-Services-Quick-Start#aspnet-core
    // TODO - does ResourceManager need to worry about nonexistent user IDs? or is that handled by PermissionGrantManager?
    // TODO - logging with Serilog
    // TODO - grant permission mgmt permission to admin
    // TODO - need to autocreate admin user in DB in a migration (though how to handle password? default password that gets changed?)
    // TODO - how to grant permission-changing permission to admin automatically? isAdmin column in the DB, look up all users with it set, give them perms?
}
