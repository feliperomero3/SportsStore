using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Moq;
using SportsStore.Components;
using SportsStore.Models;
using Xunit;
using Xunit.Sdk;

namespace SportsStore.Tests
{
    public class NavigationMenuViewComponentTests
    {
        [Fact]
        public void Can_Select_Categories()
        {
            // Arrange
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new[]
            {
                new Product { ProductId = 1, Name = "P1", Category = "Apples" },
                new Product { ProductId = 2, Name = "P2", Category = "Apples" },
                new Product { ProductId = 3, Name = "P3", Category = "Plums" },
                new Product { ProductId = 4, Name = "P4", Category = "Oranges" },
            });

            var target = new NavigationMenuViewComponent(mock.Object);

            // Act
            var results = target.Invoke() as ViewViewComponentResult;

            if (results == null) throw new XunitException("results is null.");

            var categories = (IEnumerable<string>)results.ViewData.Model;

            // Assert
            Assert.True(new[] { "Apples", "Oranges", "Plums" }.SequenceEqual(categories));
        }
    }
}