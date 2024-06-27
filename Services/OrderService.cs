using Microsoft.EntityFrameworkCore;
using Mystore.Data;
using Mystore.Models;

namespace Mystore.Services
{
    public interface IOrderService
    {
        IEnumerable<Order> GetAllOrders();
        Order GetOrderById(int id);
        void CreateOrder(Order order);
        void UpdateOrder(Order order);
        void DeleteOrder(int id);
    }

    public class OrderService : IOrderService
    {
        private readonly ApplicationDbContext _context;

        public OrderService(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.CartId).ToList();
            //   return _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product).ToList();
        }

        public Order GetOrderById(int id)
        {
            // return _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.Product)
            return _context.Orders.Include(o => o.OrderItems).ThenInclude(oi => oi.CartId)
                 .FirstOrDefault(o => o.Id == id);
        }

        public void CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public void UpdateOrder(Order order)
        {
            _context.Orders.Update(order);
            _context.SaveChanges();
        }

        public void DeleteOrder(int id)
        {
            var order = _context.Orders.Find(id);
            if (order != null)
            {
                _context.Orders.Remove(order);
                _context.SaveChanges();
            }
        }
    }

}
