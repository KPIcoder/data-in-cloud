using AutoMapper;
using DataInCloud.Model.Meal;
using Microsoft.EntityFrameworkCore;

namespace DataInCloud.Dal.Meal;
public class MealRepository : IMealRepository
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public MealRepository(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<Model.Meal.Meal> CreateAsync(Model.Meal.Meal meal)
    {
        var entity = _mapper.Map<MealDao>(meal);

        var createdEntity = await _context.AddAsync(entity);

        await _context.SaveChangesAsync();

        return _mapper.Map<Model.Meal.Meal>(createdEntity.Entity);
    }

    public async Task<List<Model.Meal.Meal>> GetAllAsync()
    {
        var entities = await _context.Meals.ToArrayAsync();

        foreach (var entity in entities)
        {
            Console.WriteLine($"{entity.Name} {entity.IsAvailable}");
        }

        var meals = _mapper.Map<List<Model.Meal.Meal>>(entities);

        return meals;
    }

    public async Task<Model.Meal.Meal> GetByIdAsync(int id)
    {
        var entity = await _context.Meals.FindAsync(id);

        var meal = _mapper.Map<Model.Meal.Meal>(entity);

        return meal;
    }

    public async Task<Model.Meal.Meal> RemoveAsync(int id)
    {
        var entity = await _context.Meals.FindAsync(id);

        _context.Meals.Remove(entity);

        await _context.SaveChangesAsync();

        return _mapper.Map<Model.Meal.Meal>(entity);
    }

    public async Task<Model.Meal.Meal> UpdateAsync(int id, Model.Meal.Meal meal)
    {
        var entity = _mapper.Map<MealDao>(meal);

        var updatedEntity = await _context.Meals.SingleAsync(m => m.Id == id);

        updatedEntity.Price = (short)meal.Price;
        updatedEntity.IsAvailable = meal.IsAvailable;
        updatedEntity.Name = meal.Name;

        await _context.SaveChangesAsync();

        return _mapper.Map<Model.Meal.Meal>(updatedEntity);
    }
}
