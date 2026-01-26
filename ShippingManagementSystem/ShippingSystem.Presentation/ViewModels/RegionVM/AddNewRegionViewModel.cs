using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.RegionVM
{
    public class AddNewRegionViewModel
    {
        [Required(ErrorMessage ="You must enter the region name!")]
        [StringLength(20,MinimumLength =5,ErrorMessage ="Region name length must be between 5 & 20")]
        public string Name { get; set; }
    }
}
