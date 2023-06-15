using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    public class Order
    {
        public int Id { get; set; }

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
        public string ZipCode { get; set; }

        [Required]
        [MaxLength(100)]
        public string Date { get; set; }

        [Required]
        public int DeliveryId { get; set; }
        public Delivery Delivery { get; set; }

        [Required]
        public int PaymentId { get; set; }
        public Payment Payment { get; set; }
    }
}
