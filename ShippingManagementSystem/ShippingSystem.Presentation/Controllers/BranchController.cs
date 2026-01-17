using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.BranchVM;
using System.Security;

namespace ShippingSystem.Presentation.Controllers
{
    public class BranchController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork unitOfWork;
        public BranchController(IMapper _mapper, IUnitOfWork unitOfWork)
        {
            this._mapper = _mapper;
            this.unitOfWork = unitOfWork;
          
        }
        [HttpGet]
        [Authorize(Policy = "ViewBranches")]
        public async Task<IActionResult> Index()
        {
            List<Branches> branchList = await unitOfWork.BranchRepository.GetAllAsync();
            var model = _mapper.Map<List<ReadBranchesViewModel>>(branchList);
            return View("Index",model);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewBranch")]
        public async Task<IActionResult> Add()
        {
            return View("Add");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(AddBranchViewModel newBranchVM)
        {
            if (ModelState.IsValid)
            {
                var newBranch = _mapper.Map<Branches>(newBranchVM);
                await unitOfWork.BranchRepository.AddAsync(newBranch);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add", newBranchVM);
        }
        [HttpGet]
        [Authorize(Policy = "ViewBranchDetails")]
        public async Task<IActionResult> Details(int Id) {

            var branch = await unitOfWork.BranchRepository.GetByIdAsync(Id);
            return View("Details", branch);
        }
        [Authorize(Policy = "DeleteBranch")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.BranchRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = "EditBranch")]
        public async Task<IActionResult> Edit(int Id)
        {
            var existingBranch = await unitOfWork.BranchRepository.GetByIdAsync(Id);
            var mappedExistingBranch = _mapper.Map<EditBranchViewModel>(existingBranch);
            return View("Edit",mappedExistingBranch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditBranchViewModel branchVM)
        {
            if (ModelState.IsValid)
            {
                var editedRecord = _mapper.Map<Branches>(branchVM);
                await unitOfWork.BranchRepository.UpdateAsync(editedRecord);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");

            }
            return View("Edit",branchVM);
        }
    }
}
