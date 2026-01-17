using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
        public GovernorateController(IUnitOfWork unitOfWork, IMapper _mapper)
        {
            this.unitOfWork = unitOfWork;
            this._mapper = _mapper;
        }
        [HttpGet]
        [Authorize(Policy = "ViewGovernorates")]
        public async Task<IActionResult> Index(int pageNumber=1,int pageSize=5)
        {
            List<Governorates> govList = await unitOfWork.GovernorateRepository.GetAllAsync();
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
            Governorates gov =await unitOfWork.SpecificGovernorateRepository.govByIdIncludeRegion(Id);
            List<Cities> cityList = await unitOfWork.SpecificCityRepository.cityListByGov(Id);
            gov.Cities = cityList;
           
          
            return View("Details",gov);
        }
        [HttpGet]
        [Authorize(Policy = "AddNewGovernorate")]
        public async Task<IActionResult> Add()
        {
            List<Regions> regionList = await unitOfWork.RegionRepository.GetAllAsync();
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
                await unitOfWork.GovernorateRepository.AddAsync(newGov);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            govRegModel.RegionList = await unitOfWork.RegionRepository.GetAllAsync();
            return View("Add",govRegModel);
        }
        [Authorize(Policy = "DeleteGovernorate")]
        public async Task<IActionResult> Delete (int Id)
        {
            await unitOfWork.SpecificGovernorateRepository.DeleteAsync(Id);
            await unitOfWork.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Authorize(Policy = "EditGovernorate")]
        public async Task<IActionResult> Edit(int Id)
        {
            List<Regions> regionList = await unitOfWork.RegionRepository.GetAllAsync();
            var recordFromDB =await unitOfWork.SpecificGovernorateRepository.govByIdIncludeRegion(Id);
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
                await unitOfWork.GovernorateRepository.UpdateAsync(savedEditedModel);
                await unitOfWork.SaveAsync();
                return RedirectToAction("Index");
            }
            govRegionVM.RegionList = await unitOfWork.RegionRepository.GetAllAsync();
            return View("Edit", govRegionVM);
        }
    }
}
