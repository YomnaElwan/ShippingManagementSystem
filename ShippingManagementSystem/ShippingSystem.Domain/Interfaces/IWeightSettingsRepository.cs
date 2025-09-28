using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Interfaces
{
    public interface IWeightSettingsRepository:IGenericRepository<WeightSettings>
    {
        Task<WeightSettings> GetById(int Id);
        Task<WeightSettings> GetWeightSettByCityId(int cityId);
    }
}
