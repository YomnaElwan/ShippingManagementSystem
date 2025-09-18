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
    public class OrderItemsRepository : GenericRepository<OrderItem>, IOrderItemsRepository
    {
        private readonly ShippingDbContext cxt;
        public OrderItemsRepository(ShippingDbContext cxt):base(cxt)
        {
            this.cxt = cxt;
        }
        public Task<List<OrderItem>> GetOrderItemsByOrderId(int OrderId)
        {
            return cxt.OrderItems.Where(order => order.OrderId == OrderId).ToListAsync();
        }
    }
}
