using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
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
        public MerchantRepository(ShippingDbContext context):base(context)
        {
        }
        public Task<List<Merchants>> SpecialMerchantsList()
        {
            return context.Merchant.Include(m => m.Branch).Include(m => m.User).ToListAsync();
        }
        public Task<Merchants> GetMerchantById(int MerchantId)
        {
            return context.Merchant.Include(m => m.User).Include(m => m.Branch).Include(m => m.Governorate).Include(m => m.City).FirstOrDefaultAsync(m => m.Id == MerchantId);
        }
       
    }
}
