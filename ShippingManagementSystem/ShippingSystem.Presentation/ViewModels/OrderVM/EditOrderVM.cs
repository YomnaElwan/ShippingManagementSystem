using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.OrderItemsVM;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels.OrderVM
{
    public class EditOrderVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Enter Customer Name!")]
        [DisplayName("Customer Name")]
        public string CustomerName { get; set; }

        [Length(minimumLength: 11, maximumLength: 11, ErrorMessage = "Phone Number Must Be 11 Numbers")]
        [Required(ErrorMessage = "Enter First Number Phone!")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("1st Phone Number")]
        public string PhoneNumber1 { get; set; }

        [Length(minimumLength: 11, maximumLength: 11, ErrorMessage = "Phone Number Must Be 11 Numbers")]
        [DataType(DataType.PhoneNumber)]
        [DisplayName("2nd Phone Number")]
        public string? PhoneNumber2 { get; set; }

        [Required(ErrorMessage = "Enter Customer Email!")]
        [DataType(DataType.EmailAddress)]
        [DisplayName("Customer Email")]
        public string CustomerEmail { get; set; }

        [Required(ErrorMessage = "Enter Customer Address")]
        [DisplayName("Customer Address")]
        public string Address { get; set; }

        [MaxLength(120, ErrorMessage = "You have a limit of 120 characters")]
        public string? Notes { get; set; }

        [DisplayName("Deliver To Village?")]
        public bool VillageDelivery { get; set; }

        [DisplayName("Governorate")]
        [Required(ErrorMessage = "Enter a Governorate!")]
        public int GovernorateId { get; set; }
        public List<Governorates>? GovList { get; set; }

        [DisplayName("City")]
        [Required(ErrorMessage = "Select a City!")]
        public int CityId { get; set; }
        public List<Cities>? CityList { get; set; }

        [DisplayName("Branch")]
        [Required(ErrorMessage = "Select a Branch!")]
        public int BranchId { get; set; }
        public List<Branches>? BranchList { get; set; }

        [DisplayName("Shipping Type")]
        [Required(ErrorMessage = "Select a Shipping Type!")]
        public int ShippingTypeId { get; set; }
        public List<ShippingType>? ShippingTypeList { get; set; }

        [DisplayName("Payment Method")]
        [Required(ErrorMessage = "Select Payment Method!")]
        public int PaymentMethodId { get; set; }
        public List<PaymentMethod>? PaymentMethodList { get; set; }

        [DisplayName("Delivery Method")]
        [Required(ErrorMessage = "Select a Delivery Method")]
        public DeliveryMethod DeliveryTypeOption { get; set; }
        public List<GetOrderItemsVM>? OrderItems { get; set; }

        //public decimal TotalCost { get; set; }  //will be calculated in the controller
        //public decimal TotalWeight { get; set; } //will be calculated in the controller

        [DisplayName("Merchant Phone Number")]
        public string? MerchantPhoneNum { get; set; } //Get it from the logged in user
        [DisplayName("Merchant Address")]
        public string? MerchantAddress { get; set; } //Get it from the logged in user
        [DisplayName("Order Status")]
        [Required(ErrorMessage = "Select an Order Status!")]
        public int OrderStatusId { get; set; }
        public List<OrderStatus>? OrderStatusList { get; set; }
        [DisplayName("Home Delivery Cost")]
        public decimal DeliveryCost { get; set; } // Ordinary Delivery Cost  -- From City Table -- readonly
        [DisplayName("Branch Pickup Cost")]
        public decimal PickupCost { get; set; } // Pickup Service Cost -- From City Table -- readonly
        public int WeightSettingsId { get; set; }
        [DisplayName("Base Weight Limit")]
        public decimal BaseWeightLimit { get; set; } //اقصى وزن هيحاسب عليه بقيمة الشحن العادية -- readonly
        [DisplayName("Additional Weight Price Per one Extra KG")]
        public decimal PricePerKg { get; set; } // additional price per kg above BaseWeightLimit -- readonly
    }
}
