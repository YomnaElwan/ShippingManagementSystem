using ShippingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels.MerchantVM
{
    public class EditMerchantVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Name Field is required!")]
        [DisplayName("Name")]
        public string MerchantName { get; set; }
        [Required(ErrorMessage = "Email Field is required")]
        [DisplayName("Email")]
        [DataType(DataType.EmailAddress)]
        public string MerchantEmail { get; set; }
      
        [Required(ErrorMessage = "Phone Number is required")]
        [DisplayName("Phone Number")]
        [DataType(DataType.PhoneNumber)]
        public string MerchantPhoneNumber { get; set; }
        [Required(ErrorMessage = "Address is required")]
        [DisplayName("Address")]
        public string MerchantAddress { get; set; }
        [Required(ErrorMessage = "Company Name is required!")]
        [DisplayName("Company Name")]
        public string CompanyName { get; set; }
        [DisplayName("Branch")]
        public int? BranchId { get; set; }
        public List<Branches>? BranchList { get; set; }
        [DisplayName("City")]
        public int? CityId { get; set; }
        public List<Cities>? CityList { get; set; }
        [DisplayName("Governorate")]
        public int? GovernorateId { get; set; }
        public List<Governorates>? GovList { get; set; }
        [Required(ErrorMessage = "Reject Cost is required!")]
        [DisplayName("Reject Cost Percent")]
        public decimal RejOrderCostPercent { get; set; }
        [Required(ErrorMessage = "Pack Up Cost is required!")]
        [DisplayName("Special PackUp Cost")]
        public decimal SpecialPackUpCost { get; set; }
    }
}
