using Microsoft.AspNetCore.Identity;
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
       public int Id { get; set; }
       [ForeignKey("User")]
       public string UserId { get; set; }
       public ApplicationUser User { get; set; }
       public bool IsActive { get; set; } = true;
       [ForeignKey("Branch")]
       public int? BranchId { get; set; }
       public Branches? Branch { get; set; }



    }
}
