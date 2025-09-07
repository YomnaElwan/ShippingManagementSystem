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
    public class CityService:GenericService<Cities>,ICityService
    {
        private readonly ICityRepository _cityRepo;
        public CityService(ICityRepository _cityRepo):base(_cityRepo)
        {
            this._cityRepo = _cityRepo;   
        }

        public async Task<Cities> cityHasGov(int cityId)
        {
            return await _cityRepo.cityHasGov(cityId);
        }

        public async Task<List<Cities>> cityListByGov(int govId)
        {
          return await _cityRepo.cityListByGov(govId);
        }
    }
}
