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
            userManager.GetIdForUsername(Arg.Any<Username>()).Returns(new UsernameDoesNotExist());

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.GetResourceAsync(ResourceId.From(Guid.NewGuid()), Username.From("someUser"));

            // Assert
            Assert.IsNull(result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetResource_ForUserWithoutReadPermission_ReturnsNull()
        {
            // Arrange
            var resourceId = ResourceId.From(Guid.NewGuid());

            // adapter needs to return something non-null so our assert can check the difference vs. failing to have permission
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetResourceAsync(Arg.Any<ResourceId>()).Returns(new Resource());

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Carl"), new HashedPassword());
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
            var resource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));

            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetResourceAsync(resource.Id).Returns(resource);

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Carl"), new HashedPassword());
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
            userManager.GetIdForUsername(Arg.Any<Username>()).Returns(new UsernameDoesNotExist());

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.GetAllResourcesAsync(Username.From("Diana"));

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllResources_ForUserWithNoPermissions_ReturnsEmptyList()
        {
            // Arrange
            var resource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { resource });

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Eric"), new HashedPassword());
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
            var resource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { resource });

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Eric"), new HashedPassword());
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            var grant = new PermissionGrant(PermissionGrantId.From(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Read);
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
            var permittedResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var nonPermittedResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("superSecret"));
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { permittedResource, nonPermittedResource });

            var queriedUser = new User(UserId.From(Guid.NewGuid()), Username.From("Faith"), new HashedPassword());
            var otherUser = new User(UserId.From(Guid.NewGuid()), Username.From("NotBeingQueried"), new HashedPassword());
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(queriedUser.Username).Returns(UserIdReturned.From(queriedUser.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            var grantToQueriedUser = new PermissionGrant(PermissionGrantId.From(Guid.NewGuid()), queriedUser.Id, permittedResource.Id, PermissionType.Read);
            var grantToOtherUser = new PermissionGrant(PermissionGrantId.From(Guid.NewGuid()), otherUser.Id, nonPermittedResource.Id, PermissionType.Read);
            permissionGrantManager.GetAllPermissionsForUserAsync(queriedUser.Id).Returns(new List<PermissionGrant> { grantToQueriedUser, grantToOtherUser });

            var resourceManager = new ResourceManager(adapter, userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(queriedUser.Username);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(permittedResource));
            Assert.IsFalse(result.Contains(nonPermittedResource));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForNonexistentUser_ReturnsError()
        {
            // Arrange
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(Arg.Any<Username>()).Returns(new UsernameDoesNotExist());

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someValue"), Username.From("someUser"));

            // Assert
            result.Switch(
                creatingUserDoesNotExist => { },
                resourceCreated => throw new Exception("Test failed")
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_ReturnsResourceId()
        {
            // Arrange
            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Gordon"), new HashedPassword());
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Username);

            // Assert
            result.Switch(
                creatingUserDoesNotExist => throw new Exception("Test failed"),
                resourceCreated =>
                {
                    Assert.IsInstanceOfType(resourceCreated.Value, typeof(ResourceId));
                }
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_CallsAdapterCreateResource()
        {
            // Arrange
            var adapter = Substitute.For<IResourceAdapter>();

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Hailey"), new HashedPassword());
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var resourceManager = new ResourceManager(adapter, userManager, Substitute.For<IPermissionGrantManager>());

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Username);

            // Assert
            result.Switch(
                creatingUserDoesNotExist => throw new Exception("Test failed"),
                resourceCreated =>
                {
                    adapter.Received().CreateResourceAsync(Arg.Any<Resource>());
                }
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_GivesUserReadPermissionsOnCreatedResource()
        {
            // Arrange
            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Gordon"), new HashedPassword());
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Username);

            // Assert
            result.Switch(
                creatingUserDoesNotExist => throw new Exception("Test failed"),
                resourceCreated =>
                {
                    permissionGrantManager.Received().CreatePermissionGrantAsync(user.Id, resourceCreated.Value, PermissionType.Read);
                }
            );
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_GivesUserWritePermissionsOnCreatedResource()
        {
            // Arrange
            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Gordon"), new HashedPassword());
            var userManager = Substitute.For<IUserManager>();
            userManager.GetIdForUsername(user.Username).Returns(UserIdReturned.From(user.Id));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), userManager, permissionGrantManager);

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Username);

            // Assert
            result.Switch(
                creatingUserDoesNotExist => throw new Exception("Test failed"),
                resourceCreated =>
                {
                    permissionGrantManager.Received().CreatePermissionGrantAsync(user.Id, resourceCreated.Value, PermissionType.Write);
                }
            );
        }

        // TODO - tests for UpdateResourceAsync()
    }
}
