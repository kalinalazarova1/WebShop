using System.Collections.Generic;
using WebShop.Models.Entities;

namespace WebShop.Web.ViewModels
{
    public class ProductInputModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public decimal PricePerUnit { get; set; }

        public int MeasureId { get; set; }
    }
}
