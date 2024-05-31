using AutoMapper;
using DataInCloud.Dal;
using DataInCloud.Dal.Meal;
using DataInCloud.Model.Meal;
using Microsoft.EntityFrameworkCore;

public class MealRepositoryTests
{
    private readonly DbContextOptions<AppDbContext> _dbContextOptions;
    private readonly IMapper _mapper;

    public MealRepositoryTests()
    {
        _dbContextOptions = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        var mappingConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new DaoMapper());
        });
        _mapper = mappingConfig.CreateMapper();
    }

    private AppDbContext GetContext()
    {
        return new AppDbContext(_dbContextOptions);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateMeal()
    {
        // Arrange
        var context = GetContext();
        var repository = new MealRepository(context, _mapper);
        var meal = new Meal { Name = "Test Meal", Price = 10, IsAvailable = true };

        // Act
        var result = await repository.CreateAsync(meal);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(meal.Name, result.Name);
        Assert.Equal(meal.Price, result.Price);
        Assert.Equal(meal.IsAvailable, result.IsAvailable);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllMeals()
    {
        // Arrange
        var context = GetContext();
        context.Meals.AddRange(
            new MealDao { Id = 1, Name = "Meal 1", Price = 10, IsAvailable = true },
            new MealDao { Id = 2, Name = "Meal 2", Price = 15, IsAvailable = false });
        await context.SaveChangesAsync();
        var repository = new MealRepository(context, _mapper);

        // Act
        var result = await repository.GetAllAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetByIdAsync_ShouldReturnMeal()
    {
        // Arrange
        var context = GetContext();
        context.Meals.Add(new MealDao { Id = 11, Name = "Meal 1", Price = 10, IsAvailable = true });
        await context.SaveChangesAsync();
        var repository = new MealRepository(context, _mapper);

        // Act
        var result = await repository.GetByIdAsync(11);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meal 1", result.Name);
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveMeal()
    {
        // Arrange
        var context = GetContext();
        context.Meals.Add(new MealDao { Id = 4, Name = "Meal 1", Price = 10, IsAvailable = true });
        await context.SaveChangesAsync();
        var repository = new MealRepository(context, _mapper);

        // Act
        var result = await repository.RemoveAsync(4);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Meal 1", result.Name);
        Assert.Null(await context.Meals.FindAsync(4));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateMeal()
    {
        // Arrange
        var context = GetContext();
        context.Meals.Add(new MealDao { Id = 5, Name = "Meal 1", Price = 10, IsAvailable = true });
        await context.SaveChangesAsync();
        var repository = new MealRepository(context, _mapper);
        var updatedMeal = new Meal { Id = 5, Name = "Updated Meal", Price = 15, IsAvailable = false };

        // Act
        var result = await repository.UpdateAsync(5, updatedMeal);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(updatedMeal.Name, result.Name);
        Assert.Equal(updatedMeal.Price, result.Price);
        Assert.Equal(updatedMeal.IsAvailable, result.IsAvailable);
    }
}
