using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.WeightSettingsVM
{
    public class AddWeightSettingsViewModel
    {
        [Required(ErrorMessage = "You Must Enter Base Weight Limit Field!")]
        [Range(minimum: 0.00, maximum: 100.00, ErrorMessage = "Base Weight Range must be between 0 & 100!")]
        [DisplayName("Base Weight Limit (KG)")]
        public decimal BaseWeightLimit { get; set; }
        [Required(ErrorMessage = "You Must Enter Price For Extra KG!")]
        [Range(minimum: 0.00, maximum: 130.00, ErrorMessage = "Price Per Extra KG must be between 0 & 130!")]
        [DisplayName("Price Per Extra KG (EGP)")]
        public decimal PricePerKg { get; set; }
        [Required(ErrorMessage = "Please Choose a City!!!")]
        [DisplayName("City")]
        public int CityId { get; set; }
        [NotMapped]
        public List<Cities>? CityList { get; set; }
    }
}
