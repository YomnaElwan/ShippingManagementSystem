using ShippingSystem.Application.Common;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;

namespace ShippingSystem.Application.Services
{
    public class RegionService:IRegionService
    {
        private readonly IUnitOfWork unitOfWork;
        public RegionService(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

       
        public async Task AddNewRegion(Regions addRegion)
        {
            var allRegions = await unitOfWork.RegionRepository.GetAllAsync();
            var existingRegion = UniquenessChecker.CheckDuplication(allRegions, n => n.Name, addRegion.Name, null);
            if (existingRegion)
            {
                throw new DuplicateEntityException("Region", "Name", addRegion.Name);
                //throw new Exception($"{addRegion.Name} is already existing");
            }
            Regions newRegion = new Regions
            {
                Name = addRegion.Name,
            };
            await unitOfWork.RegionRepository.AddAsync(newRegion);
            await unitOfWork.SaveAsync();
        }

        

        public async Task UpdateRegion(Regions updateRegion)
        {
            var regionFromDB = await unitOfWork.RegionRepository.GetByIdAsync(updateRegion.Id);
            var allRegions = await unitOfWork.RegionRepository.GetAllAsync();
            if (regionFromDB == null)
            {
                throw new EntityNotFoundException("Region", updateRegion.Id);
                //throw new Exception("Region not found!");
            }
            var existingRegion = UniquenessChecker.CheckDuplication(allRegions, n => n.Name, updateRegion.Name, n => n.Id != updateRegion.Id);
            if (existingRegion)
            {
                throw new DuplicateEntityException("Region", "Name", updateRegion.Name);
                //throw new Exception($"{updateRegion.Name} is already existing!");
            }
            regionFromDB.Name = updateRegion.Name;
            await unitOfWork.RegionRepository.UpdateAsync(regionFromDB);
            await unitOfWork.SaveAsync();
        }
    }
}
