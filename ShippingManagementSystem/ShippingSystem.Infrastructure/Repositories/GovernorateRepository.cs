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
    public class GovernorateRepository : GenericRepository<Governorates>, IGovernorateRepository
    {
        public GovernorateRepository(ShippingDbContext context):base(context)
        {
        }

        public async Task<Governorates> govByIdIncludeRegion(int govId)
        {
            return await context.Governorate.Include(r => r.Region).FirstOrDefaultAsync(g => g.Id == govId);
        }

        public async Task<List<Governorates>> regionGovsList(int regionId)
        {
            return await context.Governorate.Where(g => g.RegionId == regionId).ToListAsync();
        }
    }
}
