using BlinkIt.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlinkIt.Gateway.Controllers;
[ApiController]
[Route("[controller]")]
public class SellerController : ControllerBase
{
    private readonly IAuthService _authService;
    public SellerController(IAuthService authService)
    {
        _authService = authService;
    }
    
    [HttpPut]
    [Route("addproducttoseller")]
    public async Task<IActionResult> AddProductToSeller(string sellerId, Guid productId)
    {
        if (productId == Guid.Empty || productId == null)
        {
            return BadRequest();
        }
        var res = await _authService.AddProductToSellerAsync(sellerId, productId);
        if (res)
        {
            return Ok("Seller added to seller");
        }
        return NotFound("Seller not found");
    }
}