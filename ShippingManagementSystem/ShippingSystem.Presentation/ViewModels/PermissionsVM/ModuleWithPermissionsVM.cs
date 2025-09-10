using System.ComponentModel;

namespace ShippingSystem.Presentation.ViewModels.PermissionsVM
{
    public class ModuleWithPermissionsVM
    {
        [DisplayName("Modules/Groups")]
        public int ModuleId { get; set; }
        public bool CanView { get; set; }
        public bool CanViewDetails { get; set; }
        public bool CanAdd { get; set; }
        public bool CanEdit { get; set; }
        public bool CanDelete { get; set; }
    }
}
