using ShippingSystem.Domain.Entities;
using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels.OrderVM
{
    public class GetOrdersVM
    {
        public int OrderId { get; set; }
        [DisplayName("Order Status")]
        public string? StatusName { get; set; }
        [DisplayName("Merchant Name")]
        public string? MerchantName { get; set; }
        [DisplayName("Customer Name")]
        public string? CustomerName { get; set; }
        [DisplayName("Customer Phone Number")]
        public string? CustomerPhoneNum { get; set; }
        [DisplayName("Governorate Name")]
        public string? GovName { get; set; }
        [DisplayName("City Name")]
        public string? CityName { get; set; }
        [DisplayName("Order Total Cost")]
        public decimal OrderTotalCost { get; set; }
        [DisplayName("Order Received Cost")]
        public decimal ReceivedAmount { get; set; }
        [DisplayName("Delivery Total Cost")]
        public decimal ShippingTotalCost { get; set; }
        [DisplayName("Received Delivery Cost")]
        public decimal ReceivedDeliveryCost { get; set; }
        [DisplayName("Company Percent")]
        public decimal CompanyPercent { get; set; }
        [DisplayName("Order Date")]
        public DateTime CreateAt { get; set; }
        public int OrderStatusId { get; set; }


    }
}
