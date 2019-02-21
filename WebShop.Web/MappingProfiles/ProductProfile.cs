using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductModel>();
            CreateMap<ProductInputModel, Product>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
