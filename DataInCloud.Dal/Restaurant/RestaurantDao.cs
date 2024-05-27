using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace DataInCloud.Dal.Restaurant;
public class RestaurantDao
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    [MaxLength(256, ErrorMessage = "Must be within 1-256 chars")]
    public string Name { get; set; }
    [Range(1, 5, ErrorMessage = "Must be within 1-5")]
    public double Rating { get; set; }
    public bool IsOpen { get; set; }
}