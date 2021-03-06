﻿namespace WebShop.Web.ViewModels
{
    public class SaleItemModel
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public ProductModel Product { get; set; }

        public decimal Units { get; set; }

        public decimal Amount { get; set; }
    }
}
