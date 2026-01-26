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
        public WeightSettingsRepository(ShippingDbContext context):base(context)
        {
           
        }
        public async Task<WeightSettings> GetById(int Id)
        {
            return await context.WeightSettings.Include(w => w.Cities).FirstOrDefaultAsync(w => w.Id == Id);
        }

        public async Task<WeightSettings> GetWeightSettByCityId(int cityId)
        {
            return await context.WeightSettings.Include(w => w.Cities).FirstOrDefaultAsync(w => w.CityId == cityId);

        }
        public async Task<WeightSettings> GetWeightSettingsIncludeCity(int id)
        {
            return await context.WeightSettings.Include(w => w.Cities).FirstOrDefaultAsync(w=>w.Id==id);
        }

    }
}
