using DataInCloud.Model.Meal;
using DataInCloud.Model.Storage;

public class MealsInRestaurantOrchestrator : IMealsInRestaurantOrchestrator
{
    private readonly IBlobStorage _storage;
    private readonly IMealRepository _mealRepository;

    public MealsInRestaurantOrchestrator(IBlobStorage storage, IMealRepository mealRepository)
    {
        _storage = storage;
        _mealRepository = mealRepository;
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
        var entity = await _mealRepository.GetByIdAsync(mealId);

        await _storage.CreateFile($"{restaurantId}_{mealId}", entity);

        return entity;
    }
}