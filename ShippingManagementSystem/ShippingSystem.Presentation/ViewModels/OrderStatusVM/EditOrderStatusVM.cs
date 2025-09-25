using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.OrderStatusVM
{
    public class EditOrderStatusVM
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You must enter order status name!")]
        [DisplayName("Order Status Name")]
        public string StsName { get; set; }
    }
}
