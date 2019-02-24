using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models.Entities
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public HashSet<Product> Products { get; set; } = new HashSet<Product>();

        public int? ParentId { get; set; }

        public Category Parent { get; set; }
    }
}
