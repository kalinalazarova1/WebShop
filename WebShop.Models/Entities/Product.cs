using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebShop.Models.Entities
{
    public class Product
    {
        [ReadOnly(true)]
        public int Id { get; private set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal PricePerUnit { get; set; }

        public int MeasureId { get; set; }

        public Measure Measure { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CurrentStock { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal CurrentCost { get; set; }

        public HashSet<Photo> Photos { get; set; } = new HashSet<Photo>();
    }
}
