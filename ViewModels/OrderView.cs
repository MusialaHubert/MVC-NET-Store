using Shop.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.ViewModels
{
    public class OrderView
    {
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Town { get; set; }

        [Required]
        [MaxLength(100)]
        public string Street { get; set; }

        [Required]
        [MaxLength(100)]
        public string Number { get; set; }

        [Required]
        [MaxLength(7)]
        [RegularExpression("^[0-9]{2}(-[0-9]{3})?$", ErrorMessage = "Wrong zip code format 00-000")]
        public string ZipCode { get; set; }

        [Required]
        public int DeliveryId { get; set; }

        [Required]
        public int PaymentId { get; set; }

        public List<CartArticleModel> Articles { get; set; }
    }
}
