using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Interfaces
{
    public interface ICourierRepository:IGenericRepository<Couriers>
    {
        Task<List<Couriers>> CourierList();
        Task<Couriers> CourierWithDataById(int CourierId);
    }
}
