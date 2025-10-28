using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.ViewModels.GovernorateVM;

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
        [Authorize(Policy = "ViewGovernorates")]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<Governorates> govList = await govService.GetAllAsync();
            int totalItems = govList.Count;
            var allGovs = govList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return View("Index",allGovs);
        }
        [HttpGet]
        [Authorize(Policy = "ViewGovernorateDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Governorates gov =await governService.govByIdIncludeRegion(Id);
            List<Cities> cityList = await cityService.cityListByGov(Id);
            gov.Cities = cityList;
           
          
            return View("Details",gov);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewGovernorate")]
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
        [Authorize(Policy = "DeleteGovernorate")]
        public async Task<IActionResult> Delete (int Id)
        {
            await governService.DeleteAsync(Id);
            await governService.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "EditGovernorate")]
        public async Task<IActionResult> Edit(int Id)
        {
            List<Regions> regionList = await regionService.GetAllAsync();
            var recordFromDB =await governService.govByIdIncludeRegion(Id);
            var existRecordVM = _mapper.Map<EditGovRegionViewModel>(recordFromDB);
            existRecordVM.RegionList = regionList;
            return View("Edit", existRecordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditGovRegionViewModel govRegionVM)
        {
            if (ModelState.IsValid)
            {
                var savedEditedModel = _mapper.Map<Governorates>(govRegionVM);
                await govService.UpdateAsync(savedEditedModel);
                await govService.SaveAsync();
                return RedirectToAction("Index");
            }
            govRegionVM.RegionList = await regionService.GetAllAsync();
            return View("Edit", govRegionVM);
        }
    }
}
