using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels
{
    public class EditGovRegionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter governorate name!")]
        [MinLength(4)]
        [MaxLength(15)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You must choose a region!")]
        [DisplayName("Region Name")]
        public int RegionId { get; set; }
        [NotMapped]
        public List<Regions>? RegionList { get; set; }
        public bool IsActive { get; set; }

    }
}
