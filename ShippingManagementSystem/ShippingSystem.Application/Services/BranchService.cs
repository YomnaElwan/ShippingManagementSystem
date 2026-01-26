using ShippingSystem.Application.Common;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;


namespace ShippingSystem.Application.Services
{
    public class BranchService : IBranchService
    {
        private readonly IUnitOfWork unitOfWork;
        public BranchService(IUnitOfWork unitOfWork)
        {
         this.unitOfWork = unitOfWork;
        }

        public async Task AddBranch(Branches addBranch)
        {

            var branchesFromDB = await unitOfWork.BranchRepository.GetAllAsync();
            var existingBranch = UniquenessChecker.CheckDuplication(branchesFromDB, n => n.Name, addBranch.Name, null);

            if (existingBranch)
            {
                throw new DuplicateEntityException("Branch","Name",addBranch.Name);
                //throw new Exception($"{addBranch.Name} is already existing!");
            }
            var newBranch = new Branches
            {
                Name = addBranch.Name,
                Location = addBranch.Location,
            };
            await unitOfWork.BranchRepository.AddAsync(newBranch);
            await unitOfWork.SaveAsync();
        }


        public async Task UpdateBranch(Branches updateBranch)
        {
            var branchFromDB = await unitOfWork.BranchRepository.GetByIdAsync(updateBranch.Id);
            var allBranches = await unitOfWork.BranchRepository.GetAllAsync();
            if (branchFromDB == null)
            {
                //throw new Exception("Branch not found!");
                throw new EntityNotFoundException("Branch",updateBranch.Id);
            }
            var existingBranchName = UniquenessChecker.CheckDuplication(allBranches, n => n.Name, updateBranch.Name, n => n.Id != updateBranch.Id);
            if (existingBranchName)
            {
                throw new DuplicateEntityException("Branch", "Name", updateBranch.Name);
                //throw new Exception($"{updateBranch.Name} is already existing");
            }

            branchFromDB.Name = updateBranch.Name;
            branchFromDB.Location = updateBranch.Location;
            branchFromDB.CreateAt = updateBranch.CreateAt;
            await unitOfWork.BranchRepository.UpdateAsync(branchFromDB);
            await unitOfWork.SaveAsync();
        }
    }
}
