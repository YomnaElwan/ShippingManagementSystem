using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels
{
    public class CityViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please Enter a Name!")]
        [MinLength(3)]
        [MaxLength(20)]
        [UniqueCity]
        public string Name { get; set; }
        [Remote("CheckDeliveryCost","City",AdditionalFields = "PickupCost", ErrorMessage ="Delivery Cost Must be greater than Pickup Cost")]
        [Range(minimum:0.00,maximum:130.00,ErrorMessage ="Delivery Must be between 0 and 130")]
        [Required(ErrorMessage ="You Must Enter Delivery Cost")]
        public decimal DeliveryCost { get; set; }
        [Range(minimum:0.00,maximum:100.00,ErrorMessage ="Pick Up Cost Must be between 0 and 100")]
        [Required(ErrorMessage ="You Must Enter Pick Up Cost!")]
        public decimal PickupCost { get; set; }
        [Required(ErrorMessage ="You Must Choose a Governorate!")]
        [DisplayName("Governorate Name")]
        public int GovernorateId { get; set; }
        [NotMapped]
        public List<Governorates>? GovernoratesList { get; set; }
    }
}
