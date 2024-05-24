using AutoMapper;
using DataInCloud.Orchestrators.Meal.Contract;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/v1/restaurants/")]
public class MealsInRestaurantController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMealsInRestaurantOrchestrator _mealsInRestaurantOrchestrator;

    public MealsInRestaurantController(IMapper mapper, IMealsInRestaurantOrchestrator mealsInRestaurantOrchestrator)
    {
        _mapper = mapper;
        _mealsInRestaurantOrchestrator = mealsInRestaurantOrchestrator;
    }


    [HttpGet("{restaurantId}/meals/{mealId}")]
    public async Task<IActionResult> GetMealInRestaurant(string restaurantId, int mealId)
    {
        var model = await _mealsInRestaurantOrchestrator.GetMealInRestaurantAsync(restaurantId, mealId);

        var contract = _mapper.Map<MealContract>(model);

        return Ok(contract);
    }

    [HttpPost("{restaurantId}/meals/{mealId}")]
    public async Task<IActionResult> AddExistingMealToRestaurant(string restaurantId, int mealId)
    {
        var model = await _mealsInRestaurantOrchestrator.AddExistingMealToRestaurantAsync(restaurantId, mealId);

        var contract = _mapper.Map<MealContract>(model);

        return Ok(contract);
    }
}

