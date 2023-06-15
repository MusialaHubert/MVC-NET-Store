using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Article
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }
        [Required]
        public double Price { get; set; }
        [MaxLength(100)]
        public string Photo { get; set; }


        public int CategoryId { get; set; }
        public Category Category { get; set; }
      
    }
}
