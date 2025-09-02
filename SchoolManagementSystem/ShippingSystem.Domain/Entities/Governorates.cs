using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Governorates
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; } = true;
        public List<Cities> Cities { get; set; }
        public List<Couriers> Couriers { get; set; }
        public List<Orders> Orders { get; set; }
        [ForeignKey("Region")]
        public int RegionId { get; set; }
        public Regions Region { get; set; }
    }
}
