namespace ShippingSystem.Presentation.ViewModels.PermissionsVM
{
    public class RoleClaimsVM
    {
        public string RoleId { get; set; }
        public string RoleName { get; set; }
        public List<RoleClaimItemVM> RoleClaims { get; set; }
        public List<string> AllPermissions { get; set; }
    }
}
