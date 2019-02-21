using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebShop.Models.Entities
{
    public class Measure
    {
        [ReadOnly(true)]
        public int Id { get; private set; }

        [Required]
        public string Symbol { get; set; }
    }
}
