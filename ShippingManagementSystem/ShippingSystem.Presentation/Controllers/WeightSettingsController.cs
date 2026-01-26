using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.WeightSettingsVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class WeightSettingsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWeightSettingService weightSettingsService;
        public WeightSettingsController(IUnitOfWork unitOfWork,
                                        IMapper _mapper,
                                        IWeightSettingService weightSettingsService
            )
        {
            this.unitOfWork = unitOfWork;
            this._mapper = _mapper;
            this.weightSettingsService = weightSettingsService;
        }
        [Authorize(Policy = "ViewWeightSettings")]
        public async Task<IActionResult> Index(int pageNumber = 1,int pageSize=5)
        {
            List<WeightSettings> settingsList = await unitOfWork.WeightSettingsRepository.GetAllAsync();
            int totalItems = settingsList.Count();
            var weightSettingsInOnePage = settingsList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            var mappedSettings = _mapper.Map<List<ReadWeightSettingsViewModel>>(weightSettingsInOnePage);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return View("Index",mappedSettings);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewWeightSetting")]
        public async Task<IActionResult> New()
        {
            List<Cities> cityList = await unitOfWork.CityRepository.GetAllAsync();
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
            if (!ModelState.IsValid)
            {
                List<Cities> cityList = await unitOfWork.CityRepository.GetAllAsync();
                settingsVM.CityList = cityList;
                return View("New", settingsVM);
            }
            try
            {
                var mappedNewWeightSettings = _mapper.Map<WeightSettings>(settingsVM);
                await weightSettingsService.AddNewWeightSetting(mappedNewWeightSettings);
                return RedirectToAction("Index");
            }
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("CityId", ex.Message);
                settingsVM.CityList = await unitOfWork.CityRepository.GetAllAsync();
                return View("New", settingsVM);
            }
        }
        [HttpGet]
        [Authorize(Policy= "ViewWeightSettingsDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            var setting = await unitOfWork.SpecificWeightSettingsRepository.GetWeightSettingsIncludeCity(Id);
            if (setting == null)
            {
                return NotFound($"Weight Setting with this id:{Id} can't be found!");
            }
            var mappedSetting = _mapper.Map<ReadWeightSettingsViewModel>(setting);
            return View("Details", mappedSetting); 
        }
        [Authorize(Policy = "DeleteWeightSetting")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.WeightSettingsRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "EditWeightSetting")]
        public async Task<IActionResult> Edit(int Id)
        {
            var existingRecord = await unitOfWork.SpecificWeightSettingsRepository.GetById(Id);
            if (existingRecord == null)
            {
                return NotFound($"Weight Setting With this id:{Id} can't be found!");
            }
            var existRecVM = _mapper.Map<EditWeightSettingsViewModel>(existingRecord);
            existRecVM.CityList = await unitOfWork.CityRepository.GetAllAsync();
            return View("Edit",existRecVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditWeightSettingsViewModel editWSFromUser)
        {
            if (!ModelState.IsValid) {
                editWSFromUser.CityList = await unitOfWork.CityRepository.GetAllAsync();
                return View("Edit", editWSFromUser);
            }
            try
            {
                var mappedEditedRec = _mapper.Map<WeightSettings>(editWSFromUser);
                await weightSettingsService.UpdateWeightSetting(mappedEditedRec);
                return RedirectToAction("Index");
            }
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("CityId", ex.Message);
                editWSFromUser.CityList = await unitOfWork.CityRepository.GetAllAsync();
                return View("Edit", editWSFromUser);
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        
        }
    }
}
