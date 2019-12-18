using System.Linq;
using Microsoft.EntityFrameworkCore;
using SportsStore.Contexts;
using SportsStore.Models;

namespace SportsStore.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IQueryable<Order> Orders => _context.Orders.Include(o => o.Lines).ThenInclude(p => p.Product);

        public void SaveOrder(Order order)
        {
            _context.AttachRange(order.Lines.Select(p => p.Product));

            if (order.OrderId == 0)
            {
                _context.Orders.Add(order);
            }

            _context.SaveChanges();
        }
    }
}
