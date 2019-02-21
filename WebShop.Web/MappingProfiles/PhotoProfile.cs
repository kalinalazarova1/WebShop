using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<Photo, PhotoModel>();
            CreateMap<PhotoModel, Photo>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
