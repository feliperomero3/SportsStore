using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
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
        public void Can_Paginate()
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

        [Fact]
        public void Can_Filter_Products()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            });

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            // Act
            var result = controller.List(category: "Cat2", productPage: 1).ViewData.Model as ProductsListViewModel;

            if (result == null) throw new XunitException("result was null.");

            var products = result.Products.ToArray();

            // Assert
            Assert.Equal(2, products.Length);
            Assert.True(products[0].Name == "P2" && products[0].Category == "Cat2");
            Assert.True(products[1].Name == "P4" && products[1].Category == "Cat2");
        }

        [Fact]
        public void Generate_Category_Specific_Product_Count()
        {
            // Arrange
            var mock = new Mock<IProductRepository>();

            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product {ProductId = 1, Name = "P1", Category = "Cat1"},
                new Product {ProductId = 2, Name = "P2", Category = "Cat2"},
                new Product {ProductId = 3, Name = "P3", Category = "Cat1"},
                new Product {ProductId = 4, Name = "P4", Category = "Cat2"},
                new Product {ProductId = 5, Name = "P5", Category = "Cat3"}
            });

            var controller = new ProductController(mock.Object) { PageSize = 3 };

            Func<ViewResult, ProductsListViewModel> GetViewDataModel = result =>
                result.ViewData.Model as ProductsListViewModel;

            // Act
            if (GetViewDataModel == null) throw new XunitException("GetViewDataModel was null.");

            var result1 = GetViewDataModel(controller.List("Cat1"))?.PagingInfo.TotalItems;
            var result2 = GetViewDataModel(controller.List("Cat2"))?.PagingInfo.TotalItems;
            var result3 = GetViewDataModel(controller.List("Cat3"))?.PagingInfo.TotalItems;
            var resultAll = GetViewDataModel(controller.List(null))?.PagingInfo.TotalItems;

            // Assert
            Assert.Equal(2, result1);
            Assert.Equal(2, result2);
            Assert.Equal(1, result3);
            Assert.Equal(5, resultAll);
        }
    }
}