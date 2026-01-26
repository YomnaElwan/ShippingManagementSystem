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
    public class GovService : IGovService
    {
        private readonly IUnitOfWork unitOfWork;
        public GovService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        public async Task AddGov(Governorates addGov)
        {
            var governoratesFromDB = await unitOfWork.GovernorateRepository.GetAllAsync();
            var existingGov = UniquenessChecker.CheckDuplication(governoratesFromDB, g => g.Name, addGov.Name, null);
            if (existingGov)
            {
                throw new DuplicateEntityException("Governorates", "Name", addGov.Name);
            }
            var newGovernorate = new Governorates()
            {
                Name = addGov.Name,
                RegionId = addGov.RegionId
            };
            await unitOfWork.GovernorateRepository.AddAsync(newGovernorate);
            await unitOfWork.SaveAsync();
        }

        public async Task UpdateGov(Governorates updateGov)
        {
            var govFromDbById = await unitOfWork.SpecificGovernorateRepository.govByIdIncludeRegion(updateGov.Id);
            var allGovs = await unitOfWork.GovernorateRepository.GetAllAsync();
            if (govFromDbById == null)
                throw new EntityNotFoundException("Governorate",updateGov.Id);
            var existingGovName = UniquenessChecker.CheckDuplication(allGovs,g=>g.Name,updateGov.Name,g=>g.Id!=updateGov.Id);
            if (existingGovName)
            {
                throw new DuplicateEntityException("Governorate", "Name", updateGov.Name);
            }
            govFromDbById.Name=updateGov.Name;
            govFromDbById.RegionId = updateGov.RegionId;
            await unitOfWork.GovernorateRepository.UpdateAsync(govFromDbById);
            await unitOfWork.SaveAsync();
        }
    }
}
