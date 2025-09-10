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
    public class EmployeeRepository:GenericRepository<Employees>,IEmployeeRepository
    {
        private readonly ShippingDbContext cxt;
        public EmployeeRepository(ShippingDbContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }

        public Task<List<Employees>> EmpListWithBranch(){
            return cxt.Employee.Include(e => e.Branch).Include(u=>u.User).ToListAsync();
        }
        public async Task<Employees> EmpWithUserById(int empId)
        {
            return await cxt.Employee.Include(e=>e.User).FirstOrDefaultAsync(e=>e.Id==empId);
        }
    }
}
