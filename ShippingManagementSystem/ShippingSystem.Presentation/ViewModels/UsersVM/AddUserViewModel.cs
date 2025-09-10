using Microsoft.AspNetCore.Identity;
using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.UsersVM
{
    public class AddUserViewModel
    {
        [DisplayName("Role")]
        public string RoleId { get; set; }
        [NotMapped]
        public List<IdentityRole>? Roles { get; set; }

        [Required(ErrorMessage = "You Must Enter User Name!")]
        public string? UserName { get; set; }

        [Required(ErrorMessage = "You Must Enter Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "You Must Enter an Email!")]
        public string Email { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "You Must Enter a Phone Number!")]
        public string PhoneNumber { get; set; }
        [DisplayName("Branch")]
        public int BranchId { get; set; }
        [NotMapped]
        public List<Branches>? Branches { get; set; }

        public string? UserAddress { get; set; }


    }
}
