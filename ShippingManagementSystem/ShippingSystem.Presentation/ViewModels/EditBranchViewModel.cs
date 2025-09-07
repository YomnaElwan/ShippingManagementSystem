using ShippingSystem.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels
{
    public class EditBranchViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "You Must Enter Branch Name")]
        [MaxLength(20)]
        [MinLength(5)]
        public string Name { get; set; }

        [Required(ErrorMessage = "You Must Enter Branch Location")]
        [MaxLength(100)]
        public string Location { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreateAt { get; set; }


    }
}
