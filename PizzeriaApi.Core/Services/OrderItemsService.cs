using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.OrderItemReq;
using PizzeriaApi.Domain.UtilModels;

namespace PizzeriaApi.Core.Services
{
    public class OrderItemsService : IOrderItemsService
    {
        private readonly IOrderItemsRepo _orderItemsRepo;
        private readonly ILogger<OrderItemsService> _logger;

        public OrderItemsService(IOrderItemsRepo orderItemsRepo, ILogger<OrderItemsService> logger)
        {
            _orderItemsRepo = orderItemsRepo;
            _logger = logger;
        }

        public async Task<OperationResult<bool?>> AddManyOrderItemAsync(IEnumerable<AddOrderItemReq> reqs)
        {
            try
            {
                var orderItems = MapManyAddRequestBack(reqs);

                var result = await  _orderItemsRepo.AddManyOrderItemAsync(orderItems);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order items added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to add order items");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding many order items");
                return OperationResult<bool?>.Failure(null, "Error adding many order items");
            }
        }

        public async Task<OperationResult<bool?>> AddOneOrderItemAsync(AddOrderItemReq req)
        {
            try
            {
                var orderItem = MapAddRequestBack(req);

                var result = await _orderItemsRepo.AddOneOrderItemAsync(orderItem);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order item added successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to add order item");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding one order item");
                return OperationResult<bool?>.Failure(null, "Error adding one order item");
            }
        }

        public async Task<OperationResult<bool?>> DeleteOrderItemAsync(int orderItemId)
        {
            try
            {
                var result = await _orderItemsRepo.DeleteOrderItemAsync(orderItemId);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order item deleted successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to delete order item");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order item");
                return OperationResult<bool?>.Failure(null, "Error deleting order item");
            }
        }

        public async Task<OperationResult<OrderItemDTO?>> GetItemsByIdAsync(int orderItemsId)
        {
            try
            {
                var result = await _orderItemsRepo.GetItemsByIdAsync(orderItemsId);

                if (result != null)
                {
                    var orderItemDTO = MapOneOrderItem(result);
                    return OperationResult<OrderItemDTO?>.Success(orderItemDTO, "Order item retrieved successfully");
                }
                else
                {
                    return OperationResult<OrderItemDTO?>.Failure(null, "Order item not found");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order item by ID");
                return OperationResult<OrderItemDTO?>.Failure(null, "Error getting order item by ID");
            }
        }

        public async Task<OperationResult<IEnumerable<OrderItemDTO>>> GetOrderItemsByorderIdAsync(int orderId)
        {
            try
            {
                var result = await _orderItemsRepo.GetOrderItemsByorderIdAsync(orderId);

                if (result != null && result.Any())
                {
                    var orderItemsDTO = MapManyOrderItems(result);
                    return OperationResult<IEnumerable<OrderItemDTO>>.Success(orderItemsDTO, "Order items retrieved successfully");
                }
                else
                {
                    return OperationResult<IEnumerable<OrderItemDTO>>.Failure(null, "No order items found for the given order ID");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting order items by order ID");
                return OperationResult<IEnumerable<OrderItemDTO>>.Failure(null, "Error getting order items by order ID");
            }
        }

        public async Task<OperationResult<bool?>> UpdateOrderItemAsync(UpdateOrderItemReq req)
        {
            try
            {
                var newOrderItem = new OrderItem
                {
                    Id = req.OrderItemId,
                    Quantity = req.Quantity,
                };

                var result = await _orderItemsRepo.UpdateOrderItemAsync(newOrderItem);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order item updated successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to update order item");
                }
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error updating order item");
                return OperationResult<bool?>.Failure(null, "Error updating order item");
            }
        }

        private OrderItem MapAddRequestBack(AddOrderItemReq req)
        {
            try
            {
                return new OrderItem
                {
                    DishId = req.DishId,
                    OrderId = req.OrderId,
                    Quantity = req.Quantity,

                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping add request");
                throw;
            }
        }

        private IEnumerable<OrderItem> MapManyAddRequestBack(IEnumerable<AddOrderItemReq> reqs)
        {
            try
            {
                return reqs.Select(r => MapAddRequestBack(r));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping many add requests");
                throw;
            }
        }

        private OrderItemDTO MapOneOrderItem(OrderItem orderItem)
        {
            try
            {
                return new OrderItemDTO
                {
                    OrderItemId = orderItem.Id,
                    DishId = orderItem.DishId,
                    OrderId = orderItem.OrderId,
                    Quantity = orderItem.Quantity,
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping one order item");
                throw;
            }
        }

        private IEnumerable<OrderItemDTO> MapManyOrderItems(IEnumerable<OrderItem> orderItems)
        {
            try
            {
                return orderItems.Select(o => MapOneOrderItem(o));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping many order items");
                throw;
            }
        }
    }
}
