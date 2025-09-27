using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.ViewModels.OrderVM
{
    public class GetOrdersVM
    {
        public int OrderId { get; set; }
        public DateTime CreateAt { get; set; }=DateTime.Now;
        public string? CustomerName { get; set; }
        public string? PhoneNumber1 { get; set; }
        public string? GovernorateName { get; set; }
        public string? CityName { get; set; }
        public decimal? TotalCost { get; set; }
        public string? StatusName { get; set; }

    }
}
