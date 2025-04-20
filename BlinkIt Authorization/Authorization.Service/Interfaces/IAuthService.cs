namespace BlinkIt.Service.Interfaces;

public interface IAuthService
{
    public Task<(bool Success, string Message, string Token)> CreateNewUser(string mobileNumber, string password, string type);
    public Task<(bool Success, string Message, string Token)> ValidateCredentialsAsync(string mobileNumber, string password, string type);
    public Task<bool> ChangePasswordAsync(string mobileNumber, string currentPassword, string newPassword);
    public Task<bool> AddProductToSellerAsync(string mobileNumber, Guid productId);
}