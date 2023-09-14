using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductMSystem.Controllers;
using ProductMSystem.Data.Services;
using ProductMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProductMSystem
{
    public class UserControllerTest
    {
        [Fact]
        public void View_ValidId_ReturnsViewResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var controller = new UserController(userServiceMock.Object);
            var productId = 1; // Replace with a valid product ID
            var product = new Product { ID = productId, Title = "Test Product" };

            userServiceMock.Setup(service => service.GetProduct(productId)).Returns(product);

            // Act
            var result = controller.View(productId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(product, result.Model); // Assuming your view's model is a Product
        }

        [Fact]
        public void View_InvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var controller = new UserController(userServiceMock.Object);
            int? invalidProductId = null; // Invalid product ID

            // Act
            var result = controller.View(invalidProductId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void ViewProduct_ReturnsViewResultWithProductList()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var controller = new UserController(userServiceMock.Object);
            var productList = new List<Product>
            {
                    new Product { ID = 1, Title = "Product 1" },
                    new Product { ID = 2, Title = "Product 2" },
                    // Add more sample products as needed
            };

            userServiceMock.Setup(service => service.GetAllProducts()).Returns(productList);

            // Act
            var result = controller.ViewProduct() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(productList, result.Model); // Assuming your view's model is a list of Product
        }

        [Fact]
        public void ViewProduct_ReturnsViewResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            // Set up userServiceMock to return a list of products for GetAllProducts

            var controller = new UserController(userServiceMock.Object);

            // Act
            var result = controller.ViewProduct();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void ViewProduct_PassesCorrectModelToView()
        {
            // Arrange
            var expectedModel = new List<Product>
            {
                new Product { ID = 1, Title = "Product 1" },
                new Product { ID = 2, Title = "Product 2" }
            };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(service => service.GetAllProducts()).Returns(expectedModel);

            var controller = new UserController(userServiceMock.Object);

            // Act
            var result = controller.ViewProduct() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<List<Product>>(result.Model);
            var model = result.Model as List<Product>;
            Assert.Equal(expectedModel.Count, model.Count);
            // Add additional assertions to verify the correctness of the model data
        }

        [Fact]
        public void UserDashboard_ReturnsViewResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>(); // Mock the service interface
            var controller = new UserController(userServiceMock.Object); // Inject the mock service

            // Act
            var result = controller.UserDashboard();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>(); // Mock the service interface
            var controller = new UserController(userServiceMock.Object); // Inject the mock service

            // Act
            var result = controller.Index();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
