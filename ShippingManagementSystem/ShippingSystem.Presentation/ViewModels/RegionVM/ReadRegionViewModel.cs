using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.ViewModels.RegionVM
{
    public class ReadRegionViewModel
    {
        public int Id { get; set; }
        public string RegionName { get; set; }
        public List<Governorates>? RegionGovs { get; set; }
    }
}
