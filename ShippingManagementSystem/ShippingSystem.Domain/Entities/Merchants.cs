using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Merchants
    {
        public int Id { get; set;}
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public string CompanyName { get; set; }
        public decimal RejOrderCostPercent { get; set; }  //RejectedOrderChargePercentage 
        [NotMapped]
        public List<Orders>? Orders { get; set; }
    }
}
