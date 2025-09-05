using Microsoft.EntityFrameworkCore;
using ShippingSystem.Domain.Interfaces;
using ShippingSystem.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Infrastructure.Repositories
{
    public class GenericRepository<T>:IGenericRepository<T>where T:class
    {
        private readonly ShippingDbContext context;

        public GenericRepository(ShippingDbContext context)
        {
            this.context = context;
            
        }
        public async Task AddAsync(T obj)
        {
            await context.Set<T>().AddAsync(obj);        
        }

        //Hard Delete 
        //public async Task DeleteAsync(int Id)
        //{
        //    var obj = await context.Set<T>().FindAsync(Id);
        //    if (obj != null)
        //    {
        //        context.Set<T>().Remove(obj);
        //    }
        //}
    
        public async Task<List<T>> GetAllAsync()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetByIdAsync(int Id)
        {
            return await context.Set<T>().FindAsync(Id);
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }

        //SoftDelete
        public async Task DeleteAsync(int Id)
        {
            var obj = await context.Set<T>().FindAsync(Id);
            if (obj != null)
            {
                var prop = obj.GetType().GetProperty("IsActive");
                if (prop != null)
                {
                    prop.SetValue(obj, false);
                    context.Update(obj);
                }
                else
                {
                    context.Set<T>().Remove(obj);
                }
            }
        }

    }
}
