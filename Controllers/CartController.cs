using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Models;
using Shop.Services;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    public class CartController : Controller
    {
        private readonly ShopDbContext dbContext;
        private readonly ICartService cartService;
        public CartController(ShopDbContext dbContext, ICartService cartService)
        {
            this.dbContext = dbContext;
            this.cartService = cartService;
        }
        public ActionResult Index()
        {
            var products = cartService.GetProductsFromCart(Request)
                .OrderBy(c => c.Article.Id).ToList();
            
            if(products.Count == 0)
            {
                ViewBag.Message = "There is no products in your cart";
            }
            else
            {
                var cartPrice = cartService.CalculateCartPrice(products).ToString();
                ViewBag.Message = $"{cartPrice} zł" ;
            }
            return View(products);
        }

        
        public ActionResult Delete(int id)
        {
            string key = "art" + id.ToString();
            RemoveCookie(key);

            return RedirectToAction("Index");
        }

        public ActionResult AddToCart(int id)
        {
            string key = "art" + id.ToString();
            if (Request.Cookies[key] != null)
            {
                int quantity = Int32.Parse(Request.Cookies[key]);
                Response.Cookies.Append(key, (quantity + 1).ToString());
            }
            return RedirectToAction("Index");
        }

        public ActionResult RemoveFromCart(int id)
        {
            string key = "art" + id.ToString();
            int value = Int32.Parse(Request.Cookies[key]);

            if (value > 1)
            {
                int quantity = Int32.Parse(Request.Cookies[key]);
                Response.Cookies.Append(key, (quantity - 1).ToString());
            }
            else
            {
                RemoveCookie(key);
            }
            return RedirectToAction("Index");
        }

        [Authorize(Roles = "Logged")]
        public ActionResult RedirectToOrderOptions()
        {
            return RedirectToAction("Index", "Order");
        }     

        private void RemoveCookie(string key)
        {
            if (Request.Cookies[key] != null)
            {
                CookieOptions option = new CookieOptions();
                option.Expires = DateTime.Now.AddDays(-1d);
                Response.Cookies.Append(key, "0", option);
            }
        }       
    }
}
