namespace DataInCloud.Orchestrators.Meal.Contract;
public class MealContract
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public bool IsAvailable { get; set; }
}