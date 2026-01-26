using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Domain.IUnitWorks;
using ShippingSystem.Infrastructure.Data;
using ShippingSystem.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Infrastructure.UnitWorks
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly ShippingDbContext context;
        private IGenericRepository<Branches> branchRepo;
        private IGenericRepository<Cities> cityRepo;
        private IGenericRepository<Governorates> govRepo;
        private IGenericRepository<Couriers> courierRepo;
        private IGenericRepository<Employees> empRepo;
        private IGenericRepository<Orders> orderRep;
        private IGenericRepository<OrderStatus> orderStsRepo;
        private IGenericRepository<Regions> regionRepo;
        private IGenericRepository<Merchants> merchantRepo;
        private IGenericRepository<ShippingTypes> shippingTypesRepo;
        private IGenericRepository<PaymentMethods> paymentMethodsRepo;
        private IGenericRepository<OrderItem> orderItemsRepo;
        private IGenericRepository<WeightSettings> weightSetsRepo;
        private ICityRepository specificCityRepo;
        private ICourierRepository specificCourierRepo;
        private IEmployeeRepository specificEmpRepo;
        private IGovernorateRepository specificGovRepo;
        private IMerchantRepository specificMerchantRepo;
        private IOrderRepository specificOrderRepo;
        private IOrderItemsRepository specificOrderItemsRepo;
        private IWeightSettingsRepository specificWeightSetsRepo;
        



        public UnitOfWork(ShippingDbContext context)
        {
            this.context = context;
            
        }
        public IGenericRepository<Branches> BranchRepository
        {
            get
            {
                if (branchRepo == null)
                    branchRepo = new GenericRepository<Branches>(context);
                return branchRepo;
            }

        }
        public IGenericRepository<Cities> CityRepository { get
            {
                if (cityRepo == null)
                    cityRepo = new GenericRepository<Cities>(context);
                return cityRepo;
            }
        }
        public IGenericRepository<Governorates> GovernorateRepository { get {

                if (govRepo == null)
                    govRepo = new GenericRepository<Governorates>(context);
                return govRepo;

            }}
        public IGenericRepository<Couriers> CourierRepository { get {
                if (courierRepo == null)
                    courierRepo = new GenericRepository<Couriers>(context);
                return courierRepo;
            } }
        public IGenericRepository<Employees> EmployeeRepository { get {
                if (empRepo == null)
                    empRepo = new GenericRepository<Employees>(context);
                return empRepo;
            } }
        public IGenericRepository<Orders> OrderRepository { get
            {
                if(orderRep==null)
                    orderRep = new GenericRepository<Orders>(context);
                return orderRep;
            } }
        public IGenericRepository<OrderStatus> OrderStatusRepository { get 
            {
                if (orderStsRepo == null)
                    orderStsRepo = new GenericRepository<OrderStatus>(context);
                return orderStsRepo;
            } }
        public IGenericRepository<Regions> RegionRepository { get
            {
                if (regionRepo == null)
                    regionRepo = new GenericRepository<Regions>(context);
                return regionRepo;
            } }
        public IGenericRepository<Merchants> MerchantRepository { get 
            {
                if (merchantRepo == null)
                    merchantRepo = new GenericRepository<Merchants>(context);
                return merchantRepo;
            } }
        public IGenericRepository<ShippingTypes> ShippingTypesRepository { get
            {
                if (shippingTypesRepo == null)
                    shippingTypesRepo = new GenericRepository<ShippingTypes>(context);
                return shippingTypesRepo;
            } }
        public IGenericRepository<PaymentMethods> PaymentMethodsRepository { get 
            {
                if (paymentMethodsRepo == null)
                    paymentMethodsRepo = new GenericRepository<PaymentMethods>(context);
                return paymentMethodsRepo;
            } }
        public IGenericRepository<OrderItem> OrderItemsRepository { get 
            {
                if (orderItemsRepo == null)
                    orderItemsRepo = new GenericRepository<OrderItem>(context);
                return orderItemsRepo;
            } }
        public IGenericRepository<WeightSettings> WeightSettingsRepository { get 
            {
                if (weightSetsRepo == null)
                    weightSetsRepo = new GenericRepository<WeightSettings>(context);
                return weightSetsRepo;
            } }
        

        public ICityRepository SpecificCityRepository { get 
            {
                if (specificCityRepo == null)
                    specificCityRepo = new CityRepository(context);
                return specificCityRepo; 
            } }
        public ICourierRepository SpecificCourierRepository { get
            {
                if (specificCourierRepo == null)
                    specificCourierRepo = new CourierRepository(context);
                return specificCourierRepo;
            } }
        public IEmployeeRepository SpecificEmployeeRepository { get 
            {
                if (specificEmpRepo == null)
                    specificEmpRepo = new EmployeeRepository(context);
                return specificEmpRepo;
            } }
        public IGovernorateRepository SpecificGovernorateRepository { get 
            {
                if (specificGovRepo == null)
                    specificGovRepo = new GovernorateRepository(context);
                return specificGovRepo;
            
            } }
        public IMerchantRepository SpecificMerchantRepository { get 
            {
                if (specificMerchantRepo == null)
                    specificMerchantRepo = new MerchantRepository(context);
                return specificMerchantRepo;
            } }
        public IOrderRepository SpecificOrderRepository { get 
            {
                if (specificOrderRepo == null)
                    specificOrderRepo = new OrderRepository(context);
                return specificOrderRepo;
            } }
        public IOrderItemsRepository SpecificOrderItemsRepo{ get 
            {
                if (specificOrderItemsRepo == null)
                    specificOrderItemsRepo = new OrderItemsRepository(context);
                return specificOrderItemsRepo;
            } }
        public IWeightSettingsRepository SpecificWeightSettingsRepository { get 
            {
                if (specificWeightSetsRepo == null)
                    specificWeightSetsRepo = new WeightSettingsRepository(context);
                return specificWeightSetsRepo;
            } }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
