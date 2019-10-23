namespace AuthSystem
{
    // TODO - standardize private member naming convention - should either be fields named as _field, or get-only properties named as Property
    // TODO - rework manager methods that take UserIds to instead take usernames
    // TODO - does ResourceManager need to worry about nonexistent user IDs? or is that handled by PermissionGrantManager?
    // TODO - logging with Serilog
    // TODO - grant permission mgmt permission to admin
    // TODO - need to autocreate admin user in DB in a migration (though how to handle password? default password that gets changed?)
    // TODO - how to grant permission-changing permission to admin automatically? isAdmin column in the DB, look up all users with it set, give them perms?
}
