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
    public class GovernorateService : GenericService<Governorates>, IGovernorateService
    {
        private readonly IGovernorateRepository govsRepo;
        public GovernorateService(IGovernorateRepository govsRepo) : base(govsRepo)
        {
            this.govsRepo = govsRepo;
        }

        public async Task<List<Governorates>> govsIncludeRegion()
        {
            return await govsRepo.govsIncludeRegion();
        }

        public async Task<List<Governorates>> regionGovsList(int regionId)
        {
            return await govsRepo.regionGovsList(regionId);
        }
    }
}
