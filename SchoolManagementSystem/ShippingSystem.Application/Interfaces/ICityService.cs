using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface ICityService:IGenericService<Cities>
    {
        Task<List<Cities>> cityListByGov(int govId);
        Task<Cities> cityHasGov(int cityId);

    }
}
