using Microsoft.AspNetCore.Identity;
using ShippingSystem.Domain.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Entities
{
    public class PermissionsModule
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="You must enter the module name")]
        [UniquePermissionName]
        public string Name { get; set; }  //ModuleName==> Branches, Cities,...
        public DateTime CreateAt { get; set; } = DateTime.Now;
        List<RolePermissions>? RolePermissions { get; set; }
    }
}
