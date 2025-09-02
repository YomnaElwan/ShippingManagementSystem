using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Branches
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public bool IsActive { get; set; } = true;
        public DateTime CreateAt { get; set; }=DateTime.Now;

        public List<Couriers> Couriers { get; set; }
        public List<Employees> Employees { get; set; }
        public List<Orders> Orders { get; set; }
    }
}
