using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    //  Normal,Express24h, Economy15Days
    public class ShippingType
    {
       public int Id { get; set; }
       public string Name { get; set; }
       public List<Orders> Orders { get; set; }
    }
}
