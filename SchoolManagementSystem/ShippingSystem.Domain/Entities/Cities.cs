using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Cities
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal DeliveryCost { get; set; } // Ordinary Delivery Cost
        public decimal PickupCost { get; set; } // Pickup Service Cost

        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public Governorates Governorate { get; set; }

    }
}
