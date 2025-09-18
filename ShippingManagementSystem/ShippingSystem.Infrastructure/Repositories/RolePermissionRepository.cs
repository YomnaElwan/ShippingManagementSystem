using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Infrastructure.Repositories
{
    public class RolePermissionRepository:GenericRepository<RolePermissions>,IRolePermissionsRepository
    {
        private readonly ShippingDbContext cxt;
        public RolePermissionRepository(ShippingDbContext cxt) :base(cxt)
        {
            this.cxt = cxt;
        }

        public Task<List<RolePermissions>> GetAllRolePerms(string RoleId)
        {
            return cxt.RolePermissions.Where(rp => rp.RoleId == RoleId).ToListAsync();
        }

        public Task<IdentityRole> GetRoleByName(string RoleName)
        {
            return cxt.Roles.FirstOrDefaultAsync(r => r.Name == RoleName);
        }
    }
}
