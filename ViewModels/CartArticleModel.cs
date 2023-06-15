using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class CartArticleModel
    {
        public Article Article { get; set; }
        public int Quantity { set; get; }
    }
}
