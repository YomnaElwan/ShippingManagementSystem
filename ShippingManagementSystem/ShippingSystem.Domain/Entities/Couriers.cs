using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace ShippingSystem.Domain.Entities
{
    public enum DiscountType
    {
        Percentage=1,
        FixedAmount=2
    }

    public class Couriers
    {
        
        [Key,ForeignKey("User")]
        public string CourierId { get; set; }
        public ApplicationUser User { get; set; }
        public DiscountType DiscountTypeOption { get; set; }
        public decimal DiscountValue { get; set; } // The value of the discount (percentage or fixed amount)

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branches Branch { get; set; }
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public Governorates Governorate { get; set; }

    }
}
