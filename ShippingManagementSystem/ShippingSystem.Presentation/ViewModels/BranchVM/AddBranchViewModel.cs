using ShippingSystem.Domain.Validation;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.BranchVM
{
    public class AddBranchViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "You Must Enter Branch Name")]
        [UniqueBranch]
        [MaxLength(20)]
        [MinLength(5)]
        public string Name { get; set; }
        [Required(ErrorMessage = "You Must Enter Branch Location")]
        [MaxLength(100)]

        public string Location { get; set; }
    }
}
