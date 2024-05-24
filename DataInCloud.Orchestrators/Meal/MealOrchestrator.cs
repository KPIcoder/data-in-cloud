using DataInCloud.Model.Meal;

namespace DataInCloud.Orchestrators;

public class MealOrchestrator : IMealOrchestrator
{
    private readonly IMealRepository _mealRepository;
    public MealOrchestrator(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }

    public Task<Model.Meal.Meal> CreateAsync(Model.Meal.Meal meal)
    {
        return _mealRepository.CreateAsync(meal);
    }

    public async Task<List<Model.Meal.Meal>> GetAllAsync()
    {
        return await _mealRepository.GetAllAsync();
    }

    public async Task<Model.Meal.Meal> GetByIdAsync(int id)
    {
        return await _mealRepository.GetByIdAsync(id);
    }

    public async Task<Model.Meal.Meal> RemoveByIdAsync(int id)
    {
        return await _mealRepository.RemoveAsync(id);
    }

    public async Task<Model.Meal.Meal> UpdateByIdAsync(int id, Model.Meal.Meal meal)
    {
        return await _mealRepository.UpdateAsync(id, meal);
    }
}