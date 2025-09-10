using Microsoft.EntityFrameworkCore;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Infrastructure.Repositories
{
    public class CourierRepository : GenericRepository<Couriers>, ICourierRepository
    {
        private readonly ShippingDbContext context;
        public CourierRepository(ShippingDbContext context):base(context)
        {
            this.context = context;
        }

        public Task<List<Couriers>> CourierList()
        {
            return context.Courier.Include(c => c.User).Include(c => c.Branch).ToListAsync();
        }
        public Task<Couriers> CourierWithDataById(int CourierId)
        {
            return context.Courier.Include(c => c.User).Include(c => c.Governorate).Include(c => c.Branch).FirstOrDefaultAsync(c=>c.Id==CourierId);
        }
    }
}
