using System.ComponentModel.DataAnnotations;

namespace WebShop.Web.ViewModels
{
    public class CredentialsModel
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }

    }
}
