using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Data.DataModels;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.Models;
using static PizzeriaApi.Domain.UtilModels.UtilEnums;

namespace PizzeriaApi.Data.Repository
{
    public class PizzeriaUserRepo : IPizzeriaUserRepo
    {
        private readonly PizzeriaApiDBContext _dbContext;
        private readonly ILogger<PizzeriaUserRepo> _logger;

        const int minBonusPoints = 100; //minimum bonus required to use bonus points

        public PizzeriaUserRepo(PizzeriaApiDBContext context, ILogger<PizzeriaUserRepo> logger)
        {
            _dbContext = context;
            _logger = logger;
        }

        public async Task<int?> GetBonusByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID is null or empty");
                return null;
            }

            try
            {

                var userBonus = await _dbContext.Users
                    .Where(u => u.Id == userId)
                    .Select(u => (int?)u.BonusPoints)
                    .FirstOrDefaultAsync();

                if (userBonus == null)
                {
                    _logger.LogWarning("No user found with ID {UserId}", userId);
                    return null;
                }

                return userBonus;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bonus points for user with ID {UserId}", userId);
                return null;
            }
        }

        public async Task<IEnumerable<PizzeriaUser>> GetPremiumUsers()
        {
            try
            {
                var premiumUsersIds = await _dbContext.UserRoles
                    .Where(ur => ur.RoleId == UserRoles.PremiumUser.ToString())
                    .Select(ur => ur.UserId)
                    .ToListAsync();

                var premiumUsers = await _dbContext.Users.Where(u => premiumUsersIds.Contains(u.Id)).ToListAsync();

                return premiumUsers;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting premium users");
                return null;
            }
        }

        public async Task<IEnumerable<PizzeriaUser>> GetRegularUsers()
        {
            try
            {
                var regularUsersIds = _dbContext.UserRoles
                    .Where(ur => ur.RoleId == UserRoles.RegularUser.ToString())
                    .Select(ur => ur.UserId)
                    .ToList();

                var regularUsers = await _dbContext.Users.Where(u => regularUsersIds.Contains(u.Id)).ToListAsync();

                return regularUsers;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting regular users");
                return null;
            }
        }

        public async Task<PizzeriaUser?> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                _logger.LogWarning("Email is null or empty");
                return null;
            }

            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);

                if (user == null)
                {
                    _logger.LogWarning("No user found with email {Email}", email);
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by email {Email}", email);
                return null;
            }
        }

        public async Task<PizzeriaUser?> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                _logger.LogWarning("Username is null or empty");
                return null;
            }
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == username);

                if (user == null)
                {
                    _logger.LogWarning("No user found with username {Username}", username);
                    return null;
                }

                return user;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user by username {Username}", username);
                return null;
            }
        }

        public async Task<IEnumerable<PizzeriaUser>> GetUsersByorderStatus(string orderStatus)
        {
            if (string.IsNullOrEmpty(orderStatus))
            {
                _logger.LogWarning("Order status input is null or empty");
                return null;
            }

            try
            {
                if (Enum.TryParse<OrderStatus>(orderStatus, true, out var status) == false)
                {
                    _logger.LogWarning("Invalid order status input: {OrderStatus}", orderStatus);
                    return null;
                }

                var users = await _dbContext.Users
                    .Where(u => u.Orders.Any(o => o.Status == status))
                    .ToListAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No users found with order status {OrderStatus}", orderStatus);
                    return null;

                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users by order status {OrderStatus}", orderStatus);
                return null;
            }
        }



        public async Task<IEnumerable<PizzeriaUser>> GetUsersWithOrders()
        {
            try
            {
                var users = await _dbContext.Users.Where(u => u.Orders.Any())
                                                    .ToListAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No users found with orders");
                    return null;
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting users with orders");
                return null;
            }
        }

        public async Task<PizzeriaUser?> GetUserWithIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID is null or empty");
                return null;
            }

            try
            {
                var user = await _dbContext.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("No user found with ID {UserId}", userId);
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting user with ID {UserId}", userId);
                return null;
            }
        }

        public async Task<bool> UpdateBonusAsync(string userId, int newBonusValue)
        {
            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID is null or empty");
                return false;
            }

            if (newBonusValue < 0)
            {
                _logger.LogWarning("New bonus value is negative");
                return false;
            }

            try
            {
                var affected = await _dbContext.Users.Where(u => u.Id == userId)
                    .ExecuteUpdateAsync(u => u.SetProperty(x => x.BonusPoints, newBonusValue));

                if (affected == 0)
                {
                    _logger.LogWarning("No user found with ID {UserId} to update bonus points", userId);
                    return false;

                }

                _logger.LogInformation("Bonus points updated successfully for user with ID {UserId}", userId);
                return true;

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bonus points for user with ID {UserId}", userId);
                return false;
            }
        }

        public async Task<bool> UsercanUseBonus(string userId) 
        {
           

            if (string.IsNullOrEmpty(userId))
            {
                _logger.LogWarning("User ID is null or empty");
                return false;
            }

            try
            {
                var premiumUsersIds = await _dbContext.UserRoles
                   .Where(ur => ur.RoleId == UserRoles.PremiumUser.ToString())
                   .Select(ur => ur.UserId)
                   .ToListAsync();

                bool isPremiumUser = premiumUsersIds.Contains(userId);

                if(!isPremiumUser)
                {
                    _logger.LogWarning("User with ID {UserId} is not a premium user", userId);
                    return false;
                }

                var user = await _dbContext.Users
                    .Include(u => u.Orders)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    _logger.LogWarning("No user found with ID {UserId}", userId);
                    return false;
                }

                var canUseBonus = user.BonusPoints >= minBonusPoints && user.Orders.Any(o => o.Status == OrderStatus.Pending);

                return canUseBonus;


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user can use bonus points");
                return false;
            }
        }

        public async Task<IEnumerable<PizzeriaUser>> GetAllUsers()
        {
            try
            {
                var users = await _dbContext.Users.ToListAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No users found");
                    return null;
                }

                return users;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return null;
            }
        }

        public async Task<IEnumerable<PizzeriaUser>> GetUsersWithNoOrders()
        {
            try
            {
                var users = await _dbContext.Users
                    .Where(u => !u.Orders.Any())
                    .ToListAsync();

                if (!users.Any())
                {
                    _logger.LogWarning("No users found with no orders");
                    return null;
                }

                return users;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error getting users with no orders");
                return null;
            }
        }
    }
}
