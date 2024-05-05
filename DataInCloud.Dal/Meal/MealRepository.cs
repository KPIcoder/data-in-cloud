using DataInCloud.Model.Meal;

namespace DataInCloud.Dal.Meal;
public class MealRepository : IMealRepository
{
    public async Task<List<Model.Meal.Meal>> GetAllAsync()
    {

        List<Model.Meal.Meal> meals =
        [
            new Model.Meal.Meal { Id = 1, Name = "Spaghetti Carbonara", Price = 12, IsAvailable = true },
            new Model.Meal.Meal{ Id = 2, Name = "Margherita Pizza", Price = 15, IsAvailable = true },
            new Model.Meal.Meal { Id = 3, Name = "Vegetarian Burger", Price = 10, IsAvailable = false }
        ];

        return meals;
    }
}
