using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataInCloud.Dal.Restaurant;
public class RestaurantDao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    public string Name { get; set; }
    public double Rating { get; set; }
    public bool IsOpen { get; set; }
}