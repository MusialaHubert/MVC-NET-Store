using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    
    public class OrderViewController : Controller
    {
        private readonly ShopDbContext dbContext;
        public OrderViewController(ShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await dbContext.Orders
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .OrderByDescending(o => o.Id)
                .ToListAsync();

            return View(orders);
        }

        public async Task<IActionResult> OrderDetails(string date)
        {
            var order = await dbContext.Orders
                .Include(o => o.Delivery)
                .Include(o => o.Payment)
                .Where(o => o.Date == date)
                .ToListAsync();

            return View(order);
        }
    }
}
