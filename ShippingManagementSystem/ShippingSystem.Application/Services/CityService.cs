using ShippingSystem.Application.Common;
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
    public class CityService : ICityService
    {
        private readonly IUnitOfWork unitOfWork;
        public CityService(IUnitOfWork unitOfWork)
        {
         this.unitOfWork = unitOfWork;
        }
        public async Task AddNewCity(Cities newCity)
        {
            var allCities = await unitOfWork.CityRepository.GetAllAsync();
            var existingCities = UniquenessChecker.CheckDuplication(allCities, n => n.Name, newCity.Name, null);
            if (existingCities)
            {
                throw new DuplicateEntityException("City", "Name", newCity.Name);
                //throw new Exception($"{newCity.Name} is already existing!");
            }
            var addNewCity = new Cities
            {
                Name = newCity.Name,
                GovernorateId = newCity.GovernorateId,
                DeliveryCost = newCity.DeliveryCost,
                PickupCost = newCity.PickupCost
            };
            await unitOfWork.CityRepository.AddAsync(addNewCity);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateCity(Cities updateCity)
        {
            var cityFromDB = await unitOfWork.CityRepository.GetByIdAsync(updateCity.Id);
            var allCities = await unitOfWork.CityRepository.GetAllAsync();

            if (cityFromDB == null)
            {
                throw new EntityNotFoundException("City", updateCity.Id);
                //throw new Exception("City not found");
            }
            var existingCity = UniquenessChecker.CheckDuplication(allCities, n => n.Name, updateCity.Name, n => n.Id != updateCity.Id);

            if (existingCity)
            {
                throw new DuplicateEntityException("City", "Name", updateCity.Name);
                //throw new Exception($"{updateCity.Name} is already existing");
            }
            cityFromDB.Name = updateCity.Name;
            cityFromDB.GovernorateId = updateCity.GovernorateId;
            cityFromDB.DeliveryCost = updateCity.DeliveryCost;
            cityFromDB.PickupCost = updateCity.PickupCost;
            await unitOfWork.CityRepository.UpdateAsync(cityFromDB);
            await unitOfWork.SaveAsync();
        }

    }
}
