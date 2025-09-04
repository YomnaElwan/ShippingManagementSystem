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
        private readonly IGenericService<Governorates> govService;
        private readonly IGovernorateService governService;
        private readonly ICityService cityService;
        private readonly IMapper _mapper;
        private readonly IGenericService<Regions> regionService;
        public GovernorateController(IGenericService<Governorates> govService, 
                                     IGovernorateService governService,
                                     ICityService _cityService,
                                     IMapper _mapper,
                                     IGenericService<Regions>regionService)                     
        {
            this.govService = govService;
            this.governService = governService;
            this.cityService = _cityService;
            this._mapper = _mapper;
            this.regionService = regionService;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            List<Governorates> govList = await govService.GetAllAsync();
            return View("Index",govList);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            Governorates gov =await governService.govByIdIncludeRegion(Id);
            List<Cities> cityList = await cityService.cityListByGov(Id);
            gov.Cities = cityList;
           
          
            return View("Details",gov);
        }
        [HttpGet]
        public async Task<IActionResult> Add()
        {
            List<Regions> regionList = await regionService.GetAllAsync();
            GovRegionViewModel govRegionViewModel = new GovRegionViewModel()
            {
                RegionList = regionList
            };
            return View("Add",govRegionViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> SaveNew(GovRegionViewModel govRegModel)
        {
            if (govRegModel.RegionId == 0)
            {
                ModelState.AddModelError("RegionId", "You Must Choose a Region ");
            }

            if (ModelState.IsValid)
            {
                var newGov = _mapper.Map<Governorates>(govRegModel);
                await govService.AddAsync(newGov);
                await govService.SaveAsync();
                return RedirectToAction("Index");
            }
            govRegModel.RegionList = await regionService.GetAllAsync();
            return View("Add",govRegModel);
        }
    

        
       
        
    }
}
