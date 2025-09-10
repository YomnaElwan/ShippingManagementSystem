using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Interfaces
{
    public interface IEmployeeRepository:IGenericRepository<Employees>
    {
        public Task<List<Employees>> EmpListWithBranch();
        public Task<Employees> EmpWithUserById(int empId);
    }
}
