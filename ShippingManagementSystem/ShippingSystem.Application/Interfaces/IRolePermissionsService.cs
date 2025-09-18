using Microsoft.AspNetCore.Identity;
using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IRolePermissionsService:IGenericService<RolePermissions>
    {
        Task<List<RolePermissions>> GetAllRolePerms(string RoleId);
        Task<IdentityRole> GetRoleByName(string RoleName);


    }
}
