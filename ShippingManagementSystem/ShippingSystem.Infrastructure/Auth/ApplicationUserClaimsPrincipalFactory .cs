using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using ShippingSystem.Domain.Entities;
using System.Security.Claims;

namespace ShippingSystem.Infrastructure.Auth
{
    public class ApplicationUserClaimsPrincipalFactory:UserClaimsPrincipalFactory<ApplicationUser,IdentityRole>
    {
        private readonly RoleManager<IdentityRole> roleManager;
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
                                                     RoleManager<IdentityRole> roleManager,
                                                     IOptions<IdentityOptions> options):base
                                                         (userManager,roleManager,options)
        {
            this.roleManager = roleManager;
        
        }
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser user)
        {
            var identity =await base.GenerateClaimsAsync(user);
            var roles = await UserManager.GetRolesAsync(user);
            foreach(var roleName in roles)
            {
                var role = await roleManager.FindByNameAsync(roleName);
                if (role != null)
                {
                    var roleClaims = await roleManager.GetClaimsAsync(role);
                    foreach(var claim in roleClaims)
                    {
                        identity.AddClaim(claim);
                    }
                  
                }

            }
            return identity;
        }

    }
}
