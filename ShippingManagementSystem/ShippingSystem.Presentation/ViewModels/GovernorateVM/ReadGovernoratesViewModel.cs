using ShippingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.GovernorateVM
{
    public class ReadGovernoratesViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsActive { get; set; }
        public List<Cities>? Cities { get; set; }
        public string? RegionName { get; set; }

    }
}
