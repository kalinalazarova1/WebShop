using System;
using System.Collections.Generic;

namespace WebShop.Web.ViewModels
{
    public class OrderModel
    {
        public int Id { get; set; }

        public string AppUserId { get; set; }

        public DateTime Date { get; set; }

        public int AddressId { get; set; }

        public AddressModel Address { get; set; }

        public HashSet<SaleItemModel> OrderLines { get; set; } = new HashSet<SaleItemModel>();
    }
}
