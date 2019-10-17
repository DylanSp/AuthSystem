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
        public async Task GetResource_ForUserWithoutReadPermission_ReturnsNull()
        {
            // Arrange
            var resourceId = ResourceId.From(Guid.NewGuid());

            // adapter needs to return something non-null so our assert can check the difference vs. failing to have permission
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetResourceAsync(Arg.Any<ResourceId>()).Returns(new Resource());

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Carl"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, resourceId, PermissionType.Read).Returns(false);

            var resourceManager = new ResourceManager(adapter, permissionGrantManager);

            // Act
            var result = await resourceManager.GetResourceAsync(resourceId, user.Id);

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

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Carl"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, resource.Id, PermissionType.Read).Returns(true);

            var resourceManager = new ResourceManager(adapter, permissionGrantManager);

            // Act
            var result = await resourceManager.GetResourceAsync(resource.Id, user.Id);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(resource, result.Value);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllResources_ForUserWithNoPermissions_ReturnsEmptyList()
        {
            // Arrange
            var resource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var adapter = Substitute.For<IResourceAdapter>();
            adapter.GetAllResourcesAsync().Returns(new List<Resource> { resource });

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Eric"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.GetAllPermissionsForUserAsync(user.Id).Returns(new List<PermissionGrant>());

            var resourceManager = new ResourceManager(adapter, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(user.Id);

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

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Eric"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            var grant = new PermissionGrant(PermissionGrantId.From(Guid.NewGuid()), user.Id, resource.Id, PermissionType.Read);
            permissionGrantManager.GetAllPermissionsForUserAsync(user.Id).Returns(new List<PermissionGrant> { grant });

            var resourceManager = new ResourceManager(adapter, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(user.Id);

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

            var queriedUser = new User(UserId.From(Guid.NewGuid()), Username.From("Faith"), SaltedHashedPassword.From("someSaltedHash"));
            var otherUser = new User(UserId.From(Guid.NewGuid()), Username.From("NotBeingQueried"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            var grantToQueriedUser = new PermissionGrant(PermissionGrantId.From(Guid.NewGuid()), queriedUser.Id, permittedResource.Id, PermissionType.Read);
            var grantToOtherUser = new PermissionGrant(PermissionGrantId.From(Guid.NewGuid()), otherUser.Id, nonPermittedResource.Id, PermissionType.Read);
            permissionGrantManager.GetAllPermissionsForUserAsync(queriedUser.Id).Returns(new List<PermissionGrant> { grantToQueriedUser, grantToOtherUser });

            var resourceManager = new ResourceManager(adapter, permissionGrantManager);

            // Act
            var result = await resourceManager.GetAllResourcesAsync(queriedUser.Id);

            // Assert
            Assert.AreEqual(1, result.Count());
            Assert.IsTrue(result.Contains(permittedResource));
            Assert.IsFalse(result.Contains(nonPermittedResource));
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_CallsAdapterCreateResource()
        {
            // Arrange
            var adapter = Substitute.For<IResourceAdapter>();

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Hailey"), SaltedHashedPassword.From("someSaltedHash"));

            var resourceManager = new ResourceManager(adapter, Substitute.For<IPermissionGrantManager>());

            // Act
            await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Id);

            // Assert
            await adapter.Received().CreateResourceAsync(Arg.Any<Resource>());
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_GivesUserReadPermissionsOnCreatedResource()
        {
            // Arrange
            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Iago"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), permissionGrantManager);

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Id);

            // Assert
            await permissionGrantManager.Received().CreatePermissionGrantAsync(user.Id, result, PermissionType.Read);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task CreateResource_ForValidUser_GivesUserWritePermissionsOnCreatedResource()
        {
            // Arrange
            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Jane"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), permissionGrantManager);

            // Act
            var result = await resourceManager.CreateResourceAsync(ResourceValue.From("someSecret"), user.Id);

            // Assert
            await permissionGrantManager.Received().CreatePermissionGrantAsync(user.Id, result, PermissionType.Write);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdateResource_ForExistingUserWithoutPermission_ReturnsNotPermitted()
        {
            // Arrange
            var oldResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var newResource = new Resource(oldResource.Id, ResourceValue.From("someNewSecret"));

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Lawrence"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, oldResource.Id, PermissionType.Write).Returns(false);

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), permissionGrantManager);

            // Act
            var result = await resourceManager.UpdateResourceAsync(newResource, user.Id);

            // Assert
            Assert.AreEqual(UpdateResourceResult.UserDoesNotHavePermission, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdateResource_ForExistingUserWithPermission_ReturnsSuccess()
        {
            // Arrange
            var oldResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var newResource = new Resource(oldResource.Id, ResourceValue.From("someNewSecret"));

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Matthew"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, oldResource.Id, PermissionType.Write).Returns(true);

            var resourceManager = new ResourceManager(Substitute.For<IResourceAdapter>(), permissionGrantManager);

            // Act
            var result = await resourceManager.UpdateResourceAsync(newResource, user.Id);

            // Assert
            Assert.AreEqual(UpdateResourceResult.ResourceUpdated, result);
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task UpdateResource_ForExistingUserWithPermission_CallsAdapterUpdateResource()
        {
            // Arrange
            var oldResource = new Resource(ResourceId.From(Guid.NewGuid()), ResourceValue.From("someSecret"));
            var newResource = new Resource(oldResource.Id, ResourceValue.From("someNewSecret"));

            var adapter = Substitute.For<IResourceAdapter>();

            var user = new User(UserId.From(Guid.NewGuid()), Username.From("Matthew"), SaltedHashedPassword.From("someSaltedHash"));

            var permissionGrantManager = Substitute.For<IPermissionGrantManager>();
            permissionGrantManager.CheckIfUserHasPermissionAsync(user.Id, oldResource.Id, PermissionType.Write).Returns(true);

            var resourceManager = new ResourceManager(adapter, permissionGrantManager);

            // Act
            await resourceManager.UpdateResourceAsync(newResource, user.Id);

            // Assert
            await adapter.Received().UpdateResourceAsync(newResource);
        }
    }
}
