using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductMSystem.Controllers;
using ProductMSystem.Data;
using ProductMSystem.Data.Services;
using ProductMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProductMSystem
{
    public class AdminControllerTest
    {
        [Fact]
        public void TestIndexAction()
        {
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }


        [Fact]
        public void TestAdminAction()
        {
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);

            // Act
            var result = controller.AdminDashboard() as ViewResult;

            // Assert
            Assert.NotNull(result);

        }

        //Add valid Product
        [Fact]   
        public void Add_Valid_Product()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var product = new Product
            {
                Title = "Sample Book",
                Author = "John Doe",
                Year = 2022,
                Description = "Description",
                ISBN = 1234567890
                // Set other properties as needed
            };

            // Simulate a successful product addition
            mockAdminService.Setup(service => service.AddProduct(It.IsAny<Product>()))
                .Verifiable();

            // Act
            var result = controller.Add(product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            mockAdminService.Verify(); // Verify that the AddProduct method was called
        }

        //Add invalid Product
        [Fact]
        public Task Add_Invalid_Product()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var product = new Product
            {
                Title = string.Empty, // Sample invalid model state
                Author = "John Doe", // Valid value
                Year = 2022, // Valid value
                ISBN = 1234567890 // Valid value
            };

            // Simulate an ArgumentException when adding the product
            mockAdminService.Setup(service => service.AddProduct(product))
                .Throws<ArgumentException>();

            // Act
            var result = controller.Add(product) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains(controller.ModelState.Values, v => v.Errors.Count > 0);
            Assert.Equal("Title", result.ViewData.ModelState.Keys.First()); // Assuming "Title" is the property with an error
            return Task.CompletedTask;
        }

        [Fact]
        public void Edit_IdIsZero_ReturnsNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IAdminService>();
            var controller = new AdminController(productServiceMock.Object);

            // Act
            var result = controller.Edit(0);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_ProductNotFound_ReturnsNotFound()
        {
            // Arrange
            var productServiceMock = new Mock<IAdminService>();
            productServiceMock.Setup(service => service.GetProductById(It.IsAny<int>())).Returns((Product?)null);
            var controller = new AdminController(productServiceMock.Object);

            // Act
            var result = controller.Edit(1); // Assuming 1 is a valid ID that won't return a product

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Edit_ProductFound_ReturnsView()
        {
            // Arrange
            var product = new Product { /* Initialize with required properties */ };
            var productServiceMock = new Mock<IAdminService>();
            productServiceMock.Setup(service => service.GetProductById(It.IsAny<int>())).Returns(product);
            var controller = new AdminController(productServiceMock.Object);

            // Act
            var result = controller.Edit(1); // Assuming 1 is a valid ID that returns a product

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        //Edit valid product
        [Fact]
        public void Edit_Valid_Product()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var product = new Product
            {
                ID = 1, // Set a valid ID for editing
                Title = "Sample Book",
                Author = "John Doe",
                Year = 2022,
                Description = "Description",
                ISBN = 1234567890
                // Set other properties as needed
            };

            // Simulate a successful product update
            mockAdminService.Setup(service => service.UpdateProduct(It.IsAny<Product>()))
                .Verifiable();

            // Act
            var result = controller.Edit(product) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            mockAdminService.Verify(); // Verify that the UpdateProduct method was called
        }

        //Edit Invalid product
        [Fact]
        public void Edit_Invalid_Product()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var product = new Product
            {
                ID = 1, // Set a valid ID for editing
                Title = string.Empty, // Sample invalid model state
                Author = "John Doe", // Valid value
                Year = 2022, // Valid value
                ISBN = 1234567890 // Valid value
            };

            // Simulate an ArgumentException when updating the product
            mockAdminService.Setup(service => service.UpdateProduct(product))
                .Throws<ArgumentException>();

            // Act
            var result = controller.Edit(product) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.False(controller.ModelState.IsValid);
            Assert.Contains(controller.ModelState.Values, v => v.Errors.Count > 0);
            Assert.Equal("Title", result.ViewData.ModelState.Keys.First()); // Assuming "Title" is the property with an error
        }

        //Delete Valid Product
        [Fact]
        public void DeleteConfirmed_Valid_Product_RedirectsToIndex()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var productId = 1; // Set a valid product ID for deletion

            // Simulate a successful product deletion
            mockAdminService.Setup(service => service.DeleteProduct(productId))
                .Verifiable();

            // Act
            var result = controller.DeleteConfirmed(productId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            mockAdminService.Verify(); // Verify that the DeleteProduct method was called
        }

        //Delete Invalid Product
        [Fact]
        public void DeleteConfirmed_Invalid_Product_ReturnsNotFound()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var productId = 0; // Set an invalid product ID for deletion

            // Simulate NotFound result when trying to delete an invalid product
            mockAdminService.Setup(service => service.DeleteProduct(productId))
                .Returns(true);

            // Act
            var result = controller.DeleteConfirmed(productId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_InvalidId_ReturnsNotFound()
        {
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            // Arrange

            // Act
            var result = controller.Delete(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_NonExistingProductId_ReturnsNotFound()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var invalidProductId = 1; // Replace with an ID that doesn't exist in your data source

            // Act
            var result = controller.Delete(invalidProductId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_ExistingProductId_ReturnsViewResult()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var existingProductId = 123; // Replace with an existing product ID in your data source

            // Set up the mock to return a product when GetProductById is called
            mockAdminService.Setup(service => service.GetProductById(existingProductId))
                .Returns(new Product()); // Replace with an actual Product instance

            // Act
            var result = controller.Delete(existingProductId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void View_ExistingProductId_ReturnsViewResult()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var existingProductId = 123; // Replace with an existing product ID in your data source

            // Set up the mock to return a product when GetProductById is called
            mockAdminService.Setup(service => service.GetProductById(existingProductId))
                .Returns(new Product()); // Replace with an actual Product instance

            // Act
            var result = controller.View(existingProductId) as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void View_NonExistingProductId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);
            var nonExistingProductId = 456; // Replace with a non-existing product ID

            // Set up the mock to return null when GetProductById is called
            mockAdminService.Setup(service => service.GetProductById(nonExistingProductId))
                .Returns((Product?)null);

            // Act
            var result = controller.View(nonExistingProductId);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void View_NullProductId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockAdminService = new Mock<IAdminService>();
            var controller = new AdminController(mockAdminService.Object);

            // Act
            var result = controller.View(null) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }


    }
}
