using System.Collections.Generic;

namespace WebShop.Web.ViewModels
{
    public class ProductModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal PricePerUnit { get; set; }

        public int MeasureId { get; set; }

        public MeasureModel Measure { get; set; }

        public decimal CurrentStock { get; set; }

        public HashSet<PhotoModel> Photos { get; set; } = new HashSet<PhotoModel>();
    }
}
