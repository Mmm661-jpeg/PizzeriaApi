using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface IOrdersRepo
    {
        Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId);

        Task<Order?> GetOrdersByOrderIdAsync(int orderId);

        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime? to, DateTime? from);

        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);

        Task<bool> CreateOrderAsync(Order order);

        Task<bool> UpdateOrderAsync(Order order);

        Task<bool> DeleteOrderAsync(int orderId);
    }
}
