#pragma warning disable S3261 // Namespaces should not be empty
namespace AuthSystem
{
    
#pragma warning disable S1135 // Track uses of "TODO" tags
    // TODO - structs in .Data project should potentially overload .Equals, implement == and !=
    // TODO - rework manager methods that take UserIds to instead take usernames
    // TODO - does ResourceManager need to worry about nonexistent user IDs? or is that handled by PermissionGrantManager?
    // TODO - logging with Serilog
    // TODO - grant permission mgmt permission to admin
    // TODO - need to autocreate admin user in DB in a migration (though how to handle password? default password that gets changed?)
    // TODO - how to grant permission-changing permission to admin automatically? isAdmin column in the DB, look up all users with it set, give them perms?
    // TODO - integration test(s) for CustomAuthHandler
    // TODO - UserController integration test - create user returns header with Uri to created user's location
}
#pragma warning restore S3261 // Namespaces should not be empty
#pragma warning restore S1135 // Track uses of "TODO" tags
