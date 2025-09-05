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
    public class UniqueCityWeightSettingsAttribute:ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            int cityIdFromUsr = (int)value;
            var service = (IGenericRepository<WeightSettings>)validationContext.GetService(typeof(IGenericRepository<WeightSettings>));
            var cityIdFromDB = service.GetAllAsync().Result.FirstOrDefault(c => c.CityId ==cityIdFromUsr);
            if (cityIdFromDB != null)
            {
                return new ValidationResult("This City Already Has Weight Settings, Please Choose Another City");
            }
            return ValidationResult.Success;
        }
    }
}
