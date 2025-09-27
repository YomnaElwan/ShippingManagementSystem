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
    public class OrderService : GenericService<Orders>, IOrderService
    {
        private readonly IOrderRepository orderRepo;
        public OrderService(IOrderRepository orderRepo):base(orderRepo)
        {
            this.orderRepo = orderRepo;
        }

        public Task<Orders> GetOrderById(int Id)
        {
            return orderRepo.GetOrderById(Id);
        }

        public Task<List<Orders>> GetOrdersByOrderStsId(int orderStsId)
        {
            return orderRepo.GetOrdersByOrderStsId(orderStsId);
        }


        public Task<List<Orders>> GetSpecialOrderList()
        {
            return orderRepo.GetSpecialOrderList();
        }
    }
}
