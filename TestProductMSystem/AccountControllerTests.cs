using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Xunit;
using ProductMSystem.Controllers;
using ProductMSystem.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using ProductMSystem.Data;
using ProductMSystem.Models.ViewModel;
using Moq;
using ProductMSystem.Data.Services;

namespace AccountControllerTests
{
    public class AccountControllerTest1
    {
        [Fact]
        public void TestLoginAction()
        {
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            // Act
            var result = controller.Login() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }

        [Fact]
        public void TestRegisterAction()
        {
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            // Act
            var result = controller.Register() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }

        //Valid Login 

        [Fact]
        public async Task Login_ValidModel_RedirectsToDashboard()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            var user = new IdentityUser { UserName = "test@example.com" };
            var loginModel = new LoginViewModel
            {
                Email = "test@example.com",
                Password = "password",
                RememberMe = false
            };

            // Simulate a successful login
            mockAccountService.Setup(service => service.LoginAsync(loginModel.Email, loginModel))
                .ReturnsAsync((true, new List<string> { "User" }));

            // Simulate getting user by email
            mockAccountService.Setup(service => service.GetUserByEmailAsync(loginModel.Email))
                .ReturnsAsync(user);

            // Act
            var result = await controller.Login(loginModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("UserDashboard", result.ActionName);
            Assert.Equal("User", result.ControllerName);
        }

        //Invalid Login
        [Fact]
        public async Task Login_InvalidCredentials_ReturnsViewWithModelError()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            var loginModel = new LoginViewModel
            {
                Email = "invalid@example.com",
                Password = "invalidpassword",
                RememberMe = false
            };

            // Simulate a failed login
            mockAccountService.Setup(service => service.LoginAsync(loginModel.Email, loginModel))
                .ReturnsAsync((false, new List<string>()));

            // Act
            var result = await controller.Login(loginModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains("Please enter valid details", controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }

        //Valid Registration
        [Fact]
        public async Task Register_ValidModel_RedirectsToLogin()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            var registerModel = new RegisterViewModel
            {
                Name = "Test User",
                Email = "test@example.com",
                Password = "password",
                ConfirmPassword = "password"
            };

            // Simulate a successful registration
            mockAccountService.Setup(service => service.RegisterAsync(registerModel))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await controller.Register(registerModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName); // Check that it redirects to the "Login" action
        }


        //Invalid Registration
        [Fact]
        public async Task Register_InvalidModel_ReturnsViewWithModelError()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            var registerModel = new RegisterViewModel
            {
                Name = "Test User",
                Email = "invalidemail", // Invalid email format
                Password = "password",
                ConfirmPassword = "password"
            };

            // Simulate a failed registration
            var errorDescription = "Email is not in the correct format.";
            mockAccountService.Setup(service => service.RegisterAsync(registerModel))
                .ReturnsAsync(IdentityResult.Failed(new IdentityError { Description = errorDescription }));

            // Act
            var result = await controller.Register(registerModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains(errorDescription, controller.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
        }


        //Logout
        [Fact]
        public async Task Logout_RedirectsToLogin()
        {
            // Arrange
            var mockAccountService = new Mock<IAccountService>();
            var controller = new AccountController(mockAccountService.Object);

            // Act
            var result = await controller.Logout() as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Login", result.ActionName);
            Assert.Equal("Account", result.ControllerName);
        }

    }
}


