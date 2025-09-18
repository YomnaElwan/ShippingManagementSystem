namespace ShippingSystem.Presentation.ViewModels.OrderItemsVM
{
    public class GetOrderItemsVM
    {
        public int Id { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Weight { get; set; }
        public decimal Price { get; set; }
    }
}
