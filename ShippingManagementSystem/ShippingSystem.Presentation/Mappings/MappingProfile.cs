using AutoMapper;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Presentation.ViewModels.AccountVM;
using ShippingSystem.Presentation.ViewModels.BranchVM;
using ShippingSystem.Presentation.ViewModels.CityVM;
using ShippingSystem.Presentation.ViewModels.CourierVM;
using ShippingSystem.Presentation.ViewModels.EmployeeVM;
using ShippingSystem.Presentation.ViewModels.GovernorateVM;
using ShippingSystem.Presentation.ViewModels.MerchantVM;
using ShippingSystem.Presentation.ViewModels.RegionVM;
using ShippingSystem.Presentation.ViewModels.WeightSettingsVM;
using System.Net;

namespace ShippingSystem.Presentation.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            //src=> الحاجة اللي جاي منها الداتا
            //dest=> الحاجة اللي الداتا هيروحلها

            #region Account Mapping
            CreateMap<RegisterViewModel, ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.UserName = src.Name;
                dest.Address = src.Address;
            }).ForMember(dest => dest.PasswordHash, opt => opt.Ignore());
            //why auto mapper violate identity?
            //generic service --> validate user is unique --> code layer
            #endregion

            #region Region Mappings
            CreateMap<Regions, ReadRegionViewModel>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.RegionName = src.Name;
                dest.RegionGovs = src.Governorates;
            }).ReverseMap();
            CreateMap<AddNewRegionViewModel, Regions>().ReverseMap();
            CreateMap<EditRegionViewModel, Regions>().ReverseMap();
            #endregion

            #region Governorate Mappings
            CreateMap<Governorates, ReadGovernoratesViewModel>().AfterMap((src, dest) =>
            {
                dest.RegionName = src?.Region?.Name??"N/A";
            });
            CreateMap<Governorates, AddGovernorateViewModel>().ReverseMap();
            CreateMap<EditGovernorateViewModel, Governorates>().ReverseMap();
            #endregion

            #region City Mappings
            CreateMap<Cities, ReadCityViewModel>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.CityName = src.Name;
                dest.DeliveryCost = src.DeliveryCost;
                dest.PickupCost = src.PickupCost;
                dest.GovernorateName = src?.Governorate?.Name ?? "N/A";
            }).ReverseMap();
            CreateMap<AddCityViewModel, Cities>().ReverseMap();
            CreateMap<EditCityViewModel, Cities>().ReverseMap();
            #endregion

            #region Branch Mappings
            CreateMap<Branches, ReadBranchesViewModel>().ReverseMap();
            CreateMap<AddBranchViewModel, Branches>().ReverseMap();
            CreateMap<EditBranchViewModel, Branches>().ReverseMap();
            #endregion

            #region Weight Settings
            CreateMap<WeightSettings, ReadWeightSettingsViewModel>().AfterMap((src, dest) =>
            {
                dest.CityName = src?.Cities?.Name??"N/A";
            });
            CreateMap<AddWeightSettingsViewModel, WeightSettings>().ReverseMap();
            CreateMap<EditWeightSettingsViewModel, WeightSettings>().ReverseMap();
            #endregion

            #region Courier Mappings
            CreateMap<Couriers, ReadCourierViewModel>().AfterMap((src, dest) =>
            {
                dest.CourierId = src.Id;
                dest.CourierName = src.User.UserName;
                dest.CourierEmail = src.User.Email;
                dest.CourierPhone = src.User.PhoneNumber;
                dest.BranchName = src.Branch.Name;
                dest.IsActive = src.IsActive;
                dest.CourierAddress = src.User.Address;
                dest.DiscountTypeOptions = src.DiscountTypeOption;
                dest.CompanyDiscountValue = src.DiscountValue;
            }).ReverseMap();

            CreateMap<AddCourierViewModel, Couriers>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;
                dest.GovernorateId = src.GovernorateId;
                dest.DiscountValue = src.CompanyDiscountValue;
                dest.DiscountTypeOption = src.DiscountTypeOptions.Value;

            }).ReverseMap();
            CreateMap<AddCourierViewModel, ApplicationUser>().AfterMap((src, dest) =>
            {
                dest.UserName = src.CourierName;
                dest.Email = src.CourierEmail;
                dest.Address = src.CourierAddress;
                dest.PhoneNumber = src.CourierPhone;
            }).ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Couriers, EditCourierViewModel>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.CourierName = src?.User?.UserName??"N/A";
                dest.CourierEmail = src?.User?.Email??"N/A";
                dest.CourierPhone = src?.User?.PhoneNumber??"N/A";
                dest.CourierAddress = src?.User?.Address??"N/A";
                dest.BranchId = src?.BranchId??0;
                dest.GovernorateId = src?.GovernorateId??0;
                dest.DiscountTypeOptions= src?.DiscountTypeOption??0;
                dest.CompanyDiscountValue= src?.DiscountValue??0;
            });

               CreateMap<EditCourierViewModel, Couriers>().AfterMap((src, dest) =>
               {
                dest.BranchId = src.BranchId;
                dest.GovernorateId = src.GovernorateId;
                dest.DiscountTypeOption = src.DiscountTypeOptions.Value;
                dest.DiscountValue = src.CompanyDiscountValue;
               });

               CreateMap<EditCourierViewModel, ApplicationUser>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, op => op.Ignore())
                .AfterMap((src, dest) =>
                {
                dest.UserName = src.CourierName;
                dest.Email = src.CourierEmail;
                dest.NormalizedEmail = src.CourierEmail.ToUpper();
                dest.NormalizedUserName = src.CourierName.ToUpper();
                dest.Address = src.CourierAddress;
                dest.PhoneNumber = src.CourierPhone;
                 });
            #endregion

            #region Employee Mappings
            CreateMap<Employees, GetEmployeeListViewModel>().AfterMap((src, dest) =>
            {
                dest.EmployeeId = src.Id;
                dest.EmployeeName = src.User.UserName;
                dest.EmployeeEmail= src.User.Email;
                dest.EmployeePhone = src.User.PhoneNumber;
                dest.BranchName = src?.Branch?.Name??"N/A";
                dest.IsActive= src.IsActive;
            });

            CreateMap<AddEmployeeViewModel, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, op => op.Ignore())
                .AfterMap((src, dest) =>
                {
                    dest.UserName = src.EmployeeName;
                    dest.Email = src.EmployeeEmail;
                    dest.PhoneNumber = src.EmployeePhone;
                    dest.Address = src.EmployeeAddress;
                });

            CreateMap<AddEmployeeViewModel, Employees>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;
            });

            CreateMap<Employees, EmployeeEditViewModel>().AfterMap((src, dest) =>
            {
                dest.EmployeeName = src?.User?.UserName??"N/A";
                dest.EmployeeEmail = src?.User?.Email ?? "N/A";
                dest.EmployeePhone = src?.User?.PhoneNumber ?? "N/A";
                dest.EmployeeAddress = src?.User?.Address ?? "N/A";
                dest.BranchId = src?.Branch?.Id??0;
            });

            CreateMap<EmployeeEditViewModel, ApplicationUser>()
                 .ForMember(dest => dest.PasswordHash, op => op.Ignore())
                 .ForMember(dest => dest.Id, op => op.Ignore())
                 .AfterMap((src, dest) =>
                 {
                     dest.UserName = src.EmployeeName;
                     dest.NormalizedUserName = src.EmployeeName.ToUpper();
                     dest.Email = src.EmployeeEmail;
                     dest.NormalizedEmail = src.EmployeeEmail.ToUpper();
                     dest.PhoneNumber = src.EmployeePhone;
                     dest.Address = src.EmployeeAddress;
                    
                 });
            CreateMap<EmployeeEditViewModel, Employees>().AfterMap((src, dest) =>
            {
                dest.BranchId = src.BranchId;
            });
            #endregion

            #region Merchant Mappings
            CreateMap<Merchants, ReadMerchantsViewModel>().AfterMap((src, dest) =>
            {
                dest.Id = src.Id;
                dest.MerchantName = src.User.UserName;
                dest.MerchantEmail = src.User.Email;
                dest.MerchantPhoneNumber = src.User.PhoneNumber;
                dest.MerchantAddress = src.User.Address;
                dest.CompanyName = src.CompanyName;
                dest.RejOrderCostPercent = src.RejOrderCostPercent;
                dest.SpecialPackUpCost = src.SpecialPackUpCost;
                dest.BranchName = src?.Branch?.Name??"N/A";
                dest.CityName = src?.City?.Name ?? "N/A";
                dest.GovernorateName = src?.Governorate?.Name ?? "N/A";
                dest.IsActive = src.IsActive;
            });
            CreateMap<AddMerchantVM, ApplicationUser>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .AfterMap((src, dest) => {
                    dest.UserName = src.MerchantName;
                    dest.Email = src.MerchantEmail;
                    dest.PhoneNumber = src.MerchantPhoneNumber;
                    dest.Address = src.MerchantAddress;
                });
            CreateMap<AddMerchantVM, Merchants>().AfterMap((src, dest) =>
            {
                dest.CompanyName=src.CompanyName;
                dest.RejOrderCostPercent=src.RejOrderCostPercent;
                dest.SpecialPackUpCost = src.SpecialPackUpCost;
                dest.BranchId = src.BranchId;
                dest.CityId = src.CityId;
                dest.GovernorateId = src.GovernorateId;
            });
            #endregion

           
            
            
        }
    }
}
