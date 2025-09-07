using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class Employees
    {
        [Key,ForeignKey("User")]
        public string EmployeeId { get; set; }
        public ApplicationUser User { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branches Branch { get; set; }
    }
}
