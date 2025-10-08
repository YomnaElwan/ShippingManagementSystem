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
    public class OrderRepository : GenericRepository<Orders>, IOrderRepository
    {
        private readonly ShippingDbContext context;
        public OrderRepository(ShippingDbContext context) :base(context)
        {
            this.context = context;
        }

        public async Task<Orders> GetOrderById(int Id)
        {
            return context.Order.Include(g=>g.Governorate).Include(c=>c.City).Include(s => s.OrderStatus).FirstOrDefault(o => o.Id == Id);
        }

        public Task<List<Orders>> GetOrdersByOrderStsId(int orderStsId)
        {
            return context.Order.Where(order => order.OrderStatusId == orderStsId).Include(c => c.City).Include(g => g.Governorate).Include(o => o.OrderStatus).Include(m=>m.Merchant).Include(c=>c.Courier).ToListAsync();
        }

        public Task<List<Orders>> GetSpecialOrderList()
        {
            return context.Order.Include(c => c.City).Include(g => g.Governorate).Include(o=>o.OrderStatus).Include(cr=>cr.Courier).Include(u=>u.Merchant).ToListAsync();
        }
     
    }
}
