using Microsoft.AspNetCore.Identity;
using ShippingSystem.Domain.Entities;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShippingSystem.Presentation.ViewModels
{
    public class ModulePermissionsVM
    {
        [DisplayName("Roles")]
        public string RoleId { get; set; }
        [NotMapped]
        public List<IdentityRole>? Roles { get; set; }
        [NotMapped]
        public List<PermissionsModule> ?Modules { get; set; } //Modules
        [NotMapped]
        public List<ModuleWithPermissionsVM>? ModulePerms { get; set; }
        
    }
}
