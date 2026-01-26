using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IBranchService
    {
        Task AddBranch(Branches addBranch);
        Task UpdateBranch(Branches updateBranch);
    }
}
