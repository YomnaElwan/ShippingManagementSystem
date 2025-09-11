using Microsoft.EntityFrameworkCore;
using ShippingSystem.Domain.Entities;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Infrastructure.Repositories
{
    public class MerchantRepository : GenericRepository<Merchants>, IMerchantRepository
    {
        private readonly ShippingDbContext cxt;
        public MerchantRepository(ShippingDbContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }
        public Task<List<Merchants>> SpecialMerchantsList()
        {
            return cxt.Merchant.Include(m => m.Branch).Include(m => m.User).ToListAsync();
        }
    }
}
