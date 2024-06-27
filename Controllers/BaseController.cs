using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Mystore.Data;
using Mystore.Models;

namespace Mystore.Controllers
{
    public class BaseController : Controller
    {
        protected readonly ApplicationDbContext _context;
        protected readonly UserManager<User> _userManager;

        public BaseController(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            if (User.Identity.IsAuthenticated)
            {
                var userId = _userManager.GetUserId(User);
                var cartItemCount = _context.CartItems.Count(ci => ci.UserId == userId && ci.Status == "Pending");
                ViewBag.CartItemCount = cartItemCount;
            }
            else
            {
                ViewBag.CartItemCount = 0;
            }
        }
    }

}
