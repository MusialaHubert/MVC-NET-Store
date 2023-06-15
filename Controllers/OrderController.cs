using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
    public class OrderController : Controller
    {
        private readonly ShopDbContext dbContext;
        private readonly ICartService cartService;
        public OrderController(ShopDbContext dbContext, ICartService cartService)
        {
            this.dbContext = dbContext;
            this.cartService = cartService;
        }

        public IActionResult Index()
        {
            var products = cartService.GetProductsFromCart(Request)
                .OrderBy(c => c.Article.Id).ToList();

            if (products.Count == 0)
            {
                RedirectToAction("Index", "Cart");
            }

            ViewData["CartCost"] = cartService.CalculateCartPrice(products);
            ViewData["Deliveries"] = new SelectList(dbContext.Deliveries, "Id", "Name");
            ViewData["Payments"] = new SelectList(dbContext.Payments, "Id", "Name");

            var model = new OrderView()
            {
                Articles = products
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index([Bind("Name, Town, Street, Number, ZipCode, DeliveryId, PaymentId")] OrderView orderView)
        {
            if(ModelState.IsValid)
            {
                Order order = new Order()
                {
                    Name = orderView.Name,
                    Town = orderView.Town,
                    Street = orderView.Street,
                    Number = orderView.Number,
                    ZipCode = orderView.ZipCode,
                    Date = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss:ffff tt"),
                    DeliveryId = orderView.DeliveryId,
                    PaymentId = orderView.PaymentId
                };

                dbContext.Orders.Add(order);
                await dbContext.SaveChangesAsync();

                cartService.DeleteAllProductsFromCart(Request, Response);

                return RedirectToAction("OrderDetails", "OrderView", new { date = order.Date});
            }
            var products = cartService.GetProductsFromCart(Request)
                .OrderBy(c => c.Article.Id).ToList();

            orderView.Articles = products;
            ViewData["CartCost"] = cartService.CalculateCartPrice(products);
            ViewData["Deliveries"] = new SelectList(dbContext.Deliveries, "Id", "Name");
            ViewData["Payments"] = new SelectList(dbContext.Payments, "Id", "Name");

            return View(orderView);
        }


    }
}
