using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;

namespace ShippingSystem.Presentation.Controllers
{
    public class RegionController : Controller
    {
        private readonly IGenericService<Regions> _regionService;
        private readonly IGovernorateService _govsService;
        public RegionController(IGenericService<Regions> _regionService,
                                IGovernorateService _govsService)
        {
            this._regionService = _regionService;
            this._govsService = _govsService;
        }
        [HttpGet]
        [Authorize(Policy = "ViewRegions")]
        public async Task<IActionResult> Index()
        {
            List<Regions> regionsList = await _regionService.GetAllAsync();
            return View("Index", regionsList);
        }

        [HttpGet]
        [Authorize(Policy = "AddNewRegion")]
        public IActionResult AddNew()
        {
            return View("AddNew");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(Regions newRegion)
        {
            if (ModelState.IsValid)
            {
                await _regionService.AddAsync(newRegion);
                await _regionService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("AddNew", newRegion);
        }
        [HttpGet]
        [Authorize(Policy = "ViewRegionDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Regions region = await _regionService.GetByIdAsync(Id);
            List<Governorates> govsinRegionList = await _govsService.regionGovsList(Id);
            region.Governorates = govsinRegionList;
            return View("Details", region);
        }
        [Authorize(Policy = "DeleteRegion")]
        public async Task<IActionResult> Delete(int Id)
        {
            await _regionService.DeleteAsync(Id);
            await _regionService.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = "EditRegion")]
        public async Task<IActionResult> Edit(int Id)
        {
            Regions region = await _regionService.GetByIdAsync(Id);
            return View("Edit",region);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(Regions region)
        {
            if (ModelState.IsValid)
            {
                await _regionService.UpdateAsync(region);
                await _regionService.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit", region);
        }
    }



    
}
