using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class WeightSettings
    {
        public int Id { get; set; }
        public decimal BaseWeightLimit { get; set; } //اقصى وزن هيحاسب عليه بقيمة الشحن العادية
        public decimal PricePerKg { get; set; } // additional price per kg above BaseWeightLimit
        [ForeignKey("Cities")]
        public int CityId { get; set; }
        public Cities Cities { get; set; }
        public List<Orders>? Orders { get; set; }
    }
}
