using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class WeightSettings
    {
        public int Id { get; set; }
        public decimal MinWeight { get; set; }  
        public decimal MaxWeight { get; set; }
        public decimal BasePrice { get; set; } // min weight to max weight
        public decimal PricePerKg { get; set; } // additional price per kg above max weight
        public List<Orders> Orders { get; set; }
    }
}
