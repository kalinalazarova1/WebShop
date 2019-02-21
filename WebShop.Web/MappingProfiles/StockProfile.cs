using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<StockEntryInputModel, StockEntry>();
            CreateMap<StockEntry, StockEntryModel>().IgnoreAllPropertiesWithAnInaccessibleSetter();
        }
    }
}
