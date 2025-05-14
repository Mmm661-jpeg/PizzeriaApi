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

        Task<Order?> GetOrderByOrderIdAsync(int orderId);

        Task<IEnumerable<Order>> GetAllOrdersAsync();

        Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime? to, DateTime? from);

        Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status);

        Task<IEnumerable<Order>> GetOrdersUsingBonusAsync();

        Task<bool> CreateOrderAsync(Order order);

        Task<bool> UpdateOrderStatusAsync(int orderId,string status);

        Task<bool> DeleteOrderAsync(int orderId);

        Task<bool> CancelOrderAsync(int orderId, string userId,string? reason);

        Task<Order?> GetPendingOrderForUserAsync(string userId);

        Task<bool> SetOrderPaid(int orderId, string userId, decimal amountPaid,bool useBonus=false);





    }
}
