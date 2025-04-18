namespace BlinkIt.Gateway.Models;

public class LoginRequest
{
    public string MobileNumber { get; set; }
    public string Password { get; set; }
}
public class ChangePasswordRequest
{
    public string CurrentPassword { get; set; }
    public string NewPassword { get; set; }
}