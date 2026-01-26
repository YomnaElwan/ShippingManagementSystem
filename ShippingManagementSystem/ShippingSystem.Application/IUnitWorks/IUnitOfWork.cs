using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.IUnitWorks
{
    public interface IUnitOfWork
    {
        //Generic Repositories
        IGenericRepository<Branches> BranchRepository { get; }
        IGenericRepository<Cities> CityRepository { get; }
        IGenericRepository<Governorates> GovernorateRepository { get; }  
        IGenericRepository<Couriers> CourierRepository { get; }
        IGenericRepository<Employees> EmployeeRepository { get; }
        IGenericRepository<Orders> OrderRepository { get; }
        IGenericRepository<OrderStatus> OrderStatusRepository { get; }
        IGenericRepository<Regions> RegionRepository { get; }
        IGenericRepository<Merchants> MerchantRepository { get; }
        IGenericRepository<ShippingTypes> ShippingTypesRepository{ get; }
        IGenericRepository<PaymentMethods> PaymentMethodsRepository { get; }
        IGenericRepository<OrderItem> OrderItemsRepository { get; }
        IGenericRepository<WeightSettings> WeightSettingsRepository { get; }
        //Specific Repository
        ICityRepository SpecificCityRepository { get; }
        ICourierRepository SpecificCourierRepository { get; }
        IEmployeeRepository SpecificEmployeeRepository { get; }
        IGovernorateRepository SpecificGovernorateRepository { get; }
        IMerchantRepository SpecificMerchantRepository { get;}
        IOrderRepository SpecificOrderRepository { get; }
        IOrderItemsRepository SpecificOrderItemsRepo { get; }
        IWeightSettingsRepository SpecificWeightSettingsRepository { get; }
        //Save
        Task SaveAsync();

    }
}
