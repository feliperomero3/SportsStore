using System.Collections.Generic;
using SportsStore.Context;
using SportsStore.Models;

namespace SportsStore.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Product> Products => _context.Products;
    }
}
