using System.ComponentModel.DataAnnotations;

namespace DataInCloud.Orchestrators.Meal.Contract;

public class UpdateMealContract

{
    [MaxLength(256, ErrorMessage = "Must be within 256 chars")]
    public string Name { get; set; }
    [Range(1, 1000, ErrorMessage = "Must be within 1-1000")]
    public int Price { get; set; }
    public bool IsAvailable { get; set; }
}