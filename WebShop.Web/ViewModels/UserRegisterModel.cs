using System.ComponentModel.DataAnnotations;

namespace WebShop.Web.ViewModels
{
    public class UserRegisterModel
    {
        [Required]
        public string Email { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
