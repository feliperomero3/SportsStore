using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
using SportsStore.Models.ViewModels;
using Xunit;
using Xunit.Sdk;

namespace SportsStore.Tests
{
    public class ProductControllerTests
    {
        [Fact]
        public void CanPaginate()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(GetTestProducts());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var result = controller.List(category: null, productPage: 2).ViewData.Model as ProductsListViewModel;

            // Assert
            if (result == null) throw new XunitException();

            var prodArray = result.Products.ToArray();

            Assert.True(prodArray.Length == 2);
            Assert.Equal("Product 4", prodArray[0].Name);
            Assert.Equal("Product 5", prodArray[1].Name);
        }

        private static IEnumerable<Product> GetTestProducts()
        {
            return new[]
            {
                new Product { ProductId = 1, Name = "Product 1" },
                new Product { ProductId = 2, Name = "Product 2" },
                new Product { ProductId = 3, Name = "Product 3" },
                new Product { ProductId = 4, Name = "Product 4" },
                new Product { ProductId = 5, Name = "Product 5" }
            };
        }

        [Fact]
        public void Can_Send_Pagination_View_Model()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(GetTestProducts());

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var result = controller.List(category: null, productPage: 2).ViewData.Model as ProductsListViewModel;

            // Assert
            if (result == null) throw new ArgumentNullException("results");

            var pageInfo = result.PagingInfo;

            Assert.Equal(2, pageInfo.CurrentPage);
            Assert.Equal(3, pageInfo.ItemsPerPage);
            Assert.Equal(5, pageInfo.TotalItems);
            Assert.Equal(2, pageInfo.TotalPages);
        }
    }
}