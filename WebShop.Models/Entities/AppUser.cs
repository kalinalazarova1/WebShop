using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebShop.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public AppUser()
        {
            this.Basket = new HashSet<BasketItem>();
            this.Orders = new HashSet<Order>();
        }

        public HashSet<BasketItem> Basket { get; set; }

        public HashSet<Order> Orders { get; set; }
    }
}
