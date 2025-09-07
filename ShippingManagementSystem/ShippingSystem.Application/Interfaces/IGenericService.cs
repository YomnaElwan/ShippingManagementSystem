using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Application.Interfaces
{
    public interface IGenericService<T>where T:class
    {
        Task<List<T>> GetAllAsync();
        Task<T> GetByIdAsync(int Id);
        Task AddAsync(T obj);
        Task UpdateAsync(T obj);
        Task DeleteAsync(int Id);
        Task SaveAsync();

    }
}
