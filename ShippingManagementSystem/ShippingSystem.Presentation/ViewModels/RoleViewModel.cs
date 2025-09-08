using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels
{
    public class RoleViewModel
    {
        [Required(ErrorMessage ="You Must Enter Role Name")]
        public string RoleName { get; set; }
    }
}
