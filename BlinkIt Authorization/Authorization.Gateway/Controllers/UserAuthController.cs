
using System.Security.Claims;
using BlinkIt.Gateway.Models;
using BlinkIt.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlinkIt.Gateway.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserAuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public UserAuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] LoginRequest loginRequest, [FromQuery] string type)
    {
        if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.MobileNumber) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return BadRequest("Mobile number and password are required.");
        }

        var (success, message, token) = await _authService.CreateNewUser(loginRequest.MobileNumber, loginRequest.Password, type);

        if (success)
        {
            return Ok(new { Message = message, Token = token });
        }
        else if (message == "Mobile number already exists. Cannot create new User.")
        {
            return BadRequest(message);
        }
        return Unauthorized(new { Message = message });
    }
    
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.MobileNumber) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return BadRequest("Mobile number and password are required.");
        }

        var (success, message, token) = await _authService.ValidateCredentialsAsync(loginRequest.MobileNumber, loginRequest.Password, "buyer");

        if (success)
        {
            return Ok(new { Message = message, Token = token });
        }

        if (message == "Please signup before login!")
        {
            return NotFound(message);
        }
        return Unauthorized(new { Message = message });
    }
    
    [HttpPost("seller/login")]
    public async Task<IActionResult> SellerLogin([FromBody] LoginRequest loginRequest)
    {
        if (loginRequest == null || string.IsNullOrWhiteSpace(loginRequest.MobileNumber) || string.IsNullOrWhiteSpace(loginRequest.Password))
        {
            return BadRequest("Mobile number and password are required.");
        }

        var (success, message, token) = await _authService.ValidateCredentialsAsync(loginRequest.MobileNumber, loginRequest.Password, "seller");

        if (success)
        {
            return Ok(new { Message = message, Token = token });
        }
        if (message == "Please signup before login!")
        {
            return NotFound(message);
        }

        return Unauthorized(new { Message = message });
    }
    
    [HttpPut("changePassword")]
    [Authorize] 
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
    {
        var mobileNumber = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        if (string.IsNullOrEmpty(mobileNumber))
        {
            return Unauthorized("Invalid or missing mobile number in token.");
        }

        if (string.IsNullOrEmpty(request.CurrentPassword) || string.IsNullOrEmpty(request.NewPassword))
        {
            return BadRequest("Current password and new password cannot be empty.");
        }

        var success = await _authService.ChangePasswordAsync(mobileNumber, request.CurrentPassword, request.NewPassword);

        if (success)
        {
            return Ok("Password changed successfully.");
        }
        return BadRequest("Current password is incorrect or could not change password.");
    }
}