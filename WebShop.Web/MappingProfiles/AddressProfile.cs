using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class AddressProfile: Profile
    {
        public AddressProfile()
        {
            CreateMap<Address, AddressModel>();
            CreateMap<AddressInputModel, Address>();
        }
    }
}
