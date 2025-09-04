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
        private readonly ShippingDbContext cxt;
        public CityRepository(ShippingDbContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }
        public async Task<List<Cities>> cityListByGov(int govId)
        {
            return await cxt.City.Where(c => c.GovernorateId == govId).ToListAsync();    
        }
    }
}
