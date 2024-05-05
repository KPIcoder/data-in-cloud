namespace DataInCloud.Model.Meal;
public interface IMealOrchestrator
{
    Task<List<Meal>> GetAllAsync();
};