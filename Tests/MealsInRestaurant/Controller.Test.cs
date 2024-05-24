using System.Net.Http.Json;
using AutoMapper;
using DataInCloud.Dal;
using DataInCloud.Model.Meal;
using DataInCloud.Orchestrators;
using DataInCloud.Orchestrators.Meal.Contract;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

public class MealsInRestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IMealsInRestaurantOrchestrator> _mealsInRestaurantOrchestratorMock;
    private readonly IMapper _mapper;

    public MealsInRestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _mealsInRestaurantOrchestratorMock = new Mock<IMealsInRestaurantOrchestrator>();

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new OrchestatorMapper());
            mc.AddProfile(new DaoMapper());
        });
        _mapper = mappingConfig.CreateMapper();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mealsInRestaurantOrchestratorMock.Object);
                services.AddSingleton(_mapper);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetMealInRestaurant_ShouldReturnOk_WithMeal()
    {
        // Arrange
        var restaurantId = "restaurant1";
        var mealId = 1;
        var mealModel = new Meal { Id = mealId, Name = "Pizza", Price = 10, IsAvailable = true };
        _mealsInRestaurantOrchestratorMock.Setup(m => m.GetMealInRestaurantAsync(restaurantId, mealId))
            .ReturnsAsync(mealModel);

        // Act
        var response = await _client.GetAsync($"/api/v1/restaurants/{restaurantId}/meals/{mealId}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MealContract>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mealId, result.Id);
        Assert.Equal("Pizza", result.Name);
    }

    [Fact]
    public async Task AddExistingMealToRestaurant_ShouldReturnOk_WithMeal()
    {
        // Arrange
        var restaurantId = "restaurant1";
        var mealId = 1;
        var mealModel = new Meal { Id = mealId, Name = "Pizza", Price = 10, IsAvailable = true };
        _mealsInRestaurantOrchestratorMock.Setup(m => m.AddExistingMealToRestaurantAsync(restaurantId, mealId))
            .ReturnsAsync(mealModel);

        // Act
        var response = await _client.PostAsync($"/api/v1/restaurants/{restaurantId}/meals/{mealId}", null);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MealContract>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(mealId, result.Id);
        Assert.Equal("Pizza", result.Name);
    }
}
