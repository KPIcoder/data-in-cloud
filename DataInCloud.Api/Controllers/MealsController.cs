using AutoMapper;
using DataInCloud.Model.Meal;
using DataInCloud.Orchestrators.Meal;
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
        var meals = _mapper.Map<List<MealContract>>(entities);

        return Ok(meals);
    }


}
