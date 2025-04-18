using BlinkIt.Repository.Interfaces;
using BlinkIt.Repository.Models;
using MongoDB.Driver;

namespace BlinkIt.Repository.Implementation;

public class AuthRepository : IAuthRepository
{
    private readonly IMongoDatabase _database;
    private readonly IMongoCollection<User> _users;

    public AuthRepository()
    {
        var client = new MongoClient("mongodb://praneeth:blinkit@localhost:27017/mydatabase?authSource=admin");
        _database = client.GetDatabase("blinkit");
        _users = _database.GetCollection<User>("UserInformation");
    }
    public async Task CreateUserAsync(User newUser)
    {
        await _users.InsertOneAsync(newUser);
    }
    public async Task<User> GetUserByMobileNumberAsync(string mobileNumber)
    {
        return await _users.Find(user => user.MobileNumber == mobileNumber).FirstOrDefaultAsync();
    }
    public async void ChangePasswordAsync(string mobileNumber, string currentPassword, string newPassword)
    {
        var update = Builders<User>.Update.Set(u => u.Password, newPassword);

        var result = await _users.UpdateOneAsync(
            u => u.MobileNumber == mobileNumber,
            update
        );

    }
}