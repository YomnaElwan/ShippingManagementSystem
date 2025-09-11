using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.EmployeeVM
{
    public class AddEmployeeViewModel
    {
        [Required(ErrorMessage = "Please Enter a Name!")]
        [DisplayName("Name")]
        public string EmployeeName { get; set; }
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "Please Enter an Email!")]
        [DisplayName("E-mail")]
        public string EmployeeEmail { get; set; }
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Please Enter a Password")]
        public string Password { get; set; }
        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "You Must Enter a PhoneNumber")]
        [DisplayName("PhoneNumber")]
        public string EmployeePhone { get; set; }
        [Required(ErrorMessage = "Enter the Address Field!")]
        [DisplayName("Address")]
        public string EmployeeAddress { get; set; }
        [DisplayName("Branch")]
        public int? BranchId { get; set; }
        public List<Branches>? BranchList { get; set; }
    }
}
