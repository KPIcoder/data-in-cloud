using DataInCloud.Model.Meal;

namespace DataInCloud.Orchestrators;

public class MealOrchestrator : IMealOrchestrator
{
    private readonly IMealRepository _mealRepository;
    public MealOrchestrator(IMealRepository mealRepository)
    {
        _mealRepository = mealRepository;
    }
    public async Task<List<Model.Meal.Meal>> GetAllAsync()
    {
        return await _mealRepository.GetAllAsync();
    }
}