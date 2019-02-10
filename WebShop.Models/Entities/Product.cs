using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerUnit { get; set; }

        public int MeasureId { get; set; }

        public Measure Measure { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CurrentStock { get; set; }

        public HashSet<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
