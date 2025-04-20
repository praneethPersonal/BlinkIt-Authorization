using BlinkIt.Repository.Models;

namespace BlinkIt.Repository.Interfaces;

public interface ISellerRepository
{
    Task CreateSeller(Seller newSeller);
    Task<Seller> GetSellerByMobileNumberAsync(string mobileNumber);
    public Task UpdateSellerProducts(string sellerId, Guid productId);
}