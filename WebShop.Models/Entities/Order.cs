using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models.Entities
{
    public class Order
    {
        [ReadOnly(true)]
        public int Id { get; private set; }

        [Required]
        public string AppUserId { get; set; }

        public AppUser Buyer { get; set; }

        public DateTime Date { get; set; }

        public HashSet<SaleItem> OrderLines { get; set; } = new HashSet<SaleItem>();
    }
}
