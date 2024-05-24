namespace DataInCloud.Model.Restaurant;
public interface IRestaurantOrchestrator
{
    Task<Restaurant> CreateAsync(Restaurant restaurant);
    Task<List<Restaurant>> GetAllAsync();
    Task<Restaurant> GetByIdAsync(string id);
    Task<Restaurant> UpdateAsync(string id, Restaurant restaurant);
    Task RemoveAsync(string id);
}