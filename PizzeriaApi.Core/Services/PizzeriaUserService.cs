using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using PizzeriaApi.Core.Interfaces;
using PizzeriaApi.Data.Interfaces;
using PizzeriaApi.Domain.DTO_s;
using PizzeriaApi.Domain.Models;
using PizzeriaApi.Domain.RequestModels.PizzeriaUserReq;
using PizzeriaApi.Domain.UtilModels;
using static PizzeriaApi.Domain.UtilModels.UtilEnums;

namespace PizzeriaApi.Core.Services
{
    public class PizzeriaUserService : IPizzeriaUserService
    {
        private readonly ILogger<PizzeriaUserService> _logger;
        private readonly IPizzeriaUserRepo _userRepo;
        private readonly UserManager<PizzeriaUser> _userManager;
        private readonly ITokenGenerator _tokenGenerator;

        public PizzeriaUserService(IPizzeriaUserRepo userRepo, ILogger<PizzeriaUserService> logger, UserManager<PizzeriaUser> userManager, ITokenGenerator tokenGenerator)
        {
            _userRepo = userRepo;
            _logger = logger;
            _userManager = userManager;
            _tokenGenerator = tokenGenerator;
        }

        public async Task<OperationResult<string?>> DeleteUserAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return OperationResult<string?>.Failure("User ID cannot be null or empty.", "Invalid user ID.");
            }

            try
            {
                var userToDelete = await _userManager.FindByIdAsync(userId);

                if (userToDelete == null)
                {

                    return OperationResult<string?>.Failure(userId, "User not found.");
                }

                var isAdmin = await _userManager.IsInRoleAsync(userToDelete, UserRoles.Admin.ToString());

                if (isAdmin)
                {
                    return OperationResult<string?>.Failure(userId, "Cannot delete a user with Admin role.");
                }


                var result = await _userManager.DeleteAsync(userToDelete);

                if (result.Succeeded)
                {

                    return OperationResult<string?>.Success(userId, "User deleted successfully.");
                }

                else
                {
                    var errorMessages = GetErrorMessage(result);
                    _logger.LogDebug("Failed to delete user with ID {UserId}. Errors: {Errors}", userId, errorMessages);

                    return OperationResult<string?>.Failure(userId, "Failed to delete user.");
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user with ID {UserId}", userId);
                return OperationResult<string?>.Failure(ex.Message, "An error occurred while deleting the user.");
            }
        }

        public async Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetAllUsers()
        {
            try
            {


                var users = await _userRepo.GetAllUsers();

                if (!users.Any())
                {
                    return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "No users found.");
                }

                var userDTOs = MapManyUsers(users, true);

                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Success(userDTOs, "Users retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all users.");
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "An error occurred while retrieving all users.");
            }
        }

        public async Task<OperationResult<int?>> GetBonusByUserIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return OperationResult<int?>.Failure(null, "Invalid user ID.");
            }

            try
            {
                var userBonus = await _userRepo.GetBonusByUserIdAsync(userId);

                if (userBonus == null)
                {

                    return OperationResult<int?>.Failure(null, "User not found.");
                }

                return OperationResult<int?>.Success(userBonus, "User bonus retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving bonus for user with ID {UserId}", userId);
                return OperationResult<int?>.Failure(null, "An error occurred while retrieving the bonus.");
            }
        }

        public async Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetPremiumUsers()
        {
            try
            {
                var users = await _userRepo.GetPremiumUsers();

                if (!users.Any())
                {
                    return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "No premium users found.");
                }

                var userDTOs = MapManyUsers(users, true);

                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Success(userDTOs, "Premium users retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving premium users.");
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "An error occurred while retrieving premium users.");
            }
        }

        public async Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetRegularUsers()
        {
            try
            {
                var users = await _userRepo.GetRegularUsers();

                if (!users.Any())
                {
                    return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "No regular users found.");
                }

                var userDTOs = MapManyUsers(users, true);

                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Success(userDTOs, "Regular users retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving regular users.");
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "An error occurred while retrieving regular users.");
            }
        }

        public async Task<OperationResult<PizzeriaUserDTO?>> GetUserByEmailAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "Email cannot be null or empty.");
            }

            try
            {
                var user = await _userRepo.GetUserByEmailAsync(email);

                if (user == null)
                {
                    return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "User not found.");
                }

                var userDTO = MapOneUser(user, true);

                return OperationResult<PizzeriaUserDTO?>.Success(userDTO, "User retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by email {Email}", email);
                return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "An error occurred while retrieving the user.");
            }
        }

        public async Task<OperationResult<PizzeriaUserDTO?>> GetUserByUsernameAsync(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "Username cannot be null or empty.");
            }


            try
            {
                var user = await _userRepo.GetUserByUsernameAsync(username);

                if (user == null)
                {
                    return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "User not found.");
                }

                var userDTO = MapOneUser(user, true);

                return OperationResult<PizzeriaUserDTO?>.Success(userDTO, "User retrieved successfully.");

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user by username {Username}", username);
                return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "An error occurred while retrieving the user.");
            }
        }

        public async Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetUsersByorderStatus(string orderStatus)
        {
            if (string.IsNullOrEmpty(orderStatus))
            {
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "Order status cannot be null or empty.");
            }
            try
            {
                var users = await _userRepo.GetUsersByorderStatus(orderStatus);

                if (!users.Any())
                {
                    return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "No users found with the specified order status.");
                }

                var userDTOs = MapManyUsers(users, true);

                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Success(userDTOs, "Users retrieved successfully.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users by order status {OrderStatus}", orderStatus);
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "An error occurred while retrieving users by order status.");
            }
        }

        public async Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetUsersWithNoOrders()
        {
            try
            {
                var users = await _userRepo.GetUsersWithNoOrders();

                if (!users.Any())
                {
                    return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "No users found with no orders.");
                }

                var userDTOs = MapManyUsers(users, true);
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Success(userDTOs, "Users with no orders retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users with no orders.");
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "An error occurred while retrieving users with no orders.");
            }
        }

        public async Task<OperationResult<IEnumerable<PizzeriaUserDTO>>> GetUsersWithOrders()
        {
            try
            {
                var users = await _userRepo.GetUsersWithOrders();

                if (!users.Any())
                {
                    return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "No users found with orders.");
                }

                var userDTOs = MapManyUsers(users, true);
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Success(userDTOs, "Users with orders retrieved successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users with orders.");
                return OperationResult<IEnumerable<PizzeriaUserDTO>>.Failure(Enumerable.Empty<PizzeriaUserDTO>(), "An error occurred while retrieving users with orders.");
            }
        }

        public async Task<OperationResult<PizzeriaUserDTO?>> GetUserWithIdAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "User ID cannot be null or empty.");
            }
            try
            {
                var user = await _userRepo.GetUserWithIdAsync(userId);

                if (user == null)
                {
                    return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "User not found.");
                }

                var userDTO = MapOneUser(user, true);

                return OperationResult<PizzeriaUserDTO?>.Success(userDTO, "User retrieved successfully.");
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user with ID {UserId}", userId);
                return OperationResult<PizzeriaUserDTO?>.Failure(new PizzeriaUserDTO(), "An error occurred while retrieving the user.");
            }
        }

        public async Task<OperationResult<string?>> LoginAsync(LoginReq loginReq)
        {
            if (string.IsNullOrEmpty(loginReq.Username) || string.IsNullOrEmpty(loginReq.Password))
            {
                return OperationResult<string?>.Failure(null, "Username and password cannot be null or empty.");
            }

            try
            {
                var user = await _userRepo.GetUserByUsernameAsync(loginReq.Username);

                if (user == null)
                {
                    return OperationResult<string?>.Failure(null, "Invalid Username or Password");
                }

                var validPass = await _userManager.CheckPasswordAsync(user, loginReq.Password);

                if (!validPass)
                {
                    return OperationResult<string?>.Failure(null, "Invalid Username or Password");
                }

                var token = await _tokenGenerator.GenerateToken(user);

                return OperationResult<string?>.Success(token, "User logged in successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error logging in user with username {Username}", loginReq.Username);
                return OperationResult<string?>.Failure(null, "An error occurred while logging in the user.");
            }
        }

        public async Task<OperationResult<string?>> RegisterUserAsync(RegisterUserReq registerUserReq)
        {
            try
            {
                var existinUser = await _userManager.FindByNameAsync(registerUserReq.UserName);


                if (existinUser != null || existinUser?.Email == registerUserReq.Email)
                {
                    return OperationResult<string?>.Failure(null, "Username or Email already exists.");
                }

                var newUser = new PizzeriaUser
                {
                    UserName = registerUserReq.UserName.Trim(),
                    Email = registerUserReq.Email.Trim(),
                    PhoneNumber = registerUserReq.PhoneNumber.Trim()
                };

               

                var result = await _userManager.CreateAsync(newUser, registerUserReq.Password);

                if (result.Succeeded)
                {
                    var addedRole = await _userManager.AddToRoleAsync(newUser, UserRoles.RegularUser.ToString());

                    if (addedRole.Succeeded == false)
                    {
                        var errorMessage = GetErrorMessage(addedRole);
                        return OperationResult<string?>.Failure(null, "Failed to add role to user. " + errorMessage);
                    }

                    return OperationResult<string?>.Success(null, "User registered successfully.");

                }
                else
                {
                    var errorMessage = GetErrorMessage(result);
                    return OperationResult<string?>.Failure(null, "Failed to register user. " + errorMessage);
                }


            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error registering user with username {Username}", registerUserReq.UserName);
                return OperationResult<string?>.Failure(null, "An error occurred while registering the user.");
            }
        }

        public async Task<OperationResult<bool?>> UpdateBonusAsync(string userId, int newBonusValue)
        {
            try
            {
                var result = await _userRepo.UpdateBonusAsync(userId, newBonusValue);

                if (result)
                {
                    return OperationResult<bool?>.Success(null, "Bonus updated successfully.");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "Failed to update bonus.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bonus for user with ID {UserId}", userId);
                return OperationResult<bool?>.Failure(null, "An error occurred while updating the bonus.");
            }
        }

        public async Task<OperationResult<string?>> UpdateUserAsync(UpdateUserReq updateUserReq)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(updateUserReq.UserId);

                if (user == null)
                {
                    return OperationResult<string?>.Failure(null, "User not found.");
                }


                if (!string.IsNullOrEmpty(updateUserReq.UserName))
                {
                    user.UserName = updateUserReq.UserName.Trim();
                }

                if (!string.IsNullOrEmpty(updateUserReq.Email))
                {
                    user.Email = updateUserReq.Email.Trim();
                }

                if (!string.IsNullOrEmpty(updateUserReq.PhoneNumber))
                {
                    user.PhoneNumber = updateUserReq.PhoneNumber.Trim();
                }

                if (!string.IsNullOrEmpty(updateUserReq.Password))
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var resetResult = await _userManager.ResetPasswordAsync(user, token, updateUserReq.Password.Trim());

                    if (!resetResult.Succeeded)
                    {
                        var errorMessage = GetErrorMessage(resetResult);
                        return OperationResult<string?>.Failure(null, "Failed to update password. " + errorMessage);
                    }
                }


                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    return OperationResult<string?>.Success(null, "User updated successfully.");
                }
                else
                {
                    var errorMessage = GetErrorMessage(result);
                    return OperationResult<string?>.Failure(null, "Failed to update user. " + errorMessage);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user with ID {UserId}", updateUserReq.UserId);
                return OperationResult<string?>.Failure(null, "An error occurred while updating the user.");
            }
        }

        public async Task<OperationResult<bool?>> UsercanUseBonus(string userId)
        {
            try
            {
                var result = await _userRepo.UsercanUseBonus(userId);

                if (result)
                {
                    return OperationResult<bool?>.Success(null, "User can use bonus.");
                }
                else
                {
                    return OperationResult<bool?>.Failure(null, "User cannot use bonus.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error checking if user can use bonus with ID {UserId}", userId);
                return OperationResult<bool?>.Failure(null, "An error occurred while checking if the user can use bonus.");
            }
        }

        public async Task<OperationResult<bool?>> UpdateUserRoleAsync(string userId, string role)
        {
            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(role))
            {
                return OperationResult<bool?>.Failure(null, "User ID and role cannot be null or empty.");
            }

            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    return OperationResult<bool?>.Failure(null, "User not found.");
                }

                var currentRoles = await _userManager.GetRolesAsync(user);
                var result = await _userManager.RemoveFromRolesAsync(user, currentRoles);

                if (!result.Succeeded)
                {
                    var errorMessage = GetErrorMessage(result);
                    return OperationResult<bool?>.Failure(null, "Failed to remove current roles. " + errorMessage);
                }

                if (!Enum.TryParse<UserRoles>(role, true, out var newRole))
                {
                    return OperationResult<bool?>.Failure(null, "Invalid role.");
                }

                if(newRole == UserRoles.Admin)
                {
                    return OperationResult<bool?>.Failure(null, "Cannot assign Admin role through api.");
                }

                result = await _userManager.AddToRoleAsync(user, newRole.ToString());

                if (result.Succeeded)
                {
                    return OperationResult<bool?>.Success(null, "User role updated successfully.");
                }
                else
                {
                    var errorMessage = GetErrorMessage(result);
                    return OperationResult<bool?>.Failure(null, "Failed to update user role. " + errorMessage);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user role for user with ID {UserId}", userId);
                return OperationResult<bool?>.Failure(null, "An error occurred while updating the user role.");
            }
        }

        private string GetErrorMessage(IdentityResult result)
        {
            return string.Join("; ", result.Errors.Select(e => e.Description));
        }

        private PizzeriaUserDTO MapOneUser(PizzeriaUser user, bool withId = false)
        {
            try
            {
                return new PizzeriaUserDTO
                {
                    UserId = withId ? user.Id : null,
                    UserName = user.UserName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber
                };
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error mapping user to DTO");
                throw;
            }
        }

        private IEnumerable<PizzeriaUserDTO> MapManyUsers(IEnumerable<PizzeriaUser> users, bool withId = false)
        {
            try
            {
                return users.Select(user => MapOneUser(user, withId));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error mapping users to DTOs");
                throw;
            }

        }
    }
}
