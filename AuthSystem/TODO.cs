using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem
{
    // TODO - use ValueOf to create distinct UserId, PermissionGrantId, ResourceId types
        // TODO - can ASP.NET deserialize strings, Guids, etc. into ValueOf types?
    // TODO - genericize adapters to just take IDbConnection?
    // TODO - yank out Dapper from adapters, use ValueOf's From() when reading from DB
    // TODO - implement adapters for resources, permission grants
}
