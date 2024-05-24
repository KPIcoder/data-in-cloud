using AutoMapper;
using DataInCloud.Dal;
using DataInCloud.Model.Restaurant;
using DataInCloud.Orchestrators;
using DataInCloud.Orchestrators.Restaurant.Contract;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net.Http.Json;

public class RestaurantControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly Mock<IRestaurantOrchestrator> _restaurantOrchestratorMock;
    private readonly IMapper _mapper;

    public RestaurantControllerTests(WebApplicationFactory<Program> factory)
    {
        _restaurantOrchestratorMock = new Mock<IRestaurantOrchestrator>();

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
                services.AddSingleton(_restaurantOrchestratorMock.Object);
                services.AddSingleton(_mapper);
            });
        }).CreateClient();
    }

    [Fact]
    public async Task GetRestaurants_ReturnsOkResult_WithListOfRestaurants()
    {
        // Arrange
        var restaurants = new List<Restaurant>
        {
            new Restaurant { Id = "1", Name = "Restaurant A", Rating = 4.5, IsOpen = true },
            new Restaurant { Id = "2", Name = "Restaurant B", Rating = 4.0, IsOpen = false }
        };

        _restaurantOrchestratorMock.Setup(repo => repo.GetAllAsync()).ReturnsAsync(restaurants);

        // Act
        var response = await _client.GetAsync("/api/v1/restaurants");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<RestaurantContract>>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetRestaurantById_ReturnsOkResult_WithRestaurant()
    {
        // Arrange
        var restaurant = new Restaurant { Id = "1", Name = "Restaurant A", Rating = 4.5, IsOpen = true };
        _restaurantOrchestratorMock.Setup(repo => repo.GetByIdAsync("1")).ReturnsAsync(restaurant);

        // Act
        var response = await _client.GetAsync("/api/v1/restaurants/1");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RestaurantContract>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Restaurant A", result.Name);
    }

    [Fact]
    public async Task CreateRestaurant_ReturnsOkResult_WithCreatedRestaurant()
    {
        // Arrange
        var createRestaurant = new CreateRestaurantContract { Name = "New Restaurant" };
        var createdRestaurant = new Restaurant { Id = "1", Name = "New Restaurant", Rating = 0, IsOpen = true };

        _restaurantOrchestratorMock.Setup(repo => repo.CreateAsync(It.IsAny<Restaurant>())).ReturnsAsync(createdRestaurant);

        // Act
        var response = await _client.PostAsJsonAsync("/api/v1/restaurants", createRestaurant);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RestaurantContract>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("New Restaurant", result.Name);
    }

    [Fact]
    public async Task UpdateRestaurant_ReturnsOkResult_WithUpdatedRestaurant()
    {
        // Arrange
        var updateRestaurant = new UpdateRestaurantContract { Name = "Updated Restaurant", IsOpen = false };
        var updatedRestaurant = new Restaurant { Id = "1", Name = "Updated Restaurant", Rating = 4.5, IsOpen = false };

        _restaurantOrchestratorMock.Setup(repo => repo.UpdateAsync("1", It.IsAny<Restaurant>())).ReturnsAsync(updatedRestaurant);

        // Act
        var response = await _client.PatchAsJsonAsync("/api/v1/restaurants/1", updateRestaurant);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<RestaurantContract>();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Restaurant", result.Name);
        Assert.False(result.IsOpen);
    }

    [Fact]
    public async Task RemoveRestaurant_ReturnsNoContentResult()
    {
        // Arrange
        _restaurantOrchestratorMock.Setup(repo => repo.RemoveAsync("1")).Returns(Task.CompletedTask);

        // Act
        var response = await _client.DeleteAsync("/api/v1/restaurants/1");
        response.EnsureSuccessStatusCode();

        // Assert
        Assert.Equal(System.Net.HttpStatusCode.NoContent, response.StatusCode);
    }
}
