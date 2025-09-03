using AutoMapper;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels;

namespace ShippingSystem.Presentation.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Governorates,GovRegionViewModel>();
            CreateMap<GovRegionViewModel, Governorates>();
        }
    }
}
