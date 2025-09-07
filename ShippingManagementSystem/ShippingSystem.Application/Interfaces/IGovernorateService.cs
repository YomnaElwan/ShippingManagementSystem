using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IGovernorateService:IGenericService<Governorates>
    {
        Task<List<Governorates>> regionGovsList(int regionId);
        Task<Governorates> govByIdIncludeRegion(int govId);


    }
}
