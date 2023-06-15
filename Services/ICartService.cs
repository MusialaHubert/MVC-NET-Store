using Microsoft.AspNetCore.Http;
using Shop.ViewModels;
using System.Collections.Generic;

namespace Shop.Services
{
    public interface ICartService
    {
        List<CartArticleModel> GetProductsFromCart(HttpRequest request);
        public double CalculateCartPrice(List<CartArticleModel> products);
        public void DeleteAllProductsFromCart(HttpRequest request, HttpResponse response);

    }
}