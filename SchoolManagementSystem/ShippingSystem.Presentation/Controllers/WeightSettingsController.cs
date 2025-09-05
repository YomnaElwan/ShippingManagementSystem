using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.ViewModels;

namespace ShippingSystem.Presentation.Controllers
{
    public class WeightSettingsController : Controller
    {
        private readonly IGenericService<Cities> cityService;
        private IMapper _mapper;
        private readonly IGenericService<WeightSettings> weightSettingsService;
        private readonly IWeightSettingsService _weighSettingsService;
        public WeightSettingsController(IGenericService<Cities> cityService,
                                        IMapper _mapper,
                                        IGenericService<WeightSettings> weightSettingsService,
                                        IWeightSettingsService _weighSettingsService)
        {
            this.cityService = cityService;
            this._mapper = _mapper;
            this.weightSettingsService = weightSettingsService;
            this._weighSettingsService = _weighSettingsService;
        }
        public async Task<IActionResult> Index()
        {
            List<WeightSettings> settingsList = await weightSettingsService.GetAllAsync();
            return View("Index",settingsList);
        }
        [HttpGet]
        public async Task<IActionResult> New()
        {
            List<Cities> cityList = await cityService.GetAllAsync();
            AddWeightSettingsViewModel settingVM = new AddWeightSettingsViewModel()
            {
                CityList = cityList
            };
            return View("New",settingVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(AddWeightSettingsViewModel settingsVM)
        {
            if (settingsVM.CityId == 0)
            {
                ModelState.AddModelError("CityId","Please Choose a City!!!");
            }
            if (ModelState.IsValid)
            {
                var weightSettings = _mapper.Map<WeightSettings>(settingsVM);
                await weightSettingsService.AddAsync(weightSettings);
                await weightSettingsService.SaveAsync();
                return RedirectToAction("Index");
            }
            List<Cities> cityList = await cityService.GetAllAsync();
            settingsVM.CityList = cityList;
            return View("New", settingsVM);
        }
        [HttpGet]
        public async Task<IActionResult> Details(int Id)
        {
            var setting = await _weighSettingsService.GetById(Id);
            if (setting == null)
            {
                return NotFound();
            }
            return View("Details",setting); 
        }
        public async Task<IActionResult> Delete(int Id)
        {
            await weightSettingsService.DeleteAsync(Id);
            await weightSettingsService.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
