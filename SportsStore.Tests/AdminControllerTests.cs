using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using Xunit;

namespace SportsStore.Tests
{
    public class AdminControllerTests
    {
        [Fact]
        public void Index_Contains_All_Products()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1" },
                new Product { ProductId = 2, Name = "P2" },
                new Product { ProductId = 3, Name = "P3" },
            }.AsQueryable());

            // Arrange - create a controller
            var target = new AdminController(mock.Object);

            // Action
            var result = GetViewModel<IEnumerable<Product>>(target.Index())?.ToArray();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Length);
            Assert.Equal("P1", result[0].Name);
            Assert.Equal("P2", result[1].Name);
            Assert.Equal("P3", result[2].Name);
        }

        private static T GetViewModel<T>(IActionResult result) where T : class
        {
            return (result as ViewResult)?.ViewData.Model as T;
        }

        [Fact]
        public void Can_Edit_Product()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
            }.AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act
            var p1 = GetViewModel<Product>(target.Edit(1));
            var p2 = GetViewModel<Product>(target.Edit(2));
            var p3 = GetViewModel<Product>(target.Edit(3));

            // Assert
            Assert.Equal(1, p1.ProductId);
            Assert.Equal(2, p2.ProductId);
            Assert.Equal(3, p3.ProductId);
        }

        [Fact]
        public void Cannot_Edit_Nonexistent_Product()
        {
            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new[] {
                new Product {ProductId = 1, Name = "P1"},
                new Product {ProductId = 2, Name = "P2"},
                new Product {ProductId = 3, Name = "P3"},
            }.AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act
            var result = GetViewModel<Product>(target.Edit(4));

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public void Can_Save_Valid_Changes()
        {
            // Arrange - create mock repository
            var mock = new Mock<IProductRepository>();

            // Arrange - create mock temp data
            var tempData = new Mock<ITempDataDictionary>();

            // Arrange - create the controller
            var target = new AdminController(mock.Object)
            {
                TempData = tempData.Object
            };

            // Arrange - create a product
            var product = new Product { Name = "Test" };

            // Act - try to save the product
            var result = target.Edit(product);

            // Assert - check that the repository was called
            mock.Verify(m => m.SaveProduct(product));

            // Assert - check the result type is a redirection
            Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", (result as RedirectToActionResult)?.ActionName);
        }

        [Fact]
        public void Cannot_Save_Invalid_Changes()
        {
            // Arrange - create mock repository
            var mock = new Mock<IProductRepository>();

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Arrange - create a product
            var product = new Product { Name = "Test" };

            // Arrange - add an error to the model state
            target.ModelState.AddModelError("error", "error");

            // Act - try to save the product
            var result = target.Edit(product);

            // Assert - check that the repository was not called
            mock.Verify(m => m.SaveProduct(It.IsAny<Product>()), Times.Never());

            // Assert - check the method result type
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Can_Delete_Valid_Products()
        {
            // Arrange - create a Product
            var product = new Product { ProductId = 2, Name = "Test" };

            // Arrange - create the mock repository
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1"},
                product,
                new Product {ProductId = 3, Name = "P3"}
            }.AsQueryable());

            // Arrange - create the controller
            var target = new AdminController(mock.Object);

            // Act - delete the product
            target.Delete(product.ProductId);

            // Assert - ensure that the repository delete method was
            // called with the correct Product
            mock.Verify(m => m.DeleteProduct(product.ProductId));
        }
    }
}