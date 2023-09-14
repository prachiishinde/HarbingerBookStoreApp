using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using ProductMSystem.Data.Services;
using ProductMSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProductMSystem.TestServices
{
    public class SuperAdminServiceTest
    {

        [Fact]
        public async Task GetAdminUsersAsync_ReturnsListOfAdminUsers()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(), Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var expectedAdminUsers = new List<IdentityUser>
            {
                new IdentityUser { Id = "1", UserName = "admin1@example.com" },
                new IdentityUser { Id = "2", UserName = "admin2@example.com" },
                // Add more admin users as needed
            };

            userManagerMock.Setup(x => x.GetUsersInRoleAsync("Admin"))
                .ReturnsAsync(expectedAdminUsers);

            // Act
            var result = await superAdminService.GetAdminUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<IdentityUser>>(result);

            // Optionally, you can assert the count of admin users or other properties.
            Assert.Equal(expectedAdminUsers.Count, result.Count());
        }

        [Fact]
        public async Task CreateAdminAsync_ValidModel_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var validModel = new AdminRegisterViewModel
            {
                Email = "test@example.com",
                Password = "TestPassword123",
                ConfirmPassword = "TestPassword123"
            };

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), validModel.Password))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await superAdminService.CreateAdminAsync(validModel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task CreateAdminAsync_InvalidModel_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var invalidModel = new AdminRegisterViewModel
            {
                Email = "test@example.com", // Missing password confirmation
                Password = "TestPassword123"
            };

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), invalidModel.Password))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = "Password mismatch" }));

            // Act
            var result = await superAdminService.CreateAdminAsync(invalidModel);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetAdminUserByIdAsync_UserExists_ReturnsUser()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "testUserId";
            var user = new IdentityUser { Id = userId };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await superAdminService.GetAdminUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task GetAdminUserByIdAsync_UserNotFound_ReturnsNull()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "nonExistentUserId";

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await superAdminService.GetAdminUserByIdAsync(userId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateAdminAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "testUserId";
            var user = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com" // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.UpdateAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await superAdminService.UpdateAdminAsync(user);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateAdminAsync_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "nonExistentUserId";
            var user = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com" // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await superAdminService.UpdateAdminAsync(user);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteAdminAsync_UserExists_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "testUserId";
            var user = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com" // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await superAdminService.DeleteAdminAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteAdminAsync_UserNotFound_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "nonExistentUserId";
            var user = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com" // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await superAdminService.DeleteAdminAsync(userId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task GetUsersAsync_ReturnsUsersInRole()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var usersInRole = new List<IdentityUser>
            {
                new IdentityUser { Id = "user1", Email = "user1@example.com", UserName = "user1@example.com" },
                new IdentityUser { Id = "user2", Email = "user2@example.com", UserName = "user2@example.com" }
            };

            userManagerMock.Setup(x => x.GetUsersInRoleAsync("User"))
                .ReturnsAsync(usersInRole);

            // Act
            var result = await superAdminService.GetUsersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            Assert.Collection(result,
                user => Assert.Equal("user1@example.com", user.Email),
                user => Assert.Equal("user2@example.com", user.Email)
            );
        }

        [Fact]
        public async Task CreateUserAsync_ValidModel_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var createUserViewModel = new CreateUserViewModel
            {
                Email = "newuser@example.com",
                Password = "Password123" // Adjust the password as needed
            };

            userManagerMock.Setup(x => x.CreateAsync(It.IsAny<IdentityUser>(), createUserViewModel.Password))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.AddToRoleAsync(It.IsAny<IdentityUser>(), "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await superAdminService.CreateUserAsync(createUserViewModel);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetUserByIdAsync_ValidId_ReturnsUser()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "validUserId"; // Replace with a valid user ID
            var user = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com", // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(user);

            // Act
            var result = await superAdminService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.Id);
        }

        [Fact]
        public async Task GetUserByIdAsync_InvalidId_ReturnsNull()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var invalidUserId = "invalidUserId"; // Replace with an invalid user ID

            userManagerMock.Setup(x => x.FindByIdAsync(invalidUserId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await superAdminService.GetUserByIdAsync(invalidUserId);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task UpdateUserAsync_ValidUser_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "validUserId"; // Replace with a valid user ID
            var existingUser = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com", // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(existingUser);

            userManagerMock.Setup(x => x.UpdateAsync(existingUser))
                .ReturnsAsync(IdentityResult.Success);

            var updatedUser = new IdentityUser
            {
                Id = userId,
                Email = "updated@example.com", // Replace with updated user details
            };

            // Act
            var result = await superAdminService.UpdateUserAsync(updatedUser);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task UpdateUserAsync_InvalidUser_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var invalidUserId = "invalidUserId"; // Replace with an invalid user ID

            userManagerMock.Setup(x => x.FindByIdAsync(invalidUserId))
                .ReturnsAsync((IdentityUser?)null);

            var updatedUser = new IdentityUser
            {
                Id = invalidUserId,
                Email = "updated@example.com", // Replace with updated user details
            };

            // Act
            var result = await superAdminService.UpdateUserAsync(updatedUser);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task DeleteUserAsync_ValidUser_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "validUserId"; // Replace with a valid user ID
            var existingUser = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com", // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(existingUser);

            userManagerMock.Setup(x => x.DeleteAsync(existingUser))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await superAdminService.DeleteUserAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task DeleteUserAsync_InvalidUser_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var invalidUserId = "invalidUserId"; // Replace with an invalid user ID

            userManagerMock.Setup(x => x.FindByIdAsync(invalidUserId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await superAdminService.DeleteUserAsync(invalidUserId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PromoteUserToAdminAsync_ValidUser_ReturnsTrue()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "validUserId"; // Replace with a valid user ID
            var existingUser = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com", // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(existingUser);

            userManagerMock.Setup(x => x.AddToRoleAsync(existingUser, "Admin"))
                .ReturnsAsync(IdentityResult.Success);

            userManagerMock.Setup(x => x.RemoveFromRoleAsync(existingUser, "User"))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await superAdminService.PromoteUserToAdminAsync(userId);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task PromoteUserToAdminAsync_InvalidUser_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var invalidUserId = "invalidUserId"; // Replace with an invalid user ID

            userManagerMock.Setup(x => x.FindByIdAsync(invalidUserId))
                .ReturnsAsync((IdentityUser?)null);

            // Act
            var result = await superAdminService.PromoteUserToAdminAsync(invalidUserId);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task PromoteUserToAdminAsync_AddToRoleFailed_ReturnsFalse()
        {
            // Arrange
            var userManagerMock = new Mock<UserManager<IdentityUser>>(
                Mock.Of<IUserStore<IdentityUser>>(),
                Mock.Of<IOptions<IdentityOptions>>(),
                Mock.Of<IPasswordHasher<IdentityUser>>(),
                It.IsAny<IEnumerable<IUserValidator<IdentityUser>>>(),
                It.IsAny<IEnumerable<IPasswordValidator<IdentityUser>>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                It.IsAny<IServiceProvider>(),
                Mock.Of<ILogger<UserManager<IdentityUser>>>());

            var superAdminService = new SuperAdminService(userManagerMock.Object);

            var userId = "validUserId"; // Replace with a valid user ID
            var existingUser = new IdentityUser
            {
                Id = userId,
                Email = "user@example.com", // Replace with user details as needed
            };

            userManagerMock.Setup(x => x.FindByIdAsync(userId))
                .ReturnsAsync(existingUser);

            userManagerMock.Setup(x => x.AddToRoleAsync(existingUser, "Admin"))
                .ReturnsAsync(IdentityResult.Failed()); // Simulate a failure

            // Act
            var result = await superAdminService.PromoteUserToAdminAsync(userId);

            // Assert
            Assert.False(result);
        }
    }
}
