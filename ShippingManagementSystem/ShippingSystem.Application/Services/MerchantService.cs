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
    public class MerchantService : GenericService<Merchants>, IMerchantService
    {
        private readonly IMerchantRepository merchantRepo;
        public MerchantService(IMerchantRepository merchantRepo):base(merchantRepo)
        {
            this.merchantRepo = merchantRepo;
        }

        public Task<Merchants> GetMerchantById(int MerchantId)
        {
            return merchantRepo.GetMerchantById(MerchantId);
        }

        public Task<List<Merchants>> SpecialMerchantsList()
        {
            return merchantRepo.SpecialMerchantsList();
        }
    }
}
