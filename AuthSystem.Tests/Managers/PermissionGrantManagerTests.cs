using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuthSystem.Data;
using AuthSystem.Interfaces.Adapters;
using AuthSystem.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NSubstitute;

namespace AuthSystem.Tests.Managers
{
    [TestClass]
    public class PermissionGrantManagerTests
    {
        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllPermissionsForResource_ForUserWithoutPermissionManagementPermissions_ReturnsEmptyList()
        {
            // Arrange
            var resourceId = new ResourceId(Guid.NewGuid());
            var grant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), new UserId(Guid.NewGuid()),
                resourceId, PermissionType.Read);
            var userId = new UserId(Guid.NewGuid());

            var adapter = Substitute.For<IPermissionGrantAdapter>();
            adapter.GetAllPermissionsForResourceAsync(resourceId).Returns(new List<PermissionGrant> {grant});
            adapter.CheckIfUserHasPermissionAsync(userId, resourceId, PermissionType.ManagePermissions).Returns(false);

            var manager = new PermissionGrantManager(adapter);

            // Act
            var result = await manager.GetAllPermissionsForResourceAsync(userId, resourceId);

            // Assert
            Assert.AreEqual(0, result.Count());
        }

        [TestMethod]
        [TestCategory("UnitTest")]
        public async Task GetAllPermissionsForResource_ForUserWithPermissionManagementPermissions_ReturnsListWithGrants()
        {
            // Arrange
            var resourceId = new ResourceId(Guid.NewGuid());
            var grant = new PermissionGrant(new PermissionGrantId(Guid.NewGuid()), new UserId(Guid.NewGuid()),
                resourceId, PermissionType.Read);
            var userId = new UserId(Guid.NewGuid());

            var adapter = Substitute.For<IPermissionGrantAdapter>();
            adapter.GetAllPermissionsForResourceAsync(resourceId).Returns(new List<PermissionGrant> { grant });
            adapter.CheckIfUserHasPermissionAsync(userId, resourceId, PermissionType.ManagePermissions).Returns(true);

            var manager = new PermissionGrantManager(adapter);

            // Act
            var result = await manager.GetAllPermissionsForResourceAsync(userId, resourceId);

            // Assert
            Assert.IsTrue(result.Contains(grant));
        }
    }
}
