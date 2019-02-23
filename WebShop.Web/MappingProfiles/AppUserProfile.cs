using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class AppUserProfile : Profile
    {
        public AppUserProfile()
        {
            CreateMap<AppUser, UserRegisterModel>();
            CreateMap<AppUserModel, AppUser>();
        }
    }
}
