using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.Entities
{
    public enum StockEntryType
    {
        Buy = 0,
        Sell = 1,
        Increase = 2,
        Decrease = 3
    }

    public class StockEntry
    {
        [ReadOnly(true)]
        public int Id { get; private set; }

        public DateTime Date { get; set; }

        public StockEntryType StockEntryType { get; set; }

        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Units { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }
    }
}
