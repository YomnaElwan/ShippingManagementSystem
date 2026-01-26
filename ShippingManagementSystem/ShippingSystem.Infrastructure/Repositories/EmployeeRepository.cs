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
        public EmployeeRepository(ShippingDbContext context):base(context)
        {
        }

        public Task<List<Employees>> EmpListWithBranch(){
            return context.Employee.Include(e => e.Branch).Include(u=>u.User).ToListAsync();
        }
        public async Task<Employees> EmpWithUserById(int empId)
        {
            return await context.Employee.Include(e=>e.User).Include(b=>b.Branch).FirstOrDefaultAsync(e=>e.Id==empId);
        }
    }
}
