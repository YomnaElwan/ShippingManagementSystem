using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.WeightSettingsVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class WeightSettingsController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private IMapper _mapper;
        public WeightSettingsController(IUnitOfWork unitOfWork,
                                        IMapper _mapper
            )
        {
            this.unitOfWork = unitOfWork;
            this._mapper = _mapper;
           
        }
        [Authorize(Policy = "ViewWeightSettings")]
        public async Task<IActionResult> Index()
        {
            List<WeightSettings> settingsList = await unitOfWork.WeightSettingsRepository.GetAllAsync();
            return View("Index",settingsList);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewWeightSetting")]
        public async Task<IActionResult> New()
        {
            List<Cities> cityList = await unitOfWork.CityRepository.GetAllAsync();
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
                await unitOfWork.WeightSettingsRepository.AddAsync(weightSettings);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            List<Cities> cityList = await unitOfWork.CityRepository.GetAllAsync();
            settingsVM.CityList = cityList;
            return View("New", settingsVM);
        }
        [HttpGet]
        [Authorize(Policy= "ViewWeightSettingsDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            var setting = await unitOfWork.SpecificWeightSettingsRepository.GetById(Id);
            if (setting == null)
            {
                return NotFound();
            }
            return View("Details",setting); 
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
                return NotFound();
            }
            var existRecVM = _mapper.Map<WeightSettingsViewModel>(existingRecord);
            existRecVM.CityList = await unitOfWork.CityRepository.GetAllAsync();
            return View("Edit",existRecVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(WeightSettingsViewModel editWSFromUser)
        {
            if (ModelState.IsValid)
            {
                var mappedEditedRec = _mapper.Map<WeightSettings>(editWSFromUser);
                await unitOfWork.WeightSettingsRepository.UpdateAsync(mappedEditedRec);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            editWSFromUser.CityList = await unitOfWork.CityRepository.GetAllAsync();
            return View("Edit",editWSFromUser);
        }
    }
}
