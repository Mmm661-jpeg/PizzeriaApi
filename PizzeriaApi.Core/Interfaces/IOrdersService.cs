using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.OrderReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface IOrdersService
    {
        Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersByUserIdAsync(string userId);

        Task<OperationResult<OrderDTO?>> GetOrderByOrderIdAsync(int orderId);

        Task<OperationResult<IEnumerable<OrderDTO>>> GetAllOrdersAsync();

        Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersByDateAsync(DateTime? to, DateTime? from);

        Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersByStatusAsync(string status);

        Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersUsingBonusAsync();

        Task<OperationResult<bool?>> CreateOrderAsync(string userId);

        Task<OperationResult<bool?>> UpdateOrderStatusAsync(UpdateOrderStatusReq req);

        Task<OperationResult<bool?>> DeleteOrderAsync(int orderId);

        Task<OperationResult<bool?>> CancelOrderAsync(CancelOrderReq req);

        Task<OperationResult<OrderDTO>> GetPendingOrderForUserAsync(string userId);

        Task<OperationResult<bool?>> SetOrderPaid(SetOrderPaidReq req);
    }
}
