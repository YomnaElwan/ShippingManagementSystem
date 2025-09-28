using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Services
{
    public class WeightSettingsService:GenericService<WeightSettings>,IWeightSettingsService
    {
        private readonly IWeightSettingsRepository weightSettingsRepo;
        public WeightSettingsService(IWeightSettingsRepository weightSettingsRepo) :base(weightSettingsRepo)
        {
            this.weightSettingsRepo = weightSettingsRepo;
        }

        public async Task<WeightSettings> GetById(int Id)
        {
            return await weightSettingsRepo.GetById(Id);
        }

        public async Task<WeightSettings> GetWeightSettByCityId(int cityId)
        {
            return await weightSettingsRepo.GetWeightSettByCityId(cityId);
        }
    }
}
