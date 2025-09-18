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
    public class OrderItemsService : GenericService<OrderItem>, IOrderItemService
    {
        private readonly IOrderItemsRepository orderItemRepo;
        public OrderItemsService(IOrderItemsRepository orderItemRepo):base(orderItemRepo)
        {
            this.orderItemRepo = orderItemRepo;
        }
        public Task<List<OrderItem>> GetOrderItemsByOrderId(int OrderId)
        {
            return orderItemRepo.GetOrderItemsByOrderId(OrderId);
        }
    }
}
