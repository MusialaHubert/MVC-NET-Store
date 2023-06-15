using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class ShopController : Controller
    {
        private readonly ShopDbContext dbContext;

        public ShopController(ShopDbContext context)
        {
            dbContext = context;
        }

        public async Task<IActionResult> Index([Bind("CategoryId")]int categoryId = 1005)
        {
            var shopDbContext = dbContext.Articles
                .Include(a => a.Category)
                .Where(a => a.CategoryId == categoryId)
                .OrderBy(a => a.Id)
                .Take(2);

            ProductsListViewModel model = new ProductsListViewModel()
            {
                Articles = await shopDbContext.ToListAsync(),
                CategoryId = categoryId,
            };

            ViewData["CategoryId"] = new SelectList(dbContext.Categories, "Id", "Name", categoryId);
            return View(model);
        }

        [HttpPost]
        public void AddToCart(int id)
        {
            string key = "art" + id.ToString();
            int quantity = 1;

            var cookieValue = Request.Cookies[key];
            if(cookieValue != null)
            {
                quantity = Int32.Parse(cookieValue) + 1;
            }

            SetCookie(key, quantity.ToString());
        }

        private void SetCookie(string key, string value, int numberOfDays = 7)
        {
            CookieOptions option = new CookieOptions();         
            option.Expires = DateTime.Now.AddDays(7);
            Response.Cookies.Append(key, value, option); 
        }
    }
}
