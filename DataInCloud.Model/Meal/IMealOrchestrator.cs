
namespace DataInCloud.Model.Meal;
public interface IMealOrchestrator
{
    Task<Meal> CreateAsync(Meal entityToCreate);
    Task<List<Meal>> GetAllAsync();
    Task<Meal> GetByIdAsync(int id);
    Task<Meal> UpdateByIdAsync(int id, Meal meal);
    Task<Meal> RemoveByIdAsync(int id);


};