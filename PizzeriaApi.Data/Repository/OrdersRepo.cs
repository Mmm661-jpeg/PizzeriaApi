using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;

namespace PizzeriaApi.Data.Repository
{
    public class OrdersRepo : IOrdersRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<OrdersRepo> _logger;

        const int minBonusPoints = 100; //minimum bonus required to use bonus points

        public OrdersRepo(PizzeriaApiDBContext dbContext, ILogger<OrdersRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> CreateOrderAsync(Order order)
        {
            if (order == null)
            {
                _logger.LogWarning("CreateOrderAsync: Order input is null or empty.");
                return false;
            }


            try
            {
                var userOrderExists = await _dbContext.Orders
                    .AnyAsync(o => o.UserId == order.UserId && o.Status == OrderStatus.Pending);

                if (userOrderExists)
                {
                    _logger.LogInformation("CreateOrderAsync: Order for user {UserId} already exists.", order.UserId);
                    return false;
                }


                await _dbContext.AddAsync(order);
                await _dbContext.SaveChangesAsync();


                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CreateOrderAsync failed");
                return false;
            }
        }

        public async Task<bool> DeleteOrderAsync(int orderId)
        {
            if (orderId <= 0)
            {
                _logger.LogWarning("DeleteOrderAsync: OrderId invalid");
                return false;
            }
            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogDebug("DeleteOrderAsync: No order found with id: {OrderId}", orderId);
                    return false;
                }

                _dbContext.Orders.Remove(order);
                var affected = await _dbContext.SaveChangesAsync();

                if (affected <= 0)
                {
                    _logger.LogDebug("DeleteOrderAsync: Deleting order with id: {OrderId} failed", orderId);
                    return false;
                }

                return true;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: DeleteOrderAsync failed");
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            try
            {
                var orders = await _dbContext.Orders.ToListAsync();

                if (orders == null)
                {
                    _logger.LogDebug("GetAllOrdersAsync: No orders found.");
                    return null;
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetAllOrdersAsync failed");
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateAsync(DateTime? to, DateTime? from)
        {
            if (from == null && to == null)
            {
                _logger.LogWarning("GetOrdersByDateAsync: Both 'from' and 'to' are null.");
                return null;
            }

            try
            {
                var query = _dbContext.Orders.AsQueryable();

                if (from.HasValue)
                {
                    query = query.Where(o => o.CreatedAt >= from.Value);
                }

                if (to.HasValue)
                {
                    query = query.Where(o => o.CreatedAt <= to.Value);
                }

                var orders = await query
                    .AsNoTracking()
                    .ToListAsync();

                if (!orders.Any())
                {
                    _logger.LogDebug("GetOrdersByDateAsync: No orders found in given range.");
                }

                return orders;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOrdersByDateAsync failed.");
                return null;
            }
        }

        public async Task<Order?> GetOrdersByOrderIdAsync(int orderId)
        {
            if (orderId <= 0)
            {
                _logger.LogWarning("GetOrdersByOrderIdAsync: OrderId invalid");
                return null;
            }

            try
            {
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogDebug("GetOrdersByOrderIdAsync: No order found");
                    return null;
                }

                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOrdersByOrderIdAsync failed");
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(string status)
        {
            if(string.IsNullOrEmpty(status))
            {
                _logger.LogWarning("GetOrdersByStatusAsync: Status invalid");
                return null;
            }
           
            try
            {
                if (Enum.TryParse<OrderStatus>(status, true, out var orderStatus) == false)
                {
                    _logger.LogWarning("GetOrdersByStatusAsync: Status invalid");
                    return null;
                }

                var orders = await _dbContext.Orders
                    .Where(o => o.Status == orderStatus)
                    .ToListAsync();

                if (!orders.Any())
                {
                    _logger.LogDebug("GetOrdersByStatusAsync: No orders found with status: {Status}", status);
                    return null;
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOrdersByStatusAsync failed");
                return null;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("GetOrdersByUserIdAsync: UserId invalid");
                return null;
            }

            try
            {
                var orders = await _dbContext.Orders.Where(o => o.UserId == userId)
                                                    .ToListAsync();
                if (!orders.Any())
                {
                    _logger.LogDebug("GetOrdersByUserIdAsync: No orders found");
                    return null;
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOrdersByUserIdAsync failed!");
                return null;
            }
        }

        public async Task<bool> SetOrderPaid(int orderId, string userId, decimal amountPaid, bool useBonus = false)
        {
            if (orderId <= 0)
            {
                _logger.LogWarning("SetOrderPaid: OrderId invalid");
                return false;
            }

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("SetOrderPaid: UserId invalid");
                return false;
            }

            if (amountPaid <= 0)
            {
                _logger.LogWarning("SetOrderPaid: AmountPaid invalid");
                return false;
            }

            try
            {
                var order = await _dbContext.Orders
                    .Include(o => o.User)
                    .Include(o => o.Items)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

                if (order == null)
                {
                    _logger.LogWarning("SetOrderPaid: No order found with id: {OrderId}", orderId);
                    return false;

                }



                bool isPremiumUser = await _dbContext.UserRoles
                    .AnyAsync(ur => ur.UserId == userId && ur.RoleId.Equals("PremiumUser", StringComparison.OrdinalIgnoreCase));

                if (Enum.TryParse<OrderStatus>(order.Status.ToString(), out var status))
                {
                    if (status != OrderStatus.Pending)
                    {
                        _logger.LogWarning("SetOrderPaid: Order with id: {OrderId} is not in pending status.", orderId);
                        return false;
                    }
                }

                bool threePizzas = order.Items.Count(i => i.Dish.Category.Name.Equals("Pizza", StringComparison.OrdinalIgnoreCase)) >= 3;


                if (isPremiumUser)
                {
                    if (useBonus && order.User.BonusPoints > minBonusPoints)
                    {
                        order.UsedBonusReward = true;
                        order.User.BonusPoints = 0;
                    }
                    else
                    {
                        order.UsedBonusReward = false;
                        order.User.BonusPoints += 10;
                    }


                    if (threePizzas)
                    {
                        order.TotalPrice = Math.Max(0, order.TotalPrice - (order.TotalPrice * 0.2m)); //20% discount
                    }



                }


                order.FinalizedAt = DateTime.UtcNow;
                order.Status = OrderStatus.Paid;

                await _dbContext.SaveChangesAsync();
                return true;




            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: SetOrderPaid failed");
                return false;
            }
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId,string status) //Status
        {
            if(string.IsNullOrEmpty(status))
            {
                _logger.LogWarning("UpdateOrderAsync: Status invalid");
                return false;
            }

            try
            {
                if(Enum.TryParse<OrderStatus>(status,true,out var newOrderStatus) == false)
                {
                    _logger.LogWarning("UpdateOrderAsync: Status invalid");
                    return false;
                }

                if(newOrderStatus != OrderStatus.Paid && newOrderStatus != OrderStatus.Delivered)
                {
                    // Only allows updating an order to Paid or Deliver
                    _logger.LogWarning("UpdateOrderAsync: Status invalid");
                    return false;
                }

                var affected = await _dbContext.Orders.Where(o => o.Id == orderId)
                                         .ExecuteUpdateAsync(setter =>
                                        setter.SetProperty(o => o.Status, newOrderStatus));

                if (affected == 0)
                {
                    _logger.LogDebug("UpdateOrderAsync: Updating order with id: {OrderId} failed", orderId);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: UpdateOrderAsync failed!");
                return false;
            }
        }

        public async Task<bool> CancelOrderAsync(int orderId, string userId, string? reason)
        {
            if (string.IsNullOrEmpty(userId) || orderId <= 0)
            {
                _logger.LogWarning("CancellOrderAsync: UserId or OrderId invalid");
                return false;
            }

            try
            {
                var order = await _dbContext.Orders
                                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

                if (order == null || order.Status != OrderStatus.Pending)
                {
                    return false;
                }


                order.Status = OrderStatus.Cancelled;
                order.CancellationReason = reason;
                order.CancelledAt = DateTime.UtcNow;

                _dbContext.Orders.Update(order);

                var affected = await _dbContext.SaveChangesAsync();

                if (affected == 0)
                {
                    _logger.LogDebug("CancellOrderAsync: Cancelling order with id: {OrderId} failed", orderId);
                    return false;
                }

                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: CancellOrderAsync failed");
                return false;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersUsingBonusAsync()
        {
            try
            {
                var orders = await _dbContext.Orders
                    .Where(o => o.UsedBonusReward)
                    .ToListAsync();

                if (!orders.Any())
                {
                    _logger.LogDebug("GetOrdersUsingBonusAsync: No orders found using bonus.");
                    return null;
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetOrdersUsingBonusAsync failed");
                return null;
            }
        }

        public async Task<Order?> GetPendingOrderForUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("GetPendingOrderForUserAsync: UserId invalid");
                return null;
            }

            try
            {
                var orders = await _dbContext.Orders
                                        .Include(o => o.Items)
                                        .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == OrderStatus.Pending);

                if (orders == null)
                {
                    _logger.LogDebug("GetPendingOrderForUserAsync: No pending orders found for user: {UserId}", userId);
                    return null;
                }

                return orders;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: GetPendingOrderForUserAsync failed");
                return null;
            }
        }
    }
}
