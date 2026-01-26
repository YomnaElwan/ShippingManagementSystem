using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.BranchVM
{
    public class EditBranchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You Must Enter Branch Name")]
        [StringLength(30,MinimumLength=5,ErrorMessage ="Branch name length must be between 5 & 30 !")]
        public string Name { get; set; }

        [Required(ErrorMessage = "You Must Enter Branch Location")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Branch location length must be in range of 3 to 100 letters!")]
        public string Location { get; set; }
        [DisplayName("Creation Date & Time")]
        public DateTime CreateAt { get; set; }


    }
}
