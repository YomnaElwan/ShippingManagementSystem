using ShippingSystem.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels.CourierVM
{
    public class ReadCourierViewModel
    {
        public int CourierId { get; set; }
        public string? CourierName { get; set; }
        public string? CourierEmail { get; set; }    
        public string? CourierPhone { get; set; }
        public string? CourierAddress { get; set; }
        public string? BranchName { get; set; }
        public string? GovernorateName { get; set; }
        public decimal CompanyDiscountValue { get; set; }
        public DiscountType? DiscountTypeOptions { get; set; }
        public bool IsActive { get; set; }
    }
}
