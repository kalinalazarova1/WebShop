using System.ComponentModel.DataAnnotations;

namespace WebShop.Web.ViewModels
{
    public class MeasureModel
    {
        public int Id { get; set; }

        [Required]
        public string Symbol { get; set; }
    }
}
