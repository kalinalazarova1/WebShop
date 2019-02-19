using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models.Entities;

namespace WebShop.Web.ViewModels
{
    public class StockEntryModel
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public StockEntryType StockEntryType { get; set; }

        public int ProductId { get; set; }

        public ProductModel Product { get; set; }

        public decimal Units { get; set; }

        public decimal Amount { get; set; }
    }
}
