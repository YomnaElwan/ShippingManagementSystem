using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.ViewModels;

namespace ShippingSystem.Presentation.Controllers
{
    public class GovernorateController : Controller
    {
        private readonly IGovernorateService _govService;
        private readonly IGenericService<Regions> _regionService;
        private readonly IMapper _mapper;
        public GovernorateController(IGovernorateService _govService,
                                     IGenericService<Regions> _regionService,
                                     IMapper _mapper)
        {
            this._govService = _govService;
            this._regionService = _regionService;
            this._mapper = _mapper;
        }
        public async Task<IActionResult> Index()
        {
            List<Governorates> govsList = await _govService.govsIncludeRegion();
            return View("Index", govsList);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<Regions> regions = await _regionService.GetAllAsync();
            GovRegionViewModel regList = new GovRegionViewModel()
            {
                RegionList = regions
            };

            return View("Add",regList);
        }
        [HttpPost]
        public async Task<IActionResult> SaveNew(GovRegionViewModel newGovFromRequest)
        {
            if (newGovFromRequest.RegionId == 0)
            {
                ModelState.AddModelError("RegionId", "Please Select a Region");
            }
            if (ModelState.IsValid)
            {
                var newGovMapped = _mapper.Map<Governorates>(newGovFromRequest);
                await _govService.AddAsync(newGovMapped);
                await _govService.SaveAsync();
                return RedirectToAction("Index");

            }
            List<Regions> regions = await _regionService.GetAllAsync();
            newGovFromRequest.RegionList = regions;
            return View("Add",newGovFromRequest);
        }

        public async Task<IActionResult> Details(int Id)
        {
            Governorates governorate = await _govService.GetByIdAsync(Id);
            return View("Details",governorate);
        }

        public async Task<IActionResult> Delete(int Id)
        {
            await _govService.DeleteAsync(Id);
            await _govService.SaveAsync();
            return RedirectToAction("Index");
        }
       
        
    }
}
