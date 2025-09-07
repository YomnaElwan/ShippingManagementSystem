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
    public class UniqueBranchAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string branchFromUser = value.ToString();
            var service = (IGenericRepository<Branches>)validationContext.GetService(typeof(IGenericRepository<Branches>));
            var branchFromDB = service.GetAllAsync().Result.FirstOrDefault(b => b.Name.ToLower() == branchFromUser.ToLower());
            if (branchFromDB == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"{branchFromUser} is already existing!");
            }
           
        }
    }
}
