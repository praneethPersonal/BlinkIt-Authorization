using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BlinkIt.Repository.Models;

public class Seller
{
    public ObjectId Id { get; set; }
    [BsonElement("MobileNumber")]
    public string MobileNumber { get; set; }

    [BsonElement("Password")]
    public string Password { get; set; }
    [BsonRepresentation(BsonType.String)]
    public List<Guid> ProductIds { get; set; } = new();
}