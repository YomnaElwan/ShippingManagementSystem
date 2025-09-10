using AutoMapper;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.BranchVM;
using ShippingSystem.Presentation.ViewModels.CityVM;
using ShippingSystem.Presentation.ViewModels.GovernorateVM;
using ShippingSystem.Presentation.ViewModels.WeightSettingsVM;

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
