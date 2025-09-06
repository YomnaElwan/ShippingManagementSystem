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
            CreateMap<Cities, CityViewModel>();
            CreateMap<CityViewModel, Cities>();
            CreateMap<Branches, ReadBranchesViewModel>();
            CreateMap<ReadBranchesViewModel, Branches>();
            CreateMap<Branches, AddBranchViewModel>();
            CreateMap<AddBranchViewModel, Branches>();
            CreateMap<WeightSettings, WeightSettingsViewModel>();
            CreateMap<WeightSettingsViewModel, WeightSettings>();
            CreateMap<EditGovRegionViewModel, Governorates>();
            CreateMap<Governorates, EditGovRegionViewModel>();
            CreateMap<EditBranchViewModel, Branches>();
            CreateMap<Branches, EditBranchViewModel>();


        }
    }
}
