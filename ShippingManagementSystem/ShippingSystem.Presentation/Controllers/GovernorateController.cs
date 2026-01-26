using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ShippingSystem.Application.Common.Exceptions;
using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Presentation.ViewModels.GovernorateVM;

namespace ShippingSystem.Presentation.Controllers
{
    public class GovernorateController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGovService govService;
        public GovernorateController(IUnitOfWork unitOfWork, IMapper _mapper, IGovService govService)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = _mapper;
            this.govService = govService;
        }
        [HttpGet]
        [Authorize(Policy = "ViewGovernorates")]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<Governorates> govList;
            if (User.IsInRole("Admin")) {
                govList = await unitOfWork.GovernorateRepository.GetAllAsync();
            }
            else
            {
                govList = await unitOfWork.GovernorateRepository.ActiveList();
            }
            int totalItems = govList.Count;
            var allGovs = govList.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            List<ReadGovernoratesViewModel> mappedGovList = _mapper.Map<List<ReadGovernoratesViewModel>>(allGovs); 
            ViewBag.CurrentPage = pageNumber;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalItems / pageSize);
            return View("Index",mappedGovList);
        }
        [HttpGet]
        [Authorize(Policy = "ViewGovernorateDetails")]
        public async Task<IActionResult> Details(int Id)
        {
            Governorates gov =await unitOfWork.SpecificGovernorateRepository.govByIdIncludeRegion(Id);
            if (gov == null)
                return NotFound($"Governorate with this id:{Id} can't be found!");
            ReadGovernoratesViewModel mappedGov = _mapper.Map<ReadGovernoratesViewModel>(gov);
            List<Cities> cityList = await unitOfWork.SpecificCityRepository.cityListByGov(Id);
            mappedGov.Cities = cityList;
            return View("Details",mappedGov);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewGovernorate")]
        public async Task<IActionResult> Add()
        {
            List<Regions> regionList = await unitOfWork.RegionRepository.GetAllAsync();
            AddGovernorateViewModel govRegionViewModel = new AddGovernorateViewModel()
            {
                RegionList = regionList
            };
            return View("Add",govRegionViewModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
     
        public async Task<IActionResult> SaveNew(AddGovernorateViewModel addGovVM)
        {
            //if (addGovVM.RegionId == 0)
            //{
            //    ModelState.AddModelError("RegionId", "You Must Choose a Region!");
            //}
            if (!ModelState.IsValid) {
                addGovVM.RegionList = await unitOfWork.RegionRepository.GetAllAsync();
                return View("Add", addGovVM);
            }
            try
            {
                var newGov = _mapper.Map<Governorates>(addGovVM);
                await govService.AddGov(newGov);
                return RedirectToAction("Index");
            }
            catch(DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                addGovVM.RegionList = await unitOfWork.RegionRepository.GetAllAsync();
                return View("Add", addGovVM);
            }
        }

        [Authorize(Policy = "DeleteGovernorate")]
        public async Task<IActionResult> Delete (int Id)
        {
            await unitOfWork.GovernorateRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "EditGovernorate")]
        public async Task<IActionResult> Edit(int Id)
        {
            List<Regions> regionList = await unitOfWork.RegionRepository.GetAllAsync();
            var recordFromDB =await unitOfWork.SpecificGovernorateRepository.govByIdIncludeRegion(Id);
            if (recordFromDB == null)
                return NotFound($"Governorate with this id : {Id} can't be found!!");
            var existRecordVM = _mapper.Map<EditGovernorateViewModel>(recordFromDB);
            existRecordVM.RegionList = regionList;
            return View("Edit", existRecordVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveEdit(EditGovernorateViewModel govRegionVM)
        {
            if (!ModelState.IsValid)
            {
                govRegionVM.RegionList = await unitOfWork.RegionRepository.GetAllAsync();
                return View("Edit", govRegionVM);
            }
            try
            {
                var editedGovernorate = _mapper.Map<Governorates>(govRegionVM);
                await govService.UpdateGov(editedGovernorate);
                return RedirectToAction("Index");
            }
            catch (DuplicateEntityException ex)
            {
                ModelState.AddModelError("Name", ex.Message);
                govRegionVM.RegionList = await unitOfWork.RegionRepository.GetAllAsync();
                return View("Edit", govRegionVM);
            }
            catch(EntityNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
           
        }
        }
    }

