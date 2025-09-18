using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    //  Normal,Express24h, Economy15Days
    public class ShippingType
    {
       public int Id { get; set; }
       [Required(ErrorMessage ="Enter Shipping Type Name")]
       public string Name { get; set; }
       [NotMapped]
       public List<Orders>? Orders { get; set; }
    }
}
