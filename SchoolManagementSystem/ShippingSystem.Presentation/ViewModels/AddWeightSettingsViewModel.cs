using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels
{
    public class AddWeightSettingsViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You Must Enter Base Weight Limit Field")]
        public decimal BaseWeightLimit { get; set; }
        [Required(ErrorMessage ="You Must Enter Price For Extra KG")]
        public decimal PricePerKg { get; set; }
        [Required(ErrorMessage ="Please Choose a city!")]
        [DisplayName("City")]
        [UniqueCityWeightSettings]
        public int CityId { get; set; }
        [NotMapped]
        public List<Cities>? CityList { get; set; }
    }
}
