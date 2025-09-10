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
    public class EmployeeService:GenericService<Employees>,IEmployeeService
    {
        private readonly IEmployeeRepository empRepo;
        public EmployeeService(IEmployeeRepository empRepo) :base(empRepo)
        {
            this.empRepo = empRepo;
        }

        public Task<List<Employees>> EmpListWithBranch()
        {
            return empRepo.EmpListWithBranch();
        }

        public Task<Employees> EmpWithUserById(int empId)
        {
            return empRepo.EmpWithUserById(empId);
        }
    }
}
