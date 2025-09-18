using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.OrderVM
{
    public class AddOrderVM
    {
        [Required(ErrorMessage ="Enter Customer Name!")]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }
        [Required(ErrorMessage ="Enter First Number Phone!")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("1st Phone Number")]
        public string PhoneNumber1 { get; set; }
        [DataType(DataType.PhoneNumber)]
        [DisplayName("2nd Phone Number")]
        public string? PhoneNumber2 { get; set; }
        [Required(ErrorMessage ="Enter Customer Email!")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }
        [Required(ErrorMessage ="Enter Customer Address")]
        [DisplayName("Customer Address")]
        public string Address { get; set; }
        [Required(ErrorMessage ="Enter Your Notes")]
        public string? Notes { get; set; }
        [DisplayName("Deliver To Village?")]
        public bool VillageDelivery { get; set; }
        [DisplayName("Governorate")]
        public int GovernorateId { get; set; }
        List<Governorates>? GovList { get; set; }
        [DisplayName("City")]
        public int CityId { get; set; }
        List<Cities>? CityList { get; set; }
        [DisplayName("Branch")]
        public int BranchId { get; set; }
        List<Branches>? BranchList { get; set; }
        [DisplayName("Shipping Type")]
        public int ShippingTypeId { get; set; }
        public List<ShippingType>? ShippingTypeList { get; set; }
        [DisplayName("Payment Method")]
        public int PaymentMethodId { get; set; }
        public List<PaymentMethod>? PaymentMethodList { get; set; }
        [DisplayName("Delivery Method")]
        public DeliveryMethod DeliveryTypeOption { get; set; }

    }
}
