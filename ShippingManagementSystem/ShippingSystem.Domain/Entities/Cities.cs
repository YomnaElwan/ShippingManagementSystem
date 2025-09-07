using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Cities
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please Enter a Name!")]
        [MinLength(3)]
        [MaxLength(20)]
        public string Name { get; set; }
        [Range(minimum: 0.00, maximum: 130.00, ErrorMessage = "Delivery Must be between 0 and 130")]
        [Required(ErrorMessage = "You Must Enter Delivery Cost")]
        public decimal DeliveryCost { get; set; } // Ordinary Delivery Cost
        [Required(ErrorMessage = "You Must Enter Pick Up Cost!")]
        public decimal PickupCost { get; set; } // Pickup Service Cost

        [ForeignKey("Governorate")]
        [Required(ErrorMessage = "You Must Choose a Governorate!")]
        public int GovernorateId { get; set; }
        public Governorates? Governorate { get; set; }

        //Navigation Property
        public WeightSettings? WeightSettings { get; set; }

    }
}
