using AuthSystem.Data;
using AuthSystem.Interfaces;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AuthSystem.Tests.Managers
{
    [TestClass]
    public class ResourceManagerTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetResource_ForNonexistentUser_ReturnsNull()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(Arg.Any<string>()).Returns(new UsernameDoesNotExist());

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.GetResourceAsync(Guid.NewGuid(), "someUser");

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetResource_ForUserWithoutReadPermission_ReturnsNull()
        {
            // Arrange
            var resourceId = Guid.NewGuid();

            // adapter needs to return something non-null so our assert can check the difference vs. failing to have permission
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetResourceAsync(Arg.Any<Guid>()).Returns(new Resource());

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "Carl"
            };
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, resourceId, PermissionType.Read).Returns(false);

            var resourceManager = new ResourceManager(adapter, userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.GetResourceAsync(resourceId, user.Username);

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetResource_ForUserWithReadPermission_ReturnsResource()
        {
            // Arrange
            var resource = new Resource
            {
                Id = Guid.NewGuid(),
                Value = "someSecret",
            };

            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetResourceAsync(resource.Id).Returns(resource);

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "Carl"
            };
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, resource.Id, PermissionType.Read).Returns(true);

            var resourceManager = new ResourceManager(adapter, userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.GetResourceAsync(resource.Id, user.Username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resource, result.Value);
        }
    }
}
