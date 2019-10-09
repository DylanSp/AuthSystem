using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Interfaces.Managers;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllResources_ForNonexistentUser_ReturnsEmptyList()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(Arg.Any<string>()).Returns(new UsernameDoesNotExist());

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.GetAllResourcesAsync("Diana");

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllResources_ForUserWithNoPermissions_ReturnsEmptyList()
        {
            // Arrange
            var resource = new Resource
            {
                Id = Guid.NewGuid(),
                Value = "someSecret"
            };
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { resource });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "Eric"
            };
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.GetAllPermissionsForUserAsync(user.Id).Returns(new List<PermissionGrant>());

            var resourceManager = new ResourceManager(adapter, userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(user.Username);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllResources_ForUserWithReadPermissionsOnResource_ReturnsListWithThatResource()
        {
            // Arrange
            var resource = new Resource
            {
                Id = Guid.NewGuid(),
                Value = "someSecret"
            };
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { resource });

            var user = new User
            {
                Id = Guid.NewGuid(),
                Username = "Eric"
            };
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            var grant = new PermissionGrant
            {
                UserId = user.Id,
                ResourceId = resource.Id,
                PermissionType = PermissionType.Read
            };
            permissionGrantManager.GetAllPermissionsForUserAsync(user.Id).Returns(new List<PermissionGrant> { grant });

            var resourceManager = new ResourceManager(adapter, userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(user.Username);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(resource));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllResources_ForUserWithReadPermissionsOnSomeResource_ReturnsListWithOnlyPermittedResource()
        {
            // Arrange
            var permittedResource = new Resource
            {
                Id = Guid.NewGuid(),
                Value = "someSecret"
            };
            var nonPermittedResource = new Resource
            {
                Id = Guid.NewGuid(),
                Value = "superSecret"
            };
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { permittedResource, nonPermittedResource });

            var queriedUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "Faith"
            };
            var otherUser = new User
            {
                Id = Guid.NewGuid(),
                Username = "NotBeingQueried"
            };
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(queriedUser.Username).Returns(UserIdReturned.From(queriedUser.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            var grantToQueriedUser = new PermissionGrant
            {
                UserId = queriedUser.Id,
                ResourceId = permittedResource.Id,
                PermissionType = PermissionType.Read
            };
            var grantToOtherUser = new PermissionGrant
            {
                UserId = otherUser.Id,
                ResourceId = nonPermittedResource.Id,
                PermissionType = PermissionType.Read
            };
            permissionGrantManager.GetAllPermissionsForUserAsync(queriedUser.Id).Returns(new List<PermissionGrant> { grantToQueriedUser, grantToOtherUser });

            var resourceManager = new ResourceManager(adapter, userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(queriedUser.Username);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(permittedResource));
            Assert.IsFalse(result.Contains(nonPermittedResource));
        }

        // TODO - tests for CreateResourceAsync()

        // TODO - tests for UpdateResourceAsync()
    }
}
