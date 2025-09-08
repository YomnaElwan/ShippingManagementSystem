using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Validation
{
    public class UniquePermissionNameAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string nameFromUser = value.ToString();
            var service = (IGenericRepository<PermissionsModule>)validationContext.GetService(typeof(IGenericRepository<PermissionsModule>));
            var nameFromDB = service.GetAllAsync().Result.FirstOrDefault(p => p.Name.ToLower() == nameFromUser.ToLower());
            if (nameFromDB == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"{nameFromUser} Module is already existing !");

        }
    }
}
