using AutoMapper;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System.Net.Http.Json;
using Xunit;

using DataInCloud.Dal;
using DataInCloud.Model.Meal;
using DataInCloud.Orchestrators.Meal.Contract;
using DataInCloud.Orchestrators;

public class MealsControllerTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly HttpClient _client;
    private readonly Mock<IMealOrchestrator> _mealOrchestratorMock;
    private readonly IMapper _mapper;

    public MealsControllerTests(WebApplicationFactory<Program> factory)
    {
        _mealOrchestratorMock = new Mock<IMealOrchestrator>();

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new DaoMapper());
            mc.AddProfile(new OrchestatorMapper());
        });
        _mapper = mappingConfig.CreateMapper();

        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                services.AddSingleton(_mealOrchestratorMock.Object);
                services.AddSingleton(_mapper);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetMeals_ReturnsOkResult_WithListOfMeals()
    {
        // Arrange
        var meals = new List<Meal>
            {
                new Meal { Id = 1, Name = "Pizza", Price = 10, IsAvailable = true },
                new Meal { Id = 2, Name = "Burger", Price = 15, IsAvailable = false }
            };

        _mealOrchestratorMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(meals);

        // Act
        var response = await _client.GetAsync("/api/v1/meals");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<MealContract>>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetMealById_ReturnsOkResult_WithMeal()
    {
        // Arrange
        var meal = new Meal { Id = 1, Name = "Pizza", Price = 10, IsAvailable = true };
        _mealOrchestratorMock.Setup(repo => repo.GetByIdAsync(1)).ReturnsAsync(meal);

        // Act
        var response = await _client.GetAsync("/api/v1/meals/1");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MealContract>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Pizza", result.Name);
    }

    [Fact]
    public async Task CreateMeal_ReturnsOkResult_WithCreatedMeal()
    {
        // Arrange
        var createMeal = new CreateMealContract { Name = "Pasta", Price = 12, IsAvailable = true };
        var createdMeal = new Meal { Id = 1, Name = "Pasta", Price = 12, IsAvailable = true };

        _mealOrchestratorMock.Setup(repo => repo.CreateAsync(It.IsAny<Meal>())).ReturnsAsync(createdMeal);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/meals", createMeal);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Meal>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Pasta", result.Name);
    }

    [Fact]
    public async Task UpdateMeal_ReturnsOkResult_WithUpdatedMeal()
    {
        // Arrange
        var updateMeal = new UpdateMealContract { Name = "Salad", Price = 8, IsAvailable = true };
        var updatedMeal = new Meal { Id = 1, Name = "Salad", Price = 8, IsAvailable = true };

        _mealOrchestratorMock.Setup(repo => repo.UpdateByIdAsync(1, It.IsAny<Meal>())).ReturnsAsync(updatedMeal);

        // Act
        var response = await _client.PatchAsJsonAsync("/api/v1/meals/1", updateMeal);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Meal>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Salad", result.Name);
    }

    [Fact]
    public async Task RemoveMeal_ReturnsOkResult_WithRemovedMeal()
    {
        // Arrange
        var removedMeal = new Meal { Id = 1, Name = "Pizza", Price = 10, IsAvailable = true };

        _mealOrchestratorMock.Setup(repo => repo.RemoveByIdAsync(1)).ReturnsAsync(removedMeal);

        // Act
        var response = await _client.DeleteAsync("/api/v1/meals/1");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<Meal>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Pizza", result.Name);
    }
}
