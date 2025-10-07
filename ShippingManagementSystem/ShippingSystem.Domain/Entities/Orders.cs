using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public enum DeliveryMethod
    {
        HomeDelivery = 1,
        BranchPickup = 2
    }
    public class Orders
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string PhoneNumber1 { get; set; }
        public string? PhoneNumber2 { get; set; }
        public string CustomerEmail { get; set; }
        public string Address { get; set; }
        public string? Notes { get; set; }
        public bool VillageDelivery { get; set; }

        [ForeignKey("Merchant")]
        public int MerchantId { get; set; }
        public Merchants Merchant { get; set; }
        [NotMapped]
        public List<OrderItem>? OrderItems { get; set; }
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public Governorates? Governorate { get; set; }
        [ForeignKey("City")]
        public int CityId { get; set; }
        public Cities? City { get; set; }
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branches? Branch { get; set; }

        public decimal TotalCost { get; set; }
        public decimal TotalWeight { get; set; }
        public DeliveryMethod DeliveryTypeOption { get; set; }
        [ForeignKey("ShippingType")]
        public int ShippingTypeId { get; set; }
        public ShippingTypes? ShippingType { get; set; }
        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }
        public PaymentMethods? PaymentMethod { get; set; }
        public DateTime CreateAt { get; set; } = DateTime.Now;
        [ForeignKey("OrderStatus")]
        public int OrderStatusId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        // Real Received Cost From Customer
        public decimal ReceivedAmount { get; set; }
        // Total Delivery Cost (City Cost + Over Weight Cost if exist)
        public decimal ShippingTotalCost { get; set; }
        // Real Received Delivery Cost From Customer
        public decimal ReceivedDeliveryCost { get; set; }

        [ForeignKey("Courier")]
        public int? CourierId { get; set; }
        public Couriers? Courier { get; set; }
        





    }
}
