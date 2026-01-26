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
    public class CityRepository : GenericRepository<Cities>, ICityRepository
    {
        public CityRepository(ShippingDbContext context):base(context)
        {
        }

        public async Task<Cities> cityHasGov(int cityId)
        {
            return await context.City.Include(c => c.Governorate).FirstOrDefaultAsync(c => c.Id == cityId);
        }

        public async Task<List<Cities>> cityListByGov(int govId)
        {
            return await context.City.Where(c => c.GovernorateId == govId).ToListAsync();    
        }
    }
}
