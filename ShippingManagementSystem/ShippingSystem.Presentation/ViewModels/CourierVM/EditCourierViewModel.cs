using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.CourierVM
{
    public class EditCourierViewModel
    {
        public int CourierId { get; set; }
        [Required(ErrorMessage ="You must enter the name!")]
        [DisplayName("Name")]
        public string CourierName { get; set; }
        [Required(ErrorMessage ="You must enter email ")]
        [DisplayName("E-mail")]
        [DataType(DataType.EmailAddress)]
        public string CourierEmail { get; set; }
        [Required(ErrorMessage ="Enter the phone number!")]
        [DisplayName("Phone")]
        [DataType(DataType.PhoneNumber)]
        public string CourierPhone { get; set; }
        [Required(ErrorMessage ="Enter the address!")]
        [DisplayName("Address")]
        public string CourierAddress { get; set; }
        [DisplayName("Branch")]
        public int BranchId { get; set; }
        [NotMapped]
        public List<Branches>? BranchList { get; set; }
        [DisplayName("Governorate")]
        public int GovernorateId { get; set; }
        [NotMapped]
        public List<Governorates>? GovernorateList { get; set; }
        [DisplayName("Discount Value")]
        public decimal CompanyDiscountValue { get; set; }
        [DisplayName("Discount Type")]
        public DiscountType? DiscountTypeOptions { get; set; }
    }

}
