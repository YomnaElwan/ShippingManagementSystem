namespace ShippingSystem.Domain.Entities
{
    public class PaymentMethod
    {
        //CashOnDelivery, Prepaid, ExchangePackage
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Orders> Orders { get; set; }
    }
}
