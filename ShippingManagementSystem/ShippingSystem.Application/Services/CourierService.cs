using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Services
{
    public class CourierService:GenericService<Couriers>,ICourierService
    {
        private readonly ICourierRepository courierRepo;
        public CourierService(ICourierRepository courierRepo) :base(courierRepo)
        {
            this.courierRepo = courierRepo;
        }

        public Task<List<Couriers>> CourierList()
        {
            return courierRepo.CourierList();
        }

        public Task<Couriers> CourierWithDataById(int CourierId)
        {
            return courierRepo.CourierWithDataById(CourierId);
        }
    }
}
