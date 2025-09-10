using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ShippingSystem.Presentation.ViewModels.EmployeeVM
{
    public class GetEmployeeListViewModel
    {
        public int EmployeeId { get; set; }
        public string? EmployeeName { get; set; }
        public string? EmployeeEmail { get; set; }
        public string? EmployeePhone { get; set; }
        public string? BranchName { get; set; }
        public bool IsActive { get; set; }
    }
}
