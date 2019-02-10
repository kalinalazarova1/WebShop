using System;
using System.Collections.Generic;

namespace WebShop.Models.Entities
{
    public class Order
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public AppUser Buyer { get; set; }

        public DateTime Date { get; set; }

        public HashSet<SaleItem> OrderLines { get; set; }
    }
}
