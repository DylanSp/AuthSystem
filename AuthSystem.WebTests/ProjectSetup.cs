using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AuthSystem.WebTests
{
    [TestClass]
    public static class ProjectSetup
    {
        public static WebApplicationFactory<Startup> Factory { get; private set; }

        [AssemblyInitialize]
        public static void SetUp(TestContext context)
        {
            Factory = new WebApplicationFactory<Startup>()
               .WithWebHostBuilder(builder => builder.UseSetting("https_port", "443"));    // need this setting for HTTPS, so Secure cookies work
        }
    }
}
