using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.Serializers;

namespace BlinkIt.Repository.Models;

public class User
{
    public ObjectId Id { get; set; }
    [BsonElement("MobileNumber")]
    public string MobileNumber { get; set; }

    [BsonElement("Password")]
    public string Password { get; set; }

    [BsonElement("Addresses")]
    public List<string> Addresses { get; set; }
    public string Type {get; set;}
}