using AuthSystem.Controllers;
using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Interfaces;
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
            userManager.GetIdForUsernameAsync(Arg.Any<Username>()).Returns(new UserId(Guid.NewGuid()));
            var controller = new AuthenticationController(userManager, Substitute.For<IJwtService>());

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
            userManager.GetIdForUsernameAsync(username).Returns(userId);

            var jwtService = Substitute.For<IJwtService>();
            jwtService.CreateToken(userId, Arg.Any<DateTimeOffset>()).Returns(new JsonWebToken("someToken"));

            var controller = new AuthenticationController(userManager, jwtService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

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
        public async Task Login_WithCorrectPassword_ReturnsAccessTokenAsCookie()
        {
            // Arrange
            var username = new Username("Alice");
            var userId = new UserId(Guid.NewGuid());
            var userManager = Substitute.For<IUserManager>();
            userManager.ValidatePasswordAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(true);
            userManager.GetIdForUsernameAsync(username).Returns(userId);

            var jwtService = Substitute.For<IJwtService>();
            var accessToken = new JsonWebToken("someToken");
            jwtService.CreateToken(userId, Arg.Any<DateTimeOffset>()).Returns(accessToken);

            var controller = new AuthenticationController(userManager, jwtService);
            controller.ControllerContext = new ControllerContext();
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var dto = new UserAuthenticationDTO
            {
                Username = username.Value,
                Password = ""
            };
            var result = await controller.LoginAsync(dto);

            // Assert
            Assert.IsTrue(controller.HttpContext.Response.Headers["Set-Cookie"]
                .Any(cookieHeader => cookieHeader.Contains(accessToken.Value)));
        }
    }
}
