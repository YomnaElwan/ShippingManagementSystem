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
    public class WeightSettingsRepository : GenericRepository<WeightSettings>, IWeightSettingsRepository
    {
        private readonly ShippingDbContext cxt;
        public WeightSettingsRepository(ShippingDbContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }

        public async Task<WeightSettings> GetById(int Id)
        {
            return await cxt.WeightSettings.Include(w => w.Cities).FirstOrDefaultAsync(w => w.Id == Id);
        }

        public async Task<WeightSettings> GetWeightSettByCityId(int cityId)
        {
            return await cxt.WeightSettings.Include(w => w.Cities).FirstOrDefaultAsync(w => w.CityId == cityId);

        }
    }
}
