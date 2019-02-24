using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Web.ViewModels
{
    public class CategoryModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int? ParentId { get; set; }

        public HashSet<ProductModel> Products { get; set; }
    }
}
