using AutoMapper;
using DataInCloud.Orchestrators.Meal;

namespace DataInCloud.Orchestrators;

public class OrchestatorMapper : Profile
{
    public OrchestatorMapper()
    {
        CreateMap<Model.Meal.Meal, MealContract>();
    }
}