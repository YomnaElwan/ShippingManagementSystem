using Microsoft.AspNetCore.Identity;
using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Interfaces
{
    public interface IRolePermissionsRepository:IGenericRepository<RolePermissions>
    {
        Task<List<RolePermissions>> GetAllRolePerms(string RoleId);
        Task<IdentityRole> GetRoleByName(string RoleName);
    }
}
