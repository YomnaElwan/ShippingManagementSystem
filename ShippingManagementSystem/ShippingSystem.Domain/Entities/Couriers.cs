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
        public int Id { get; set; }
        [ForeignKey("User")]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public DiscountType DiscountTypeOption { get; set; }
        public decimal DiscountValue { get; set; } // The value of the discount (percentage or fixed amount)
        public bool IsActive { get; set; } = true;
        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branches Branch { get; set; }
        [ForeignKey("Governorate")]
        public int GovernorateId { get; set; }
        public Governorates Governorate { get; set; }
        public List<Orders>? Orders { get; set; }
        

    }
}
