 using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels
{
    public class GovRegionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter governorate name!")]
        [UniqueGov]
        [MinLength(4)]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required(ErrorMessage ="You must choose a region!")]
        public int RegionId { get; set; }
        [NotMapped]
        public List<Regions>? RegionList { get; set; }


    }
}
