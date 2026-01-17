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
    public class UniqueCityAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            string cityNameFromUser = value.ToString();
            var cityService = (IGenericRepository<Cities>)validationContext.GetService(typeof(IGenericRepository<Cities>));
            var cityFromDB = cityService.GetAllAsync().Result.FirstOrDefault(c => c.Name.ToLower() == cityNameFromUser.ToLower());
            if (cityFromDB == null)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"{cityNameFromUser} is already existing");
            }
        }
    }
}
