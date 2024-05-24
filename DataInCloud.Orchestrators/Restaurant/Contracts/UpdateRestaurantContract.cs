using System.ComponentModel.DataAnnotations;

namespace DataInCloud.Orchestrators.Restaurant.Contract;
public class UpdateRestaurantContract
{

    [MaxLength(100, ErrorMessage = "Name should be less than 100 chars")]
    public string Name { get; set; }
    public bool IsOpen { get; set; }
}