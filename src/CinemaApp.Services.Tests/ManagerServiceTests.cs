namespace CinemaApp.Services.Tests
{
    using Core;
    using Core.Contracts;
    using Data.Models;
    using Data.Repository.Contracts;

    using Moq;

    [TestFixture]
    public class ManagerServiceTests
    {
        private IManagerService managerService;

        [SetUp]
        public void Setup()
        {
            // Arrange
            Guid managerGuid = Guid.Parse("001994e4-75a5-4717-88f9-95cf0009fbf2");
            Guid regUserGuid = Guid.Parse("63c48299-b002-496e-8ce2-ec5bc27e6b25");

            Mock<IManagerRepository> managerRepositoryMock
                = new Mock<IManagerRepository>();
            managerRepositoryMock
                .Setup(mr => mr.GetManagerByUserIdAsync(managerGuid))
                .ReturnsAsync(new Manager()
                {
                    Id = Guid.Parse("17845c0e-4e0d-4c2e-98de-a50759c70ef5"),
                    Level = 1,
                    UserId = managerGuid
                });
            managerRepositoryMock
                .Setup(mr => mr.GetManagerByUserIdAsync(regUserGuid))
                .ReturnsAsync((Manager?)null);

            managerService = new ManagerService(managerRepositoryMock.Object);
        }

        /// <summary>
        /// Tests the IsUserManagerAsync method of the ManagerService class when the user is a manager. It verifies that the method returns true for a valid manager user ID.
        /// </summary>
        /// <tests>RS-ISUM-003</tests>
        /// <returns></returns>
        [Test]
        public async Task IsUserManagerAsync_UserIsManager_ReturnsTrue()
        {
            // Arrange
            string userId = "001994e4-75a5-4717-88f9-95cf0009fbf2";

            // Act
            bool result = await managerService
                .IsUserManagerAsync(userId);

            // Assert
            Assert.IsTrue(result);
        }

        /// <summary>
        /// Tests the IsUserManagerAsync method of the ManagerService class when the user is not a manager. It verifies that the method returns false for a valid user ID that does not belong to a manager.
        /// </summary>
        /// <tests>RS-ISUM-003</tests>
        /// <returns></returns>
        [Test]
        public async Task IsUserManagerAsync_UserIsNotManager_ReturnsFalse()
        {
            // Arrange
            string userId = "63c48299-b002-496e-8ce2-ec5bc27e6b25";

            // Act
            bool result = await managerService
                .IsUserManagerAsync(userId);

            // Assert
            Assert.IsFalse(result);
        }

        /// <summary>
        /// Tests the IsUserManagerAsync method of the ManagerService class when the user ID is invalid. It verifies that the method returns false for an invalid user ID that cannot be parsed as a GUID.
        /// </summary>
        /// <tests>RS-ISUM-001</tests>
        /// <returns></returns>
        [Test]
        public async Task IsUserManagerAsync_InvalidGuid_ReturnsFalse()
        {
            // Arrange
            string userId = "Invalid guid!";

            // Act
            bool result = await managerService
                .IsUserManagerAsync(userId);

            // Assert
            Assert.IsFalse(result);
        }
    }
}
