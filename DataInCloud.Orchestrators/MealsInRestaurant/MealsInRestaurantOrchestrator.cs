using DataInCloud.Model.Log;
using DataInCloud.Model.Meal;
using DataInCloud.Model.Storage;
using DataInCloud.Platform.EventHub;
using DataInCloud.Platform.MessageBroker;

public class MealsInRestaurantOrchestrator : IMealsInRestaurantOrchestrator
{
    private readonly IBlobStorage _storage;
    private readonly IMealRepository _mealRepository;
    private readonly IPublisher<Log> _publisher;

    public MealsInRestaurantOrchestrator(IBlobStorage storage, IMealRepository mealRepository, IPublisher<Log> publisher)
    {
        _storage = storage;
        _mealRepository = mealRepository;
        _publisher = publisher;
    }
    public async Task<Meal> GetMealInRestaurantAsync(string restaurantId, int mealId)
    {
        var fileName = $"{restaurantId}_{mealId}";
        var exists = await _storage.FileExistsAsync(fileName);

        if (exists)
        {
            return await _storage.ReadFileAsync<Meal>(fileName);
        }

        throw new FileNotFoundException($"File {fileName} does not exist");
    }

    public async Task<Meal> AddExistingMealToRestaurantAsync(string restaurantId, int mealId)
    {

        await _publisher.PublishAsync(new Log
        {
            Message = $"Meal {mealId} added",
            DateUTC = DateTime.Now
        });

        var entity = await _mealRepository.GetByIdAsync(mealId);

        await _storage.CreateFileAsync($"{restaurantId}_{mealId}", entity);

        return entity;
    }
}