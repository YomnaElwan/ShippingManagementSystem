using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.ViewModels.OrderVM
{
    public class OrdersHomeVM
    {
        public Dictionary<int, int> OrderCountByStatus { get; set; } = new();
        public List<OrderStatus>? OrderStatusList { get; set; }
    }
}
