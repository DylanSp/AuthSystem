using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem
{
    // TODO - in UserManager, make (check for username uniqueness +  user creation) atomic to avoid race conditions?
    // TODO - genericize adapters to just take IDbConnection?
    // TODO - yank out Dapper from adapters, use ValueOf's From() when reading from DB
    // TODO - implement adapters for resources, permission grants
}
