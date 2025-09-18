using Microsoft.AspNetCore.Identity;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Services
{
    public class RolePermissionsService : GenericService<RolePermissions>, IRolePermissionsService
    {
        private readonly IRolePermissionsRepository rolePermsRepo;
        public RolePermissionsService(IRolePermissionsRepository rolePermsRepo):base(rolePermsRepo)
        {
            this.rolePermsRepo = rolePermsRepo;
        }
        public Task<List<RolePermissions>> GetAllRolePerms(string RoleId)
        {
            return rolePermsRepo.GetAllRolePerms(RoleId);
        }

        public Task<IdentityRole> GetRoleByName(string RoleName)
        {
            return rolePermsRepo.GetRoleByName(RoleName);
        }
    }
}
