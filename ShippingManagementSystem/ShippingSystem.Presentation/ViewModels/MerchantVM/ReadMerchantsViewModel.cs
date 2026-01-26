using ShippingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels.MerchantVM
{
    public class ReadMerchantsViewModel
    {
        public int Id { get; set; }
        public string? MerchantName { get; set; }
        public string? MerchantEmail { get; set; }
        public string? MerchantPhoneNumber { get; set; }
        public string? MerchantAddress { get; set; }
        public string CompanyName { get; set; }
        public decimal RejOrderCostPercent { get; set; }  //RejectedOrderChargePercentage 
        public decimal SpecialPackUpCost { get; set; }
        public string? BranchName { get; set; }
        public string? CityName { get; set; }
        public string? GovernorateName { get; set; }
        public bool IsActive { get; set; }
    }
}
