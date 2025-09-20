using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.OrderItemsVM
{
    public class AddOrderItemsVM
    {
        [Required(ErrorMessage ="Enter Product Name")]
        [DisplayName("Product Name")]
        public string ProductName { get; set; }
        [Required(ErrorMessage ="Enter Product Quantity")]
        [Range(minimum:1,maximum:12,ErrorMessage ="Product Quantity Must Be Between 1 and 12")]
        public int Quantity { get; set; }
        [Required(ErrorMessage ="Enter Product Weight")]
        public decimal Weight { get; set; }
        [Required(ErrorMessage ="Enter Product Price")]
        public decimal Price { get; set; }
        public int? OrderId { get; set; }
    }
}
