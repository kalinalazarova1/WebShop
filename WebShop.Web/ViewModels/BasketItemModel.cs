namespace WebShop.Web.ViewModels
{
    public class BasketItemModel
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductModel Product { get; set; }

        public string AppUserId { get; set; }

        public decimal Units { get; set; }

        public decimal Amount { get; set; }
    }
}
