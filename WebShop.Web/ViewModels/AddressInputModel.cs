using WebShop.Models.Entities;

namespace WebShop.Web.ViewModels
{
    public class AddressInputModel
    {
        public string AddresLine1 { get; set; }

        public string AddresLine2 { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public Country Country { get; set; }
    }
}
