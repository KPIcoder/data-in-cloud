using AutoMapper;
using DataInCloud.Orchestrators.Meal.Contract;

namespace DataInCloud.Orchestrators;

public class OrchestatorMapper : Profile
{
    public OrchestatorMapper()
    {
        CreateMap<Model.Meal.Meal, MealContract>().ReverseMap();

        CreateMap<CreateMealContract, Model.Meal.Meal>()
        .ForMember(dest => dest.Id, src => src.Ignore())
        .ReverseMap();

        CreateMap<UpdateMealContract, Model.Meal.Meal>();
        // .ForMember(dest => dest.Id, src => src.Ignore());
    }

}