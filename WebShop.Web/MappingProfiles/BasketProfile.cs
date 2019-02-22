using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class BasketProfile : Profile
    {
        public BasketProfile()
        {
            CreateMap<BasketItem, BasketItemModel>().ForMember(m => m.Amount, opt => opt.MapFrom(src => src.Product.PricePerUnit * src.Units));
            CreateMap<BasketItemInputModel, BasketItem>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
