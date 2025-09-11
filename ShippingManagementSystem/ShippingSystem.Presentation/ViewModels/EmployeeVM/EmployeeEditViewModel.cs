using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.EmployeeVM
{
    public class EmployeeEditViewModel
    {
        public int Id { get; set; }

        [DisplayName("Name")]
        [Required(ErrorMessage = "Name is Required")]
        public string EmployeeName { get; set; }

        [DisplayName("PhoneNumber")]
        [Required(ErrorMessage = "Phone Number is Required")]
        [DataType(DataType.PhoneNumber)]
        public string EmployeePhone { get; set; }
        [DataType(DataType.EmailAddress)]
        [DisplayName("E-mail")]
        [Required(ErrorMessage = "E-mail is Required")]
        public string EmployeeEmail { get; set; }

        [DisplayName("Address")]
        [Required(ErrorMessage = "Address is Required")]
        public string? EmployeeAddress { get; set; }
        [DisplayName("Branch")]
        public int? BranchId { get; set; }
        public List<Branches>? BranchList { get; set; }



    }
}
