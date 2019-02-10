using AutoMapper;
using WebShop.Models.Entities;
using WebShop.Web.ViewModels;

namespace WebShop.Web.MappingProfiles
{
    public class MeasureProfile : Profile
    {
        public MeasureProfile()
        {
            CreateMap<Measure, MeasureModel>();
            CreateMap<MeasureModel, Measure>();
        }
    }
}
