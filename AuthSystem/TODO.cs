using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem
{
    // TODO - make ResourceManager work with UserIds instead of Usernames
    // TODO - in UserManager, make (check for username uniqueness +  user creation) atomic to avoid race conditions?
    // TODO - implement adapters for resources, permission grants
    // TODO - CI
    // TODO - unit tests - dotnet test --filter TestCategory=UnitTest
    // TODO - integration tests spin up Postgres container, run migrations + dotnet test --filter TestCategory=IntegrationTest ?
}
