using System.ComponentModel.DataAnnotations;

namespace WebShop.Web.ViewModels
{
    public class PhotoModel
    {
        public int Id { get; set; }

        [Required]
        public byte[] Image { get; set; }
    }
}
