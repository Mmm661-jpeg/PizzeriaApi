using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Interfaces
{
    public interface IOrderItemsRepo
    {
        Task<OrderItem?> GetItemsByIdAsync(int orderItemsId);

        Task<IEnumerable<OrderItem>> GetOrderItemsByorderIdAsync(int orderId);

        Task<bool> AddOneOrderItemAsync(OrderItem item);

        Task<bool> AddManyOrderItemAsync(IEnumerable<OrderItem> items);
        Task<bool> DeleteOrderItemAsync(int orderItemId);
        Task<bool> UpdateOrderItemAsync(OrderItem orderItem);
    }
}
