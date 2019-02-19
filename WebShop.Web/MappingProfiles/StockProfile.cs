using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class StockProfile : Profile
    {
        public StockProfile()
        {
            CreateMap<StockEntryModel, StockEntry>();
            CreateMap<StockEntry, StockEntryModel>();
        }
    }
}
