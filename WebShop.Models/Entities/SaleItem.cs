using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.Entities
{
    public class SaleItem
    {
        [ReadOnly(true)]
        public int Id { get; private set; }

        public int OrderId { get; set; }

        public Order Order { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Units { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Cost { get; set; }
    }
}
