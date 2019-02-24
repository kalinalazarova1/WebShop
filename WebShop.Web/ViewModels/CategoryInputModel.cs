using System.ComponentModel.DataAnnotations;

namespace WebShop.Web.ViewModels
{
    public class CategoryInputModel
    {
        [Required]
        public string Title { get; set; }

        public int? ParentId { get; set; }
    }
}
