using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Validation
{
    public class UniqueRegionAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var regionRepo = (IGenericRepository<Regions>)validationContext.GetService(typeof(IGenericRepository<Regions>));

            string regionName = value.ToString();

            var regionFromDb = regionRepo.GetAllAsync().Result.FirstOrDefault(r => r.Name.ToLower() == regionName.ToLower());
            if (regionFromDb == null)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult($"{regionName} is already existing!");

        }

    }
}
