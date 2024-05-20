using AutoMapper;
using DataInCloud.Model.Meal;
using DataInCloud.Orchestrators.Meal.Contract;
using Microsoft.AspNetCore.Mvc;


namespace DataInCloud.Controllers;

[ApiController]
[Route("api/v1/meals")]
public class MealsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IMealOrchestrator _mealOrchestrator;

    public MealsController(IMapper mapper, IMealOrchestrator mealOrchestrator)
    {
        _mapper = mapper;
        _mealOrchestrator = mealOrchestrator;
    }


    [HttpGet]
    public async Task<IActionResult> GetMeals()
    {
        var entities = await _mealOrchestrator.GetAllAsync();

        foreach (var entity in entities)
        {
            Console.WriteLine($"{entity.Name} {entity.IsAvailable}");
        }

        var meals = _mapper.Map<List<MealContract>>(entities);

        return Ok(meals);
    }
    [HttpGet("{id}")]
    public async Task<IActionResult> GetMealById(int id)
    {
        var entity = await _mealOrchestrator.GetByIdAsync(id);

        var contract = _mapper.Map<MealContract>(entity);

        return Ok(contract);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMeal(CreateMealContract meal)
    {
        var entityToCreate = _mapper.Map<Meal>(meal);

        var createdEntity = await _mealOrchestrator.CreateAsync(entityToCreate);

        return Ok(createdEntity);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateMeal(int id, UpdateMealContract meal)
    {
        var entityToCreate = _mapper.Map<Meal>(meal);

        var createdEntity = await _mealOrchestrator.UpdateByIdAsync(id, entityToCreate);

        return Ok(createdEntity);
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveMeal(int id)
    {

        var removedEntity = await _mealOrchestrator.RemoveByIdAsync(id);

        return Ok(removedEntity);
    }
}
