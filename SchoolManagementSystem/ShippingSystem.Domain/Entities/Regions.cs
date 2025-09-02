using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Regions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Governorates> Governorates { get; set; }
    }
}
