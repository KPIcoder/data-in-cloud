using System.ComponentModel.DataAnnotations;

namespace DataInCloud.Orchestrators.Restaurant.Contract;

public class CreateRestaurantContract
{
    [MaxLength(100, ErrorMessage = "Name should be less than 100 chars")]
    public string Name { get; set; }
}