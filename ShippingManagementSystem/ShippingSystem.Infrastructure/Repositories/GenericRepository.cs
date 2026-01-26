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
        protected readonly ShippingDbContext context;

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
        public async Task<List<T>> ActiveList()
        {
            #region Cause error
            /* (bool)prop.GetValue(x)) is a method call on c# object
               .where --> EF --> turn it into sql
                method calls like --> GetValue --> can't be turned into SQL
                error --> The LINQ expression ... could not be translated*/
            //var prop = typeof(T).GetProperty("IsActive");
            //if(prop==null || prop.PropertyType != typeof(bool))
            //    throw new InvalidOperationException("This entity doesn't have a property called 'IsActive'");
            //return await context.Set<T>().Where(x => (bool)prop.GetValue(x)).ToListAsync();
            #endregion
            #region Solve
            // all is c# object so I can use method calls like 'Get Value' because I won't deal with SQL
            var all=await context.Set<T>().ToListAsync();
            var prop = typeof(T).GetProperty("IsActive");
            if (prop == null || prop.PropertyType != typeof(bool))
                throw new InvalidOperationException("This entity doesn't have a property called 'IsActive'");
            return all.Where(x => (bool)prop.GetValue(x)).ToList();
            #endregion
        }
        public async Task<T> GetByIdAsync(int Id)
        {
            return await context.Set<T>().FindAsync(Id);
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

        public async Task UpdateAsync(T obj)
        {
            var existing = await context.Set<T>().FindAsync(obj.GetType().GetProperty("Id").GetValue(obj));
            if (existing != null)
            {
                context.Entry(existing).CurrentValues.SetValues(obj);
            }
        }
    }
}
