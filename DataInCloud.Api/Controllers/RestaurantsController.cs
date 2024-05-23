
using AutoMapper;
using DataInCloud.Model.Restaurant;
using DataInCloud.Orchestrators.Restaurant.Contract;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver;

namespace DataInCloud.Controllers;

[ApiController]
[Route("api/v1/restaurants")]
public class RestaurantController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IRestaurantOrchestrator _restaurantOrchestrator;

    public RestaurantController(IMapper mapper, IRestaurantOrchestrator restaurantOrchestrator)
    {
        _mapper = mapper;
        _restaurantOrchestrator = restaurantOrchestrator;
    }

    [HttpGet]
    public async Task<IActionResult> GetRestaurants()
    {
        var entities = await _restaurantOrchestrator.GetAllAsync();

        var consumerContracts = _mapper.Map<List<RestaurantContract>>(entities);

        return Ok(consumerContracts);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetRestaurantById(string id)
    {
        var model = await _restaurantOrchestrator.GetByIdAsync(id);

        var contract = _mapper.Map<RestaurantContract>(model);

        return Ok(contract);
    }

    [HttpPost]
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantContract contract)
    {
        var model = _mapper.Map<Restaurant>(contract);

        Console.WriteLine(model.ToString());

        await _restaurantOrchestrator.CreateAsync(model);

        var consumerContract = _mapper.Map<RestaurantContract>(model);

        return Ok(consumerContract);
    }
    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateRestaurant(string id, UpdateRestaurantContract contract)
    {
        var model = _mapper.Map<Restaurant>(contract);

        var updatedModel = await _restaurantOrchestrator.UpdateAsync(id, model);

        var consumerContract = _mapper.Map<RestaurantContract>(updatedModel);

        return Ok(consumerContract);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> RemoveRestaurant(string id)
    {
        await _restaurantOrchestrator.RemoveAsync(id);

        return NoContent();
    }
}