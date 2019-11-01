using AuthSystem.Controllers;
using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Interfaces.Managers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateUser_WithNewUsername_ReturnsCreatedResult()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.CreateUserAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(new UserId());
            var controller = new UserController(userManager);
            var dto = new UserAuthenticationDTO
            {
                Username = "Alice",
                Password = "AlicePassword",
            };

            // Act
            var response = await controller.CreateUserAsync(new ApiVersion(1, 0), dto);

            // Assert
            Assert.IsInstanceOfType(response.Result, typeof(CreatedResult));
        }

        [TestMethod]
        public async Task CreateUser_WithNewUsername_EchoesAuthenticationDetails()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.CreateUserAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(new UserId());
            var controller = new UserController(userManager);
            var dto = new UserAuthenticationDTO
            {
                Username = "Alice",
                Password = "AlicePassword",
            };

            // Act
            var response = await controller.CreateUserAsync(new ApiVersion(1, 0), dto);
            var returnedValue = (response.Result as CreatedResult)?.Value as UserAuthenticationDTO;

            // Assert
            Assert.AreEqual(dto.Username, returnedValue?.Username);
            Assert.AreEqual(dto.Password, returnedValue?.Password);
        }

        [TestMethod]
        public async Task CreateUser_WithDuplicateUsername_ReturnsBadRequest()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.CreateUserAsync(Arg.Any<Username>(), Arg.Any<PlaintextPassword>()).Returns(null as UserId?);
            var controller = new UserController(userManager);
            var dto = new UserAuthenticationDTO
            {
                Username = "Bob",
                Password = "BobPassword",
            };

            // Act
            var response = await controller.CreateUserAsync(new ApiVersion(1, 0), dto);

            // Assert
            Assert.IsInstanceOfType(response.Result, typeof(BadRequestObjectResult));
        }
    }
}
