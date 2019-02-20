using System.Collections.Generic;
using WebShop.Models.Entities;

namespace WebShop.Web.ViewModels
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal PricePerUnit { get; set; }

        public int MeasureId { get; set; }

        public Measure Measure { get; set; }

        public decimal CurrentStock { get; set; }

        public decimal CurrentCost { get; set; }

        public HashSet<Photo> Photos { get; set; }
    }
}
