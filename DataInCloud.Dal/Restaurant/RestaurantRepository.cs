using AutoMapper;
using DataInCloud.Dal.Restaurant;
using DataInCloud.Model.Restaurant;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MongoDB.Bson;
using MongoDB.Driver;

public class RestaurantRepository : IRestaurantRepository
{

    private readonly MongoDbContext _context;
    private readonly IMapper _mapper;

    public RestaurantRepository(MongoDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    public async Task<Restaurant> CreateAsync(Restaurant restaurant)
    {
        var dbModel = _mapper.Map<RestaurantDao>(restaurant);

        await _context.Restaurants.InsertOneAsync(dbModel);

        return restaurant;
    }

    public async Task<List<Restaurant>> GetAllAsync()
    {
        var entities = await _context.Restaurants.Find(_ => true).ToListAsync();

        return _mapper.Map<List<Restaurant>>(entities);
    }

    public async Task<Restaurant> GetByIdAsync(string id)
    {
        var entity = await _context.Restaurants.Find(r => r.Id == id).FirstOrDefaultAsync();

        var model = _mapper.Map<Restaurant>(entity);

        return model;
    }

    public async Task RemoveAsync(string id) =>
     await _context.Restaurants.DeleteOneAsync(r => r.Id == id);


    public async Task<Restaurant> UpdateAsync(string id, Restaurant restaurant)
    {
        var dao = _mapper.Map<RestaurantDao>(restaurant);

        var updateBuilder = Builders<RestaurantDao>.Update
            .Set(r => r.Name, dao.Name)
            .Set(r => r.IsOpen, dao.IsOpen);

        await _context.Restaurants.UpdateOneAsync(r => r.Id == id, updateBuilder);

        var entity = await _context.Restaurants.Find(r => r.Id == id).FirstOrDefaultAsync();

        var model = _mapper.Map<Restaurant>(entity);

        return model;
    }
}