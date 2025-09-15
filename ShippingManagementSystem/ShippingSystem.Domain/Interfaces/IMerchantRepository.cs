using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Interfaces
{
    public interface IMerchantRepository:IGenericRepository<Merchants>
    {
        public Task<List<Merchants>> SpecialMerchantsList();
        public Task<Merchants> GetMerchantById(int MerchantId);
    }
}
