using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IRegionService
    {
        Task AddNewRegion(Regions addRegion);
        Task UpdateRegion(Regions updateRegion);
    }
}
