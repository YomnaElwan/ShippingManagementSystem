using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Presentation.ViewModels.WeightSettingsVM;

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
        [Authorize(Policy = "ViewWeightSettings")]
        public async Task<IActionResult> Index()
        {
            List<WeightSettings> settingsList = await weightSettingsService.GetAllAsync();
            return View("Index",settingsList);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewWeightSetting")]
        public async Task<IActionResult> New()
        {
            List<Cities> cityList = await cityService.GetAllAsync();
            WeightSettingsViewModel settingVM = new WeightSettingsViewModel()
            {
                CityList = cityList
            };
            return View("New",settingVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(WeightSettingsViewModel settingsVM)
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
        [Authorize(Policy= "ViewWeightSettingsDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            var setting = await _weighSettingsService.GetById(Id);
            if (setting == null)
            {
                return NotFound();
            }
            return View("Details",setting); 
        }
        [Authorize(Policy = "DeleteWeightSetting")]
        public async Task<IActionResult> Delete(int Id)
        {
            await weightSettingsService.DeleteAsync(Id);
            await weightSettingsService.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "EditWeightSetting")]
        public async Task<IActionResult> Edit(int Id)
        {
            var existingRecord = await _weighSettingsService.GetById(Id);
            if (existingRecord == null)
            {
                return NotFound();
            }
            var existRecVM = _mapper.Map<WeightSettingsViewModel>(existingRecord);
            existRecVM.CityList = await cityService.GetAllAsync();
            return View("Edit",existRecVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(WeightSettingsViewModel editWSFromUser)
        {
            if (ModelState.IsValid)
            {
                var mappedEditedRec = _mapper.Map<WeightSettings>(editWSFromUser);
                await weightSettingsService.UpdateAsync(mappedEditedRec);
                await weightSettingsService.SaveAsync();
                return RedirectToAction("Index");
            }
            editWSFromUser.CityList = await cityService.GetAllAsync();
            return View("Edit",editWSFromUser);
        }
    }
}
