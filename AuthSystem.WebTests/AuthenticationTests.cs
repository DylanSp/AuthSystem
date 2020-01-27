using AuthSystem.DTOs;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthSystem.WebTests
{
    [TestClass]
    public class AuthenticationTests
    {
        private static WebApplicationFactory<Startup> _factory;
        private static string _username;
        private static string _password;

        [ClassInitialize]   // run once before all tests
        public static async Task Setup(TestContext context)
        {
            _factory = new WebApplicationFactory<Startup>()
                .WithWebHostBuilder(builder => builder.UseSetting("https_port", "443"));    // need this setting for HTTPS, so Secure cookies work

            // initialize user
            _username = Guid.NewGuid().ToString();
            _password = Guid.NewGuid().ToString();

            var client = _factory.CreateClient();
            var userCreationResponse = await client.PostAsJsonAsync("/v1/users", new UserAuthenticationDTO
            {
                Username = _username,
                Password = _password,
            });

            if (!userCreationResponse.IsSuccessStatusCode)
            {
                throw new Exception($"Failed to initialize user for Web {nameof(AuthenticationTests)}");
            }
        }

        [TestMethod]
        public async Task Login_WithValidCredentials_ReturnsOk()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act 
            var response = await client.PostAsJsonAsync("/v1/login", new UserAuthenticationDTO
            {
                Username = _username,
                Password = _password,
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Login_WithInvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/v1/login", new UserAuthenticationDTO
            {
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString(),
            });

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [TestMethod]
        public async Task Login_WithValidCredentials_SetsSessionCookie()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var response = await client.PostAsJsonAsync("/v1/login", new UserAuthenticationDTO
            {
                Username = _username,
                Password = _password,
            });

            // Assert
            Assert.IsTrue(response.Headers.Contains("Set-Cookie"));
            Assert.IsTrue(response.Headers.GetValues("Set-Cookie")
                .Any(headerValue => headerValue.Contains(Constants.SESSION_COOKIE_NAME)));
        }

        [TestMethod]
        public async Task LoginThenLogout_WithValidCredentials_ReturnsNoContent()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            await client.PostAsJsonAsync("/v1/login", new UserAuthenticationDTO
            {
                Username = _username,
                Password = _password,
            });

            var logoutResponse = await client.PostAsync("/v1/logout", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.NoContent, logoutResponse.StatusCode);
        }

        [TestMethod]
        public async Task Logout_WithNoSessionCookie_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();

            // Act
            var logoutResponse = await client.PostAsync("/v1/logout", null);
            
            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, logoutResponse.StatusCode);
        }

        [TestMethod]
        public async Task Logout_WithInvalidSessionCookie_ReturnsUnauthorized()
        {
            // Arrange
            var client = _factory.CreateClient();
            client.DefaultRequestHeaders.Add("Cookie", $"SessionCookie={Guid.NewGuid()}; path=/; domain=localhost; Secure; HttpOnly;"); 

            // Act
            var logoutResponse = await client.PostAsync("/v1/logout", null);

            // Assert
            Assert.AreEqual(HttpStatusCode.Unauthorized, logoutResponse.StatusCode);
        }
    }
}
