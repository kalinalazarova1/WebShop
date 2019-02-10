using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.Entities
{
    public class BasketItem
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        public string AppUserId { get; set; }

        public AppUser Buyer { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Units { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
    }
}
