using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.CityVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class CityController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICityService cityService;
        public CityController(IUnitOfWork unitOfWork,IMapper _mapper, ICityService cityService)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = _mapper;
            this.cityService = cityService;
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
            int totalItems =cityList.Count();
            var allCities = cityList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            List<ReadCityViewModel> mappedCityList = _mapper.Map<List<ReadCityViewModel>>(allCities);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return View("Index",mappedCityList);
        }
        [HttpGet]
        [Authorize(Policy = "ViewCityDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Cities cityDetails = await unitOfWork.SpecificCityRepository.cityHasGov(Id);
            if (cityDetails == null)
                return NotFound($"City with id {Id} isn't found!");
            var mappedCityDetails = _mapper.Map<ReadCityViewModel>(cityDetails);
            return View("Details",mappedCityDetails);
        }

        [HttpGet]
        [Authorize(Policy = "AddNewCity")]
        public async Task<IActionResult> Add()
        {
            List<Governorates> govList = await unitOfWork.GovernorateRepository.GetAllAsync();
            AddCityViewModel cityVM = new AddCityViewModel()
            {
                GovernoratesList = govList
            };
            return View("Add",cityVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveNew(AddCityViewModel cityVM)
        {
            //if (cityVM.GovernorateId == 0)
            //{
            //    ModelState.AddModelError("GovernorateId", "You Must Choose a Governorate!");
            //}
            if (!ModelState.IsValid)
            {
                cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
                return View("Add", cityVM);
            }
            try
            {
                Cities newCity = _mapper.Map<Cities>(cityVM);
                await cityService.AddNewCity(newCity);
                return RedirectToAction("Index");
            }
            #region General Exception
            //catch (Exception ex)
            //{
            //    ModelState.AddModelError("Name", ex.Message);
            //    cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
            //    return View("Add", cityVM);
            //}
            #endregion
            #region Custom Exception
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
                return View("Add", cityVM);
            }
            #endregion 

        }
        [Authorize(Policy = "DeleteCity")]
        public async Task<IActionResult> Delete(int Id)
        {
            await unitOfWork.CityRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        [Authorize(Policy = "EditCity")]
        public async Task<IActionResult> Edit(int Id)
        {
            List<Governorates> govList = await unitOfWork.GovernorateRepository.GetAllAsync();
            var existingCity = await unitOfWork.SpecificCityRepository.cityHasGov(Id);
            if (existingCity == null)
                return NotFound($"This city with id {Id} can't be found!");
            var existingCityVM = _mapper.Map<EditCityViewModel>(existingCity);
            existingCityVM.GovernoratesList = govList;
            return View("Edit",existingCityVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditCityViewModel cityVM)
        {
            if (!ModelState.IsValid)
            {
                cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
                return View("Edit", cityVM);
            }
            try
            {
                Cities editCity = _mapper.Map<Cities>(cityVM);
                await cityService.UpdateCity(editCity);
                return RedirectToAction("Index");

            }
            #region General Exception
            //catch (Exception ex) {
            //    ModelState.AddModelError("Name", ex.Message);
            //    cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
            //    return View("Edit", cityVM);
            //}
            #endregion
            #region Custom Exception
            catch (DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                cityVM.GovernoratesList = await unitOfWork.GovernorateRepository.GetAllAsync();
                return View("Edit", cityVM);
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            #endregion
        }
    }
}
