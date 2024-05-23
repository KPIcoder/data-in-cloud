using DataInCloud.Model.Restaurant;

namespace DataInCloud.Orchestrators;

public class RestaurantOrchestrator : IRestaurantOrchestrator
{

    private readonly IRestaurantRepository _restaurantRepository;

    public RestaurantOrchestrator(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<Model.Restaurant.Restaurant> CreateAsync(Model.Restaurant.Restaurant restaurant)
    {
        return await _restaurantRepository.CreateAsync(restaurant);
    }

    public async Task<List<Model.Restaurant.Restaurant>> GetAllAsync()
    {
        return await _restaurantRepository.GetAllAsync();
    }

    public async Task<Model.Restaurant.Restaurant> GetByIdAsync(string id)
    {
        return await _restaurantRepository.GetByIdAsync(id);
    }

    public async Task RemoveAsync(string id) =>
        await _restaurantRepository.RemoveAsync(id);


    public async Task<Model.Restaurant.Restaurant> UpdateAsync(string id, Model.Restaurant.Restaurant restaurant) =>
        await _restaurantRepository.UpdateAsync(id, restaurant);

}