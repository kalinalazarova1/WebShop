using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebShop.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public HashSet<BasketItem> Basket { get; set; } = new HashSet<BasketItem>();

        public HashSet<Order> Orders { get; set; } = new HashSet<Order>();
    }
}
