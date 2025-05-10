using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PizzeriaApi.Data.Repository
{
    public class OrderItemsRepo:IOrderItemsRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<OrderItemsRepo> _logger;

        public OrderItemsRepo(PizzeriaApiDBContext dbContext, ILogger<OrderItemsRepo> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<bool> AddManyOrderItemAsync(IEnumerable<OrderItem> items)
        {
          

            if (!items.Any())

            {
                _logger.LogWarning("AddManyOrderItemAsync: Items input is null or empty.");
                return false;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var disIds = items.Select(i => i.DishId).Distinct().ToList(); //distinct letar efter unika endast.

                var dishesToAdd = await _dbContext.Dishes.Where(d =>disIds.Contains(d.Id)).ToListAsync();

                if (dishesToAdd.Count != disIds.Count)
                {
                    _logger.LogWarning("AddManyOrderItemAsync: Some dishes not found.");
                    await transaction.RollbackAsync();
                    return false;
                }


                var totalPrice = items.Sum(oi =>
                {
                    var dish = dishesToAdd.FirstOrDefault(d => d.Id == oi.DishId);

                    return dish != null ? oi.Quantity * dish.Price : 0;
                });

                var orderId = items.First().OrderId;
                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning("AddManyOrderItemAsync: Order with ID {OrderId} not found.", orderId);
                    await transaction.RollbackAsync();
                    return false;
                }

                order.TotalPrice += totalPrice;

                //_dbContext.Orders.Update(order);

                await _dbContext.OrderItems.AddRangeAsync(items);
                await _dbContext.SaveChangesAsync(); //tar hand om updatering Update() behövs ej.

                await transaction.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error: AddManyOrderItemAsync failed");
                return false;
            }
        }

        public async Task<bool> AddOneOrderItemAsync(OrderItem item)
        {
            if (item == null)

            {
                _logger.LogWarning("AddOneOrderItemAsync: Items input is null or empty.");
                return false;
            }

            using var transactions = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var dishToAdd = await _dbContext.Dishes.FirstOrDefaultAsync(d => d.Id == item.DishId);

                if (dishToAdd == null)
                {
                    _logger.LogWarning("AddOneOrderItemAsync: Dish with ID {DishId} not found.", item.DishId);
                    await transactions.RollbackAsync();
                    return false;
                }

                var totalPrice = item.Quantity * dishToAdd.Price;

                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == item.OrderId);

                if (order == null)
                {
                    _logger.LogWarning("AddOneOrderItemAsync: Order with ID {OrderId} not found.", item.OrderId);
                    await transactions.RollbackAsync();
                    return false;
                }

                order.TotalPrice += totalPrice;

                await _dbContext.OrderItems.AddAsync(item);
                await _dbContext.SaveChangesAsync();

                await transactions.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                await transactions.RollbackAsync();
                _logger.LogError(ex, "Error: AddOneOrderItemAsync failed");
                return false;
            }
        }

        public Task<bool> DeleteOrderItemAsync(int orderItemId)
        {
            throw new NotImplementedException();
        }

        public async Task<OrderItem?> GetItemsByIdAsync(int orderItemsId)
        {
            if(orderItemsId <= 0)
            {
                _logger.LogWarning("GetItemsByIdAsync: orderItemsId invalid");
                return null;
            }

            try
            {
                var orderItem = await _dbContext.OrderItems.FirstOrDefaultAsync(oi => oi.Id == orderItemsId);

                if (orderItem == null)
                {
                    _logger.LogDebug("GetItemsByIdAsync: no order items found");
                    return null;
                }

                return orderItem;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetItemsByIdAsync failed");
                return null;
            }
        }

        public async Task<IEnumerable<OrderItem>> GetOrderItemsByorderIdAsync(int orderId)
        {
            if( orderId <= 0)
            {
                _logger.LogWarning("GetOrderItemsByorderIdAsync: orderId invalid");
                return null;
            }

            try
            {
                var orderItems = await _dbContext.OrderItems.Where(oi => oi.OrderId == orderId)
                                                                .ToListAsync();
                if(!orderItems.Any())
                {
                    _logger.LogDebug(" GetOrderItemsByorderIdAsync: No orderItems found");
                    return null;
                }

                return orderItems;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error: GetOrderItemsByorderIdAsync failed");
                return null;
            }
        }

        public async Task<bool> UpdateOrderItemAsync(OrderItem orderItem)
        {
            if (orderItem == null || orderItem.Id <= 0)
            {
                _logger.LogWarning("UpdateOrderItemAsync: Invalid input.");
                return false;
            }

            using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                var existingItem = await _dbContext.OrderItems
                    .Include(oi => oi.Dish)
                    .FirstOrDefaultAsync(oi => oi.Id == orderItem.Id);

                if (existingItem == null)
                {
                    _logger.LogWarning("UpdateOrderItemAsync: OrderItem with ID {Id} not found.", orderItem.Id);
                    await transaction.RollbackAsync();
                    return false;
                }

                var order = await _dbContext.Orders.FirstOrDefaultAsync(o => o.Id == existingItem.OrderId);
                if (order == null)
                {
                    _logger.LogWarning("UpdateOrderItemAsync: Order with ID {Id} not found.", existingItem.OrderId);
                    await transaction.RollbackAsync();
                    return false;
                }

      
                var oldTotal = existingItem.Quantity * existingItem.Dish.Price;
                var newTotal = orderItem.Quantity * existingItem.Dish.Price;

                order.TotalPrice += (newTotal - oldTotal);

           
                existingItem.Quantity = orderItem.Quantity;

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                return true;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "Error: UpdateOrderItemAsync failed");
                return false;
            }
        }
    }
}
