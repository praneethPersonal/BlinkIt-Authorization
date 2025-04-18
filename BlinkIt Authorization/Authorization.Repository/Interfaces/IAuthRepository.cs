using BlinkIt.Repository.Models;

namespace BlinkIt.Repository.Interfaces;

public interface IAuthRepository
{
    Task<User> GetUserByMobileNumberAsync(string mobileNumber);
    Task CreateUserAsync(User newUser);
    
    void ChangePasswordAsync(string mobileNumber, string currentPassword, string newPassword);
}