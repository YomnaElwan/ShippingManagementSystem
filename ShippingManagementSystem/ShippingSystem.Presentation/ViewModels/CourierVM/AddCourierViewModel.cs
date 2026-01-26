using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.CourierVM
{
    public class AddCourierViewModel
    {
        [Required(ErrorMessage ="Enter Courier Name!")]
        [DisplayName("Name")]
        public string CourierName { get; set; }
        [Required(ErrorMessage ="Enter Courier Email!")]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string CourierEmail { get; set; }
        [Required(ErrorMessage ="Enter a Password!")]
        [DisplayName("Password")]
        [DataType(DataType.Password)]
        public string CourierPassword { get; set; }
        [Required(ErrorMessage ="Enter Phone Number!")]
        [DisplayName("PhoneNumber")]
        [DataType(DataType.PhoneNumber)]
        public string CourierPhone { get; set; }
        [Required(ErrorMessage ="Courier Address is Required!")]
        [DisplayName("Address")]
        public string CourierAddress { get; set; }
        [DisplayName("Branch")]
        public int BranchId { get; set; }
        [NotMapped]
        public List<Branches>? BranchList { get; set; }
        [DisplayName("Governorate")]
        public int GovernorateId { get; set; }
        [NotMapped]
        public List<Governorates>? GovernoratesList { get; set; }
        [Required(ErrorMessage ="Enter a Discount Value!")]
        [DisplayName("DiscountValue")]
        public decimal CompanyDiscountValue { get; set; }
        [Required(ErrorMessage ="Please Select a Discount Type")]
        [DisplayName("Discount Type")]
        public DiscountType? DiscountTypeOptions { get; set; }
    }

}
