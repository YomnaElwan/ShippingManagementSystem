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
        private readonly ShippingDbContext _context;
        public GovernorateRepository(ShippingDbContext _context):base(_context)
        {
            this._context = _context;
        }

        public async Task<Governorates> govByIdIncludeRegion(int govId)
        {
            return await _context.Governorate.Include(r => r.Region).FirstOrDefaultAsync(g => g.Id == govId);
        }

        public async Task<List<Governorates>> regionGovsList(int regionId)
        {
            return await _context.Governorate.Where(g => g.RegionId == regionId).ToListAsync();
        }
    }
}
