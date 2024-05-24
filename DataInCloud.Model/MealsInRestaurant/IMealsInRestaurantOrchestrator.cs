using DataInCloud.Model.Meal;

public interface IMealsInRestaurantOrchestrator
{
    Task<Meal> GetMealInRestaurantAsync(string restaurantId, int mealId);

    Task<Meal> AddExistingMealToRestaurantAsync(string restaurantId, int mealId);
}