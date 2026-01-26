using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
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
        private readonly IBranchService branchService;
        public BranchController(IMapper _mapper, IUnitOfWork unitOfWork,IBranchService branchService)
        {
            this._mapper = _mapper;
            this.unitOfWork = unitOfWork;
            this.branchService = branchService;

        }
        [HttpGet]
        [Authorize(Policy = "ViewBranches")]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<Branches> branchList;
            if (User.IsInRole("Admin"))
            {
                branchList = await unitOfWork.BranchRepository.GetAllAsync();
            }
            else {
                branchList = await unitOfWork.BranchRepository.ActiveList();
            }
            int totalItems = branchList.Count();
            var branchesInOnePage = branchList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var mappedBranches = _mapper.Map<List<ReadBranchesViewModel>>(branchesInOnePage);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return View("Index",mappedBranches);
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
            if (!ModelState.IsValid)
            {
                return View("Add", newBranchVM);
            }
            try
            {
                Branches addNewBranch = _mapper.Map<Branches>(newBranchVM);
                await branchService.AddBranch(addNewBranch);
            }
            #region  Generic Exception
            //catch(Exception ex)
            //{
            //    ModelState.AddModelError("Name", ex.Message);
            //    return View("Add", newBranchVM);
            //}
            #endregion
            #region Custom Exception
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View("Add", newBranchVM);
            }
            #endregion
            return RedirectToAction("Index");

        }
        [HttpGet]
        [Authorize(Policy = "ViewBranchDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            var branch = await unitOfWork.BranchRepository.GetByIdAsync(Id);
            if (branch == null)
                return NotFound($"Branch with id:{Id} can't be found");
            var mappedBranch = _mapper.Map<ReadBranchesViewModel>(branch);
            return View("Details", mappedBranch);
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
            if (existingBranch == null)
                return NotFound($"Branch with id:{Id} can't be found");
            var mappedExistingBranch = _mapper.Map<EditBranchViewModel>(existingBranch);
            return View("Edit",mappedExistingBranch);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditBranchViewModel branchVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit",branchVM);
            }
            try
            {
                Branches editBranch = _mapper.Map<Branches>(branchVM);
                await branchService.UpdateBranch(editBranch);
                return RedirectToAction("Index");
            }
            #region General Exception
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("Name", ex.Message);
            //    return View("Edit", branchVM);
            //}
            #endregion
            #region Custom Exception 
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View("Edit", branchVM);
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            #endregion
        }
    }
}
