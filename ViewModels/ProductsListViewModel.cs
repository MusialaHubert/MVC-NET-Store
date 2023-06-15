using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class ProductsListViewModel
    {
        public List<Article> Articles { get; set; }
        public int CategoryId { get; set; }
    }
}
