using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebShop.Models.Entities
{
    public class AppUser : IdentityUser
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public HashSet<BasketItem> Basket { get; set; } = new HashSet<BasketItem>();

        public HashSet<Order> Orders { get; set; } = new HashSet<Order>();

        public HashSet<Address> DeliveryAdresses { get; set; } = new HashSet<Address>();
    }
}
