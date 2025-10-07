using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Domain.Entities
{
    public class PaymentMethods
    {
        //CashOnDelivery, Prepaid, ExchangePackage
        public int Id { get; set; }
        [Required(ErrorMessage ="Enter Payment Method Name!")]
        public string Name { get; set; }
        [NotMapped]
        public List<Orders>? Orders { get; set; }
    }
}
