using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using ProductMSystem.Controllers;
using ProductMSystem.Data;
using ProductMSystem.Data.Services;
using ProductMSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace TestProductMSystem.TestServices
{
    public class AdminServiceTest
    {
        [Fact]
        public void Add_ReturnsViewResult()
        {
            // Arrange
            var AdminServiceMock = new Mock<IAdminService>(); 
            var controller = new AdminController(AdminServiceMock.Object); 

            // Act
            var result = controller.Add() as ViewResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Delete_Get_ReturnsViewResult_WhenProductExists()
        {
            // Arrange
            var adminServiceMock = new Mock<IAdminService>();
            var controller = new AdminController(adminServiceMock.Object);

            var productId = 1;
            var productFromDb = new Product { ID = productId, /* other properties */ };

            adminServiceMock.Setup(service => service.GetProductById(productId))
                .Returns(productFromDb);

            // Act
            var result = controller.Delete(productId) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Delete", result.ViewName); // Update the view name if necessary
            Assert.NotNull(result.Model); // Ensure the model is not null
           
            Assert.True(productFromDb.Equals(result.Model));
        }


        [Fact]
        public void Delete_Get_ReturnsNotFoundResult_WhenProductNotFound()
        {
            // Arrange
            var adminServiceMock = new Mock<IAdminService>();
            var controller = new AdminController(adminServiceMock.Object);

            var productId = 1;
            adminServiceMock.Setup(service => service.GetProductById(productId))
                .Returns((Product?)null);

            // Act
            var result = controller.Delete(productId) as NotFoundResult;

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void DeleteConfirmed_ReturnsRedirectToActionResult()
        {
            // Arrange
            var adminServiceMock = new Mock<IAdminService>();
            var controller = new AdminController(adminServiceMock.Object);

            var productId = 1;

            // Act
            var result = controller.DeleteConfirmed(productId) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName); // Assuming it redirects to "Index"
        }

       


    }
}
