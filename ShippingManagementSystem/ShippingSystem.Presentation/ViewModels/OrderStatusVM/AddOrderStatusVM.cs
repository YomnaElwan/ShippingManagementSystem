
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.OrderStatusVM
{
    public class AddOrderStatusVM
    {
        [Required(ErrorMessage ="You must enter status name!")]
        [DisplayName("Order Status Name")]
        public string StsName { get; set; }
    }
}
