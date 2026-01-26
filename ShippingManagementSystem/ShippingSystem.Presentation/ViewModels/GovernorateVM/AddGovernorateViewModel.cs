using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.GovernorateVM
{
    public class AddGovernorateViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You must enter governorate name!")]
        [StringLength(15,MinimumLength=4,ErrorMessage ="Governorate name length must be between 4 & 15 letters!")]
        
        public string Name { get; set; }

        [Required(ErrorMessage = "You Must Choose a Region!")]
        [DisplayName("Region Name")]
        public int RegionId { get; set; }
        [NotMapped]
        public List<Regions>? RegionList { get; set; }



    }
}
