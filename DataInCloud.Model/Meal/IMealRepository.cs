namespace DataInCloud.Model.Meal;
public interface IMealRepository
{
    Task<Meal> CreateAsync(Meal meal);
    Task<List<Meal>> GetAllAsync();
    Task<Meal> GetByIdAsync(int id);

    Task<Meal> UpdateAsync(int id, Meal meal);

    Task<Meal> RemoveAsync(int id);
}