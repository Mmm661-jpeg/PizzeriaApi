using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.OrderItemReq;
using PizzeriaApi.Domain.UtilModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Core.Interfaces
{
    public interface IOrderItemsService
    {
        Task<OperationResult<OrderItemDTO?>> GetItemsByIdAsync(int orderItemsId);

        Task<OperationResult<IEnumerable<OrderItemDTO>>> GetOrderItemsByorderIdAsync(int orderId);

        Task<OperationResult<bool?>> AddOneOrderItemAsync(AddOrderItemReq req);

        Task<OperationResult<bool?>> AddManyOrderItemAsync(IEnumerable<AddOrderItemReq> reqs);
        Task<OperationResult<bool?>> DeleteOrderItemAsync(int orderItemId);
        Task<OperationResult<bool?>> UpdateOrderItemAsync(UpdateOrderItemReq req);
    }
}
