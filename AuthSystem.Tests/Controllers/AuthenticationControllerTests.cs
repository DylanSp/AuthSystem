using AuthSystem.Controllers;
using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Interfaces.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Controllers
{
    [TestClass]
    public class AuthenticationControllerTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Login_WithIncorrectPassword_ReturnsUnauthorized()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.ValidatePasswordAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(false);
            var controller = new AuthenticationController(userManager, Substitute.For<ISessionCookieManager>());

            // Act
            var result = await controller.LoginAsync(new UserAuthenticationDTO());

            // Assert
            Assert.IsInstanceOfType(result, typeof(UnauthorizedResult));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Login_WithCorrectPassword_ReturnsOk()
        {
            // Arrange
            var username = new Username("Alice");
            var userId = new UserId(Guid.NewGuid());
            var userManager = Substitute.For<IUserManager>();
            userManager.ValidatePasswordAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(true);

            var sessionCookieManager = Substitute.For<ISessionCookieManager>();
            sessionCookieManager.CreateSessionCookieAsync(username).Returns(new SessionCookieId(Guid.NewGuid()));

            var controller = new AuthenticationController(userManager, sessionCookieManager)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var dto = new UserAuthenticationDTO
            {
                Username = username.Value,
                Password = ""
            };
            var result = await controller.LoginAsync(dto);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkResult));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task Login_WithCorrectPassword_ReturnsSessionCookie()
        {
            // Arrange
            var username = new Username("Alice");
            var userId = new UserId(Guid.NewGuid());
            var userManager = Substitute.For<IUserManager>();
            userManager.ValidatePasswordAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(true);

            var sessionCookieManager = Substitute.For<ISessionCookieManager>();
            var sessionCookieId = new SessionCookieId(Guid.NewGuid());
            sessionCookieManager.CreateSessionCookieAsync(username).Returns(sessionCookieId);

            var controller = new AuthenticationController(userManager, sessionCookieManager)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = new DefaultHttpContext()
                }
            };

            // Act
            var dto = new UserAuthenticationDTO
            {
                Username = username.Value,
                Password = ""
            };
            var result = await controller.LoginAsync(dto);

            // Assert
            Assert.IsTrue(controller.HttpContext.Response.Headers["Set-Cookie"]
                .Any(cookieHeader => cookieHeader.Contains(sessionCookieId.Value.ToString())));
        }
    }
}
