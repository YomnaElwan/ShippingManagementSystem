using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels;

namespace ShippingSystem.Presentation.Controllers
{
    public class BranchController : Controller
    {
        private readonly IGenericService<Branches> branchService;
        private readonly IMapper _mapper;
        public BranchController(IGenericService<Branches> branchService,IMapper _mapper)
        {
            this.branchService = branchService;
            this._mapper = _mapper;
          
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Branches> branchList = await branchService.GetAllAsync();
            var model = _mapper.Map<List<ReadBranchesViewModel>>(branchList);
            return View("Index",model);
        }
        [HttpGet]
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
                await branchService.AddAsync(newBranch);
                await branchService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Add", newBranchVM);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id) {

            var branch = await branchService.GetByIdAsync(Id);
            return View("Details", branch);
        }
        public async Task<IActionResult> Delete(int Id)
        {
            await branchService.DeleteAsync(Id);
            await branchService.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int Id)
        {
            var existingBranch = await branchService.GetByIdAsync(Id);
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
                await branchService.UpdateAsync(editedRecord);
                await branchService.SaveAsync();
                return RedirectToAction("Index");

            }
            return View("Edit",branchVM);
        }
    }
}
