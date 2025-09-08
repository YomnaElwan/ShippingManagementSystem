using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class RolePermissions
    {
        [ForeignKey("Role")]
        public string RoleId { get; set; }
        public IdentityRole Role { get; set; }
        [ForeignKey("PermissionsModule")]
        public int PermissionsModuleId { get; set; }
        public PermissionsModule PermissionsModule { get; set; }
        public bool CanView { get; set; }
        public bool CanViewDetails { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
