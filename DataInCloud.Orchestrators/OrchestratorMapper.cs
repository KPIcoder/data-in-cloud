using AutoMapper;
using DataInCloud.Orchestrators.Meal.Contract;
using DataInCloud.Orchestrators.Restaurant.Contract;
using MongoDB.Bson;

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

        CreateMap<RestaurantContract, Model.Restaurant.Restaurant>().ReverseMap();

        CreateMap<CreateRestaurantContract, Model.Restaurant.Restaurant>()
           .ForMember(dest => dest.Id, opt => opt.MapFrom(src => ObjectId.GenerateNewId()))
           .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => 0.0))
           .ForMember(dest => dest.IsOpen, opt => opt.MapFrom(src => true));

        CreateMap<UpdateRestaurantContract, Model.Restaurant.Restaurant>()
            .ForMember(dest => dest.Id, src => src.Ignore())
            .ForMember(dest => dest.Rating, src => src.Ignore());
    }

}