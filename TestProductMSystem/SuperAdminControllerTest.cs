using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductMSystem.Controllers;
using ProductMSystem.Data.Services;
using ProductMSystem.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProductMSystem
{
    public class SuperAdminControllerTest
    {


        [Fact]
        public async Task SuperAdminDashboard_ReturnsViewWithAdminUsers()
        {
            // Arrange
            var adminUsers = new List<IdentityUser>
            {
                new IdentityUser { UserName = "admin1" },
                new IdentityUser { UserName = "admin2" }
            };

            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.GetAdminUsersAsync())
                .ReturnsAsync(adminUsers);

            var controller = new SuperAdminController(superAdminServiceMock.Object);

            // Act
            var result = await controller.SuperAdminDashboard() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(adminUsers, result.Model as List<IdentityUser>);
            Assert.Equal("SuperAdminDashboard", result.ViewName);
        }

        [Fact]
        public async Task EditAdmin_ValidId_ReturnsViewResult()
        {
            // Arrange
            var userId = "validUserId"; // Replace with a valid user ID
            var user = new IdentityUser
            {
                Id = userId,
                Email = "test@example.com" // Replace with appropriate values
            };

            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.GetAdminUserByIdAsync(userId))
                .ReturnsAsync(user);

            var controller = new SuperAdminController(superAdminServiceMock.Object);

            // Act
            var result = await controller.EditAdmin(userId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("EditAdmin", result.ViewName); // Ensure it returns the correct view
        }

        [Fact]
        public async Task DeleteAdmin_ValidId_ReturnsViewResultWithViewName()
        {
            // Arrange
            var userId = "validUserId"; // Replace with a valid user ID
            var user = new IdentityUser { Id = userId }; // Create a sample user

            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.GetAdminUserByIdAsync(userId))
                .ReturnsAsync(user);

            var controller = new SuperAdminController(superAdminServiceMock.Object);

            // Act
            var result = await controller.DeleteAdmin(userId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("DeleteAdmin", result.ViewName); // Check that the view name is explicitly set
                                                          // You can also add more assertions as needed
        }

        [Fact]
        public async Task UserDashboard_ReturnsViewResultWithViewName()
        {
            // Arrange
            var userList = new List<IdentityUser>
            {
                new IdentityUser { Id = "user1", Email = "user1@example.com" },
                new IdentityUser { Id = "user2", Email = "user2@example.com" }
            }; // Create a sample list of users

            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.GetUsersAsync())
                .ReturnsAsync(userList);

            var controller = new SuperAdminController(superAdminServiceMock.Object);

            // Act
            var result = await controller.UserDashboard() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ViewName); // Check that the view name is explicitly set
                                                            // You can also add more assertions as needed
        }

        [Fact]
        public void CreateUser_ReturnsViewResult()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>(); // Create a mock of your service interface
            var controller = new SuperAdminController(superAdminServiceMock.Object); // Inject the mock into the controller

            // Act
            var result = controller.CreateUser() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public void CreateUser_InvalidModelState_ReturnsViewResult()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(superAdminServiceMock.Object);
            controller.ModelState.AddModelError("someField", "Invalid model state"); // Simulate invalid model state

            // Act
            var result = controller.CreateUser() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }


        [Fact]
        public async Task EditUser_ValidId_ReturnsViewResult()
        {
            // Arrange
            var userId = "validUserId"; // Replace with a valid user ID
            var userServiceMock = new Mock<ISuperAdminService>();
            userServiceMock.Setup(service => service.GetUserByIdAsync(userId))
                .ReturnsAsync(new IdentityUser { Id = userId, Email = "test@example.com" });

            var controller = new SuperAdminController(userServiceMock.Object);

            // Act
            var result = await controller.EditUser(userId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public async Task EditUser_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var invalidUserId = "invalidUserId"; // Replace with an invalid user ID
            var userServiceMock = new Mock<ISuperAdminService>();
            userServiceMock.Setup(service => service.GetUserByIdAsync(invalidUserId))
                .ReturnsAsync((IdentityUser?)null); // Simulate a null user

            var controller = new SuperAdminController(userServiceMock.Object);

            // Act
            var result = await controller.EditUser(invalidUserId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void TestCreateAdminAction()
        {
            var mockSuperAdminService = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(mockSuperAdminService.Object);

            // Act
            var result = controller.CreateAdmin() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void TestDashboardAction()
        {
            var mockSuperAdminService = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(mockSuperAdminService.Object);

            // Act
            var result = controller.MainDashboard() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public async Task DeleteUser_ReturnsNotFound_WhenUserNotFound()
        {
            // Arrange
            // Arrange
            var mockSuperAdminService = new Mock<ISuperAdminService>();
            var controller = new SuperAdminController(mockSuperAdminService.Object);

            // Act
            var result = await controller.DeleteUser("NonExistentUserId");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        //Create Valid Admin
        [Fact]
        public async Task CreateAdmin_ValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.CreateAdminAsync(It.IsAny<AdminRegisterViewModel>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validModel = new AdminRegisterViewModel();

            // Act
            var result = await controller.CreateAdmin(validModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);
        }

        //Create Invalid Admin
        [Fact]
        public async Task CreateAdmin_InvalidModel_ReturnsViewWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.CreateAdminAsync(It.IsAny<AdminRegisterViewModel>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidModel = new AdminRegisterViewModel();

            // Act
            var result = await controller.CreateAdmin(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
        }

        //Edit valid admin
        [Fact]
        public async Task EditAdmin_ValidUser_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.UpdateAdminAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validUser = new IdentityUser(); // Replace with a valid IdentityUser

            // Act
            var result = await controller.EditAdmin(validUser) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);
        }

        //Edit admin invalid
        [Fact]
        public async Task EditAdmin_InvalidUser_ReturnsViewWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.UpdateAdminAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidUser = new IdentityUser(); // Replace with an invalid IdentityUser

            // Act
            var result = await controller.EditAdmin(invalidUser) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Single(controller.ModelState.Keys);
        }

        //Delete admin Valid
        [Fact]
        public async Task DeleteAdminConfirmed_ValidAdmin_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.DeleteAdminAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validAdminId = "validId"; // Replace with a valid admin ID

            // Act
            var result = await controller.DeleteAdminConfirmed(validAdminId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);
        }

        //Delete admin invalid
        [Fact]
        public async Task DeleteAdminConfirmed_InvalidAdmin_ReturnsViewWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.DeleteAdminAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidAdminId = "invalidId"; // Replace with an invalid admin ID

            // Act
            var result = await controller.DeleteAdminConfirmed(invalidAdminId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Single(controller.ModelState.Keys);
        }

        //Create valid user
        [Fact]
        public async Task CreateUser_ValidUser_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<CreateUserViewModel>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validUserModel = new CreateUserViewModel(); // Replace with a valid user model

            // Act
            var result = await controller.CreateUser(validUserModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
        }

        //Create invalid user 
        [Fact]
        public async Task CreateUser_InvalidUser_ReturnsViewWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<CreateUserViewModel>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidUserModel = new CreateUserViewModel(); // Replace with an invalid user model

            // Act
            var result = await controller.CreateUser(invalidUserModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Single(controller.ModelState.Keys);
        }

        //Edit valid user
        [Fact]
        public async Task EditUser_ValidModel_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.UpdateUserAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validModel = new IdentityUser();

            // Act
            var result = await controller.EditUser(validModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
        }

        //Edit Invalid user
        [Fact]
        public async Task EditUser_InvalidModel_ReturnsViewWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.UpdateUserAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidModel = new IdentityUser();

            // Act
            var result = await controller.EditUser(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
        }

        //Delete user valid
        [Fact]
        public async Task DeleteUserConfirmed_ValidId_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validId = "123"; // Replace with a valid user id

            // Act
            var result = await controller.DeleteUserConfirmed(validId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
        }

        //Delete user invalid
        [Fact]
        public async Task DeleteUserConfirmed_InvalidId_ReturnsViewWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidId = "invalidId"; // Replace with an invalid user id

            // Act
            var result = await controller.DeleteUserConfirmed(invalidId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);


        }

        //User to admin valid
        [Fact]
        public async Task PromoteToAdmin_ValidUserId_ReturnsRedirectToAction()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.PromoteUserToAdminAsync(It.IsAny<string>()))
                .ReturnsAsync(true);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var validUserId = "123"; // Replace with a valid user id

            // Act
            var result = await controller.PromoteToAdmin(validUserId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
        }

        //user to admin invalid
        [Fact]
        public async Task PromoteToAdmin_InvalidUserId_ReturnsRedirectToActionWithModelError()
        {
            // Arrange
            var superAdminServiceMock = new Mock<ISuperAdminService>();
            superAdminServiceMock.Setup(service => service.PromoteUserToAdminAsync(It.IsAny<string>()))
                .ReturnsAsync(false);

            var controller = new SuperAdminController(superAdminServiceMock.Object);
            var invalidUserId = "invalidUserId"; // Replace with an invalid user id

            // Act
            var result = await controller.PromoteToAdmin(invalidUserId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("SuperAdminDashboard", result.ActionName);
            Assert.False(controller.ModelState.IsValid);
            Assert.Single(controller.ModelState.Keys);
        }
    }
}