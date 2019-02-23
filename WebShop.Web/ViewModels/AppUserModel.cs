using System.Collections.Generic;

namespace WebShop.Web.ViewModels
{
    public class AppUserModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public HashSet<BasketItemModel> Basket { get; set; } = new HashSet<BasketItemModel>();

        public HashSet<OrderModel> Orders { get; set; } = new HashSet<OrderModel>();
    }
}
