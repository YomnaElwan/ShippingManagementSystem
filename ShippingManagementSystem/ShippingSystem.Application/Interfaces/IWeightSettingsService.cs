using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IWeightSettingsService:IGenericService<WeightSettings>
    {
        Task<WeightSettings> GetById(int Id);
    }
}
