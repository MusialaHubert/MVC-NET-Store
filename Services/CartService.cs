using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Shop.Database;
using Shop.Models;
using Shop.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Shop.Services
{
    public class CartService : ICartService
    {
        private readonly ShopDbContext dbContext;
        public CartService(ShopDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public List<CartArticleModel> GetProductsFromCart(HttpRequest request)
        {
            List<CartArticleModel> products = new List<CartArticleModel>();

            foreach (var cookie in request.Cookies)
            {
                string prefix = GetPrefixFromCookie(cookie.Key);

                if (prefix is not null && prefix == "art")
                {
                    int articleId = Int32.Parse(cookie.Key.Substring(3));
                    var article = GetArticleFromDataBase(articleId);
                    int quantity = Int32.Parse(cookie.Value);

                    if (article is not null)
                    {
                        CartArticleModel cartArticle = new CartArticleModel()
                        {
                            Article = article,
                            Quantity = quantity
                        };
                        products.Add(cartArticle);
                    }
                }
            }
            return products;
        }

        public void DeleteAllProductsFromCart(HttpRequest request, HttpResponse response)
        {
            foreach (var cookie in request.Cookies)
            {
                string prefix = GetPrefixFromCookie(cookie.Key);
                if(prefix is not null && prefix == "art")
                {
                    CookieOptions option = new CookieOptions();
                    option.Expires = DateTime.Now.AddDays(-1d);
                    response.Cookies.Append(cookie.Key, "0", option);
                }
            }
        }

        public double CalculateCartPrice(List<CartArticleModel> products)
        {
            double result = 0;
            foreach (var article in products)
            {
                result += article.Article.Price * article.Quantity;
            }
            return result;
        }

        private string GetPrefixFromCookie(string key)
        {
            string prefix = null;
            if (key.Length > 3)
            {
                prefix = key.Substring(0, 3);
            }

            return prefix;
        }

        private Article GetArticleFromDataBase(int articleId)
        {
            var article = dbContext.Articles
                        .Include(s => s.Category)
                        .FirstOrDefault(a => a.Id == articleId);

            return article;
        }

        
    }
}
