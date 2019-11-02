namespace AuthSystem
{
    // TODO - structs in .Data project should potentially overload .Equals, implement == and !=
    // TODO - rework manager methods that take UserIds to instead take usernames
    // TODO - does ResourceManager need to worry about nonexistent user IDs? or is that handled by PermissionGrantManager?
    // TODO - logging with Serilog
    // TODO - grant permission mgmt permission to admin
    // TODO - need to autocreate admin user in DB in a migration (though how to handle password? default password that gets changed?)
    // TODO - how to grant permission-changing permission to admin automatically? isAdmin column in the DB, look up all users with it set, give them perms?
    // TODO - integration tests (separate categories for DB integration tests and fullstack integration tests; probably separate project for the latter)
    // TODO - integration test(s) for CustomAuthHandler
    // TODO - UserController/AuthenticationController integration test - create user than login with same password succeeds
    // TODO - UserController integration test - create user returns header with Uri to created user's location
}
