using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
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
        public List<Governorates>? GovList { get; set; }

        [DisplayName("City")]
        public int CityId { get; set; }
        public List<Cities>? CityList { get; set; }

        [DisplayName("Branch")]
        public int BranchId { get; set; }
        public List<Branches>? BranchList { get; set; }

        [DisplayName("Shipping Type")]
        public int ShippingTypeId { get; set; }
        public List<ShippingType>? ShippingTypeList { get; set; }

        [DisplayName("Payment Method")]
        public int PaymentMethodId { get; set; }
        public List<PaymentMethod>? PaymentMethodList { get; set; }

        [DisplayName("Delivery Method")]
        [Required(ErrorMessage ="You must select delivery method")]
        public DeliveryMethod DeliveryTypeOption { get; set; }
        public List<GetOrderItemsVM>? OrderItems { get; set; }

        //[DisplayName("Order Total Cost")]
        //public decimal TotalCost { get; set; }  //will be calculated in the controller

        //[DisplayName("Order Total Weight")]
        //public decimal TotalWeight { get; set; } //will be calculated in the controller

        [DisplayName("Merchant")]
        public int MerchantId { get; set; }
        public List<Merchants>? Merchants { get; set; }

        ////Merchant Data
        //public int MerchantId { get; set; } //will be set in the controller based on the logged in user
        //public string MerchantPhoneNumber { get; set; } //will be set in the controller based on the logged in user
        //public string MerchantAddress { get; set; } //will be set in the controller based on the logged in user



    }
}
