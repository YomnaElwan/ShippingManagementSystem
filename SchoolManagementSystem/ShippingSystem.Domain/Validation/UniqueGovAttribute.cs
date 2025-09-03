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
    public class UniqueGovAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string govNameFromUser = value.ToString();
            var govService = (IGenericRepository<Governorates>)validationContext.GetService(typeof(IGenericRepository<Governorates>));
            var govFromDB = govService.GetAllAsync().Result.FirstOrDefault(g => g.Name.ToLower() == govNameFromUser.ToLower());
            if (govFromDB == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"{govNameFromUser} is already existing!");

        }
    }
}
