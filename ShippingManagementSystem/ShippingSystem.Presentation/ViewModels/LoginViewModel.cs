using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Please Enter a Name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Enter a Password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool RememberMe { get; set; }
       
    }
}
