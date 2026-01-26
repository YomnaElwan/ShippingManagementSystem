using ShippingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels.GovernorateVM
{
    public class EditGovernorateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter governorate name!")]
        [StringLength(15,MinimumLength=4,ErrorMessage ="Governorate name length must be between 4 & 15!")]

        public string Name { get; set; }

        [Required(ErrorMessage = "You must choose a region!")]
        [DisplayName("Region Name")]
        public int RegionId { get; set; }
        [NotMapped]
        public List<Regions>? RegionList { get; set; }

    }
}
