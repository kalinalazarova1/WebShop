using WebShop.Models.Entities;

namespace WebShop.Web.ViewModels
{
    public class StockEntryInputModel
    {
        public StockEntryType StockEntryType { get; set; }

        public int ProductId { get; set; }

        public decimal Units { get; set; }

        public decimal Amount { get; set; }
    }
}
