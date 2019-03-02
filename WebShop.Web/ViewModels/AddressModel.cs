using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebShop.Models.Entities;

namespace WebShop.Web.ViewModels
{
    public class AddressModel
    {
        public int Id { get; set; }

        public string CustomerId { get; set; }

        public AppUser Customer { get; set; }

        public string AddresLine1 { get; set; }

        public string AddresLine2 { get; set; }

        public string Town { get; set; }

        public string Postcode { get; set; }

        public Country Country { get; set; }
    }
}
