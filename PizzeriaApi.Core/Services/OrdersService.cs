using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.OrderReq;
using PizzeriaApi.Domain.UtilModels;

namespace PizzeriaApi.Core.Services
{
    public class OrdersService : IOrdersService

    {
        private readonly IOrdersRepo _ordersRepo;
        private readonly ILogger<OrdersService> _logger;

        public OrdersService(IOrdersRepo ordersRepo, ILogger<OrdersService> logger)
        {
            _ordersRepo = ordersRepo;
            _logger = logger;
        }

        public async Task<OperationResult<bool?>> CancelOrderAsync(CancelOrderReq req)
        {
            try
            {
                var result = await _ordersRepo.CancelOrderAsync(req.OrderId, req.UserId, req.Reason);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order cancelled successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to cancel order");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order");
                return OperationResult<bool?>.Failure(null, "Error cancelling order");
            }
        }

        public async Task<OperationResult<bool?>> CreateOrderAsync(CreateOrderReq req)
        {
            try
            {
                var orderToCreate = new Order()
                {
                    UserId = req.UserId,
                };

                var result = await _ordersRepo.CreateOrderAsync(orderToCreate);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order created successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to create order");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order");
                return OperationResult<bool?>.Failure(null, "Error creating order");
            }
        }

        public async Task<OperationResult<bool?>> DeleteOrderAsync(int orderId)
        {
            try
            {
                var result = await _ordersRepo.DeleteOrderAsync(orderId);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order deleted successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to delete order");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order");
                return OperationResult<bool?>.Failure(null, "Error deleting order");
            }
        }

        public async Task<OperationResult<IEnumerable<OrderDTO>>> GetAllOrdersAsync()
        {
            try
            {
                var result = await _ordersRepo.GetAllOrdersAsync();

                if (result == null || !result.Any())
                {
                    return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "No orders found");
                }

                var mappedResult = MapManyOrder(result);

                return OperationResult<IEnumerable<OrderDTO>>.Success(mappedResult, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all orders");
                return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "Error getting all orders");
            }
        }

        public async Task<OperationResult<OrderDTO?>> GetOrderByOrderIdAsync(int orderId)
        {
            try
            {
                var result = await _ordersRepo.GetOrderByOrderIdAsync(orderId);

                if (result == null)
                {
                    return OperationResult<OrderDTO?>.Failure(null, "Order not found");
                }

                var mappedResult = MapOneOrder(result);

                return OperationResult<OrderDTO?>.Success(mappedResult, "Order retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting order by orderId");
                return OperationResult<OrderDTO?>.Failure(null, "Error getting order by orderId");
            }
        }

        public async Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersByDateAsync(DateTime? to, DateTime? from)
        {
            try
            {
                var result = await _ordersRepo.GetOrdersByDateAsync(to, from);
                if (result == null || !result.Any())
                {
                    return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "No orders found");
                }
                var mappedResult = MapManyOrder(result);
                return OperationResult<IEnumerable<OrderDTO>>.Success(mappedResult, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by date");
                return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "Error getting orders by date");
            }
        }

        public async Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersByStatusAsync(string status)
        {
            try
            {
                var result = await _ordersRepo.GetOrdersByStatusAsync(status);

                if (result == null || !result.Any())
                {
                    return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "No orders found");
                }

                var mappedResult = MapManyOrder(result);
                return OperationResult<IEnumerable<OrderDTO>>.Success(mappedResult, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, " Error getting orders by status");
                return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "Error getting orders by status");
            }
        }

        public async Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersByUserIdAsync(string userId)
        {
            try
            {
                var result = await _ordersRepo.GetOrdersByUserIdAsync(userId);
                if (result == null || !result.Any())
                {
                    return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "No orders found for the given user ID");
                }

                var mappedResult = MapManyOrder(result);

                return OperationResult<IEnumerable<OrderDTO>>.Success(mappedResult, "Orders retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders by userId");
                return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "Error getting orders by userId");
            }
        }

        public async Task<OperationResult<IEnumerable<OrderDTO>>> GetOrdersUsingBonusAsync()
        {
            try
            {
                var result = await _ordersRepo.GetOrdersUsingBonusAsync();

                if (result == null || !result.Any())
                {
                    return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "No orders found using bonus");
                }

                var mappedResult = MapManyOrder(result);

                return OperationResult<IEnumerable<OrderDTO>>.Success(mappedResult, "Orders using bonus retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting orders using bonus");
                return OperationResult<IEnumerable<OrderDTO>>.Failure(null, "Error getting orders using bonus");
            }
        }

        public async Task<OperationResult<OrderDTO>> GetPendingOrderForUserAsync(string userId)
        {
            try
            {
                var result = await _ordersRepo.GetPendingOrderForUserAsync(userId);

                if (result == null)
                {
                    return OperationResult<OrderDTO>.Failure(null, "No pending order found for the given user ID");
                }

                var mappedResult = MapOneOrder(result);

                return OperationResult<OrderDTO>.Success(mappedResult, "Pending order retrieved successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting pending order for user");
                return OperationResult<OrderDTO>.Failure(null, "Error getting pending order for user");
            }
        }

        public async Task<OperationResult<bool?>> SetOrderPaid(SetOrderPaidReq req)
        {
            try
            {
                var result = await _ordersRepo.SetOrderPaid(req.OrderId, req.UserId, req.AmountPaid, req.UseBonus);

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order set as paid successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to set order as paid");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error setting order as paid");
                return OperationResult<bool?>.Failure(null, "Error setting order as paid");
            }
        }

        public async Task<OperationResult<bool?>> UpdateOrderStatusAsync(UpdateOrderStatusReq req)
        {
            try
            {
                if (Enum.IsDefined(typeof(OrderStatus), req.OrderStatus) == false)
                {
                    return OperationResult<bool?>.Failure(null, "Invalid order status");
                }

                var parsedStatus = (OrderStatus)req.OrderStatus;

                var result = await _ordersRepo.UpdateOrderStatusAsync(req.OrderID, parsedStatus.ToString());

                if (result)
                {
                    return OperationResult<bool?>.Success(true, "Order status updated successfully");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to update order status");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order status");
                return OperationResult<bool?>.Failure(null, "Error updating order status");
            }
        }


        private OrderDTO MapOneOrder(Order order)
        {
            return new OrderDTO()
            {
                Id = order.Id,
                UserId = order.UserId,
                CreatedAt = order.CreatedAt,
                TotalPrice = order.TotalPrice,
                Status = order.Status,
                UsedBonusReward = order.UsedBonusReward,
                CancelledAt = order.CancelledAt,
                CancellationReason = order.CancellationReason,
                FinalizedAt = order.FinalizedAt
            };
        }

        private IEnumerable<OrderDTO> MapManyOrder(IEnumerable<Order> orders)
        {
            try
            {
                return orders.Select(o => MapOneOrder(o));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error mapping orders");
                throw;
            }
        }
    }
}
