using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IWeightSettingService
    {
        Task AddNewWeightSetting(WeightSettings newWeightSetting);
        Task UpdateWeightSetting(WeightSettings updateWeightSetting);
    }
}
