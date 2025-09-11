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
        public decimal SpecialPackUpCost { get; set; }
        [ForeignKey("Branch")]
        public int? BranchId { get; set; }
        public Branches? Branch { get; set; }
        [ForeignKey("City")]
        public int? CityId { get; set; }
        public Cities? City { get; set; }
        [ForeignKey("Governorate")]
        public int? GovernorateId { get; set; }
        public Governorates? Governorate { get; set; }
        [NotMapped]
        public List<Orders>? Orders { get; set; }
        public bool IsActive { get; set; } = true;

    }
}
