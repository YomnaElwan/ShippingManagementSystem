using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.CityVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class CityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        public CityController(IUnitOfWork unitOfWork,IMapper _mapper)
        {
            this.unitOfWork = unitOfWork;
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
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<Cities> cityList = await unitOfWork.CityRepository.GetAllAsync();
            int totalItems = cityList.Count();
            var allCities = cityList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return View("Index",allCities);
        }
        [HttpGet]
        [Authorize(Policy = "ViewCityDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Cities cityDetails = await unitOfWork.SpecificCityRepository.cityHasGov(Id);
            return View("Details",cityDetails);
        }

        [HttpGet]
        [Authorize(Policy = "AddNewCity")]
        public async Task<IActionResult> Add()
        {
            List<Governorates> govList = await unitOfWork.GovernorateRepository.GetAllAsync();
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
                await unitOfWork.CityRepository.AddAsync(cityModel);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Add",cityVM);
        }
        [Authorize(Policy = "DeleteCity")]
        public async Task<IActionResult> Delete(int Id)
        {
           await unitOfWork.SpecificCityRepository.DeleteAsync(Id);
           await unitOfWork.SaveAsync();
           return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = "EditCity")]
        public async Task<IActionResult> Edit(int Id)
        {
            List<Governorates> govList = await unitOfWork.GovernorateRepository.GetAllAsync();
            var existingCity = await unitOfWork.SpecificCityRepository.cityHasGov(Id);
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
                await unitOfWork.CityRepository.UpdateAsync(editedCity);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
            return View("Edit",cityVM);
        }
     


    }
}
