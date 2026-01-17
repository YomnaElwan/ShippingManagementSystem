using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;

namespace ShippingSystem.Presentation.Controllers
{
    public class RegionController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        public RegionController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;   
        }
        [HttpGet]
        [Authorize(Policy = "ViewRegions")]
        public async Task<IActionResult> Index(int pageNumber=1, int pageSize=5)
        {
            List<Regions> regionsList = await unitOfWork.RegionRepository.GetAllAsync();
            var totalItems = regionsList.Count();
            var totalRegions = regionsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = Math.Ceiling((double)totalItems / pageSize);
            return View("Index", totalRegions);
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
                await unitOfWork.RegionRepository.AddAsync(newRegion);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("AddNew", newRegion);
        }
        [HttpGet]
        [Authorize(Policy = "ViewRegionDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Regions region = await unitOfWork.RegionRepository.GetByIdAsync(Id);
            List<Governorates> govsinRegionList = await unitOfWork.SpecificGovernorateRepository.regionGovsList(Id);
            region.Governorates = govsinRegionList;
            return View("Details", region);
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
            return View("Edit",region);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(Regions region)
        {
            if (ModelState.IsValid)
            {
                await unitOfWork.RegionRepository.UpdateAsync(region);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            return View("Edit", region);
        }
    }



    
}
