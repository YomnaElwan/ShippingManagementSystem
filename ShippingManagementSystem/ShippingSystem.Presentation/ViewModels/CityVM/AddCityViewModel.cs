using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.CityVM
{
    public class AddCityViewModel
    {
        [Required(ErrorMessage = "Please Enter a Name!")]
        [StringLength(20,MinimumLength=3,ErrorMessage ="City name length must be between 3 & 20!")]
        public string Name { get; set; }
        [Remote("CheckDeliveryCost", "City", AdditionalFields = "PickupCost", ErrorMessage = "Delivery Cost Must be greater than Pickup Cost")]
        [Range(minimum: 0.00, maximum: 130.00, ErrorMessage = "Delivery Must be between 0 and 130!")]
        [Required(ErrorMessage = "You Must Enter Delivery Cost")]
        [DisplayName("Delivery Cost")]
        public decimal DeliveryCost { get; set; }
        [Range(minimum: 0.00, maximum: 100.00, ErrorMessage = "Pick Up Cost Must be between 0 and 100!")]
        [Required(ErrorMessage = "You Must Enter Pick Up Cost!")]
        [DisplayName("Pickup Cost")]
        public decimal PickupCost { get; set; }
        [Required(ErrorMessage = "You Must Choose a Governorate!")]
        [DisplayName("Governorate Name")]
        public int GovernorateId { get; set; }
        [NotMapped]
        public List<Governorates>? GovernoratesList { get; set; }
    }
}
