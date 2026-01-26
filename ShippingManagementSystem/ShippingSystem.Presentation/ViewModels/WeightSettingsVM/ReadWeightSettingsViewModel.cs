using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.WeightSettingsVM
{
    public class ReadWeightSettingsViewModel
    {
        public int Id { get; set; }
        public decimal BaseWeightLimit { get; set; }
        public decimal PricePerKg { get; set; }
        public string? CityName { get; set; }
    }
}
