using Newtonsoft.Json.Serialization;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.BranchVM
{
    public class AddBranchViewModel
    {
        [Required(ErrorMessage = "You Must Enter Branch Name!")]
        [StringLength(30,MinimumLength=5,ErrorMessage ="Branch name length must be between 5 & 30 !")]

        public string Name { get; set; }
        [Required(ErrorMessage = "You Must Enter Branch Location!")]
        [StringLength(100,MinimumLength=3,ErrorMessage ="Branch Location Length Must Be In Range Of 3 to 100 Letters!")]

        public string Location { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.UtcNow.AddHours(2);

    }
}
