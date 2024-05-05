namespace DataInCloud.Model.Meal;
public interface IMealRepository
{
    Task<List<Meal>> GetAllAsync();
}