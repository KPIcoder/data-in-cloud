using AutoMapper;
using DataInCloud.Dal.Meal;
using DataInCloud.Dal.Restaurant;

namespace DataInCloud.Dal;
public class DaoMapper : Profile
{
    public DaoMapper()
    {
        CreateMap<MealDao, Model.Meal.Meal>().ReverseMap();

        CreateMap<RestaurantDao, Model.Restaurant.Restaurant>().ReverseMap();
    }
}