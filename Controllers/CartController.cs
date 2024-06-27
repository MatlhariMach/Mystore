using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Mystore.Data;
using Mystore.Models;

namespace Mystore.Controllers
{
   
    public class CartController : BaseController
    {
        public CartController(ApplicationDbContext context, UserManager<User> userManager)
            : base(context, userManager)
        {
        }

        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cartItems = await _context.CartItems
                                          .Where(ci => ci.UserId == user.Id && ci.Status == "Pending")
                                          .Select(ci => new CartItemViewModel
                                          {
                                              Id = ci.Id,
                                              TotalPrice = ci.TotalPrice,
                                              Quantity = ci.Quantity,
                                              ContentType = ci.ContentType,
                                              Data = ci.Data,
                                              Name = ci.Name
                                          })
                                          .ToListAsync();

            return View(cartItems);
        }

        [HttpPost]
        public async Task<IActionResult> IncreaseQuantity(int Id)
        {
            var cartItem = await _context.CartItems.FindAsync(Id);
            if (cartItem != null)
            {
                cartItem.Quantity++;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> DecreaseQuantity(int Id)
        {
            var cartItem = await _context.CartItems.FindAsync(Id);
            if (cartItem != null && cartItem.Quantity > 1)
            {
                cartItem.Quantity--;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveItem(int Id)
        {
            var cartItem = await _context.CartItems.FindAsync(Id);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SimulatePayPalPayment()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == user.Id);

            if (cart == null || !cart.CartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            // Redirect to the mock PayPal payment page
            return RedirectToAction("MockPayPalPayment", new { totalAmount = cart.CartItems.Sum(ci => ci.Quantity * ci.TotalPrice) });
        }
        [HttpGet]
        public IActionResult MockPayPalPayment(decimal totalAmount)
        {
            return View(totalAmount);
        }

        [HttpPost]
        public async Task<IActionResult> CompletePayment(decimal totalAmount)
        {
            var userId = _userManager.GetUserId(User);
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                .FirstOrDefaultAsync(c => c.UserId == userId && c.CartItems.Any(ci => ci.Status == "Pending"));

            if (cart == null)
            {
                return RedirectToAction("Index");
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                TotalAmount = cart.CartItems.Sum(ci => (decimal)ci.TotalPrice),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = (decimal)ci.TotalPrice * ci.Quantity,
                    CartId = ci.CartId, // Set CartId here
                    UserId = ci.UserId
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // Update the status of cart items
            foreach (var cartItem in cart.CartItems)
            {
                cartItem.Status = "Completed";
            }
            await _context.SaveChangesAsync();

            // Populate the OrderSummaryViewModel
            var orderSummary = new OrderSummaryViewModel
            {
                Id = order.Id,
                OrderDate = order.OrderDate,
                TotalAmount = order.TotalAmount,
                OrderItems = await _context.OrderItems
                    .Where(oi => oi.OrderId == order.Id)
                    .Join(_context.Products,
                          oi => oi.ProductId,
                          p => p.Id,
                          (oi, p) => new OrderItemViewModel
                          {
                              ProductName = p.Name,
                              Quantity = oi.Quantity,
                              UnitPrice = oi.UnitPrice
                          })
                    .ToListAsync()
            };

            // Pass the order summary to the view
            return View("OrderSummary", orderSummary);
        }




        [HttpPost]

        public async Task<IActionResult> OrderSummary(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
              //  .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }


    }

}
