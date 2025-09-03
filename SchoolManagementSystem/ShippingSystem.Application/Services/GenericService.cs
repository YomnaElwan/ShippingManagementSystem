using ShippingSystem.Application.Interfaces;
using ShippingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Services
{
    public class GenericService<T> : IGenericService<T> where T : class
    {
        private readonly IGenericRepository<T> _genericRepo;
        public GenericService(IGenericRepository<T> genericRepo)
        {
            _genericRepo = genericRepo;   
        }
        public async Task AddAsync(T obj)
        {
            await _genericRepo.AddAsync(obj);
        }

        public async Task DeleteAsync(int Id)
        {
            await _genericRepo.DeleteAsync(Id);
        }

        public async Task<List<T>> GetAllAsync()
        {
           return await _genericRepo.GetAllAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await _genericRepo.GetByIdAsync(Id);
        }

        public async Task SaveAsync()
        {
            await _genericRepo.SaveAsync();
        }

       
    }
}
