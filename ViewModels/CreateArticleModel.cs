using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class CreateArticleModel
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int CategoryId { get; set; }

        public IFormFile Photo { get; set; }
        
    }
}
