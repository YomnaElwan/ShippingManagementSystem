using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.AccountVM
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Please Enter a Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter a Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password doesn't match")]
        public string ConfirmPassword { get; set; }
        [Required(ErrorMessage = "You Must Enter Address")]

        public string Address { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
