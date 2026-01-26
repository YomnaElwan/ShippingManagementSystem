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
        public OrderItemsRepository(ShippingDbContext context):base(context)
        {
        }
        public Task<List<OrderItem>> GetOrderItemsByOrderId(int OrderId)
        {
            return context.OrderItems.Where(order => order.OrderId == OrderId).ToListAsync();
        }
    }
}
