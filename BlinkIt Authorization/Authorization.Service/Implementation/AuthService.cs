using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BlinkIt.Repository.Interfaces;
using BlinkIt.Repository.Models;
using BlinkIt.Service.Interfaces;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;

namespace BlinkIt.Service.Implementation;

public class AuthService : IAuthService
{
    private readonly IAuthRepository _authRepository;
    private readonly ISellerRepository _sellerRepository;
    private readonly IConfiguration _configuration;
    public AuthService(IAuthRepository authRepository, IConfiguration config, ISellerRepository sellerRepository)
    {
        _authRepository = authRepository;
        _configuration = config;
        _sellerRepository = sellerRepository;
    }
    public async Task<(bool Success, string Message, string Token)> CreateNewUser(string mobileNumber, string password, string type)
    {
        if (type == "buyer")
        {
            var existingUser = await _authRepository.GetUserByMobileNumberAsync(mobileNumber);
            if (existingUser != null)
            {
                return (false, "Mobile number already exists. Cannot create new User.", null);
            }
            var newUser = new User
            {
                MobileNumber = mobileNumber,
                Password = password,
                Type = type
            };
            await _authRepository.CreateUserAsync(newUser);
        }
        else
        {
            var existingSeller = await _sellerRepository.GetSellerByMobileNumberAsync(mobileNumber);
            if (existingSeller != null)
            {
                return (false, "Mobile number already exists. Cannot create new User.", null);
            }
            var newSeller = new Seller
            {
                MobileNumber = mobileNumber,
                Password = password,
                ProductIds = new List<Guid>()
            };
            await _sellerRepository.CreateSeller(newSeller);
        }

        var token = GenerateToken(mobileNumber, type);

        return (true, "New user created and login successful!", token);
    }
    
    public async Task<(bool Success, string Message, string Token)> ValidateCredentialsAsync(string mobileNumber, string password, string type)
    {
        string userPassword;
        if (type == "buyer")
        {
            var user = await _authRepository.GetUserByMobileNumberAsync(mobileNumber);
            userPassword = user?.Password;
        }
        else
        {
            var user = await _sellerRepository.GetSellerByMobileNumberAsync(mobileNumber);
            userPassword = user?.Password;
        }
        
        
        if (userPassword == null)
        {
            return (false, "Please signup before login!", null);
        }
        if (userPassword == password)
        {
              
            var token = GenerateToken(mobileNumber, type);
            return (true, "Login successful!", token);
        }

        return (false, "Invalid password.", null);
    }
    
    public async Task<bool> ChangePasswordAsync(string mobileNumber, string currentPassword, string newPassword)
    {
        var user = await _authRepository.GetUserByMobileNumberAsync(mobileNumber);
        if (user == null || user.Password != currentPassword) return false;
        _authRepository.ChangePasswordAsync(mobileNumber, currentPassword, newPassword);
        return true;
    }
    
    private string GenerateToken(string mobileNumber, string type)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
           
        var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]); 

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.Name, mobileNumber),
                new Claim(ClaimTypes.Role, type)
            }),
            Expires = DateTime.UtcNow.AddHours(1),
                
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}