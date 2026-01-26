using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Services
{
    public class WeightSettingService:IWeightSettingService
    {
        private readonly IUnitOfWork unitOfWork;
        public WeightSettingService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task AddNewWeightSetting(WeightSettings newWeightSetting)
        {
            var allWeightSettings = await unitOfWork.WeightSettingsRepository.GetAllAsync();
            var existing = allWeightSettings.Any(w => w.CityId == newWeightSetting.CityId);
            if (existing)
            {
                var city = await unitOfWork.CityRepository.GetByIdAsync(newWeightSetting.CityId);
                throw new DuplicateEntityException("Weight Settings", "City",city?.Name??"Unknown City");
            }
            WeightSettings newWeightSettings = new WeightSettings()
            {
                CityId = newWeightSetting.CityId,
                BaseWeightLimit = newWeightSetting.BaseWeightLimit,
                PricePerKg = newWeightSetting.PricePerKg,
            };
            await unitOfWork.WeightSettingsRepository.AddAsync(newWeightSettings);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateWeightSetting(WeightSettings updateWeightSetting)
        {
            var allWeightSettings =await unitOfWork.WeightSettingsRepository.GetAllAsync();
            var existing = allWeightSettings.Any(w => w.CityId == updateWeightSetting.CityId && w.Id != updateWeightSetting.Id);
            var weightSettingFromDB = await unitOfWork.WeightSettingsRepository.GetByIdAsync(updateWeightSetting.Id);
            if (existing)
            {
                var city = await unitOfWork.CityRepository.GetByIdAsync(updateWeightSetting.CityId);
                throw new DuplicateEntityException("WeightSetting", "City", city.Name);
            }
            if (weightSettingFromDB == null)
                throw new EntityNotFoundException("WeightSetting", updateWeightSetting.Id);
            weightSettingFromDB.CityId = updateWeightSetting.CityId;
            weightSettingFromDB.BaseWeightLimit = updateWeightSetting.BaseWeightLimit;
            weightSettingFromDB.PricePerKg = updateWeightSetting.PricePerKg;
            await unitOfWork.WeightSettingsRepository.UpdateAsync(weightSettingFromDB);
            await unitOfWork.SaveAsync();

        }
    }
}
