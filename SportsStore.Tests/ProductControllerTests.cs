using System.Collections.Generic;
using System.Linq;
using Moq;
using SportsStore.Controllers;
using SportsStore.Models;
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

            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "Product 1" },
                new Product { ProductId = 2, Name = "Product 2" },
                new Product { ProductId = 3, Name = "Product 3" },
                new Product { ProductId = 4, Name = "Product 4" },
                new Product { ProductId = 5, Name = "Product 5" }
            });

            var controller = new ProductController(mock.Object)
            {
                PageSize = 3
            };

            // Act
            var result = controller.List(productPage: 2).ViewData.Model as IEnumerable<Product>;

            // Assert
            if (result == null) throw new XunitException();

            var prodArray = result.ToArray();

            Assert.True(prodArray.Length == 2);
            Assert.Equal("Product 4", prodArray[0].Name);
            Assert.Equal("Product 5", prodArray[1].Name);
        }
    }
}