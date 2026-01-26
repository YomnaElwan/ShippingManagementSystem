using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.RegionVM;
using System.Collections.Generic;

namespace ShippingSystem.Presentation.Controllers
{
    public class RegionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IRegionService regionService;
        public RegionController(IUnitOfWork unitOfWork, IMapper mapper, IRegionService regionService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.regionService = regionService;
        }
        [HttpGet]
        [Authorize(Policy = "ViewRegions")]
        public async Task<IActionResult> Index(int pageNumber=1, int pageSize=5)
        {
            #region Pagination with original list
            //List<Regions> regionsList = await unitOfWork.RegionRepository.GetAllAsync();
            //var totalItems = regionsList.Count();
            //var totalRegions = regionsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            //ViewBag.CurrentPage = pageNumber;
            //ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            //return View("Index", totalRegions);
            #endregion
            //Pagination - Mapping - كده ما حولش الليست كلها - best for memory and performance
            #region Pagination with mapped list  
            List<Regions> regionsList = await unitOfWork.RegionRepository.GetAllAsync();
            var totalItems = regionsList.Count();
            var totalRegions = regionsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            List<ReadRegionViewModel> readRegionVM = mapper.Map<List<ReadRegionViewModel>>(totalRegions);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            return View("Index",readRegionVM);
            #endregion
        }

        [HttpGet]
        [Authorize(Policy = "AddNewRegion")]
        public IActionResult AddNew()
        {
            return View("AddNew");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(AddNewRegionViewModel newRegionVM)
        {
            if (!ModelState.IsValid)
            {
                return View("AddNew", newRegionVM);
            }
            try {
                Regions newRegion = mapper.Map<Regions>(newRegionVM);
                await regionService.AddNewRegion(newRegion);
                return RedirectToAction("Index");
            }
            #region General Exception
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("Name", ex.Message);
            //    return View("AddNew", newRegionVM);
            //}
            #endregion
            #region Custom Exception
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View("AddNew", newRegionVM);
            }
            #endregion
        }
        [HttpGet]
        [Authorize(Policy = "ViewRegionDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Regions region = await unitOfWork.RegionRepository.GetByIdAsync(Id);
            if (region == null)
                return NotFound($"Region with id:{Id} can't be found!");
            ReadRegionViewModel regionVM = mapper.Map<ReadRegionViewModel>(region);
            List<Governorates> govsinRegionList = await unitOfWork.SpecificGovernorateRepository.regionGovsList(Id);
            regionVM.RegionGovs = govsinRegionList;
            return View("Details", regionVM);
        }
        [Authorize(Policy = "DeleteRegion")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.RegionRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = "EditRegion")]
        public async Task<IActionResult> Edit(int Id)
        {
            Regions region = await unitOfWork.RegionRepository.GetByIdAsync(Id);
            if (region == null)
                return NotFound($"Region with ID {Id} was not found."); 
            EditRegionViewModel editRegion = mapper.Map<EditRegionViewModel>(region);
            return View("Edit",editRegion);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditRegionViewModel editRegionVM)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", editRegionVM);
            }
            try
            {
                Regions updatedRegion = mapper.Map<Regions>(editRegionVM);
                await regionService.UpdateRegion(updatedRegion);
                return RedirectToAction("Index");
            }
            #region General Exception
            //catch(Exception ex)
            //{
            //    ModelState.AddModelError("Name", ex.Message);
            //    return View("Edit",editRegionVM);
            //}
            #endregion
            #region Custom Exception
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                return View("Edit", editRegionVM);
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            #endregion
        }
    }



    
}
