using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.ViewModels.OrderVM
{
    public class EditOrderStsVM
    {
        public int Id { get; set; }
        public int OrderStsId { get; set; }
        public List<OrderStatus>? OrderStsList { get; set; }
    }
}
