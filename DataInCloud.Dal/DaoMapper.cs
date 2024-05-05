using AutoMapper;
using DataInCloud.Dal.Meal;

namespace DataInCloud.Dal;
public class DaoMapper : Profile
{
    public DaoMapper()
    {
        CreateMap<MealDao, Model.Meal.Meal>();
    }
}