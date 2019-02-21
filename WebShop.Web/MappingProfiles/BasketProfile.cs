using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketItem, BasketItemModel>();
            CreateMap<BasketItemModel, BasketItem>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
