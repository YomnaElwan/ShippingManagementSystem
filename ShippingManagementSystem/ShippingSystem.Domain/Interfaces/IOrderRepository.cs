using ShippingSystem.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShippingSystem.Domain.Interfaces
{
    public interface IOrderRepository:IGenericRepository<Orders>
    {
        public Task<List<Orders>> GetSpecialOrderList();
        public Task<Orders> GetOrderById(int Id);
        public Task<List<Orders>> GetOrdersByOrderStsId(int orderStsId);
        public Task<List<Orders>> GetOrdersByDate(DateTime FromDate, DateTime ToDate);
    }
}
