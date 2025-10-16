using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.CityVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class CityController : Controller
    {
        private readonly IGenericService<Cities> cityService;
        private readonly ICityService citiesService;
        private readonly IGenericService<Governorates> governoratesService;
        private readonly IMapper _mapper;
        public CityController(IGenericService<Cities> cityService,
                              ICityService citiesService,
                              IGenericService<Governorates> governoratesService,
                              IMapper _mapper)
        {
            this.cityService = cityService;
            this.citiesService = citiesService;
            this.governoratesService = governoratesService;
            this._mapper = _mapper;
        }
        [HttpGet]
        public IActionResult CheckDeliveryCost(decimal DeliveryCost,decimal PickupCost)
        {
            if (DeliveryCost > PickupCost)
            {
                return Json(true);
            }
            else
            {
                return Json(false);
            }
        }
        [HttpGet]
        [Authorize(Policy = "ViewCities")]
        public async Task<IActionResult> Index()
        {
            List<Cities> cityList = await cityService.GetAllAsync();
            return View("Index",cityList);
        }
        [HttpGet]
        [Authorize(Policy = "ViewCityDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Cities cityDetails = await citiesService.cityHasGov(Id);
            return View("Details",cityDetails);
        }

        [HttpGet]
        [Authorize(Policy = "AddNewCity")]
        public async Task<IActionResult> Add()
        {
            List<Governorates> govList = await governoratesService.GetAllAsync();
            CityViewModel cityVM = new CityViewModel()
            {
                GovernoratesList = govList
            };
            return View("Add",cityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(CityViewModel cityVM)
        {
            if (cityVM.GovernorateId == 0)
            {
                ModelState.AddModelError("GovernorateId", "You Must Choose a Governorate!");
            }
            if (ModelState.IsValid) {
                var cityModel = _mapper.Map<Cities>(cityVM);
                await cityService.AddAsync(cityModel);
                await cityService.SaveAsync();
                return RedirectToAction("Index");
            }
            cityVM.GovernoratesList = await governoratesService.GetAllAsync();
            return View("Add",cityVM);
        }
        [Authorize(Policy = "DeleteCity")]
        public async Task<IActionResult> Delete(int Id)
        {
           await citiesService.DeleteAsync(Id);
           await citiesService.SaveAsync();
           return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = "EditCity")]
        public async Task<IActionResult> Edit(int Id)
        {
            List<Governorates> govList = await governoratesService.GetAllAsync();
            var existingCity = await citiesService.cityHasGov(Id);
            var existingCityVM = _mapper.Map<CityViewModel>(existingCity);
            existingCityVM.GovernoratesList = govList;
            return View("Edit",existingCityVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(CityViewModel cityVM)
        {
            if (ModelState.IsValid)
            {
                var editedCity = _mapper.Map<Cities>(cityVM);
                await cityService.UpdateAsync(editedCity);
                await cityService.SaveAsync();
                return RedirectToAction("Index");
            }
            cityVM.GovernoratesList = await governoratesService.GetAllAsync();
            return View("Edit",cityVM);
        }
     


    }
}
