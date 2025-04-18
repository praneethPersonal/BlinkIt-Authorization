using BlinkIt.Repository.Interfaces;
using BlinkIt.Repository.Models;
using MongoDB.Driver;

namespace BlinkIt.Repository.Implementation;

public class SellerRepository: ISellerRepository
{
    
    private readonly IMongoCollection<Seller> _sellers;
    private readonly IMongoDatabase _database;

    public SellerRepository()
    {
        var client = new MongoClient("mongodb://praneeth:blinkit@localhost:27017/mydatabase?authSource=admin");
        _database = client.GetDatabase("blinkit");
        _sellers = _database.GetCollection<Seller>("SellerInformation");
    }
    
    public async Task CreateSeller(Seller newSeller)
    {
        await _sellers.InsertOneAsync(newSeller);
    }
    public async Task<Seller> GetSellerByMobileNumberAsync(string mobileNumber)
    {
        return await _sellers.Find(user => user.MobileNumber == mobileNumber).FirstOrDefaultAsync();
    }
}