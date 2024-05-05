namespace DataInCloud.Orchestrators.Meal;
public class MealContract
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Price { get; set; }
    public bool IsAvailable { get; set; }
}