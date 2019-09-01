using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportsStore.Models;

namespace SportsStore.Context
{
    public static class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            context.Database.Migrate();

            // Look for any products.
            if (context.Products.Any())
            {
                return; // Db has been seeded
            }

            context.Products.AddRange(
                new Product
                {
                    Name = "Kayak",
                    Description = "A boat for one person",
                    Category = "Water sports",
                    Price = 275
                },
                new Product
                {
                    Name = "Life jacket",
                    Description = "Protective and fashionable",
                    Category = "Water sports",
                    Price = 48.95
                },
                new Product
                {
                    Name = "Soccer Ball",
                    Description = "FIFA-approved size and weight",
                    Category = "Soccer",
                    Price = 19.50
                },
                new Product
                {
                    Name = "Corner Flags",
                    Description = "Give your playing field a professional touch",
                    Category = "Soccer",
                    Price = 34.95
                },
                new Product
                {
                    Name = "Stadium",
                    Description = "Flat-packed 35,000-seat stadium",
                    Category = "Soccer",
                    Price = 79500
                },
                new Product
                {
                    Name = "Thinking Cap",
                    Description = "Improve brain efficiency by 75%",
                    Category = "Chess",
                    Price = 16
                },
                new Product
                {
                    Name = "Unsteady Chair",
                    Description = "Secretly give your opponent a disadvantage",
                    Category = "Chess",
                    Price = 29.95
                },
                new Product
                {
                    Name = "Human Chess Board",
                    Description = "A fun game for the family",
                    Category = "Chess",
                    Price = 75
                },
                new Product
                {
                    Name = "Bling-Bling King",
                    Description = "Gold-plated, diamond-studded King",
                    Category = "Chess",
                    Price = 1200
                });

            context.SaveChanges();
        }
    }
}